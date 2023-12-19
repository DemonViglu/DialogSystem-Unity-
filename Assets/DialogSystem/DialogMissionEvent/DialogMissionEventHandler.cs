using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMissionEventHandler : MonoBehaviour
{
    public List<DialogMissionEvent> missionEvents;

    public event Action<int> _OnEveryMissionEnd;

    public void OnMissionEnd(int index) {
        _OnEveryMissionEnd(index);
    }
}

