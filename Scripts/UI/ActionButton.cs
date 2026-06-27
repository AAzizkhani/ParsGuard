using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private Button ActionBtn;
    [SerializeField] private Image IconImage;

    void OnActDestroy()
    {
        ActionBtn.onClick.RemoveAllListeners();    
    }

    public void Init(Sprite _icon , UnityAction _action)
    {
        IconImage.sprite = _icon;
        ActionBtn.onClick.AddListener(_action);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
