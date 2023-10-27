using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LogOut : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string sceneToLoad;
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Cerró Sesión");
        FirebaseAuth.DefaultInstance.SignOut();
        FirebaseDatabase.DefaultInstance.RootReference.Child("users-online").Child(PlayerPrefs.GetString("ID")).RemoveValueAsync();
        SceneManager.LoadScene(sceneToLoad);
    }
}
