using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonViglu.DialogSystemManager {
    [CreateAssetMenu(fileName = "New IconSO", menuName = "DialogMission/New IconSO")]
    public class DialogIconManager : ScriptableObject { 
        public List<string> iconName;

        public List<Sprite> icons;


    }
}

