using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviour
{
    #region Public Variables

    public RoomInfo RoomInfo { get; private set; }

    #endregion
    
    #region Private Variables

    [SerializeField] 
    private TextMeshProUGUI roomListingText;

    public RoomListingMenu(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
    }

    #endregion

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        roomListingText.text = roomInfo.Name + ", " + roomInfo.MaxPlayers;
    }
}
