using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {
    public class DialogMissionEventHandler : MonoBehaviour {
        public List<DialogMissionEvent> missionEvents;

        public event Action<int> _OnEveryMissionEnd;
        public event Action<int> _OnMissionTreeEnd;
        public event Action<int, int> _OnOptionClick;
        public void OnMissionEnd(int index) {
            _OnEveryMissionEnd?.Invoke(index);
        }

        public void OnMissionTreeEnd(int index) {
            _OnMissionTreeEnd?.Invoke(index);
        }

        public void OnOptionClick(int SOindex,int OptionIndex) { 
            _OnOptionClick?.Invoke(SOindex, OptionIndex);
        }
    }
}