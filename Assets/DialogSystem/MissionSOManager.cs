using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New MissionSO", menuName = "Mission/New MissionSOManager")]
public class MissionSOManager :ScriptableObject
{
    public List<MissionSO> missionList = new List<MissionSO>();
}
