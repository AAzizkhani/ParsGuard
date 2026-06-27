
using UnityEngine;

public abstract class SinglatonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual void Awake()
    {
        T[] managers = FindObjectsOfType<T>(includeInactive:true);
        if(managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static T Get()
    {
        var tag = typeof(T).Name;
        GameObject managerObj = GameObject.FindWithTag(tag);
        if(managerObj != null)
        {
            return managerObj.GetComponent<T>();
        }

        GameObject go = new(tag);
        go.tag = tag;
        return go.AddComponent<T>();
    }
}
