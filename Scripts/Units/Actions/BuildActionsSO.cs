using UnityEngine;

[CreateAssetMenu(fileName ="BuildAction" , menuName = "Pg/Actions/BuildAction")]   
public class BuildActionsSO : ActionsSO
{    
    [SerializeField] private StructureUnit MStructPrefab;
    [SerializeField] private float MConstructionTime;
    [SerializeField] private Sprite MPlacementSprite;
    [SerializeField] private Sprite MFoundationSprite;
    [SerializeField] private Sprite MCompletionSprite;

    [SerializeField] private int MGoldCost;
    [SerializeField] private int MWoodCost;

    [SerializeField] private Vector3Int MBuildingSize;
    [SerializeField] private Vector3Int MOriginOffset;


    public StructureUnit StructPrefab => MStructPrefab;
    public float ConstructionTime => MConstructionTime;
    public Sprite PlacementSprite  => MPlacementSprite;
    public Sprite FoundationSprite => MFoundationSprite;
    public Sprite CompletionSprite => MCompletionSprite;

    public int GoldCost => MGoldCost;
    public int WoodCost => MWoodCost;

    public Vector3Int BuildingSize => MBuildingSize;
    public Vector3Int OriginOffset => MOriginOffset;


    public override void Execute(GameManager _manager)
    {
        _manager.StartBuildPrc(this);
    }
}