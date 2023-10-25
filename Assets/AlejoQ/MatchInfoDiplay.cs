using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MatchInfoDiplay : MonoBehaviour
{
    [SerializeField] private TMP_Text opponentID;

    [SerializeField] private TMP_Text playerID;

    [SerializeField] private TMP_Text matchIdentifier;

    [SerializeField] private GameObject matchFoundPanel;
    MatchmakingManager matchmaking;

    private void Awake()
    {
        matchmaking = FindObjectOfType<MatchmakingManager>();
        matchmaking.OnMatchFound += UpdateMatchInfo;
    }
    private void OnEnable()
    {
        // Subscribe to the OnMatchFoundEvent to receive match information
        matchmaking.OnMatchFound += UpdateMatchInfo;
        matchmaking.OnMatchExit += ExitMatch;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        matchmaking.OnMatchFound -= UpdateMatchInfo;
        matchmaking.OnMatchExit -= ExitMatch;
    }

    public  void UpdateMatchInfo(string oponentID, string userId, string matchID)
    {
        opponentID.text = "OpponentID: " + oponentID;
        playerID.text = "PlayerID: " + userId;
        this.matchIdentifier.text = "Match Identifier: " + matchID;
        matchFoundPanel.SetActive(true);
    }
    public void ExitMatch()
    {
        matchFoundPanel.SetActive(false);
    }
}
