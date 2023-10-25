using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainPanelsController : MonoBehaviour
{
    [SerializeField] GameObject friendsPanel;
    [SerializeField] GameObject partyPanel;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject invitationsPanel;

    public void ActiveFriendsPanel() {
        friendsPanel.SetActive(true);
        partyPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void ActivePartyPanel() {
        friendsPanel.SetActive(false);
        partyPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public void ActiveLobbyPanel() {
        friendsPanel.SetActive(false);
        partyPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void ActiveInvitationsPanel() {
        invitationsPanel.SetActive(true);
    }

    public void DesactivateInvitationsPanel() {
        invitationsPanel.SetActive(false);
    }
}
