using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int enemiesToSpawnMaxIncrement;
    [SerializeField] private float playerRestTimerMax;
    [SerializeField] private float startRoundTimerMax;
    private int waveNumber;
    private int enemiesToSpawn;
    private bool isWaveInProgress;
    private bool allEnemiesDead;
    private int enimiesAlive;
    private float playerRestTimer;
    private float startRoundTimer;

    public enum State
    {
        StartingRound,
        WaitingForRoundToEnd,
        PlayerRestTime,
        CountDownToRoundStart,
    }

    public State state;

    private void Awake()
    {
        EnemyAI.OnEnemyDie += EnemyAI_OnEnemyDie;
        EnemyAI.OnEnemySpawn += EnemyAI_OnEnemySpawn;
    }

    private void EnemyAI_OnEnemySpawn(object sender, System.EventArgs e)
    {
        enimiesAlive++;
        Debug.Log("Enemies alive = " + enimiesAlive);
        allEnemiesDead = false;
    }

    private void EnemyAI_OnEnemyDie(object sender, System.EventArgs e)
    {
        enimiesAlive--;
        Debug.Log("An enemy just died. Enemies left = " + enimiesAlive);
        if (enimiesAlive == 0)
        {
            allEnemiesDead = true;
        }
    }

    private void Start()
    {
        //enimiesAlive = 0;
        //enemiesToSpawn = 0;
        //waveNumber = 1;
        //enemiesToSpawn += enemiesToSpawnMaxIncrement;
        //SpawnManager.Instance.SetAmountToSpawn(enemiesToSpawn);
        //isWaveInProgress = true;
        //allEnemiesDead = true;
        //state = State.WaitingForRoundToEnd;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        switch (state)
        {
            case State.StartingRound:
                waveNumber++;
                enemiesToSpawn += enemiesToSpawnMaxIncrement;
                SpawnManager.Instance.SetAmountToSpawn(enemiesToSpawn);
                isWaveInProgress = true;
                state = State.WaitingForRoundToEnd;
                break;
            case State.WaitingForRoundToEnd:
                if (allEnemiesDead && !SpawnManager.Instance.isSpawing)
                {
                    state = State.PlayerRestTime;
                    playerRestTimer = playerRestTimerMax;
                }
                break;
            case State.PlayerRestTime:
                playerRestTimer -= Time.deltaTime;
                if (playerRestTimer <= 0)
                {
                    state = State.CountDownToRoundStart;
                    startRoundTimer = startRoundTimerMax;
                }
                break;
            case State.CountDownToRoundStart:
                startRoundTimer -= Time.deltaTime;
                if (startRoundTimer <= 0)
                {
                    state = State.StartingRound;
                }
                break;
        }

        if (!isWaveInProgress)
        {

        }
    }
}
