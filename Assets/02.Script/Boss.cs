using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
    Rigidbody2D Rigid2D;
    public float minX = -10f;
    public float maxX = 10f;
    public float speed = 2f;
    private Vector3 targetPosition;
    public Animator animator;

    public float minTime = 2f;
    public float maxTime = 5f;

    private float timer;
    private float timeToNextAttack;
    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask  PlayerLayers;

    public Transform player;
    public float moveSpeed = 5f; 
    public float attackDelay = 2f;
    public float fff;

    private bool isCharging = false;

    void Start()
    {
        ResetTimer();
        SetNewTargetPosition();
        Rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveToTarget();
        Flip();

        timer += Time.deltaTime;

        if (timer >= timeToNextAttack)
        {
            PerformRandomAttack();
            ResetTimer();
        }
    }
    void FixedUpdate()
    {
        if (isCharging)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveToTarget()
    {
        animator.SetBool("isMove", true);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
    public void Flip()
    {
        if (targetPosition.x != transform.position.x) 
        {
            transform.localScale = new Vector3(Mathf.Sign(targetPosition.x - transform.position.x), 1, 1);
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(minX, maxX);
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
    }

    void PerformRandomAttack()
    {
        int attackIndex = Random.Range(0, 3);

        switch (attackIndex)
        {
            case 0:
                AttackPatternOne();
                break;
            case 1:
                AttackPatternTwo();
                break;
            case 2:
                AttackPatternThree();
                break;
        }
    }
    void AttackPatternOne()
    {
        Debug.Log("Performing Attack Pattern One");
        StartCoroutine(ApproachAndAttack());
    }
    IEnumerator ApproachAndAttack()
    {
        isCharging = true;
        float startTime = Time.time;
        while (Time.time < startTime + attackDelay)
        {
            yield return null;
        }
        isCharging = false;
        AttackPlayer();
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(Mathf.Sign(player.position.x - transform.position.x), 1, 1);
    }

    void AttackPlayer()
    {
        animator.SetTrigger("isAttack");
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, PlayerLayers);

        foreach (Collider2D enemy in hitPlayers)
        {
            Debug.Log("We hit" + enemy.name);
        }
    }

    void AttackPatternTwo()
    {
        Debug.Log("Performing Attack Pattern Two");
    }

    void AttackPatternThree()
    {
        Debug.Log("Performing Attack Pattern Three");
    }
    void ResetTimer()
    {
        timer = 0;
        timeToNextAttack = Random.Range(minTime, maxTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

}
