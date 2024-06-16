using System;
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
    public Vector3 movePos;
    public float RollSpeed;
    private float defaultSpeed;
    public bool isRoll;
    public float RollTime;
    public float DefaultTime;
    public float cooldownTime;

    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask EnemyLayers;



    private void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        defaultSpeed = MoveSpeed; // 초기 기본 속도 설정
    }

    private void Update()
    {
        MoveInput();
        JumpInput();
        Flip();
        RollInput();
    }

    private void FixedUpdate()
    {
        Roll();
    }

    public void MoveInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        movePos = new Vector3(moveX * defaultSpeed, Rigid2D.velocity.y, 0);
        Rigid2D.velocity = movePos;

        Animator.SetFloat("xVelocity", Mathf.Abs(Rigid2D.velocity.x));
        Animator.SetFloat("yVelocity", Rigid2D.velocity.y);
    }

    public void Flip()
    {
        Vector2 localScale = transform.localScale;
        localScale.x = movePos.x > 0f ? 1f : movePos.x < 0f ? -1f : localScale.x;
        transform.localScale = localScale;
    }

    public void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, JumpPower);
            Animator.SetBool("isJump", isGround);
            isGround = false;
        }
    }

    public void RollInput()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
        if (cooldownTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.C) && isGround && !isRoll)
            {   
                isRoll = true;
                Animator.SetTrigger("Roll");
                RollTime = DefaultTime;
                defaultSpeed = RollSpeed; // 롤 속도 설정
                cooldownTime = 3;
            }
        }
    }

    public void Roll()
    {
        if (isRoll && RollTime > 0f)
        {
            RollTime -= Time.deltaTime;
        }
        else if (isRoll)
        {
            isRoll = false;
            defaultSpeed = MoveSpeed; // 롤이 끝나면 기본 속도로 복원
        }
    }

    public void AttackInput() {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Animator.SetTrigger("Attack");
            Collider2D[] hitEnimes = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers); 

            foreach(Collider2D enemy in hitEnimes)
            {
                Debug.Log("We hit" + enemy.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            Animator.SetBool("isJump", false);
        }
    }
}