using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks
 {
     [Header("Login Panel")]
     public GameObject LoginPanel;

     public CanvasGroup playerNameLabelCanvasGroup;
     public TextMeshProUGUI playerNameLabelText;
     public TMP_InputField playerNameInputField;

     [Header("Selection Panel")]
     public GameObject SelectionPanel;

     [Header("Create Room Panel")]
     public GameObject CreateRoomPanel;

     public TMP_InputField roomNameInputField;

     [Header("Room List Panel")]
     public GameObject RoomListPanel;

     public GameObject RoomListContent;
     public GameObject RoomListEntryPrefab;

     [Header("Inside Room Panel")]
     public GameObject InsideRoomPanel;

     public Button StartGameButton;
     public GameObject PlayerListEntryPrefab;
     public Transform playerListContent;

     private Dictionary<string, RoomInfo> cachedRoomList;
     private Dictionary<string, GameObject> roomListEntries;
     private Dictionary<int, GameObject> playerListEntries;

     #region Private Variables

     private LobbyBreadcrumbsController _lobbyBreadcrumbsController;

     #endregion
     
     #region UNITY

     public void Awake()
     {
         PhotonNetwork.AutomaticallySyncScene = true;

         cachedRoomList = new Dictionary<string, RoomInfo>();
         roomListEntries = new Dictionary<string, GameObject>();
         
         playerNameInputField.text = "Player " + Random.Range(1000, 10000);
     }

     public void Start()
     {
         // Set the reference to the Controller
         _lobbyBreadcrumbsController = FindObjectOfType<LobbyBreadcrumbsController>();
         
         // Set the character limit to the Input field
         playerNameInputField.characterLimit = 15;
     }

     #endregion

     #region PUN CALLBACKS

     public override void OnConnectedToMaster()
     {
         this.SetActivePanel(SelectionPanel.name);
     }

     public override void OnRoomListUpdate(List<RoomInfo> roomList)
     {
         ClearRoomListView();

         UpdateCachedRoomList(roomList);
         UpdateRoomListView();
     }

     public override void OnLeftLobby()
     {
         cachedRoomList.Clear();

         ClearRoomListView();
     }

     public override void OnCreateRoomFailed(short returnCode, string message)
     {
         SetActivePanel(SelectionPanel.name);
     }

     public override void OnJoinRoomFailed(short returnCode, string message)
     {
         SetActivePanel(SelectionPanel.name);
     }

     public override void OnJoinRandomFailed(short returnCode, string message)
     {
         string roomName = "Room " + Random.Range(1000, 10000);

         RoomOptions options = new RoomOptions {MaxPlayers = 8};

         PhotonNetwork.CreateRoom(roomName, options, null);
     }

     public override void OnJoinedRoom()
     {
         SetActivePanel(InsideRoomPanel.name);

         if (playerListEntries == null)
         {
             playerListEntries = new Dictionary<int, GameObject>();
         }

         foreach (Player p in PhotonNetwork.PlayerList)
         {
             GameObject entry = Instantiate(PlayerListEntryPrefab, playerListContent);
             entry.transform.localScale = Vector3.one;
             entry.GetComponent<PlayerEntryController>().SetPlayerEntryFields(p.ActorNumber, p.NickName);

             object isPlayerReady;
             if (p.CustomProperties.TryGetValue(InspiritVRQuizGame.PLAYER_READY, out isPlayerReady))
             {
                 entry.GetComponent<PlayerEntryController>().SetPlayerReady((bool) isPlayerReady);
             }

             playerListEntries.Add(p.ActorNumber, entry);
         }

         StartGameButton.gameObject.SetActive(CheckPlayersReady() );

         Hashtable props = new Hashtable
         {
             {InspiritVRQuizGame.PLAYER_LOADED_LEVEL, false}
         };
         PhotonNetwork.LocalPlayer.SetCustomProperties(props);
     }

     public override void OnLeftRoom()
     {
         SetActivePanel(SelectionPanel.name);

         foreach (GameObject entry in playerListEntries.Values)
         {
             Destroy(entry.gameObject);
         }

         playerListEntries.Clear();
         playerListEntries = null;
     }

     public override void OnPlayerEnteredRoom(Player newPlayer)
     {
         GameObject entry = Instantiate(PlayerListEntryPrefab, playerListContent);
         entry.transform.localScale = Vector3.one;
         entry.GetComponent<PlayerEntryController>().SetPlayerEntryFields(newPlayer.ActorNumber, newPlayer.NickName);

         playerListEntries.Add(newPlayer.ActorNumber, entry);

         StartGameButton.gameObject.SetActive(CheckPlayersReady());
     }

     public override void OnPlayerLeftRoom(Player otherPlayer)
     {
         Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
         playerListEntries.Remove(otherPlayer.ActorNumber);

         StartGameButton.gameObject.SetActive(CheckPlayersReady());
     }

     public override void OnMasterClientSwitched(Player newMasterClient)
     {
         if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
         {
             StartGameButton.gameObject.SetActive(CheckPlayersReady());
         }
     }

     public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
     {
         if (playerListEntries == null)
         {
             playerListEntries = new Dictionary<int, GameObject>();
         }

         GameObject entry;
         if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
         {
             object isPlayerReady;
             if (changedProps.TryGetValue(InspiritVRQuizGame.PLAYER_READY, out isPlayerReady))
             {
                 entry.GetComponent<PlayerEntryController>().SetPlayerReady((bool) isPlayerReady);
             }
         }

         StartGameButton.gameObject.SetActive(CheckPlayersReady());
     }

     #endregion

     #region UI CALLBACKS

     public void OnBackButtonClicked()
     {
         if (PhotonNetwork.InLobby)
         {
             PhotonNetwork.LeaveLobby();
         }

         SetActivePanel(SelectionPanel.name);
     }

     public void OnCreateRoomButtonClicked()
     {
         string roomName = roomNameInputField.text;
         roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

         RoomOptions options = new RoomOptions {MaxPlayers = 4, PlayerTtl = 10000 };

         PhotonNetwork.CreateRoom(roomName, options, null);
         
         _lobbyBreadcrumbsController.ToggleAllPlayersLabel(true);
     }

     public void OnLeaveGameButtonClicked()
     {
         PhotonNetwork.LeaveRoom();
     }

     public void OnLoginButtonClicked()
     {
         // Set the PlayerName
         string playerName = playerNameInputField.text;

         if (!playerName.Equals(""))
         {
             PhotonNetwork.LocalPlayer.NickName = playerName;
             PhotonNetwork.ConnectUsingSettings();
             
             togglePlayerNameLabel(true, playerName);
             
             _lobbyBreadcrumbsController.ToggleCreateJoinRoomLabel(true);
         }
         else
         {
             Debug.LogError("Player Name is invalid.");
         }
     }

     public void OnRoomListButtonClicked()
     {
         if (!PhotonNetwork.InLobby)
         {
             PhotonNetwork.JoinLobby();
         }

         SetActivePanel(RoomListPanel.name);
     }

     public void OnStartGameButtonClicked()
     {
         PhotonNetwork.CurrentRoom.IsOpen = false;
         PhotonNetwork.CurrentRoom.IsVisible = false;

         PhotonNetwork.LoadLevel(1);
     }

     #endregion

     private bool CheckPlayersReady()
     {
         if (!PhotonNetwork.IsMasterClient)
         {
             return false;
         }

         foreach (Player p in PhotonNetwork.PlayerList)
         {
             object isPlayerReady;
             if (p.CustomProperties.TryGetValue(InspiritVRQuizGame.PLAYER_READY, out isPlayerReady))
             {
                 if (!(bool) isPlayerReady)
                 {
                     return false;
                 }
             }
             else
             {
                 return false;
             }
         }

         if (!areMinPlayersAvailable())
             return false;
         
         return true;
     }
     
     private void ClearRoomListView()
     {
         foreach (GameObject entry in roomListEntries.Values)
         {
             Destroy(entry.gameObject);
         }

         roomListEntries.Clear();
     }

     public void LocalPlayerPropertiesUpdated()
     {
         StartGameButton.gameObject.SetActive(CheckPlayersReady());
     }

     public void SetActivePanel(string activePanel)
     {
         LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
         SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
         CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
         RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
         InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
     }

     private void UpdateCachedRoomList(List<RoomInfo> roomList)
     {
         foreach (RoomInfo info in roomList)
         {
             // Remove room from cached room list if it got closed, became invisible or was marked as removed
             if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
             {
                 if (cachedRoomList.ContainsKey(info.Name))
                 {
                     cachedRoomList.Remove(info.Name);
                 }

                 continue;
             }

             // Update cached room info
             if (cachedRoomList.ContainsKey(info.Name))
             {
                 cachedRoomList[info.Name] = info;
             }
             // Add new room info to cache
             else
             {
                 cachedRoomList.Add(info.Name, info);
             }
         }
     }

     private void UpdateRoomListView()
     {
         foreach (RoomInfo info in cachedRoomList.Values)
         {
             GameObject entry = Instantiate(RoomListEntryPrefab, RoomListContent.transform, true);
             entry.transform.localScale = Vector3.one;
             entry.GetComponent<RoomEntryController>().SetRoomEntryFields(info.Name, info.PlayerCount, info.MaxPlayers);

             roomListEntries.Add(info.Name, entry);
         }
     }

     /// <summary>
     /// Which shows/hides the Player Name based on Player's Login Status
     /// </summary>
     /// <param name="toggleValue"></param>
     /// <param name="playerName"></param>
     private void togglePlayerNameLabel(bool toggleValue, string playerName)
     {
         // Set the PlayerName Label
         playerNameLabelText.text = playerName;

         if (toggleValue)
             // Show the PlayerName Label
             playerNameLabelCanvasGroup.alpha = 1;
         else 
             // Hide the PlayerName Label
             playerNameLabelCanvasGroup.alpha = 0;
     }

     private bool areMinPlayersAvailable()
     {
         return PhotonNetwork.PlayerList.Length > 1;
     }
 }