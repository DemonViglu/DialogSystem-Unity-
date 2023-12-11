using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "New DialogMissionSO", menuName = "DialogMission/New DialogMissionSO")]
public class DialogMissionSO:ScriptableObject
{
    [TextArea]
    public string textString;

    public TextAsset textAsset;

    public List<int> optionMissionIndex;

    public List<string> optionDescription;

    public int eventIndex=-1;

}
