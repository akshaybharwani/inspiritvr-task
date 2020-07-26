using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListings : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] 
    private Transform roomListingParent;
    
    [SerializeField]
    private RoomListing roomListingPrefab;
    
    private List<RoomListing> _listings = new List<RoomListing>();

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
