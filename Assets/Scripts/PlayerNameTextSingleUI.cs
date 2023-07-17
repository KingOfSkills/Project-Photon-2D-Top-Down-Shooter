using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameTextSingleUI : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerNameText;

    private Player player;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
