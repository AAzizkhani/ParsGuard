using UnityEngine;

public class AIPawn : MonoBehaviour
{
    [SerializeField]private float MSpeed = 5f;
    private Vector3? MDestination;
    public Vector3? Destination => MDestination;
    void Start()
    {
    }

    void Update()
    { 
        if(MDestination.HasValue)
        {
            var dir = MDestination.Value - transform.position;
            transform.position += dir.normalized  * Time.deltaTime * MSpeed;

            var distanceToDestination = Vector3.Distance(transform.position , MDestination.Value);
            if(distanceToDestination < 0.1f)
            {
                MDestination = null;
            }
        }
    }
    public void SetDestination(Vector3 _dest)
    {
        MDestination = _dest;
    }

}
