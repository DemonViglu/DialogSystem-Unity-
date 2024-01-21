using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {
    [System.Serializable]
    public class DialogMission {
        public string textString;

        public TextAsset textAsset;
        public List<int> optionMissionIndex;

        public List<string> optionDescription;

        public int eventIndex;
        //Who call for the dialogSystem;
        public List<int> nextPlayerID;

        //if the dialogMission is come from SO, the missionID will be the index in the list by defautlt
        public int dialogMissionID;
        public DialogMission(string textString = "", TextAsset textAsset = null) {
            this.textString = textString;
            this.textAsset = textAsset;
            this.eventIndex = -1;
            this.dialogMissionID = -1;
        }

        //In My system, dialogMissionID is the index in SOManager as default;
        public DialogMission(string textString = "", TextAsset textAsset = null, List<int> optionIndex = null, List<string> optionString = null, int eventIndex = -1, int dialogMissionID = -1) {
            this.textString = textString;
            this.textAsset = textAsset;
            this.optionMissionIndex = optionIndex;
            this.optionDescription = optionString;
            this.eventIndex = eventIndex;
            this.dialogMissionID = dialogMissionID;
        }

    }
}