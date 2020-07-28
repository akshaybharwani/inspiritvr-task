using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
 
    [Header("UI References")]
    public TextMeshProUGUI InfoText;

    public CanvasGroup gamePanelCanvasGroup;

    public CountdownTimerController countdownTimerController;

    private QuestionsManager _questionsManager;

    #region UNITY

    public void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        countdownTimerController.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public void Start()
    {
        Hashtable props = new Hashtable
        {
            {InspiritVRQuizGame.PLAYER_LOADED_LEVEL, true}
        };
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        
        // Set the QuestionsManager reference
        _questionsManager = FindObjectOfType<QuestionsManager>();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        countdownTimerController.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #endregion

    #region COROUTINES

    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            // Hide the Game Panel 
            gamePanelCanvasGroup.DOFade(0, 2f);
            
            InfoText.text =
                $"Player {winner} won with {score} points.\n\n\nReturning back to All Players screen in {timer.ToString("n2")} seconds.";

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        //PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(InspiritVRQuizGame.PLAYER_LIVES))
        {
            CheckEndOfGame();
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimerController.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(InspiritVRQuizGame.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimerController.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                InfoText.text = "Waiting for other players...";
            }
        }
    }

    #endregion

    
    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        // SHow the Game Panel
        gamePanelCanvasGroup.DOFade(1, 2f);
        
        // Show the first question
        _questionsManager.SetCurrentQuestionFields();
        
        // Start the Timer
        StartCoroutine(_questionsManager.StartQuestionTimer());
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(InspiritVRQuizGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool) playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    public void CheckEndOfGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StopAllCoroutines();
        }

        string winner = "";
        int score = -1;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.GetScore() > score)
            {
                winner = p.NickName;
                score = p.GetScore();
            }
        }

        Debug.Log("YOO");
        StartCoroutine(EndOfGame(winner, score));
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
}
