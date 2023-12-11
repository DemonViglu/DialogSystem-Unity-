using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "New MissionSO", menuName = "Mission/New MissionSO")]
public class MissionSO:ScriptableObject
{
    [TextArea]
    public string textString;

    public TextAsset textAsset;

    public List<int> optionMissionIndex;

    public List<string> optionDescription;

    public int eventIndex=-1;

}
