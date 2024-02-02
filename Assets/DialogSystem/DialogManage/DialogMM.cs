using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DemonViglu.DialogSystemManager {

    public class DialogMM :MonoBehaviour{

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

            dialogSystems = FindAllDialogObjects();
        }
        #endregion

        private void Update() {
            AutoDealRequest();
        }

        public void FindAllDialog() {
            dialogSystems = FindAllDialogObjects();
        }
        
        public List<DialogSystemManager> dialogSystems;
        public bool doSingleton = true;

        public List<MultiDialogRequest> multiDialogRequests;

        private MultiDialogRequest currentRequest = null;
        [SerializeField] private DialogSystemManager d1;
        [SerializeField] private DialogSystemManager d2;

        private List<DialogSystemManager> FindAllDialogObjects() {

            IEnumerable<DialogSystemManager> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                  .OfType<DialogSystemManager>();
            return new List<DialogSystemManager>(dataPersistenceObjects);
        }

        public DialogSystemManager FindDialogSystem(int dialogsystemID) {
            foreach (DialogSystemManager dialogSystem in dialogSystems) {
                if (dialogSystem.GetSystemID() == dialogsystemID) {
                    return dialogSystem;
                }
            }
            return null;
        }

        public void CallForOtherDialogSystemSO(int requestId, int requestedId, int SO) {
            d1 = FindDialogSystem(requestId);
            d2 = FindDialogSystem(requestedId);
            if (d1.GetChatDialogID() == requestedId && d2.GetChatDialogID() == requestId) {
                d1.MultiDialogUnlock();
                d2.MultiDialogUnlock();
                d1.isMultiMissionChat = true;
                d2.isMultiMissionChat = true;
                if(SO!=-1)d2.AddMissionSO(SO);
            }
            else {
                Debug.Log("Bad Request!");
            }
        }

        private void AutoDealRequest() {
            // If there are request in the list, deal
            if (multiDialogRequests.Count > 0) {
                if (currentRequest == null) {
                    currentRequest = multiDialogRequests[0];
                    d1 = FindDialogSystem(currentRequest.requestID);
                    d2 = FindDialogSystem(currentRequest.requestedID);
                }

                switch (currentRequest.state) {
                    case MultiDialogState.Asking:
                        d1.SetChatDialogID(currentRequest.requestedID);
                        if (d1.MultiDialogLock() && d2.MultiDialogLock()) {
                            d2.SetChatDialogID(currentRequest.requestID);
                            currentRequest.state = MultiDialogState.Comfirm;
                        }
                        else {
                            currentRequest.state = MultiDialogState.Finished;
                        }
                        break;
                    case MultiDialogState.Comfirm:
                        multiDialogRequests.RemoveAt(0);
                        CallForOtherDialogSystemSO(currentRequest.requestID, currentRequest.requestedID, currentRequest.SO);
                        currentRequest = null;
                        d2 = null;
                        d1 = null;

                        break;
                    case MultiDialogState.Finished:
                        //对话请求结束，释放掉
                        Debug.Log("请求失败");
                        if (!d1.isMultiMissionChat) {
                            d1.SetChatDialogID(-1);
                            d1.MultiDialogUnlock();
                            d1= null; 
                        }
                        if (d2.isMultiMissionChat) {
                            d2.MultiDialogUnlock();
                            d2.SetChatDialogID(-1);
                            d2=null;
                        }
                        multiDialogRequests.RemoveAt(0);
                        currentRequest = null;
                        break;
                }
            }
        }
        /// <summary>
        /// If you want to make connection, add request at first
        /// </summary>
        /// <param name="multi"></param>
        public void AddRequest(MultiDialogRequest multi) {
            multiDialogRequests.Add(multi);
        }
        public void MultiChatCancel(int requestId, int requestedId) {
            DialogSystemManager d1 = FindDialogSystem(requestId);
            DialogSystemManager d2 = FindDialogSystem(requestedId);
            ///只有当事人才能终止对话
            if(d1.GetChatDialogID() == requestedId && d2.GetChatDialogID() == requestId) {
                d1.SetChatDialogID(-1);
                d1.MultiDialogUnlock();
                d1.isMultiMissionChat = false;
                d2.SetChatDialogID(-1);
                d2.MultiDialogUnlock();
                d2.isMultiMissionChat = false;
            }
        }
    }

    [Serializable]
    public class MultiDialogRequest {

        public MultiDialogRequest(int _requestID, int _requestedID, int sO) {
            requestedID = _requestedID;
            requestID = _requestID;
            state = MultiDialogState.Asking;
            SO = sO;
        }
        public int requestID;
        public int requestedID;
        public MultiDialogState state;

        public int SO;
    }

    public enum MultiDialogState {
        Asking, Comfirm, Finished
    }


    ////////////////////NETWORK
}