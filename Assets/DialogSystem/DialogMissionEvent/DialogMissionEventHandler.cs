using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {
    public class DialogMissionEventHandler : MonoBehaviour {
        public List<DialogMissionEvent> missionEvents;

        public event Action<int> _OnEveryMissionEnd;
        public event Action<int> _OnMissionTreeEnd;
        public void OnMissionEnd(int index) {
            _OnEveryMissionEnd?.Invoke(index);
        }

        public void OnMissionTreeEnd(int index) {
            _OnMissionTreeEnd?.Invoke(index);
        }
    }
}