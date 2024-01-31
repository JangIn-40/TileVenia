
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody2D noaRB2D;
    private Animator noaAnimator;
    private CapsuleCollider2D noaBodyCollider;
    private BoxCollider2D noaFeetCollider;
    private float playerShape = 1;
    private float gravityScaleAtStart;
    private bool isAlive = true;

    void Start()
    {
        noaRB2D = GetComponent<Rigidbody2D>();
        noaAnimator = GetComponent<Animator>();
        noaBodyCollider = GetComponent<CapsuleCollider2D>();
        noaFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = noaRB2D.gravityScale;
    }

    void Update()
    {   
        if(isAlive == false)
            return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }


    void OnMove(InputValue value)
    {
    if(isAlive == false)
        return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(isAlive == false)
            return;
        UnityEngine.Debug.Log("점프!");
        if(!noaFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;

        if(value.isPressed )
        {
            noaRB2D.velocity += new Vector2(0f , jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 PlayerVelocity = new Vector2(moveInput.x * baseSpeed, noaRB2D.velocity.y);
        noaRB2D.velocity = PlayerVelocity;
        bool isRunning = moveInput.x != 0 ? true: false; //삼항연산자 사용해봄
        noaAnimator.SetBool("isRunning", isRunning);
    }

    //Mathf함수를 사용하여도 되지만 직관성때문에 이렇게 설정함 
    //추가 EnemyMovement스크립트 사용결과 Mathf함수 사용하는 것이 더 좋은듯 
    void FlipSprite()
    {
        if(moveInput.x > 0)
            transform.localScale = new Vector2(playerShape, 1f);
        else if(moveInput.x < 0)
            transform.localScale = new Vector2(-playerShape, 1f);

    }

     void ClimbLadder()
    {
        if(!noaFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            noaRB2D.gravityScale = gravityScaleAtStart;
            return;
        }

        Vector2 climbVelocity = new Vector2(noaRB2D.velocity.x, moveInput.y * climbSpeed);
        noaRB2D.velocity = climbVelocity;
        noaRB2D.gravityScale = 0f;
        bool isClimbing = moveInput.y != 0 ? true: false;
        noaAnimator.SetBool("isClimbing", isClimbing);
    }

    void Die()
    {
        if(noaBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            noaAnimator.SetTrigger("Dying");
            isAlive = false;
        }
    }
}
