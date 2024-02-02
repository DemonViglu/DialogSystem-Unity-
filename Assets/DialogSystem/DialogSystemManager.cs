using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DemonViglu.DialogSystemManager {
    public class DialogSystemManager : MonoBehaviour {
        public static DialogSystemManager instance;
        
        #region Singleton
        /// <summary>
        /// Singleton the DialogSystemManager
        /// </summary>
        private void Awake() {
            if (doSingleton) { 
                if (instance != null) {
                    Destroy(instance);
                    Debug.LogError("DIALOGSYSTEM: Find another DialogSystem!");
                }
                instance = this;
            }
        }
        #endregion
        
        
        #region parameter
        [HorizontalLine("UI_Component")]
        [ForceFill(errorMessage ="")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Text textLabel;

#pragma warning disable 0414
        [SerializeField] private bool hasImage=false;

        //[ShowIf(nameof(hasImage))]
        //[SerializeField] private Sprite npc1;
        //[ShowIf(nameof(hasImage))]
        //[SerializeField] private Sprite npc2;
        [ShowIf(nameof(hasImage))]
        [SerializeField] private Image faceImage;
        [ShowIf(nameof(hasImage))]
        [SerializeField] private DialogIconManager iconManager;
        [ShowIf(nameof(hasImage))]
        [SerializeField] private Text iconRealName;


        [SerializeField] private GameObject ButtonPanel;
        [SerializeField] private GameObject buttonPrefab;

        [SerializeField] private Button AutoPlayButton;

        [HorizontalLine("DialogSentenceDebug")]
        [SerializeField] private bool autoPlaySentence = true;
        [SerializeField] private bool hasClickDownPlayButton = false;

        [HorizontalLine("MissionSO_Source")]
        [ForceFill]
        [SerializeField] private DialogMissionSOManager missionSOManager;
        private bool hasOption;

        [Header("MissionEventHandler")]
        [ForceFill]
        [SerializeField] public DialogMissionEventHandler missionEventHandler;

        [HorizontalLine("Player Setting")]
        ///                                     MISSION
        ///_________________________________________________________________________________________________|                       |___________________________________________________By DemonViglu
        ///        wordTimeGap        sentenceTimeGap                                    sentenceTimeGap             missiontTimeGap
        ///|- - - - - - - - - - -|                       |- - - - - - - - - - -|                            |                       |- - - - - - - - - - -|
        ///                                                                     (close the panel here)----->
        ///

        [SerializeField] private bool doSingleton = true;
        [Tooltip("The time between every char")]
        [SerializeField] private float wordTimeGap=0.2f;
        [Tooltip("The time between every sentence")]
        [SerializeField] private float sentenceTimeGap=2f;
        [Tooltip("The time between every mission")]
        [SerializeField] private float missionTimeGap=3f;
        [Tooltip("The time that set to judge whether the mission was just finished")]
        [SerializeField] private float missionEdgeTime=0.1f;
        [FixedValues(KeyCode.P,KeyCode.B)]
        [SerializeField] private KeyCode keyToPassTheSentence = KeyCode.P;
        [FixedValues(KeyCode.Space, KeyCode.P)]
        [SerializeField] private KeyCode keyToPassToNextSentence = KeyCode.Space;

        [Tooltip("Maybe you have multiple system, so use this to cor with dmm(DialogSystemManagerManager)")]
        [SerializeField] private int dialogsystemID = -1;



        [HorizontalLine("DialogTreeDebug")]
        [SerializeField] private DialogMission currentMission;
        [SerializeField] private List<GameObject> options;
        [SerializeField] List<DialogMission> missionList = new List<DialogMission>();

        [Header("MultiDialogPlayer")]
        [SerializeField] private int chatDialogSystemID = -1;


        private List<string> textList = new List<string>();
        private int sentenceIndex;
        private bool onMission = false;
        [SerializeField] private bool justFinishMission = false;





        #endregion

        #region Two Charge
        //当前是否在输入字符，选择快速码
        private bool isOnCoroutine = false;
        private bool cancelTyping = false;




        private void Start() {
            InitialAutoPlayMode();  
        }

        /// <summary>
        /// Initial the auto playButton and some parameter
        /// </summary>
        private void InitialAutoPlayMode() {
            AutoPlayButton.onClick.AddListener(() => {
                autoPlaySentence = !autoPlaySentence;
                if (autoPlaySentence) {
                    AutoPlayButton.transform.gameObject.GetComponentInChildren<Text>().text = "Auto";
                }
                else {
                    AutoPlayButton.transform.gameObject.GetComponentInChildren<Text>().text = "Not_Auto";
                }
                hasClickDownPlayButton = false;
            });
            autoPlaySentence = true;
        }
        private void TextCharge() {
            if (sentenceIndex >= textList.Count) {
                if (sentenceIndex > textList.Count) {
                    return;
                }

                missionEventHandler.OnMissionEnd(currentMission.dialogMissionID);
                sentenceIndex += 1;
                if (hasOption) {
                    SetUpButton();
                    return;
                }
                missionEventHandler.OnMissionTreeEnd(currentMission.dialogMissionID);

                OnMissionEnd();
                return;
            }

            if (isOnCoroutine) {
                if (Input.GetKeyDown(keyToPassTheSentence)) { cancelTyping = true; }
                return;
            }
            StartCoroutine("SetTextUI");
        }
        private void UICharge() {
            if (!panel.activeSelf) panel.SetActive(true);
        }
        IEnumerator SetTextUI() {
            isOnCoroutine = true;
            textLabel.text = "";
            if(sentenceIndex<textList.Count) {
                //switch (textList[sentenceIndex]) {
                //    case "A":
                //        if (npc1 != null) faceImage.sprite = npc1;
                //        ++sentenceIndex;
                //        break;
                //    case "B":
                //        if (npc2 != null) faceImage.sprite = npc2;
                //        ++sentenceIndex;
                //        break;
                //}
                if (SetIconUI(textList[sentenceIndex])) {
                    ++sentenceIndex;
                }

                for (int i = 0; i < textList[sentenceIndex].Length; i++) {
                    // IF YOU WANT TO PASS THE SENTENCE, PRESS THE KEY THAT
                    // SET IN TEXTCHARGE
                    if (cancelTyping) break;
                    textLabel.text += textList[sentenceIndex][i];
                    yield return new WaitForSeconds(wordTimeGap);
                }
                textLabel.text = textList[sentenceIndex];
                ++sentenceIndex;
            }
            isOnCoroutine = false;
            cancelTyping = false;
        }

        private bool SetIconUI(string name) {
            if(name=="NULL"){
                iconRealName.text = "";
                faceImage.sprite = null;
                return true;
            }
            if (iconManager.iconName.Contains(name)) {
                int iconIndex=iconManager.iconName.IndexOf(name);
                faceImage.sprite = iconManager.icons[iconIndex];
                string realName=name.Split(':')[0];
                iconRealName.text = realName;
                return true;
            }
            return false;

        }
        #endregion
        private void Update() {
            AutoLoadMission();
        }

        #region MissionLogic
        private float _SentenceTimeGap;
        /// <summary>
        /// It's just used around OnMissionEnd, so as to pass the missionTimeGap
        /// </summary>
        private bool missionLock = false;
        [SerializeField] private bool multiMissionlock = false;
        [SerializeField] public bool isMultiMissionChat = false;

        /// <summary>
        /// Auto play the logic every frame
        /// </summary>
        private void AutoLoadMission() {
            //when is typing words, press the key to pass
            if (isOnCoroutine) {
                if (Input.GetKeyDown(keyToPassTheSentence)) { cancelTyping = true; }
                return;
            }
            //if the mission is lock return;
            if (missionLock||multiMissionlock) {
                return;
            }
            if (onMission) {
                if (isOnCoroutine) {
                    return;
                }
                if (_SentenceTimeGap > 0) {
                    //防止该TimeGap过于少
                    if(_SentenceTimeGap>-10f)_SentenceTimeGap -= Time.deltaTime;
                }
                else {
                    //_SentenceTimeGap<0且not isOnCoroutine 且等等情况满足，随时可以调用该句子进入下一阶段,但是有auto这个变量卡住

                    if (Input.GetKeyDown(keyToPassToNextSentence)) {
                        hasClickDownPlayButton = true;
                    }
                    if (!autoPlaySentence) {
                        if (!hasClickDownPlayButton&&sentenceIndex!=textList.Count) {
                            return;
                        }
                        else {
                            hasClickDownPlayButton = false;
                        }
                    }
                    _SentenceTimeGap = sentenceTimeGap;
                    UICharge();
                    TextCharge();
                }
                return;
            }
            else {
                if (missionList.Count > 0) {
                    LoadMissionAtFirst();
                    missionList.RemoveAt(0);
                    onMission = true;
                }
            }
        }

        /// <summary>
        /// Load the Mission at the missionList[0]
        /// </summary>
        private void LoadMissionAtFirst() {

            //每个对话的第一句话前不需要卡顿
            hasClickDownPlayButton = true;

            currentMission = missionList[0];
            if (currentMission.optionMissionIndex != null) {
                hasOption = currentMission.optionMissionIndex.Count > 0;
            }
            else {
                hasOption = false;
            }
            // If the mission record eventIndex, it will invoke the event from the eventList on eventHandler
            bool hasEvent = TryLoadMissionEvent(currentMission.eventIndex);
            if (currentMission.textString != "") {
                var lineData = currentMission.textString.Split('\n');
                foreach (var line in lineData) {
                    textList.Add(line);
                }
            }
            else if (currentMission.textAsset != null) {
                TextAsset file = currentMission.textAsset;
                textList.Clear();
                sentenceIndex = 0;
                var lineData = file.text.Split('\n');
                foreach (var line in lineData) {
                    textList.Add(line);
                }
            }
            else if (hasEvent) {
                return;
            }
            else {
                Debug.LogWarning("DIALOGSYSTEM: There is nothing in the mission!");
                return;
            }
        }

        #region Setting Function to some parameter when the mission is over
        //WARNING, the function below won't be load if the mission has optionMission
        private void OnMissionEnd() {
            //Debug.Log("mission complete");
            panel.SetActive(false);
            sentenceIndex = 0;
            textList.Clear();
            textLabel.text = "";
            _SentenceTimeGap = 0;
            missionLock = true;
            justFinishMission = true;

            //OnEveryMissionEnd();
            currentMission = null;
            Invoke("ResetJustFinishMission", missionEdgeTime);
            Invoke("SetMissionAvalible", missionTimeGap);
        }

        private void ResetJustFinishMission() {
            justFinishMission = false;
        }
        private void SetMissionAvalible() {
            onMission = false;
            missionLock = false;
        }
        #endregion
        /// <summary>
        /// Add mission by C# class
        /// </summary>
        /// <param name="mission"></param>
        public void AddMission(DialogMission mission) {
            missionList.Add(mission);
        }

        #endregion
        /// <summary>
        /// Refresh the mission state or jump to next mission;
        /// </summary>
        public void ClearMissionRightNow() {
            missionLock = true;
            StopCoroutine("SetTextUI");
            isOnCoroutine = false;
            {
                if(!hasOption)panel.SetActive(false);
                sentenceIndex = 0;
                textList.Clear();
                textLabel.text = "";
                _SentenceTimeGap = 0;
            }
            //Debug.Log("ClearMissionSuccess");
            CancelInvoke("SetMissionAvalible");
            //the 0.1 second is a must so as not to load the next mission
            Invoke("SetMissionAvalible", 0.1f);
        }

        /// <summary>
        /// Clean the missionList totally
        /// </summary>
        public void ClearAllMission() {
            //如果本来就没对话，就别搞了
            if (missionList.Count == 0) { return; }
            ClearMissionRightNow();
            missionList.Clear();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////   ByDemonViglu                       Dialog Tree                   ////////////////////////////////////////////////////////////////
        /// <summary>
        /// This function will clean the missionList, if you don't wan't this, use AddMissionSO or AddMissionSOAtFirst instead;
        /// </summary>
        /// <param name="index"> call for the missionSO in the managerList </param>
        public void StartMissionSO(int index) {
            ClearAllMission();
            hasOption = missionSOManager.missionList[index].optionMissionIndex.Count > 0;
            if (index >= missionSOManager.missionList.Count || index < 0) {
                Debug.LogError("DIALOGSYSTEM: Wrong index, it's out of the missionSO range!");
                return;
            }
            currentMission = new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex, index);
            bool hasEvent = TryLoadMissionEvent(currentMission.eventIndex);

            if (currentMission.textString != "" || currentMission.textAsset != null) {
                instance.AddMission(currentMission);
                return;
            }
            else if (hasEvent) {
                return;
            }
            else {
                Debug.LogWarning("DIALOGSYSTEM: There is nothing in the missionSO!");
                return;
            }
        }

        /// <summary>
        /// Add Mission from SOManager to the missionList at last;
        /// </summary>
        /// <param name="index"></param>
        public void AddMissionSO(int index) {
            if (index >= missionSOManager.missionList.Count || index < 0) {
                Debug.LogError("DIALOGSYSTEM: Wrong index, it's out of the missionSO range!");
                return;
            }
            missionList.Add(new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex, index));
        }

        /// <summary>
        /// Add Mission from SOManager to the missionList at first, it will be load at soon
        /// </summary>
        /// <param name="index"></param>
        public void AddMissionSOAtFirst(int index) {
            if (index >= missionSOManager.missionList.Count || index < 0) {
                Debug.LogError("DIALOGSYSTEM: Wrong index, it's out of the missionSO range!");
                return;
            }
            missionList.Insert(0, new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex, index));
        }


        /// <summary>
        /// Open the buttons and refresh the text with optionDescription
        /// </summary>
        private void SetUpButton() {
            if (!hasOption) { return; }
            if(options.Count > 0) { return; }
            for(int i = 0; i < currentMission.optionMissionIndex.Count;i++) {
                GameObject tmpButton = Instantiate(buttonPrefab,ButtonPanel.transform);
                tmpButton.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[i];
                tmpButton.gameObject.SetActive(true);
                tmpButton.GetComponent<DialogButton>().index = i;
                tmpButton. GetComponent<Button>().onClick.AddListener(() => {
                    missionEventHandler.OnOptionClick(currentMission.dialogMissionID,tmpButton.GetComponent<DialogButton>().index);

                    Debug.Log("DIALOGSYSTEM: The event with mission " + currentMission.dialogMissionID + " is called by option NO." + tmpButton.GetComponent<DialogButton>().index + " ,and the description is " + currentMission.optionDescription[tmpButton.GetComponent<DialogButton>().index]);
                    CloseButton();
                    ClearMissionRightNow();
                    AddMissionSOAtFirst(currentMission.optionMissionIndex[tmpButton.GetComponent<DialogButton>().index]) ;
                });
                options.Add(tmpButton);
            }
            ButtonPanel.SetActive(true);
        }

        /// <summary>
        /// Just clost the both buttons;
        /// </summary>
        private void CloseButton() {
            ButtonPanel.SetActive(false);
            foreach(GameObject tmpButton in options) {
                Destroy(tmpButton);
            }
            options.Clear();
        }

        /// <summary>
        /// Try load the missionEvent under the eventHandler with index
        /// </summary>
        /// <param name="index"></param>
        /// <returns> Return whether load the event successfully </returns>
        private bool TryLoadMissionEvent(int index) {
            if (index < 0 || index > missionEventHandler.missionEvents.Count) {
                return false;
            }
            else {
                missionEventHandler.missionEvents[index].optionEvent?.Invoke();
                Debug.Log("DIALOGSYSTEM: Watch out the eventIndex under the SO, the missionList index of:" + index + " is call by the SO that has eventIndex of:" + index);
                return true;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns> The mission state </returns>
        public bool IsOnMission() {
            return onMission;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> whether is typing the word </returns>
        public bool IsOutPutWord() {
            return isOnCoroutine;
        }

        /// <summary>
        /// Only will be true when just on mission end and it will insist for "Mission Time Gap".but the parameter will influence the mission playing logic and it will 
        /// be true for a long time , use "JustFinishPlay" instead;
        /// </summary>
        /// <returns></returns>
        public bool IsOnMissionGap() {
            return missionLock;
        }

        /// <summary>
        /// Only will be true when just on mission end and it will insist for "Mission Edge Time"
        /// </summary>
        /// <returns></returns>
        public bool JustFinishPlay() {
            return justFinishMission;
        }

        public int GetSystemID() {
            return dialogsystemID;
        }

        /// <summary>
        /// Use for DialogMM to prepare for two dialogsystem Chat to each other
        /// </summary>
        /// <returns></returns>
        public bool MultiDialogLock() {
            if ( multiMissionlock||isMultiMissionChat) {
                return false;
            }
            else {
                multiMissionlock = true;
                return true;
            }
        }

        public void MultiDialogUnlock() {
            multiMissionlock = false;
        }

        public void SetChatDialogID(int id) {
            chatDialogSystemID = id;
        }

        public int GetChatDialogID() {
            return chatDialogSystemID;
        }
    }
}