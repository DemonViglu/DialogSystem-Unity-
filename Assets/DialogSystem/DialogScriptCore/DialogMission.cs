using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {
    [System.Serializable]
    public class DialogMission {
        [Tooltip("对话的注释，便于寻找，可不写")]
        public string dialogTitle;

        public int dialogMissionID;
        [TextArea]
        public string textString;
        public TextAsset textAsset;
        public List<int> optionMissionIndex;
        public List<string> optionDescription;

        public int eventIndex;
        //Who call for the dialogSystem;
        public List<int> nextPlayerID;


        public DialogMission(string textString = "", TextAsset textAsset = null) {
            this.textString = textString;
            this.textAsset = textAsset;
            this.eventIndex = -1;
            this.dialogMissionID = -1;
        }

        //In My system, dialogMissionID is the index in SOManager as default;
        public DialogMission(string textString = "", TextAsset textAsset = null, List<int> optionIndex = null, List<string> optionString = null, int eventIndex = -1, int dialogMissionID = -1, string dialogTitle = null) {
            this.textString = textString;
            this.textAsset = textAsset;
            this.optionMissionIndex = optionIndex;
            this.optionDescription = optionString;
            this.eventIndex = eventIndex;
            this.dialogMissionID = dialogMissionID;
            this.dialogTitle = dialogTitle;
        }

    }
}