using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using DemonViglu.DialogSystemManager;
public class PlayerNetwork :NetworkBehaviour
{
    private NetworkVariable<int>randomNumber= new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    //public override void OnNetworkSpawn()
    //{

    //    //TestClientRpc(new ClientRpcParams {Send = new ClientRpcSendParams { TargetClientIds=new List<ulong> {1} } });
    //    /*
    //    randomNumber.OnValueChanged += (int previousValue, int newValue) =>
    //    {
    //        Debug.Log(OwnerClientId + "; randomNumber:" + randomNumber.Value);
    //    };
    //    */
    //}

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        DialogMM.instance.FindAllDialog();
    }
    private void Update()
    {

        if (!IsOwner) return;


        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0,100);
            //TestServerRpc(new ServerRpcParams());
            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });

        }




        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        if (IsHost)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            TestClientRpc(moveDir * moveSpeed * Time.deltaTime, new ClientRpcParams());
        }
        else
        {
            TestServerRpc( moveDir * moveSpeed * Time.deltaTime, new ServerRpcParams());
        }
    }

    [ServerRpc]
    private void TestServerRpc(Vector3 deltaPosition,ServerRpcParams serverRpcParams)
    {
        Debug.Log("Test Server RPC "+OwnerClientId+"; "+serverRpcParams.Receive.SenderClientId);

        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
            //client.PlayerObject.transform.position += deltaPosition;
        }
        this.transform.position += deltaPosition;
        TestClientRpc(deltaPosition, new ClientRpcParams());
    }

    [ClientRpc]
    private void TestClientRpc(Vector3 deltaPosition,ClientRpcParams clientRpcParams)
    {
        this.transform.position += deltaPosition;
        Debug.Log("Test Client RPC ");
    }
}
