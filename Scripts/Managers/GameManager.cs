
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ClickType{Move,Attack,Build}
public class GameManager : SinglatonManager<GameManager>
{
    [SerializeField] private PointToClick PointToClickObj;
    [SerializeField] private PointToClick PointToBuildObj;

    [SerializeField] private Units ActiveUnit;
    [SerializeField] private ActionBar ActionBar;
    [SerializeField] private BuildConfirmBar BuildConfirmBar;

    [SerializeField] private float moveCooldown = 0.4f;    

    [SerializeField] private ParticleSystem ConstructionEffect;
    
    private PlacementPrc PlacementProcess;
    private float lastMoveTime;

    private int MGold = 10000;
    private int MWood = 10000;

    public int Gold => MGold;
    public int Wood => MWood;

    void Start()
    {
        ClearActionBarUI();
    }

    void Update()
    {
        if(PlacementProcess != null)
            PlacementProcess.Update();
        else if(PgUtils.GetClickPosition(out Vector2 _inputPos))
        {
            DetectClick(_inputPos);
        }
    }

    public void StartBuildPrc(BuildActionsSO _input)
    {
        if(PlacementProcess != null) return;

        var _tilemapMan = TilemapManager.Get();
        PlacementProcess = new PlacementPrc(_input , _tilemapMan);

        PlacementProcess.ShowPlacmentPrc();
        BuildConfirmBar.Show(_input.GoldCost , _input.WoodCost);
        BuildConfirmBar.SetupHooks(ConfirmBuild , CancelBuild);
    }

    void DetectClick(Vector2 _inputPos)
    {
        if(PgUtils.IsPointerOverUIElement())
            return;

        Vector2 _worldPnt = Camera.main.ScreenToWorldPoint(_inputPos);
        RaycastHit2D _hit = Physics2D.Raycast(_worldPnt , Vector2.zero );

        if(IsClickedOnUnit(_hit , out var _unit))
        {
            HandleClickOnUnit(_unit);
        }
        else
        {
            HandleClickOnGrn(_worldPnt);
        }
    }

    bool IsClickedOnUnit(RaycastHit2D _hit , out Units _unit)
    {
        if(_hit.collider != null && _hit.collider.TryGetComponent<Units>(out var _clkUnt))
        {
            _unit = _clkUnt;
            return true;
        }
        _unit = null;
        return false;
    }
    void HandleClickOnGrn(Vector2 _worldPnt)
    {
        if(ActiveUnit != null && IsHumanuid(ActiveUnit))
        {
            if (Time.time - lastMoveTime < moveCooldown) 
                return;

            DisplayClickEffect(_worldPnt,ClickType.Move);
            ActiveUnit.MoveTo(_worldPnt);
        }

    }

    void HandleClickOnUnit(Units _unit)
    {
        if(ActiveUnit != null)
        {
            if(HasClickedOnActveUnt(_unit))
            {
                CancelActiveUnit();
                return;
            }

            else if (WorkerClickedOnUnFinishedBuild(_unit))
            {
                DisplayClickEffect(_unit.transform.position , ClickType.Build);
                ((WorkerUnit)ActiveUnit).SendToBuild(_unit as StructureUnit);
                return;
            }
        }

        SelectNewUnit(_unit);
    }

    bool WorkerClickedOnUnFinishedBuild(Units _clckUnt)
    {
        return ActiveUnit is WorkerUnit &&
                 _clckUnt is StructureUnit structure && structure.IsUnderConstruct;
    }
    void CancelActiveUnit()
    {
        ActiveUnit.SetSprGlowOn(false);
        ActiveUnit = null;

        ClearActionBarUI();
    }

    void SelectNewUnit(Units _unit)
    {
        if(ActiveUnit != null)
            ActiveUnit.SetSprGlowOn(false);

        ActiveUnit = _unit;
        ActiveUnit.SetSprGlowOn(true);
        ShowUnitsActions(_unit);
    }

    bool HasClickedOnActveUnt(Units _curUnit)
    {
        return _curUnit == ActiveUnit;
    }

    void DisplayClickEffect(Vector2 _worldPnt , ClickType _clkTyp)
    {
        if (Time.time - lastMoveTime < moveCooldown) 
            return;
        lastMoveTime = Time.time;

        if(_clkTyp == ClickType.Move)
            Instantiate(PointToClickObj,(Vector3)_worldPnt,Quaternion.identity);

        if(_clkTyp == ClickType.Build)
            Instantiate(PointToBuildObj,(Vector3)_worldPnt,Quaternion.identity);
        
        if(_clkTyp == ClickType.Attack){}

    }

    bool IsHumanuid(Units _uint)
    {
        return _uint is HumanoidUnits;
    }

    void ShowUnitsActions(Units _unit)
    {
        ClearActionBarUI();

        if(_unit.Actions.Length == 0)
            return;

        ActionBar.Show();

        foreach(var action in _unit.Actions)
            ActionBar.RejisterActions(action.Icon , ()=>action.Execute(this));
    }

    void ClearActionBarUI()
    {
        ActionBar.ClearActions();
        ActionBar.Hide();
    }

    void ConfirmBuild()
    {
        if(!TryDecreasSource(PlacementProcess.BuildAction.GoldCost,
                             PlacementProcess.BuildAction.WoodCost))
        {
            Debug.Log("too few rec!");
            return;
        }

        if(PlacementProcess.TryConfirmPlacement(out Vector3 _buildPos))
        {
            DisplayClickEffect(_buildPos , ClickType.Build);

            BuildConfirmBar.Hide();

            new BuildingProc(PlacementProcess.BuildAction ,_buildPos ,(WorkerUnit)ActiveUnit ,ConstructionEffect);

            PlacementProcess = null;
        }

        else
        {
            RevertResources(PlacementProcess.BuildAction.GoldCost , PlacementProcess.BuildAction.WoodCost);
        }
    }

    void RevertResources(int _gold , int _wood)
    {
        MGold += _gold;
        MWood += _wood;
    }
    void CancelBuild()
    {
        BuildConfirmBar.Hide();
        PlacementProcess.ClearPlacmentPrc();
        PlacementProcess = null;
    }

    bool TryDecreasSource(int _goldCost , int _woodCost)
    {
        if(MGold >= _goldCost && MWood >= _woodCost)
        {
            MGold -= _goldCost;
            MWood -= _woodCost;
            return true;
        }

        return false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20,40,200,20) , "Gold:" + MGold.ToString() , new GUIStyle{fontSize = 50});
        GUI.Label(new Rect(20,100,200,20) , "Wood:" + MWood.ToString() , new GUIStyle{fontSize = 50});

        if(ActiveUnit != null)
        {
            GUI.Label(new Rect(20,160,200,20) , "State:" + ActiveUnit.CurrentState.ToString() , new GUIStyle{fontSize = 50});
            GUI.Label(new Rect(20,220,200,20) , "Task:" + ActiveUnit.CurrentTask.ToString() , new GUIStyle{fontSize = 50}); 
        }
    }
}
