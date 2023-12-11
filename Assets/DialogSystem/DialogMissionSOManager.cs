using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New DialogMissionSO", menuName = "DialogMission/New DialogMissionSOManager")]
public class DialogMissionSOManager :ScriptableObject
{
    public List<DialogMissionSO> missionList = new List<DialogMissionSO>();
}
