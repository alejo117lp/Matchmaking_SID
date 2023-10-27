using Firebase.Database;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class MatchmakingManager : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    string currentPlayerID;
    string currentOpponentID;
    string currentMatchID;
    bool isSubscribed = false;


    public delegate void MatchFound(string oponentID, string userId, string matchID);
    public MatchFound OnMatchFound;
    [SerializeField] MatchInfoDiplay matchInfoDiplay;
    public delegate void MatchExit();
    public MatchExit OnMatchExit;
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync();
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            currentPlayerID = user.UserId;
        }
        else
        {
            currentPlayerID = "ccgfqdhQrYV7qj6GrA0cK5zObxQ2";
        }
    }
    /// <summary>
    /// Function to add the current user to the "looking_for_match" node and start playing for a match
    /// </summary>
    public void StartLookingForMatch()
    {
        DatabaseReference lookingForMatchRef = reference.Child("looking_for_match");
        lookingForMatchRef.Child(currentPlayerID).SetValueAsync(true);
        if (!isSubscribed)
        {
            lookingForMatchRef.ChildAdded += HandleLookingForMatchAdded;
            isSubscribed = true;
        }
    }

    /// <summary>
    /// Function to cancel the search and remove the player from the "looking_for_match" and "playing_for_match" nodes
    /// </summary>
    public void CancelSearch()
    {
        DatabaseReference lookingForMatchRef = reference.Child("looking_for_match");
        lookingForMatchRef.Child(currentPlayerID).RemoveValueAsync();

        if (isSubscribed)
        {
            lookingForMatchRef.ChildAdded -= HandleLookingForMatchAdded;
            isSubscribed = false;
        }
    }

    /// <summary>
    /// Event handler for changes in the "looking_for_match" node
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void HandleLookingForMatchAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null )
        {
            string playerKey = args.Snapshot.Key;

            if (playerKey != currentPlayerID)
            {
                // You have enough players for a match, create a match
                CreateUniqueMatch(playerKey);
            }
        }
    }
    /// <summary>
    /// Function to create a unique match with verification
    /// </summary>
    public void CreateUniqueMatch(string opponentID)
    {
        string uniqueMatchID = (IDCreator.ExtractID(currentPlayerID) + IDCreator.ExtractID(opponentID)).ToString();
        currentMatchID = uniqueMatchID;
        currentOpponentID = opponentID;
        DatabaseReference matchRef = reference.Child("matches").Child(uniqueMatchID);
        matchRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.Result.Exists)
            {
                Debug.Log("match created");
                CreateMatch(opponentID, uniqueMatchID);
            }
            else if (task.Result.Exists)
            {
                Debug.Log("Match already exists with ID: " + uniqueMatchID);
                OnMatchFound?.Invoke(opponentID, currentPlayerID, uniqueMatchID);
                matchRef.ChildRemoved += HandleGameExit;
            }
        });
    }
    private void CreateMatch(string opponentPlayerID, string uniqueMatchID)
    {
        DatabaseReference matchRef = reference.Child("matches").Child(uniqueMatchID);
        Dictionary<string, object> matchData = new Dictionary<string, object>();

        matchData["status"] = "in_progress";
        matchData[currentPlayerID] = true;
        matchData[opponentPlayerID] = true;

        reference.Child("looking_for_match").Child(currentPlayerID).RemoveValueAsync();
        reference.Child("looking_for_match").Child(opponentPlayerID).RemoveValueAsync();
        matchRef.UpdateChildrenAsync(matchData);
        OnMatchFound?.Invoke(opponentPlayerID, currentPlayerID, uniqueMatchID);
        matchRef.ChildRemoved += HandleGameExit;
        DatabaseReference player = FirebaseDatabase.DefaultInstance.GetReference("users").Child(currentPlayerID);
        DatabaseReference oponent = FirebaseDatabase.DefaultInstance.GetReference("users").Child(opponentPlayerID);

        player.Child("current_match").SetValueAsync(uniqueMatchID);
        oponent.Child("current_match").SetValueAsync(uniqueMatchID);

    }
    public void ResetMatchIDs()
    {
        DatabaseReference player = FirebaseDatabase.DefaultInstance.GetReference("users").Child(currentPlayerID);
        DatabaseReference oponent = FirebaseDatabase.DefaultInstance.GetReference("users").Child(currentOpponentID);

        player.Child("current_match").SetValueAsync("");
        oponent.Child("current_match").SetValueAsync("");
        currentOpponentID = "";
    }
    private void OnApplicationQuit()
    {
        DatabaseReference lookingForMatchRef = reference.Child("looking_for_match");
        if (isSubscribed)
        {

            lookingForMatchRef.ChildChanged -= HandleLookingForMatchAdded;
            isSubscribed = false;
        }
        ExitMatch();
        lookingForMatchRef.Child(currentPlayerID).RemoveValueAsync();
    }
    public void ExitMatch()
    {
        if(currentMatchID != "")
        {
            DatabaseReference MatchRef = reference.Child("matches").Child(currentMatchID);
            MatchRef.RemoveValueAsync();
            OnMatchExit?.Invoke();
        }
    }
    private void HandleGameExit(object sender, ChildChangedEventArgs args)
    {
        ExitMatch();
        OnMatchExit?.Invoke();
    }
}
