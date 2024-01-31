
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
    private CapsuleCollider2D noaCollider2D;
    private float playerShape = 1;
    private float gravityScaleAtStart;

    void Start()
    {
        noaRB2D = GetComponent<Rigidbody2D>();
        noaAnimator = GetComponent<Animator>();
        noaCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = noaRB2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }


    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    
    }

    void OnJump(InputValue value)
    {
        UnityEngine.Debug.Log("점프!");
        if(!noaCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
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

    //Mathf함수를 사용하여 돼지만 직관성때문에 이렇게 설정함
    void FlipSprite()
    {
        if(moveInput.x > 0)
            transform.localScale = new Vector2(playerShape, 1f);
        else if(moveInput.x < 0)
            transform.localScale = new Vector2(-playerShape, 1f);

    }

     void ClimbLadder()
    {
        if(!noaCollider2D.IsTouchingLayers(LayerMask.GetMask("Climb")))
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
}
