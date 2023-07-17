using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;

    private RoomInfo roomInfo;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.Instance.ShowMenu(Launcher.MenuName.LoadingMenu.ToString());
    }

    public void SetRoom(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        roomNameText.text = roomInfo.Name;
    }
}
