
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private Image MActionBarImage;
    [SerializeField] private ActionButton MActionBtnPrefab;

    private Color CurrentColor;
    private List<ActionButton> MActionBtnList = new();

    void Awake()
    {
        CurrentColor = MActionBarImage.color;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RejisterActions(Sprite _icon, UnityAction _action)
    {
        var _actBtn = Instantiate(MActionBtnPrefab , transform);
        _actBtn.Init(_icon , _action); 
        MActionBtnList.Add(_actBtn);
    }

    public void ClearActions()
    {
        for(int i = MActionBtnList.Count - 1; i>=0; --i)
        {
            Destroy(MActionBtnList[i].gameObject);
            MActionBtnList.RemoveAt(i);
        }
    }
    public void Hide()
    {
        MActionBarImage.color = new Color(0,0,0,0);
    }

    public void Show()
    {
        MActionBarImage.color = CurrentColor;
    }
}
