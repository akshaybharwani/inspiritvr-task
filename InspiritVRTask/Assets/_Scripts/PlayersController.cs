using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayersController : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField]
    private PlayerInGame playerInGamePrefab;

    [SerializeField] 
    private Transform playersParent;

    #endregion

    private Dictionary<int, PlayerInGame> playerListEntries;

    #region UNITY

    public void Awake()
    {
        playerListEntries = new Dictionary<int, PlayerInGame>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            PlayerInGame entry = Instantiate(playerInGamePrefab, playersParent);
            entry.SetPlayerName(p.NickName);
            playerListEntries.Add(p.ActorNumber, entry);
        }
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        PlayerInGame entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            entry.SetPlayerScore(targetPlayer.GetScore());
        }
    }

    #endregion
}
