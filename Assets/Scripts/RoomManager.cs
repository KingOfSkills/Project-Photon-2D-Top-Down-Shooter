using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }

    public List<PlayerController> playersList;

    public event EventHandler OnPlayerListUpdate;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        playersList = new List<PlayerController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Debug.Log("We are in scene " + scene.name);
        if (scene.buildIndex == 1)
        {
            //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            PhotonNetwork.Instantiate(PhotonPrefabs.SpawnPrefab(PhotonPrefabs.Prefab.PlayerManager), Vector3.zero, Quaternion.identity);
            //Debug.Log(Path.Combine("PhotonPrefabs", "PlayerManager"));
        }
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public void AddToPlayersList(PlayerController player)
    {
        playersList.Add(player);
        OnPlayerListUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveFromPlayersList(PlayerController player)
    {
        playersList.Remove(player);
        OnPlayerListUpdate?.Invoke(player, EventArgs.Empty);
    }
}
