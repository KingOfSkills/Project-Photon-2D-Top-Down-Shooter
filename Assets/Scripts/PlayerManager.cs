using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float respawnTime;

    private PhotonView photonView;
    private PlayerController playerController;
    private GameObject playerGameObject;
    private GameObject playerCameraGameObject;
    private bool hasCreatedCamera;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //playerCameraGameObject = Camera.main.gameObject;

        if (photonView.IsMine)
        {
            hasCreatedCamera = false;
            //Debug.Log("Starting Game. Player camera = " + playerCameraGameObject.name);
            CreatePlayerCamera();
            CreateController();
        }
    }

    private void CreateController()
    {
        Debug.Log("Creating Controller");
        playerGameObject = PhotonNetwork.Instantiate(PhotonPrefabs.SpawnPrefab(PhotonPrefabs.Prefab.Player), Vector3.zero, Quaternion.identity, 0, new object[] { photonView.ViewID });
        //Debug.Log(playerGameObject.name);
        playerController = playerGameObject.GetComponent<PlayerController>();
        //Debug.Log(playerController);
        RoomManager.Instance.AddToPlayersList(playerController);
        
        //CreatePlayerCamera();
        playerGameObject.GetComponent<PlayerController>().SetCamera(playerCameraGameObject);
        playerCameraGameObject.GetComponent<CameraPrefab>().SetTarget(playerGameObject.transform);
        Debug.Log("Done Creating Controller");

        //Debug.Log("Creating Camera");
        //playerCameraGameObject = Instantiate(playerCamera);
        //playerGameObject.GetComponent<PlayerController>().SetCamera(playerCameraGameObject);
        //playerCameraGameObject.GetComponent<CameraPrefab>().SetTarget(playerGameObject.transform);
    }

    private void CreatePlayerCamera()
    {
        if (!hasCreatedCamera)
        {
            Debug.Log("Creating Camera");
            //playerCameraGameObject = Instantiate(playerCamera);
            hasCreatedCamera = true;
        }
    }

    public void Die()
    {
        RoomManager.Instance.RemoveFromPlayersList(playerController);
        //playerGameObject.SetActive(false);
        PhotonNetwork.Destroy(playerController.GetComponent<PhotonView>());
        StartCoroutine("IRespawn");
    }

    private IEnumerator IRespawn()
    {
        Debug.Log("Respawning in " + respawnTime);
        yield return new WaitForSeconds(respawnTime);
        //Destroy(playerCameraGameObject);
        //playerGameObject.SetActive(true);
        //playerGameObject.transform.position = Vector3.zero;
        CreateController();
    }

    public Camera GetPlayerCamera()
    {
        return playerCameraGameObject.GetComponent<Camera>();
    }
}
