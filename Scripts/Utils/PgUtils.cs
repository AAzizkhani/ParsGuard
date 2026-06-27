
using UnityEngine;
using UnityEngine.EventSystems;


public static class PgUtils
{
    private static Vector2 InputPosition => Input.touchCount > 0 ? 
                                            Input.GetTouch(0).position : Input.mousePosition;
    private static bool ClickBegan => Input.GetMouseButtonDown(0) || 
                                     (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    private static bool ClickEnd => Input.GetMouseButtonUp(0) || 
                                   (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    private static Vector2 MInitialTouchPos;
    public static bool GetClickPosition(out Vector2 _inputPos , float _maxDis = 10f)
    {
        _inputPos = InputPosition;
            if(ClickBegan) 
            {
                MInitialTouchPos = _inputPos;
            }

            if(ClickEnd) 
            {   
                if(Vector2.Distance(MInitialTouchPos , _inputPos) < _maxDis)
                { 
                    return true;
                }
            }
        return false;
    }

    public static bool IsPointerOverUIElement()
    {
        if(Input.touchCount > 0)
        {
            var _touch = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(_touch.fingerId);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();   
        }
    }


    public static bool GetHoldPosition(out Vector3 _inputpos)
    {
       if(Input.touchCount > 0)
       {
            _inputpos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            return true;
       } 
       else if(Input.GetMouseButton(0))
       {
            _inputpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return true;
       }

       _inputpos = Vector3.zero;
       return false;

    }
}
