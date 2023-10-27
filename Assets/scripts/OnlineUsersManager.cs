using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class OnlineUsersManager : MonoBehaviour
{
    private DatabaseReference database;
    private FirebaseAuth auth;
    string currentUsername;
    string currentUserID;
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
        }
        SetUserOnline();
    }
    private void SetUserOnline()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").Child(currentUserID).SetValueAsync(currentUsername);
    }
}
[System.Serializable]
public class User
{
    public string userID;
    public string userUsername;
    public Dictionary<string, string> userFriends;

    public User()
    {
        userFriends = new Dictionary<string, string>();
    }
}
