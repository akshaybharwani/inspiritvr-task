using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerScoreController : MonoBehaviour
{
    #region Private Variables

    [Header("UI References")] 
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    
    [SerializeField]
    private TextMeshProUGUI playerScoreText;

    private string _roomName;
    
    #endregion

    /// <summary>
    /// Sets Player Fields based on Player ID and Name
    /// </summary>
    /// <param name="playerName"></param>
    public void SetPlayerScoreName(string playerName)
    {
        playerNameText.text = playerName;
    }
    
    /// <summary>
    /// Sets Player Fields based on Player ID and Name
    /// </summary>
    /// <param name="playerScore"></param>
    public void SetPlayerScore(int playerScore)
    {
        playerScoreText.text = playerScore.ToString();
    }
}
