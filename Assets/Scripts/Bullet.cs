using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player Owner { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float damage;

    private float lagTime;

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
        transform.position += transform.up * moveSpeed * Time.deltaTime;// * lagTime;
        //Vector3 transformVector3 = new Vector3(transform.up.x, transform.up.y, 0);
        //transform.position += transformVector3 * moveSpeed * Time.deltaTime;

        //Vector2 transformVector2 = new Vector3(transform.up.x, transform.up.y);
        //rigidbody2D.MovePosition(rigidbody2D.position + transformVector2 * lagTime * Time.fixedDeltaTime);
        //rigidbody2D.position += rigidbody2D.velocity * lagTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit " + collision.gameObject.name);
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }

    public void InitializeBullet(Player owner, Quaternion direction, float lag)
    {
        Owner = owner;

        transform.rotation = direction;

        lagTime = lag;
        //Debug.Log(lag);

        //rigidbody2D.velocity = direction * moveSpeed;
        //rigidbody2D.position += rigidbody2D.velocity * lag;
        //rigidbody.velocity = originalDirection * 200.0f;
        //rigidbody.position += rigidbody.velocity * lag;
    }

    public void InitializeBullet(Player owner, Quaternion direction)
    {
        Owner = owner;

        transform.rotation = direction;
    }
}
