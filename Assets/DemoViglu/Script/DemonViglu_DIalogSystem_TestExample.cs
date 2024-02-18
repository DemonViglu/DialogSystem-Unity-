using CustomInspector;
using DemonViglu.DialogSystemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonViglu_DIalogSystem_TestExample : MonoBehaviour {


    [SerializeField] private DialogSystemManager m_dialogSystemManager;

    [Button(nameof(AddMission))]
    public int dialogID;
    public void AddMission() {
        m_dialogSystemManager.AddMissionAtLast(dialogID);
    }
    private void Start() {
        m_dialogSystemManager.missionEventHandler._OnEveryMissionEnd += OnMissionSOEnd;
        m_dialogSystemManager.missionEventHandler._OnMissionTreeEnd += OnMissionTreeEnd;
        m_dialogSystemManager.missionEventHandler._OnOptionClick += ClickOption;
    }

    private void OnMissionSOEnd(int index) {
        Debug.Log("DialogSO is finish which is index :" + index +" in the SOManager");
    }

    private void OnMissionTreeEnd(int index) {
        Debug.Log("DialogSO Tree is finish which is index :" + index + " in the SOManager");
    }

    public void UseObjectToCallBack() {
        Debug.Log("I'm Trigger by the MissionObject in the Hierachy, look at the eventHandler for help");
    }

    public void ClickOption(int SOindex,int optionIndex) {
        Debug.Log(SOindex + "->" + optionIndex + " 's event is called");

        //if(SOindex==2&&optionIndex==0) {
        //    if (MissionService.instance.FindMission(1).missionState == MissionState.onGoing) {
        //        MissionService.instance.AddProgress(1);
        //    }

        //}

        //if(SOindex==1&&optionIndex==0) {

        //    if (MissionService.instance.FindMission(2).missionState == MissionState.onGoing) {
        //        MissionService.instance.AddProgress(2);
        //    }
        //    if (MissionService.instance.FindMission(4).missionState == MissionState.onGoing) {
        //        MissionService.instance.AddProgress(4);
        //    }
        //}

        //if(SOindex==1||SOindex==2) {
        //    MissionService.instance.AddMissionNum(3, 1);
        //}
    }
}
