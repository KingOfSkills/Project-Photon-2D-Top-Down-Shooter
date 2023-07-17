using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContainer;
    [SerializeField] Transform roomListTemplate;
    [SerializeField] Transform playerListContainer;
    [SerializeField] Transform playerListTemplate;
    [SerializeField] Button startGameButton;

    public enum MenuName
    {
        LoadingMenu,
        TitleMenu,
        CraeteRoomMenu,
        RoomMenu,
        ErrorMenu,
        FindRoomMenu,
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        MenuManager.Instance.ShowMenu(MenuName.LoadingMenu.ToString());
        //Debug.Log("We are connect to the " + PhotonNetwork.CloudRegion + " server!");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("Joined lobby" + PhotonNetwork.CurrentLobby.Name);
        MenuManager.Instance.ShowMenu(MenuName.TitleMenu.ToString());
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        //Debug.Log("Creating Room");

        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        MenuManager.Instance.ShowMenu(MenuName.LoadingMenu.ToString());
        //Debug.Log("Creating Room");
        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    public override void OnCreatedRoom()
    {
        //Debug.Log("Created Room");

        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            playerListTemplate.gameObject.SetActive(true);
            Transform playerTemplateTransform = Instantiate(playerListTemplate, playerListContainer);
            playerTemplateTransform.GetComponent<PlayerNameTextSingleUI>().SetPlayer(playerList[i]);
            playerListTemplate.gameObject.SetActive(false);
        }
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

        MenuManager.Instance.ShowMenu(MenuName.RoomMenu.ToString());

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] playerList = PhotonNetwork.PlayerList;
        //Debug.Log("playerlist = " + playerList.Length);

        foreach (Transform child in playerListContainer)
        {
            if (child == playerListTemplate) continue;
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerList.Length; i++)
        {
            playerListTemplate.gameObject.SetActive(true);
            Transform playerTemplateTransform = Instantiate(playerListTemplate, playerListContainer);
            playerTemplateTransform.GetComponent<PlayerNameTextSingleUI>().SetPlayer(playerList[i]);
            playerListTemplate.gameObject.SetActive(false);
        }

        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        //Debug.Log("Players in this room = " + PhotonNetwork.CountOfPlayersInRooms);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.Instance.ShowMenu(MenuName.ErrorMenu.ToString());

        errorText.text = "Room Creation Failed: " + message;
    }

    public void FindRooms()
    {
        MenuManager.Instance.ShowMenu(MenuName.FindRoomMenu.ToString());

        foreach (Transform child in roomListContainer)
        {
            if (child == roomListTemplate)
            {
                roomListTemplate.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void RefreshRoomList()
    {
        //Debug.Log(PhotonNetwork.CountOfRooms);
        foreach (Transform child in roomListContainer)
        {
            if (child == roomListTemplate) continue;
            child.gameObject.SetActive(true);
        }
    }

    public void LeaveRoom()
    {
        //Debug.Log("Leaving Room");
        MenuManager.Instance.ShowMenu(MenuName.LoadingMenu.ToString());

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //Debug.Log("Left Room");
        //MenuManager.Instance.ShowMenu(MenuName.TitleMenu.ToString());
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList)
    {
        //Debug.Log("Updated Room List!");
        //Debug.Log("Rooms made " + roomInfoList.Count);
        //Debug.Log("count in room list = " + roomInfoList.Count);
        foreach (Transform child in roomListContainer)
        {
            if (child == roomListTemplate) continue;
            //Debug.Log("Destroying child gameObjects " + child.name);
            Destroy(child.gameObject);
        }

        foreach (RoomInfo roomInfo in roomInfoList)
        {
            if (roomInfo.RemovedFromList) continue;

            roomListTemplate.gameObject.SetActive(true);
            //Debug.Log("Spawning Template for room " + roomInfo.Name);
            Transform roomTemplateTransform = Instantiate(roomListTemplate, roomListContainer);
            roomTemplateTransform.GetComponent<RoomListSingleUI>().SetRoom(roomInfo);
            roomListTemplate.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerListTemplate.gameObject.SetActive(true);
        Transform playerTemplateTransform = Instantiate(playerListTemplate, playerListContainer);
        playerTemplateTransform.GetComponent<PlayerNameTextSingleUI>().SetPlayer(newPlayer);
        playerListTemplate.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
