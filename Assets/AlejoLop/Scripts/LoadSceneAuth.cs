using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAuth : MonoBehaviour
{
    [SerializeField] string sceneToLoad = "GameScene";
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }

    private void HandleAuthStateChange(object sender, EventArgs e) {

        if(FirebaseAuth.DefaultInstance.CurrentUser != null) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnDestroy() {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChange;
    }

}
