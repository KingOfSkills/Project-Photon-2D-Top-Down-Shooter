using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            playerController.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Destroy(gameObject);
    }
}
