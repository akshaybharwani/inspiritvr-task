using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{
    #region Private Variables

    private UIManager _UIManager;

    #endregion

    private void Start()
    {
        _UIManager = FindObjectOfType<UIManager>();
    }

    public void onClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _UIManager.ToggleCurrentRoomCanvas(false);
    }
}
