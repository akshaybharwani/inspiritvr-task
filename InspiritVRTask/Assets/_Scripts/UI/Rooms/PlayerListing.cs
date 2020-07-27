using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [Header("UI References")] 
    public TextMeshProUGUI PlayerNameText;

    private int ownerId;
    private bool isPlayerReady;

    #region UNITY

    public void Start()
    {
        /*if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                }
            });
        }*/
    }

    #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    public void SetPlayerReady(bool playerReady)
    {
        //PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
        //PlayerReadyImage.enabled = playerReady;
    }
}