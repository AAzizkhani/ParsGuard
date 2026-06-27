using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorkerUnit : HumanoidUnits
{
    protected override void UpdateBehaviour()
    {
        if(CurrentTask == UnitTasks.Build && HasTarget)
            CheckForCunstruction();
    }

    protected override void OnSetDest()
    {
        ResetState();
    }

    /*private void CheckForCloseObjs() // CheckForCunstruction is better
    {
        var _hits = RumProxObjDetect();
        foreach(var _h in _hits)
        {
            if(_h.gameObject == this.gameObject) continue;

            if(CurrentTask == UnitTasks.Build && _h.gameObject == Target.gameObject)
            {
                if(_h.TryGetComponent<StructureUnit>(out var _unt))
                {
                    if(Vector3.Distance(transform.position, _unt.transform.position) < 0.5f)
                    {
                        StartBuilding(_unt);
                    }                
                }
            }
            
        }
    }*/

    public void OnBuildingFinished()
    {
        ResetState();
    }
    public void SendToBuild(StructureUnit structure)
    {
        ResetState();

        MoveTo(structure.transform.position);
        SetTask(UnitTasks.Build);
        SetTarget(structure);
    }
    
    void CheckForCunstruction()
    {
        var _dstToCnstrctn = Vector3.Distance(transform.position , Target.transform.position);

        if(_dstToCnstrctn <= MObjDetectRadius && CurrentState == UnitStates.Idle)
        {
            StartBuilding(Target as StructureUnit);
        }
    }
    void StartBuilding(StructureUnit _Structunit)
    {
        SetState(UnitStates.Building);
        MAnimator.SetBool("isBuild" , true);
        _Structunit.AssignWorkerToBuildProc(this);
    }

    void ResetState()
    {
        SetTask(UnitTasks.None);

        if(HasTarget)
            CleanUpTarget();

        MAnimator.SetBool("isBuild" , false);
    }

    void CleanUpTarget()
    {
        if(Target is StructureUnit _struct)
        {
            _struct.UnAssignWorkerToBuildProc();
        }
        SetTarget(null);
    }
}
