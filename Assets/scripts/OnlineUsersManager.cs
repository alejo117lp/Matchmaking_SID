using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System;

public class OnlineUsersManager : MonoBehaviour
{
    private DatabaseReference database;
    private FirebaseAuth auth;
    string currentUsername;
    string currentUserID;
    public Dictionary<string,string> UsersOnline = new Dictionary<string, string>();
    UsersUIController UIController;
    private void Awake()
    {
        UIController = FindObjectOfType<UsersUIController>();
    }
    void Start()
    {
        //FirebaseAuth.DefaultInstance.StateChanged += Check_Login;
        database = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            currentUserID = user.UserId;
            currentUsername = user.DisplayName;
            PlayerPrefs.SetString("username", currentUsername);
            PlayerPrefs.SetString("ID", currentUserID);
        }
        SetUserOnline();
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").Child(currentUserID).SetValueAsync(currentUsername);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").ChildAdded += HandleUserOnlineAdded;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").ChildRemoved += HandleUserOnlineRemoved;
    }
    private void OnDisable()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").ChildAdded -= HandleUserOnlineAdded;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").ChildRemoved -= HandleUserOnlineRemoved;
    }


    private void SetUserOnline()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").Child(currentUserID).SetValueAsync(currentUsername);
    }
     private void HandleUserOnlineAdded (object sender, ChildChangedEventArgs args)
     {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null)
        {
            Debug.Log(args.Snapshot.Value.ToString());
            UsersOnline.Add(args.Snapshot.Key, args.Snapshot.Value.ToString());
            UIController.setList(UsersOnline);
        }
     }
    private void HandleUserOnlineRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null)
        {
            Debug.Log(args.Snapshot.Key);
            UsersOnline.Remove(args.Snapshot.Key);
            UIController.setList(UsersOnline);
        }
    }
    private void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").Child(currentUserID).RemoveValueAsync();
    }
}
[System.Serializable]
public class User
{
    public string userID;
    public string userUsername;
    public Dictionary<string, string> userFriends;

    public User(string userID, string userUsername)
    {
        this.userID = userID;
        this.userUsername = userUsername;
        userFriends = new Dictionary<string, string>();
    }
}
