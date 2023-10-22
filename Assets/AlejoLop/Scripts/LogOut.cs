using Firebase.Auth;
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
        SceneManager.LoadScene(sceneToLoad);
    }
}
