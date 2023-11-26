using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    public string textString;

    public TextAsset textAsset;

    public Mission(string textString="", TextAsset textAsset=null) {
        this.textString = textString;
        this.textAsset = textAsset;
    }
}
