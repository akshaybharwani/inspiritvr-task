using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListing : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] 
    private Transform roomListingParent;
    
    [SerializeField]
    private RoomListingMenu roomListingPrefab;
    
    private List<RoomListingMenu> _listings = new List<RoomListingMenu>();

    #endregion

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
                RoomListingMenu roomListing = Instantiate(roomListingPrefab, roomListingParent);

                if (roomListing)
                {
                    roomListing.SetRoomInfo(roomInfo);
                    _listings.Add(roomListing);
                }
            }
            
        }
    }
}
