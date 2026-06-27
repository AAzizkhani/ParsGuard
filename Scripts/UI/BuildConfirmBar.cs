using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildConfirmBar : MonoBehaviour
{
    [SerializeField] private ResourceReqUI ResourceUI;
    [SerializeField] private Button BtnConfirm;
    [SerializeField] private Button BtnCancel;

    public void Show(int _gold , int _wood)
    { 
        gameObject.SetActive(true);
        ResourceUI.Show(_gold , _wood);
    }
    public void Hide(){gameObject.SetActive(false);}

    public void SetupHooks(UnityAction _onConfirm , UnityAction _onCancel)
    {
        BtnCancel.onClick.AddListener(_onCancel);
        BtnConfirm.onClick.AddListener(_onConfirm);
    }
    void OnDisable()
    {
        BtnCancel.onClick.RemoveAllListeners();
        BtnConfirm.onClick.RemoveAllListeners();
    }

}
