using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRB2D;
    void Start()
    {
        enemyRB2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        enemyRB2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void FlipEnemySprite()
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRB2D.velocity.x)), 1f);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        FlipEnemySprite();
    }
}
