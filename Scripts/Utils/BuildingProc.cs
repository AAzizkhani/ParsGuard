using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingProc
{
    private BuildActionsSO BuildAction;
    private WorkerUnit Worker;
    private StructureUnit Structure;
    private ParticleSystem ConstructionEffect;
    private float ProgressTimer;
    private bool IsFinished;
    private bool InProgress => HasActiveWorker && Worker.CurrentState == UnitStates.Building;
    public bool HasActiveWorker => Worker != null;
    
    public BuildingProc(BuildActionsSO _buildAction, Vector3 _placementPos, WorkerUnit _worker ,ParticleSystem _cnstrctnEffct)
    {
        var _effctOffst = new Vector3(0 , -1f , 0);

        BuildAction = _buildAction;
        Worker = _worker;

        ConstructionEffect =Object.Instantiate(_cnstrctnEffct , _placementPos+_effctOffst , Quaternion.identity);

        Structure = Object.Instantiate(_buildAction.StructPrefab);
        Structure.SprRender.sprite = BuildAction.FoundationSprite;
        Structure.transform.position = _placementPos; 
        Structure.RegisterPrc(this);

        _worker.SendToBuild(Structure);
    }

    public void UpdateProc()
    {
        if (IsFinished) return;

        if (InProgress)
        {
            ProgressTimer += Time.deltaTime;

            if (!ConstructionEffect.isPlaying)
                ConstructionEffect.Play();
            
            if (ProgressTimer >= BuildAction.ConstructionTime)
            {
                CompleteBuilding();
            }
        }
        else
        {
            if (ConstructionEffect.isPlaying)
                ConstructionEffect.Stop();
        }
    }

    private void CompleteBuilding()
    {
        IsFinished = true;

        Structure.SprRender.sprite = BuildAction.CompletionSprite;
        
        Structure.SprRender.color = Color.white; 

        if (ConstructionEffect != null)
        {
            ConstructionEffect.Stop();
            Object.Destroy(ConstructionEffect.gameObject, 1.5f);
        }

        Worker.OnBuildingFinished();
        Structure.OnConstructionFinished();
    }

    public void AddWorker(WorkerUnit _worker)
    {
        if(HasActiveWorker)
            return;
        Worker = _worker;
    }

    public void RemoveWorker()
    {
        if(!HasActiveWorker)
            return;
        Worker = null;
        ConstructionEffect.Stop();
    }
}