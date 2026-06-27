using UnityEngine;

public abstract class ActionsSO : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Guid = System.Guid.NewGuid().ToString();

    public abstract void Execute(GameManager _manager);
}