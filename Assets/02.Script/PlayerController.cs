using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rigid2D;
    public SpriteRenderer SR;
    public Animator Animator;
    public float MoveSpeed;
    public float JumpPower;
    public bool isGround;
    public float RollPower;
    public float RollTime;

    private void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveInput();
        JumpInput();
        RollInput();
    }

    public void MoveInput()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX < 0) SR.flipX = true;
        else if (moveX > 0) SR.flipX = false;

        Vector3 movePos = new Vector3(moveX * MoveSpeed, Rigid2D.velocity.y, 0);
        Rigid2D.velocity = movePos;

        Animator.SetFloat("xVelocity", Mathf.Abs(Rigid2D.velocity.x));
        Animator.SetFloat("yVelocity", Rigid2D.velocity.y);
    }

    public void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, JumpPower);;
            Animator.SetBool("isJump", isGround);
            isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            Animator.SetBool("isJump", false);
        }
    }

    public void RollInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        float gravity = Rigid2D.gravityScale;
        Rigid2D.gravityScale = 0f;
        Rigid2D.velocity = new Vector2(transform.localScale.x * RollPower, 0f);
        yield return new WaitForSeconds(RollTime);
        Rigid2D.gravityScale = gravity;
    }
   
}