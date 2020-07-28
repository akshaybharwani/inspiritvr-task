using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInGame : MonoBehaviour
{
    #region Private Variables

    [Header("Player Fields")] 
    [SerializeField]
    private TextMeshProUGUI playerName;
    
    [SerializeField]
    private TextMeshProUGUI playerScore;

    #endregion

    /// <summary>
    /// Which sets Player Name who is In Game
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
    
    /// <summary>
    /// Which sets Player Score who is In Game
    /// </summary>
    /// <param name="score"></param>
    public void SetPlayerScore(int score)
    {
        playerScore.text = score.ToString();
    }
}
