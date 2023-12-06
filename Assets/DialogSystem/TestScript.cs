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
        DialogSystemManager.instance.AddMission(new Mission(str,null));
    }

    public void AddMissionFile() {
        DialogSystemManager.instance.AddMission(new Mission(str, file));
    }


    public void Test() {
        Debug.Log("...");
    }
}
