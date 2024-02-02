using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DemonViglu.DialogSystemManager;
using CustomInspector;

public class TestScript : MonoBehaviour
{
    [SerializeField]private TextAsset file;

    [SerializeField] [TextArea]private string str;

    [SerializeField] private UnityEvent eve;

    public void AddMissionStr() {
        //DialogSystemManager.instance.AddMissionByStr(str);
        DialogSystemManager.instance.AddMission(new DialogMission(str,null,null,null,-1,0));
    }

    public void AddMissionFile() {
        DialogSystemManager.instance.AddMission(new DialogMission(str, file));
    }


    public void Test() {
        Debug.Log("...");
    }
    [ContextMenu("Test")]
    public void What() {
        DialogSystemManager.instance.missionEventHandler._OnEveryMissionEnd += DoOnMission;
    }

    public void DoOnMission(int a) {
        Debug.Log("Now is playing the SO which index is :" + a + " in the SOManager. If there is no dialogsentense, means the options finish the dialog!");
    }

    [Tooltip("Only use for multiDialogSystem")]
    [Button(nameof(OneToTwo))]
    public int a;
    void OneToTwo() {
        DialogMM.instance.AddRequest(new MultiDialogRequest(0,1,0));
    }

}
