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
    public float defaultSpeed;
    public bool isRoll;
    public float RollTime;
    public float DefaultTime;
    public float cooldownTime;
    public float RunSpeed;

    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask EnemyLayers;

    public float attackCooldown = 0f;
    public float attackCooldownTime = 1f;
    public bool isAttacking = false;

    public Transform ShotPoint;
    public GameObject Bullet;
    public float shootCooldown = 0f;
    public float shootCooldownTime = 1f;
    public bool isShooting = false;

    private void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        defaultSpeed = MoveSpeed;
    }

    private void Update()
    {
        MoveInput();
        JumpInput();
        Flip();
        RollInput();
        GunShot();
        AttackInput();
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        Roll();
    }

    public void MoveInput()
    {   

        if (!isAttacking && !isRoll && !isShooting)
        {
            float moveX = Input.GetAxis("Horizontal");
            movePos = new Vector3(moveX * defaultSpeed, Rigid2D.velocity.y, 0);
            Rigid2D.velocity = movePos;
            if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveX) > 0.3f)
            {   
                defaultSpeed = RunSpeed;
            }
            else
            {
                defaultSpeed = MoveSpeed;
            }
            Animator.SetFloat("xVelocity", Mathf.Abs(Rigid2D.velocity.x));
            Animator.SetFloat("yVelocity", Rigid2D.velocity.y);
        }
        else if (isRoll)
        {
            float moveX = Input.GetAxis("Horizontal");
            movePos = new Vector3(moveX * RollSpeed, Rigid2D.velocity.y, 0);
            Rigid2D.velocity = movePos;
            Animator.SetBool("isRunning", false);
            Animator.SetFloat("xVelocity", Mathf.Abs(Rigid2D.velocity.x));
            Animator.SetFloat("yVelocity", Rigid2D.velocity.y);
        }
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
                defaultSpeed = RollSpeed;
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
            defaultSpeed = MoveSpeed; 
        }
    }

    public void AttackInput() {
        if (Input.GetKeyDown(KeyCode.F) && isGround && attackCooldown <= 0)
        {
            isAttacking = true;
            Animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit" + enemy.name);
            }

            attackCooldown = attackCooldownTime;

            StartCoroutine(ResetAttackState());
        }
    }

    public void GunShot()
    {
        if (Input.GetKeyDown(KeyCode.G) && isGround && shootCooldown <= 0)
        {   
            Animator.SetTrigger("Shoot");
            isShooting = true;
            Bullet bullet =Instantiate(Bullet, ShotPoint.position, ShotPoint.rotation).GetComponent<Bullet>();
            bullet.div.x = transform.localScale.x ;

            shootCooldown = shootCooldownTime;

            StartCoroutine(ResetShootState());
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    IEnumerator ResetShootState()
    {
        yield return new WaitForSeconds(0.75f);
        isShooting = false;
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