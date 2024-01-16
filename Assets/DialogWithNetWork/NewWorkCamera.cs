using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;
public class NewWorkCamera : NetworkBehaviour
{
    public GameObject playerCamera;

    private void Start() {
        if (IsOwner) {
            playerCamera.SetActive(true);
        }
    }
}
