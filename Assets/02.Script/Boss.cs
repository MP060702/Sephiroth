using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float minX = -10f;
    public float maxX = 10f;
    public float speed = 2f;
    private Vector3 targetPosition;

    public float minTime = 2f;
    public float maxTime = 5f;

    private float timer;
    private float timeToNextAttack;
    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask  PlayerLayers;

    public Transform player; 
    public float attackDuration = 2.0f;
    public float moveSpeed = 5.0f;

    void Start()
    {
        ResetTimer();
        SetNewTargetPosition();
    }

    void Update()
    {
        MoveToTarget();

        timer += Time.deltaTime;

        if (timer >= timeToNextAttack)
        {
            PerformRandomAttack();
            ResetTimer();
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
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
        StartCoroutine(MoveAndAttack());
        Debug.Log("Performing Attack Pattern One");
    }
    IEnumerator MoveAndAttack()
    {
        
        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

       
        float startTime = Time.time;
        while (Time.time - startTime < attackDuration)
        {
            NealAttack();
            yield return null;  
        }

    }
    void NealAttack()
    {
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
