using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Firebase.Database;
using Firebase.Extensions;
public class MatchInfoDiplay : MonoBehaviour
{
    [SerializeField] private TMP_Text opponentID;
    [SerializeField] private TMP_Text opponentUsername;

    [SerializeField] private TMP_Text playerID;
    [SerializeField] private TMP_Text playerUsername;

    [SerializeField] private TMP_Text matchIdentifier;

    [SerializeField] private GameObject matchFoundPanel;
    [SerializeField] private GameObject quickMatchObject;
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
        matchmaking.OnMatchExit += HandleExitMatch;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        matchmaking.OnMatchFound -= UpdateMatchInfo;
        matchmaking.OnMatchExit -= HandleExitMatch;
    }

    public void UpdateMatchInfo(string oponentID, string userId, string matchID)
    {
        matchFoundPanel.SetActive(true);
        opponentID.text = "ID " + oponentID;
        playerID.text = "ID " + userId;
        this.matchIdentifier.text = "Match# " + matchID;
        GetUsername(oponentID, opponentUsername);
        GetUsername(userId, playerUsername);
    }
    private void GetUsername(string userID, TMP_Text userText)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                userText.text = task.Result.Value.ToString();
                Debug.Log("Username" + task.Result.Value.ToString());
            }
        });
    }
    public void HandleExitMatch()
    {
        matchFoundPanel.SetActive(false);
        GameObject[] array = GameObject.FindGameObjectsWithTag("InQueue");
        foreach(var item in array)
        {
            item.SetActive(false);
        }
        quickMatchObject.SetActive(true);
        matchmaking.ResetMatchIDs();
    }
}
