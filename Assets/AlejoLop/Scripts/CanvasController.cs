using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject panelLogin;
    [SerializeField] GameObject panelSignUp;


    public void ActiveLoginPanel() {
        panelLogin.SetActive(true);
        panelSignUp.SetActive(false);
    }

    public void ActiveSignUpPanel() {
        panelLogin.SetActive(false);
        panelSignUp.SetActive(true);
    }
}
