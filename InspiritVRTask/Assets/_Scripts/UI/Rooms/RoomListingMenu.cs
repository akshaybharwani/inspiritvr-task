using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] 
    private Transform roomListingParent;
    
    [SerializeField]
    private RoomListing roomListingPrefab;
    
    private List<RoomListing> _listings = new List<RoomListing>();

    private UIManager _UIManager;

    #endregion

    private void Start()
    {
        // Set UIManager reference
        _UIManager = FindObjectOfType<UIManager>();
    }
    
    public override void OnJoinedRoom()
    {
        _UIManager.ToggleCurrentRoomCanvas(true);
        roomListingParent.DestroyChildren();
        
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);

                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);

                if (index == -1)
                {
                    RoomListing roomListing = Instantiate(roomListingPrefab, roomListingParent);

                    if (roomListing)
                    {
                        roomListing.SetRoomInfo(roomInfo);
                        _listings.Add(roomListing);
                    }
                }
            }
        }
    }
}
