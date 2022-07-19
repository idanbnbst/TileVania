using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PlayerInput playerInput;
    Vector2 moveInput;
    bool isAlive = true;
    [Header("Forces")]
    [SerializeField] float baseGravityScale;
    float moveSpeed;
    [SerializeField] float defaultMoveSpeed = 5f;
    [SerializeField] float BoostedMoveSpeed = 7f;
    [SerializeField] float jumpForce = 25f;
    [SerializeField] float boostedJumpForce = 27.5f;
    [SerializeField] int scoreForBoost = 1500;
    [SerializeField] Color boostedColor;

    [Header("Equipment")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunTransform;

    [Header("Player Death Options")]
    [SerializeField] Vector2 deathKick = new Vector2(0f, 30f);
    [SerializeField] Color deathColor;

    SpriteRenderer playerSprite;
    Rigidbody2D playerRB2D;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;

    void Start()
    {
        // Should it be in Awake() function?
        //playerInput = GetComponent<PlayerInput>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRB2D = GetComponent<Rigidbody2D>();
        baseGravityScale = playerRB2D.gravityScale;
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        gunTransform = GetComponent<Transform>();
    }
    void Update()
    {
        if (!isAlive)
            return;
        Move();
        FlipPlayerSpite();
        ClimbLadder();
        Die();
    }

    // Function OnMove() is implemented from Player Input component
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Function OnJump() is implemented from Player Input component
    void OnJump(InputValue value)
    {
        if (!isAlive)
            return;

        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
            return;

        if (value.isPressed)
        {
            // We need to get out of climbing collision for jump from a ladder
            int score = FindObjectOfType<GameSession>().GetScore();
            if (score < scoreForBoost)
                playerRB2D.velocity += new Vector2(0f, jumpForce);
            else
            {
                playerRB2D.velocity += new Vector2(0f, boostedJumpForce);
                playerSprite.color = boostedColor;
            }
        }
    }

    // Function OnFire() is implemented from Player Input component
    void OnFire(InputValue value)
    {
        int magazineRounds = FindObjectOfType<GameSession>().GetMagazineRounds();
        if (!isAlive || magazineRounds == 0)
            return;

        if (value.isPressed)
        {
            Instantiate(bullet, gunTransform.position, transform.rotation);
            FindObjectOfType<GameSession>().DecreaseMagazineRounds();
        }
    }
    void Move()
    {
        if (!isAlive)
            return;

        int score = FindObjectOfType<GameSession>().GetScore();
        if (score > scoreForBoost)
            moveSpeed = BoostedMoveSpeed;
        else
            moveSpeed = defaultMoveSpeed;

        // Make player move horizontally
        Vector2 horizontalVelocity = new Vector2(moveInput.x * moveSpeed, playerRB2D.velocity.y);
        playerRB2D.velocity = horizontalVelocity;

        // Set 'Running' animation while left/right key is pressed
        bool playerIsMoving = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerIsMoving);
        playerAnimator.SetBool("isClimbing", false);
    }
    void FlipPlayerSpite()
    {
        bool playerIsMoving = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;
        if (playerIsMoving)
            transform.localScale = new Vector2(Mathf.Sign(playerRB2D.velocity.x), 1f);
    }
    void ClimbLadder()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRB2D.gravityScale = baseGravityScale;
            return;
        }
        // Make player move vertically
        playerRB2D.gravityScale = 0f; // Set zero gravity for not sliding down the ladder
        Vector2 verticalVelocity = new Vector2(playerRB2D.velocity.x, moveInput.y * moveSpeed);
        playerRB2D.velocity = verticalVelocity;

        // Set 'Climbing' animation while up/down key is pressed
        bool playerIsClimbing = Mathf.Abs(playerRB2D.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerIsClimbing);
    }
    void Die()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
            return;

        isAlive = false;
        //We can deactivate controls through playerInput component instead of bool isAlive
        //playerInput.DeactivateInput();
        playerAnimator.SetTrigger("Dying");
        playerSprite.color = deathColor;
        playerRB2D.velocity = deathKick;

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
            Destroy(other.gameObject);
    }
}