using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : SinglatonManager<TilemapManager>
{
    [SerializeField] private Tilemap WalkableTile;
    [SerializeField] private Tilemap OverlayTile;
    [SerializeField] private Tilemap[] UnreachableTiles;
    private PathFinding l_PathFinding;

    public Tilemap PathfindingTilemap => WalkableTile;

    //test
    [SerializeField] private Transform strt;
    [SerializeField] private Transform end; 
    void Start()
    {
        l_PathFinding = new PathFinding(this);
    }

    void Update()
    {
        l_PathFinding.FindingPath(strt.position , end.position);
    }

    public bool CanWalkOnTile(Vector3Int _tilePos)
    {
        return WalkableTile.HasTile(_tilePos) && 
                !InUnreachableTiles(_tilePos) &&
                !InBlockedGameObjs (_tilePos);
    }
    public bool CanPlaceTile(Vector3Int _tilePos)
    {
        return WalkableTile.HasTile(_tilePos) && 
                !InUnreachableTiles(_tilePos) &&
                !InBlockedGameObjs (_tilePos);
    }

    public bool InUnreachableTiles(Vector3Int _tilePos)
    {
        foreach(var _tiles in UnreachableTiles)
        {
            if(_tiles.HasTile(_tilePos))
                return true;
        }
        return false;
    }

    public bool InBlockedGameObjs(Vector3Int _tilePos)
    {
        Vector3 _tileSize = WalkableTile.cellSize;
        Collider2D[] _colliders = Physics2D.OverlapBoxAll(_tilePos + _tileSize / 2 , _tileSize * 0.8f , 0);

        foreach(var _coll in _colliders)
        {
            var _layer = _coll.gameObject.layer;
            if(_layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }

        return false; 
    }

    public void SetOverlayTile(Vector3Int _tilePos , Tile _tile)
    {
        OverlayTile.SetTile(_tilePos , _tile);
    }

}
