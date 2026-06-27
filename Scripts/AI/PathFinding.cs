

using UnityEngine;

public class PathFinding
{
    private TilemapManager tilemapManager;
    private int width , height;
    private Node[,] MGrid;
    private  Vector3Int GridOffst;
    public Node[,] Grid => MGrid;

    public PathFinding(TilemapManager _tlmpMngr)
    {
        tilemapManager = _tlmpMngr;
        tilemapManager.PathfindingTilemap.CompressBounds();
        var _bounds = tilemapManager.PathfindingTilemap.cellBounds;
        width = _bounds.size.x; height = _bounds.size.y;  
        MGrid = new Node[width,height];
        GridOffst = tilemapManager.PathfindingTilemap.cellBounds.min;

        initializeGrids();
    }

    void initializeGrids()
    {
        Vector3 _cellsize = tilemapManager.PathfindingTilemap.cellSize;

        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                Vector3Int nodeLBPos = new Vector3Int(x+GridOffst.x , y+GridOffst.y);
                bool isWlkble = tilemapManager.CanWalkOnTile(nodeLBPos);
                var node = new Node(nodeLBPos,_cellsize,isWlkble);
                MGrid[x,y] = node;
            }
        }
    }

    public void FindingPath(Vector3 _strtPos , Vector3 _endPos)
    {
        Node _strtNode = FindNode(_strtPos);
        Node _endNode = FindNode(_endPos);

        Debug.Log("strt" + _strtNode);
        Debug.Log("end" + _endNode);
    }

    Node FindNode(Vector3 _pos)
    {
        Vector3Int _flrdPos = new Vector3Int(Mathf.FloorToInt(_pos.x),Mathf.FloorToInt(_pos.y));

        int _gridX = _flrdPos.x - GridOffst.x; 
        int _gridY = _flrdPos.y - GridOffst.y;

        if(_gridX >= 0 && _gridX < width && _gridY >= 0 && _gridY < height)
        {
            return MGrid[_gridX,_gridY];
        }

        return null;
    }

}
