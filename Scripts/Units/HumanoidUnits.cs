using UnityEngine;

public class HumanoidUnits : Units
{
    protected Vector2 MVelocity;
    protected Vector3 MLastPosition;

    [SerializeField] private float CurrentSpeed => MVelocity.magnitude;

    void Start()
    {
        MLastPosition = transform.position;
    }
    void Update()
    {
        UpdateVelocity();
        UpdateBehaviour();
    }

    protected virtual void UpdateBehaviour()
    {
        
    }
    protected virtual void UpdateVelocity()
    {
        MVelocity = new Vector2(
            (transform.position.x - MLastPosition.x),
            (transform.position.y - MLastPosition.y)
        ) / Time.deltaTime;

        MLastPosition = transform.position;
        var _state = MVelocity.magnitude > 0 ? UnitStates.Moving : UnitStates.Idle;
        SetState(_state);
        
        MAnimator?.SetFloat("Speed" , Mathf.Clamp01(CurrentSpeed));
    }

}
 