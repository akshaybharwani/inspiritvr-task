using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;

public class PlayerEntryController : MonoBehaviour
{
    #region Private Variables

    [Header("UI References")] 
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    
    [SerializeField]
    private TextMeshProUGUI playerReadyButtonText;

    [SerializeField] 
    private Image playerReadyButtonImage;

    [SerializeField] 
    private Button playerReadyButton;

    private int _ownerID;
    private bool _isPlayerReady;
    
    // Color values to be used
    private Color activeColor = Color.green;
    private Color inActiveColor = Color.red;
    
    #endregion

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != _ownerID)
        {
            playerReadyButton.enabled = false;
        }
        else
        {
            Hashtable initialProps = new Hashtable() {{InspiritVRQuizGame.PLAYER_READY, _isPlayerReady}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);
        }
    }

    /// <summary>
    /// Sets Player Fields based on Player ID and Name
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="playerName"></param>
    public void SetPlayerEntryFields(int playerId, string playerName)
    {
        _ownerID = playerId;
        playerNameText.text = playerName;
    }
    
    public void SetPlayerReady(bool playerReady)
    {
        playerReadyButtonText.text = playerReady ? "Ready!" : "Ready?";
        playerReadyButtonImage.color = playerReady ? activeColor : inActiveColor;
    }

    public void onClick_PlayerReadyButton()
    {
        _isPlayerReady = !_isPlayerReady;
        SetPlayerReady(_isPlayerReady);

        Hashtable props = new Hashtable() {{InspiritVRQuizGame.PLAYER_READY, _isPlayerReady}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<LobbyManager>().LocalPlayerPropertiesUpdated();
        }
    }
}
