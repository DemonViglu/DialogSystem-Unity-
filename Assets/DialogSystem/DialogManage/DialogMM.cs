using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {

    public class DialogMM : MonoBehaviour {

        public static DialogMM instance;

        #region Singleton
        /// <summary>
        /// Singleton the DialogSystemManager
        /// </summary>
        private void Awake() {
            if (doSingleton) {
                if (instance != null) {
                    Destroy(instance);
                    Debug.LogError("Find another DMM!");
                }
                instance = this;
            }
        }
        #endregion
        public List<DialogSystemManager> dialogSystems;
        public bool doSingleton = true;
        public void CallForOtherDialogSystemSO(int  dialogSystemID, int SO) {
            foreach(DialogSystemManager dialogSystem in dialogSystems) {
                if(dialogSystem.GetSystemID()==dialogSystemID) {
                    dialogSystem.AddMissionSO(SO);
                    return;
                }
            }
            Debug.LogError("Dialog with ID " + dialogSystemID + " is not Found!");

        }
    }
}


