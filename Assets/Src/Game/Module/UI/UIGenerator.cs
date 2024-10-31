using UnityEngine;


public class UIGenerator : MonoBehaviour
{
    public UILayer layer = UILayer.Panel;
    public int orderInLayer;
    public UIHideType hideType = UIHideType.WaitDestroy;
    public UIHideFunc hideFunc = UIHideFunc.Deactive;
    public UIEscClose escClose = UIEscClose.DontClose;
    public bool loadSync = false;
    
    //[HideInInspector]
    public Transform[] controls;
    public int controlCount => controls.Length;

    public Transform GetControl(int index)
    {
        return controls[index];
    }
}