using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystemManager : MonoBehaviour {
    public static DialogSystemManager instance;

    #region Singleton
    /// <summary>
    /// Singleton the DialogSystemManager
    /// </summary>
    private void Awake() {
        if (instance != null) {
            Destroy(instance);
            Debug.LogError("Find another DialogSystem!");
        }
        instance = this;
    }
    #endregion

    #region parameter
    [Header("UI_Component")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Text textLabel;

    [Header("Image")]
    [SerializeField] private Sprite npc1;
    [SerializeField] private Sprite npc2;
    [SerializeField] private Image faceImage;

    [Header("Player Setting")]
    ///                                     MISSION
    ///_________________________________________________________________________________________________|                       |___________________________________________________By DemonViglu
    ///        wordTimeGap        sentenceTimeGap                                    sentenceTimeGap             missiontTimeGap
    ///|- - - - - - - - - - -|                       |- - - - - - - - - - -|                            |                       |- - - - - - - - - - -|
    ///                                                                     (close the panel here)----->
    ///
    [Tooltip("The time between every char")]
    [SerializeField] private float wordTimeGap;
    [Tooltip("The time between every sentence")]
    [SerializeField] private float sentenceTimeGap;
    [Tooltip("The time between every mission")]
    [SerializeField] private float missionTimeGap;
    [Tooltip("The time that set to judge whether the mission was just finished")]
    [SerializeField] private float missionEdgeTime;
    [SerializeField] private KeyCode keyToPassTheSentence = KeyCode.P;

    [SerializeField]
    List<DialogMission> missionList = new List<DialogMission>();
    private List<string> textList = new List<string>();
    private int sentenceIndex;
    private bool onMission = false;
    [SerializeField] private bool justFinishMission = false;

    [Header("MissionSO")]
    [SerializeField] private DialogMissionSOManager missionSOManager;
    private bool hasOption;

    [Header("MissionEventHandler")]
    [SerializeField] private DialogMissionEventHandler missionEventHandler;
    #endregion

    #region Two Charge
    //当前是否在输入字符，选择快速码
    private bool isOnCoroutine = false;
    private bool cancelTyping = false;
    private void TextCharge() {
        if (sentenceIndex == textList.Count) {
            SetUpButton();
            if (hasOption) return;

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
        switch (textList[sentenceIndex]) {
            case "A":
                if (npc1 != null) faceImage.sprite = npc1;
                ++sentenceIndex;
                break;
            case "B":
                if (npc2 != null) faceImage.sprite = npc2;
                ++sentenceIndex;
                break;
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
        isOnCoroutine = false;
        cancelTyping = false;
    }
    #endregion
    private void Update() {
        AutoLoadMission();
    }

    #region MissionLogic
    private float _SentenceTimeGap;
    private bool missionLock = false;

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
        if (missionLock) {
            return;
        }
        if (onMission) {
            if (isOnCoroutine) {
                return;
            }
            if (_SentenceTimeGap > 0) {
                _SentenceTimeGap -= Time.deltaTime;
            }
            else {
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
            Debug.LogError("There is nothing in the mission!");
            return;
        }
    }

    #region Setting Function to some parameter when the mission is over
    //WARNING, the function below won't be load if the mission has optionMission
    private void OnMissionEnd() {
        Debug.Log("mission complete");
        panel.SetActive(false);
        sentenceIndex = 0;
        textList.Clear();
        textLabel.text = "";
        _SentenceTimeGap = 0;
        missionLock = true;
        justFinishMission = true;

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
            panel.SetActive(false);
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
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        currentMission = new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex);
        bool hasEvent = TryLoadMissionEvent(currentMission.eventIndex);

        if (currentMission.textString != "" || currentMission.textAsset != null) {
            instance.AddMission(currentMission);
            return;
        }
        else if (hasEvent) {
            return;
        }
        else {
            Debug.LogError("There is nothing in the missionSO!");
            return;
        }
    }

    /// <summary>
    /// Add Mission from SOManager to the missionList at last;
    /// </summary>
    /// <param name="index"></param>
    public void AddMissionSO(int index) {
        if (index >= missionSOManager.missionList.Count || index < 0) {
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        missionList.Add(new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex));
    }

    /// <summary>
    /// Add Mission from SOManager to the missionList at first, it will be load at soon
    /// </summary>
    /// <param name="index"></param>
    public void AddMissionSOAtFirst(int index) {
        if (index >= missionSOManager.missionList.Count || index < 0) {
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        missionList.Insert(0, new DialogMission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex));
    }

    [Header("DialogTree")]
    private DialogMission currentMission;
    [SerializeField] private Button Option_1;
    [SerializeField] private Button Option_2;

    /// <summary>
    /// Open the buttons and refresh the text with optionDescription
    /// </summary>
    private void SetUpButton() {
        if (!hasOption) { return; }
        if (currentMission.optionMissionIndex.Count == 1) {
            Option_1.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[0];
            Option_1.gameObject.SetActive(true);
        }
        else if (currentMission.optionMissionIndex.Count == 2) {
            Option_1.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[0];
            Option_2.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[1];
            Option_1.gameObject.SetActive(true);
            Option_2.gameObject.SetActive(true);
        }
        return;
    }

    /// <summary>
    /// Just clost the both buttons;
    /// </summary>
    private void CloseButton() {
        Option_1.gameObject.SetActive(false);
        Option_2.gameObject.SetActive(false);
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
            return true;
        }
    }

    /// <summary>
    /// Should be play by the button1
    /// </summary>
    public void PlayMissionOption_1() {
        CloseButton();
        ClearMissionRightNow();
        //StartMissionSO(currentMission.optionMissionIndex[0]);
        AddMissionSOAtFirst(currentMission.optionMissionIndex[0]);
    }

    /// <summary>
    /// Should be play by the button2
    /// </summary>
    public void PlayMissionOption_2() {
        CloseButton();
        ClearMissionRightNow();
        //StartMissionSO(currentMission.optionMissionIndex[1]);
        AddMissionSOAtFirst(currentMission.optionMissionIndex[1]);
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
}