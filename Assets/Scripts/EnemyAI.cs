using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    public static event EventHandler OnEnemySpawn;
    public static event EventHandler OnEnemyDie;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxShield;
    private float currentHealth;
    private float currentShield;

    private PhotonView photonView;
    private float fireRateTimer;
    private PlayerController targetPlayer;

    private bool hasTarget;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        RoomManager.Instance.OnPlayerListUpdate += RoomManager_OnPlayerListUpdate;
    }

    private void RoomManager_OnPlayerListUpdate(object sender, System.EventArgs e)
    {
        if (targetPlayer == null)
        {
            // Player that died was target
            targetPlayer = RoomManager.Instance.playersList[UnityEngine.Random.Range(0, RoomManager.Instance.playersList.Count)];
            hasTarget = true;
        }
        else if (RoomManager.Instance.playersList.Count == 0)
        {
            // Player that died was last one alive/ or only one left
            targetPlayer = null;
            hasTarget = false;
        }
    }

    private void Start()
    {
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);
        //target = FindFirstObjectByType<PlayerController>().gameObject;
        if (RoomManager.Instance.playersList.Count == 0)
        {
            targetPlayer = null;
            hasTarget = false;
        }
        else
        {
            targetPlayer = RoomManager.Instance.playersList[UnityEngine.Random.Range(0, RoomManager.Instance.playersList.Count)];
            hasTarget = true;
        }
        fireRateTimer = fireRate;
        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    private void Update()
    {
        if (hasTarget)
        {
            if (fireRateTimer <= 0)
            {
                Fire();
                fireRateTimer = fireRate;
            }
            else
            {
                fireRateTimer -= Time.deltaTime;
            }
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = targetPlayer.transform.position - transform.position;           // Gets direction of mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;                                    // Gets angle changed
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);                                     // Store rotation in a variable
        //transform.rotation = rotation * Quaternion.Euler(0, 0, -90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation * Quaternion.Euler(0, 0, -90), Time.deltaTime * turnSpeed).normalized;
    }

    private void Fire()
    {
        //GameObject bullet = 
        Instantiate(bulletPrefab, bulletSpawnTransform.position, transform.rotation);
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentShield >= 0)
        {
            currentShield -= damageAmount;
        }
        else
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            OnEnemyDie?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            gameObject.SetActive(false);
            //GetComponent<Renderer>().enabled = false;
        }
    }
}
