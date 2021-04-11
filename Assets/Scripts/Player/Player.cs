﻿using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float slideForce;

    bool isGround = true;
    bool isWalking = false;
    bool isJump = false;
    public bool CanSlide { get; set; } = true;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        MouseEvent();
        if (!isWalking)
        {
            if (audioSource.clip.name.Equals("Steps"))
                audioSource.Stop();
            if (isJump)
            {
                JumpSound();
            }
        }
        if (transform.position.y < -15f)
        {
            GameManager.INSTANCE.GameOverScreen();
            return;
        }
    }
    void MouseEvent()
    {
        if (Input.GetMouseButton(0))
        {
            Invoke(GameManager.LeftButton, 0f);
        }
        if (Input.GetMouseButton(1))
        {
            Invoke(GameManager.RightButton, 0f);
        }
        if (!Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            isJump = false;
            isWalking = false;
            animator.SetFloat(StaticFields.VelocityX, 0f);
        }
    }
    void Left()
    {
        isJump = false;
        isWalking = true;
        WalkSound();
        spriteRenderer.flipX = true;
        animator.SetFloat(StaticFields.VelocityX, 1f);
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
    void Right()
    {
        isJump = false;
        isWalking = true;
        WalkSound();
        spriteRenderer.flipX = false;
        animator.SetFloat(StaticFields.VelocityX, 1f);
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    void Jump()
    {
        isJump = true;
        isWalking = false;
        if (CheckStatus())
        {
            JumpSound();
            animator.SetTrigger(StaticFields.Jump);
            isGround = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void JumpRight()
    {
        isJump = true;
        isWalking = false;
        if (CheckStatus())
        {
            JumpSound();
            spriteRenderer.flipX = false;
            animator.SetTrigger(StaticFields.Jump);
            isGround = false;
            rb.AddForce(Vector2.one * jumpForce, ForceMode2D.Impulse);
        }
    }
    void JumpLeft()
    {
        isJump = true;
        isWalking = false;
        if (CheckStatus())
        {
            JumpSound();
            spriteRenderer.flipX = true;
            animator.SetTrigger(StaticFields.Jump);
            isGround = false;
            rb.AddForce(new Vector2(-1, 1) * jumpForce, ForceMode2D.Impulse);
        }
    }
    void SlideRight()
    {
        isJump = false;
        isWalking = false;
        if (CheckStatus())
        {
            CanSlide = false;
            spriteRenderer.flipX = false;
            animator.SetTrigger(StaticFields.Slide);
            rb.AddForce(Vector2.right * slideForce, ForceMode2D.Impulse);
        }
    }
    void SlideLeft()
    {
        isJump = false;
        isWalking = false;
        if (CheckStatus())
        {
            CanSlide = false;
            spriteRenderer.flipX = true;
            animator.SetTrigger(StaticFields.Slide);
            rb.AddForce(Vector2.left * slideForce, ForceMode2D.Impulse);
        }
    }

    bool CheckStatus()
    {
        if (!CanSlide || !isGround)
        {
            return false;
        }
        return true;
    }

    void JumpSound()
    {
        int n = Random.Range(0, 3);
        AudioClip clip = Resources.Load<AudioClip>("Audio/Jump/" + n.ToString());
        audioSource.clip = clip;
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(clip);
    }
    void WalkSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Steps");
        audioSource.clip = clip;
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(clip);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Tags.Ground))
            isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Tags.Ground))
            isGround = false;
    }
}
