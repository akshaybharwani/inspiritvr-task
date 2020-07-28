using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomEntryController : MonoBehaviour
{
    #region Private Variables

    [Header("UI References")] 
    [SerializeField]
    private TextMeshProUGUI roomNameText;
    
    [SerializeField]
    private TextMeshProUGUI playersInfoText;

    private string _roomName;
    
    #endregion

    public void SetRoomEntryFields(string roomName, int currentPlayers, int maxPlayers)
    {
        // Set RoomName
        _roomName = roomName;
        roomNameText.text = roomName;
        
        // Set Players Info
        playersInfoText.text = currentPlayers + "/" + maxPlayers;

    }

    public void JoinThisRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
    }
}
