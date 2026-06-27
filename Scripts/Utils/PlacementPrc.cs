
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementPrc
{
    private GameObject PlacementOutline;
    private BuildActionsSO MainBuildAction;
    private Vector3Int[] HighlightPositions;
    private const float PlaceZOrder = 10;

    private Sprite PlaceHolderTileSpr;

    private TilemapManager MainTilemapManager;

    private Color Color_HightLight = new Color(0,1,0,0.5f);
    private Color Color_Block = new Color(1,0,0,0.8f);


    public BuildActionsSO BuildAction => MainBuildAction;

    public PlacementPrc(BuildActionsSO _input , TilemapManager _tilemapmanager)
    {
        PlaceHolderTileSpr = Resources.Load<Sprite>("Images/PlaceholderTileSprite");
        MainBuildAction = _input;
        MainTilemapManager = _tilemapmanager;
    }

    public void Update()
    {
        if(PlacementOutline != null)
            HighlightTile(PlacementOutline.transform.position);
        
        if(PgUtils.IsPointerOverUIElement()) return;

        if(PgUtils.GetHoldPosition(out Vector3 _inputpos))
            PlacementOutline.transform.position = SnapToGrid(_inputpos);
    }


    public void ClearPlacmentPrc()
    {
        Object.Destroy(PlacementOutline);
        ClearHighlight();
    }

    public void ShowPlacmentPrc()
    {
        PlacementOutline = new GameObject("PlacementOutline");  
        var _rndr = PlacementOutline.AddComponent<SpriteRenderer>();
        _rndr.sortingOrder = 100;
        _rndr.color = new Color(1,1,1,0.5f);
        _rndr.sprite = MainBuildAction.PlacementSprite;
    }

    public bool TryConfirmPlacement(out Vector3 _buildPos)
    {
        if(IsPalcmentAreaValid())
        {
            ClearHighlight();
            _buildPos = PlacementOutline.transform.position;
            Object.Destroy(PlacementOutline);

            return true;
        }

        _buildPos = Vector3.zero;
        return false;
    }

    bool IsPalcmentAreaValid()
    {
        foreach(var _tile in HighlightPositions)
        {
            if(!MainTilemapManager.CanPlaceTile(_tile)) return false;
        }

        return true;
    }
    Vector3 SnapToGrid(Vector3 _inputPos)
    {
        return new Vector3(Mathf.FloorToInt(_inputPos.x) , Mathf.FloorToInt(_inputPos.y) , PlaceZOrder);
    }

    void HighlightTile(Vector3 _outlinePos)
    {
        Vector3Int buildingSize = MainBuildAction.BuildingSize;
        Vector3 pivotPos = _outlinePos + MainBuildAction.OriginOffset;

        ClearHighlight();
        HighlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];

        for(int x=0 ; x < buildingSize.x ; x++)
        {
            for(int y = 0; y<buildingSize.y ; y++)
            {
                HighlightPositions[x+y * buildingSize.x] = 
                    new Vector3Int ((int)pivotPos.x + x, (int)pivotPos.y + y, 0);
            }
        }

        foreach (var _tilePos in HighlightPositions)
        {
            var _tile = ScriptableObject.CreateInstance<Tile>();
            _tile.sprite = PlaceHolderTileSpr;

            if(MainTilemapManager.CanPlaceTile(_tilePos))
                _tile.color = Color_HightLight;
            else 
                _tile.color = Color_Block;

            MainTilemapManager.SetOverlayTile(_tilePos , _tile);
        }
    }

    void ClearHighlight()
    {
        if(HighlightPositions == null)
            return;
        
        foreach(var _tilePos in HighlightPositions)
            MainTilemapManager.SetOverlayTile(_tilePos , null);
    }

}
