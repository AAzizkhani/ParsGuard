using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using Unity.VisualScripting;
using UnityEngine;

public enum UnitStates{Idle,Moving,Attacking,Chopping,Mining,Building}
public enum UnitTasks{None,Build,Attack,Chop,Mine}

public class Units : MonoBehaviour
{
    [SerializeField] private ActionsSO[] MActions;
    [SerializeField] protected float MObjDetectRadius = 3f;
    private SpriteGlowEffect MSprGlow;

    protected Animator MAnimator;
    protected AIPawn MAIPawn;
    protected SpriteRenderer MSprRender;
    protected Material MDefaultMaterial;

    public ActionsSO[] Actions => MActions; 
    public SpriteRenderer SprRender => MSprRender;
    public bool HasTarget => Target != null;

    public UnitStates CurrentState{get; protected set;} = UnitStates.Idle;
    public UnitTasks CurrentTask{get; protected set;} = UnitTasks.None;
    public Units Target {get; protected set;}


    protected void Awake()
    {
        if(TryGetComponent<SpriteGlowEffect>(out var _sprglow))
        {
             MSprGlow = _sprglow;   
        }

        if(TryGetComponent<Animator>(out var _animator))
        {
             MAnimator = _animator;   
        }

        if(TryGetComponent<AIPawn>(out var _aipawn))
        {
             MAIPawn = _aipawn;   
        }

        MSprRender = GetComponent<SpriteRenderer>();
        MDefaultMaterial = Resources.Load<Material>("Materials/SpriteOutline");
        MSprRender.material = MDefaultMaterial;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetSprGlowOn(bool _on)
    {
        MSprGlow.enabled = _on;
    }

    public void SetTarget(Units _tar)
    {
        Target = _tar;
    }

    public void MoveTo(Vector3 _dest)
    {
        var _dir = (_dest - transform.position).normalized;
        MSprRender.flipX = _dir.x < 0;   
        MAIPawn.SetDestination(_dest);
        OnSetDest();
    }

    public void SetTask(UnitTasks _tsk)
    {
        OnSetTask(CurrentTask , _tsk);
    }
    public void SetState(UnitStates _stt)
    {
        OnSetState(CurrentState , _stt);
    }


    protected virtual void OnSetDest()
    {
        
    }
    protected virtual void OnSetTask(UnitTasks _oldTsk , UnitTasks _newTsk)
    {
        CurrentTask = _newTsk;
    }
    protected virtual void OnSetState(UnitStates _oldStt , UnitStates _newStt)
    {
        CurrentState = _newStt;
    }

    protected Collider2D[] RumProxObjDetect()
    {
        return Physics2D.OverlapCircleAll(transform.position , MObjDetectRadius);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1 , 0 , 0 , 0.3f);
        Gizmos.DrawSphere(transform.position , MObjDetectRadius);
    }

}
