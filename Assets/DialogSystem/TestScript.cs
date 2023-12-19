using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        DialogSystemManager.instance.missionEventHandler._OnEveryMissionEnd += DoOnMission_1End;
    }

    public void DoOnMission_1End(int a) {
        if(a!= 0) {
            return;
        }
        Debug.Log("On mission_1");
    }
}
