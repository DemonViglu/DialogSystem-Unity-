using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New MissionSO", menuName = "Mission/New MissionSO")]
public class MissionSO:ScriptableObject
{
    public string textString;

    public TextAsset textAsset;

    public List<int> optionMissionIndex;

    public List<string> optionDescription;
}
