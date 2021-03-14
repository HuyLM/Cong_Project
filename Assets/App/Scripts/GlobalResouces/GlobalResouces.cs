using AtoLib;
using UnityEngine;

public class GlobalResouces : SingletonBind<GlobalResouces>
{
    [SerializeField] private UIConfigResource uiConfigResource;
    

    public UIConfigResource UIConfigResource { get { return uiConfigResource; } }
}
