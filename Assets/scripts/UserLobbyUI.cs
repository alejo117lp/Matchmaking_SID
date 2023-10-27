using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UserLobbyUI : MonoBehaviour
{
    [SerializeField] TMP_Text username;
    public void setUsername(string username)
    {
        this.username.text = username;
    }
}
