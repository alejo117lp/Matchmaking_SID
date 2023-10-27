using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class UsersManager : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    public static UsersManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync();
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void GetUsername(string userID)
    {
        DatabaseReference usersRef = reference.Child("users").Child(userID);

        usersRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && task.Result.Exists)
            {
                string username = task.Result.Value.ToString();
                Debug.Log("Username for user ID " + userID + ": " + username);
            }
            else
            {
                Debug.LogError("Failed to retrieve the username for user ID " + userID);
            }
        });
    }
    
}
