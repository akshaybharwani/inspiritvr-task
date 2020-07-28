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

    [Header("All Player Scores UI References")]
    [SerializeField]
    private PlayerScoreController playerScoreControllerPrefab;

    [SerializeField] 
    private Transform playersParent;

    [Header("Local Player UI References")] 
    [SerializeField]
    private PlayerScoreController localPlayerScoreController;

    #endregion

    private Dictionary<int, PlayerScoreController> playerListEntries;

    #region UNITY

    public void Awake()
    {
        playerListEntries = new Dictionary<int, PlayerScoreController>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // Check if the Player is Local
            if (p.IsLocal)
            {
                // If it is, update the name
                localPlayerScoreController.SetPlayerScoreName(p.NickName);
            }
            else
            {
                // If it's not, Instantiate new GameObjects of PlayerScore
                PlayerScoreController entry = Instantiate(playerScoreControllerPrefab, playersParent);
                entry.SetPlayerScoreName(p.NickName);
                playerListEntries.Add(p.ActorNumber, entry);
            }
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
        // Check if the Player is Local
        if (targetPlayer.IsLocal)
        {
            // Set the score
            localPlayerScoreController.SetPlayerScore(targetPlayer.GetScore());
        }
        else
        {
            PlayerScoreController entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                entry.SetPlayerScore(targetPlayer.GetScore());
            }
        }
    }

    #endregion
}
