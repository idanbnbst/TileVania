using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float bulletDestroyTime = 1f;
    [SerializeField] int bulletHitPoints = 150;
    float xSpeed;
    Rigidbody2D bulletRB2D;
    PlayerController player;
    void Start()
    {
        bulletRB2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        transform.localScale = new Vector2(Mathf.Sign(xSpeed), transform.localScale.y);
    }
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        bulletRB2D.velocity = new Vector2(xSpeed, 0f);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            DestroyBullet();
            FindObjectOfType<GameSession>().SetScore(bulletHitPoints);
        }
        else
            Invoke("DestroyBullet", bulletDestroyTime);
    }
}
