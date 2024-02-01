using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D rb2D;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2D.velocity = new Vector2 (moveSpeed, 0f);
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemySprite();

    }

    void FlipEnemySprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2D.velocity.x)), 1f);
    }
}
