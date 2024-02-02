using DemonViglu.DialogSystemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_NPC_Dialog : MonoBehaviour
{
    [SerializeField] private DialogSystemManager dialogManager;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            dialogManager.AddMissionSO(0);
            Debug.Log("I'm going to talk");
        }
    }
}
