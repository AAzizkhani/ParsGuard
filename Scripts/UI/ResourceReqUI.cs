using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceReqUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TxtGold;
    [SerializeField] private TextMeshProUGUI TxtWood;

    public void Show(int _reqGld ,int _reqWd)
    {
        TxtGold.text = _reqGld.ToString();
        TxtWood.text = _reqWd.ToString();
        UpdateColorReq(_reqGld , _reqWd);
    }

    void UpdateColorReq(int _reqGld ,int _reqWd)
    {
        var _mngr = GameManager.Get();
        var _grnColor = new Color(0 , 0.6f , 0.1f);
        TxtGold.color = _mngr.Gold >= _reqGld ? _grnColor : Color.red;
        TxtWood.color = _mngr.Wood >=  _reqWd ? _grnColor : Color.red;

    }

}
