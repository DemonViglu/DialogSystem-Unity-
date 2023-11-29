using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    public string textString;

    public TextAsset textAsset;
    public List<int> optionMissionIndex;

    public List<string> optionDescription;

    public Mission(string textString="", TextAsset textAsset=null) {
        this.textString = textString;
        this.textAsset = textAsset;
    }
    public Mission(string textString = "", TextAsset textAsset = null,List<int>optionIndex=null,List<string>optionString=null) {
        this.textString = textString;
        this.textAsset = textAsset;
        this.optionMissionIndex = optionIndex;
        this.optionDescription = optionString;
    }
}
