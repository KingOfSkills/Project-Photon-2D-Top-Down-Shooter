using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private List<Transform> spawnPoints;
    //[SerializeField] private int enemiesToSpawn;
    [SerializeField] private float enemySpawnTimeMax;
    private float enemySpawnTimer;
    private Transform spawningPoint;
    private int enemiesToSpawn;
    public bool isSpawing;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        //photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        foreach (Transform visual in spawnPoints)
        {
            visual.gameObject.SetActive(false);
        }
        //InvokeRepeating("SpawnEnemy", 2.5f, 5f);
        //Invoke("SpawnEnemy", 2.5f);
        enemiesToSpawn = 1; //0 Testing
        spawningPoint = spawnPoints[0];
        enemySpawnTimer = enemySpawnTimeMax;
        isSpawing = true; //false Testing
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (enemySpawnTimer > 0)
        {
            enemySpawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemiesToSpawn > 0)// && enemySpawnTimer <= 0)
        {
            spawningPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            PhotonNetwork.InstantiateRoomObject(PhotonPrefabs.SpawnPrefab(PhotonPrefabs.Prefab.Enemy), spawningPoint.position, Quaternion.identity);
            enemiesToSpawn--;
            enemySpawnTimer = enemySpawnTimeMax;
        }
        else
        {
            //isSpawing = false;
        }
    }

    public void SetAmountToSpawn(int amount)
    {
        enemiesToSpawn = amount;
        enemySpawnTimer = enemySpawnTimeMax;
        isSpawing = true;
    }
}
