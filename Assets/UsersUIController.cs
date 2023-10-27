using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersUIController : MonoBehaviour
{
    [SerializeField] UserLobbyUI[] UIusers;
    
    public void setList (Dictionary<string,string> usersOnline)
    {
        var usernames = new List<string>(usersOnline.Values);
        for (int i =0;i< UIusers.Length;i++)
        {
            if(usernames[i] != null || usernames[i] != "")
            {
                UIusers[i].gameObject.SetActive(true);
                UIusers[i].setUsername(usernames[i]);
            }
        }
    }
}
