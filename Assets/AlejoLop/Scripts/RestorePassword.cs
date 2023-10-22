using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestorePassword : MonoBehaviour
{
    [SerializeField] TMP_InputField forgotPasswordEmail;
    [SerializeField] private TMP_Text warningForgetPasswordText;
    [SerializeField] TMP_Text confirmationPasswordText;
    [SerializeField] GameObject forgotPasswordPanel;

    public void ForgotPasswordButton() {
        if (string.IsNullOrEmpty(forgotPasswordEmail.text)) {
            warningForgetPasswordText.text = $"Debes ingresar tu Email asociado";
            return;
        }

        FogotPassword(forgotPasswordEmail.text);
    }

    void FogotPassword(string forgotPasswordEmail) {

        FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(forgotPasswordEmail)
            .ContinueWithOnMainThread(RestoreTask => {

                if (RestoreTask.IsCanceled) {
                    Debug.LogError($"El cambio de contraseña ha sido cancelado");
                }

                else if (RestoreTask.IsFaulted) {
                    foreach (FirebaseException exception in RestoreTask.Exception.Flatten().InnerExceptions) {
                        FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                        if (firebaseEx != null) {
                            var errorCode = (AuthError)firebaseEx.ErrorCode;
                        }
                    }
                }
                confirmationPasswordText.text = "El link para restablecer tu contraseña ha sido enviado";
            });
    }

    public void OpenRestorePasswordPanel() {
        forgotPasswordPanel.SetActive(true);
    }

    public void CloseRestorePasswordPanel() {
        forgotPasswordPanel.SetActive(false);
    }
}
