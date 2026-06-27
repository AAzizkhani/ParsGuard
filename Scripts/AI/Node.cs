
using UnityEngine;

public class Node
{
    public int x,y;
    public float cntr_x , cntr_y;
    public bool isWalkable;

    public Node(Vector3Int _lbPos,Vector3 _cllsize, bool _wlkbl)
    {
        x = _lbPos.x; 
        y = _lbPos.y;

        var _nodeCntrPos = _lbPos + (_cllsize/2f);
        cntr_x = _nodeCntrPos.x;
        cntr_y = _nodeCntrPos.y;

        isWalkable = _wlkbl;
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }

}
