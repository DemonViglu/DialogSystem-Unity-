using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]private TextAsset file;

    [SerializeField] [TextArea]private string str;

    public void AddMissionStr() {
        //DialogSystemManager.instance.AddMissionByStr(str);
        DialogSystemManager.instance.AddMission(new Mission(str));
    }

    public void AddMissionFile() {
        DialogSystemManager.instance.AddMission(new Mission(str, file));
    }
}
