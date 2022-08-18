using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public TMP_InputField roomNameInput;
    public TMP_InputField nameInput;

    private void Awake()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings("V1");
    }

    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions() { MaxPlayers = 5 }, TypedLobby.Default);

    }

    void OnJoinedLobby()
    {
        Debug.Log("Joined the room");
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.playerName = nameInput.text;
        PlayerPrefs.SetString("PlayerName", nameInput.text);
        PhotonNetwork.LoadLevel("MultiGameScene");



        Debug.Log("Name of the room: " + PhotonNetwork.room.Name + "\nPlayer is name: " + PhotonNetwork.playerName);
        Debug.Log("Joined the lobby");
    }
}
