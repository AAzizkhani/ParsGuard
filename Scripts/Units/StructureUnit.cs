using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureUnit : Units
{
    private BuildingProc BuildPrc;
    public bool IsUnderConstruct => BuildPrc != null;

    void Update()
    {
        if(IsUnderConstruct)
            BuildPrc.UpdateProc();
    }

    public void OnConstructionFinished()
    {
        BuildPrc = null;
    }
    public void RegisterPrc(BuildingProc _prc)
    {
        BuildPrc = _prc;
    }

    public void AssignWorkerToBuildProc(WorkerUnit _worker)
    {
        BuildPrc?.AddWorker(_worker);
    }
    public void UnAssignWorkerToBuildProc()
    {
        BuildPrc?.RemoveWorker();
    }
}
