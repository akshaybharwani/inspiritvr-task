using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] 
    private Transform playerListingParent;
    
    [SerializeField]
    private GameObject playerListingPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    #endregion

    public override void OnJoinedRoom()
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerListingPrefab, playerListingParent);
            entry.GetComponent<PlayerListing>().Initialize(p.ActorNumber, p.NickName);
     
            object isPlayerReady;
     
            playerListEntries.Add(p.ActorNumber, entry);
        }
    }

    public override void OnLeftRoom()
    {
     
        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
     
        playerListEntries.Clear();
        playerListEntries = null;
        playerListingParent.DestroyChildren();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(playerListingPrefab);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListing>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
     
        playerListEntries.Add(newPlayer.ActorNumber, entry);
    }
     
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }
}
