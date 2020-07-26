using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] 
    private TextMeshProUGUI roomName;

    #endregion

    public void onClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        
        RoomOptions roomOptions = new RoomOptions {MaxPlayers = 4};

        if (roomName)
            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed " + message);
    }
}
