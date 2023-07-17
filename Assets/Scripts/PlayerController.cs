using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPunObservable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxShield;

    private float currentHealth;
    private float currentShield;
    private float fireRateTimer;

    //private Vector2 inputVector;
    private new Rigidbody2D rigidbody2D;
    private PhotonView photonView;
    private Camera playerCamera;
    private PlayerManager playerManager;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        //playerCamera = Camera.main;
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        playerCamera = playerManager.GetPlayerCamera();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            //GameObject playerCameraGameObject = PhotonNetwork.Instantiate(PhotonPrefabs.SpawnPrefab(PhotonPrefabs.Prefab.PlayerCamera), Vector3.zero, Quaternion.identity);
            //playerCamera = playerCameraGameObject.GetComponent<Camera>();
            //playerCamera.GetComponent<CameraPrefab>().SetTarget(photonView.transform);
        }
        else
        {
            //playerCamera = null;
            if (playerCamera.gameObject != null)
            {
                Destroy(playerCamera.gameObject);
            }
        }
        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        Move();
        LookAtMouse();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && fireRateTimer <= 0)
        {
            //Vector2 direction = new Vector2(transform.position.x, transform.position.y);
            
            photonView.RPC("FirePunRpc", RpcTarget.AllViaServer, bulletSpawnTransform.position, transform.rotation);
            fireRateTimer = fireRate;
            //PhotonNetwork.Instantiate(
            //    PhotonPrefabs.SpawnPrefab(PhotonPrefabs.Prefab.Bullet), 
            //    bulletSpawnTransform.position, 
            //    bulletSpawnTransform.rotation);
        }
        else if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
    }

    [PunRPC]
    private void FirePunRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, rotation);
    }

    private void Move()
    {
        Vector2 inputVector = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }

        Vector2 inputVectorNormalized = inputVector.normalized;
        Vector3 inputVectorNormalizedVector3 = new Vector3(inputVectorNormalized.x, inputVectorNormalized.y, 0);

        transform.position += inputVectorNormalizedVector3 * moveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //if (!photonView.IsMine) return;
        //MoveRigidbody();
    }

    private void MoveRigidbody()
    {
        Vector2 inputVector = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }

        Vector2 inputVectorNormalized = inputVector.normalized;
        rigidbody2D.MovePosition(rigidbody2D.position + inputVectorNormalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void LookAtMouse()
    {
        Vector3 direction = Input.mousePosition - playerCamera.WorldToScreenPoint(transform.position);           // Gets direction of mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;                                    // Gets angle changed
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);                                     // Store rotation in a variable
        transform.rotation = rotation * Quaternion.Euler(0, 0, -90);
        // Can use Quanternion.Slerp to have it slow look toward mouse position
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation * Quaternion.Euler(0, 0, -90), Time.deltaTime * turnSpeed).normalized;
    }

    public void SetCamera(GameObject cameraGameObject)
    {
        if (playerCamera == Camera.main)
        {
            playerCamera = cameraGameObject.GetComponent<Camera>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentShield > 0)
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
        //Transform deathTransform = transform;
        playerManager.Die();
        //playerCamera.GetComponent<CameraPrefab>().SetTarget(null);
        //Destroy(playerCameraGameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            //networkPosition += (GetComponent<Rigidbody>().velocity * lag);
        }
    }
}
