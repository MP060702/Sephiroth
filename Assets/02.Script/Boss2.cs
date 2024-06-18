using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public float minX = -10f; // 최소 x 위치
    public float maxX = 10f;  // 최대 x 위치
    public float speed = 2f;  // 이동 속도
    private Vector3 targetPosition; // 목표 위치

    void Start()
    {
        // 초기 목표 위치 설정
        SetNewTargetPosition();
    }

    void Update()
    {
        // 몬스터를 목표 위치로 이동
        MoveToTarget();
    }

    void MoveToTarget()
    {
        // 현재 위치를 목표 위치 방향으로 선형 보간
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 새로운 목표 위치 설정
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        // 랜덤한 x 값 설정
        float randomX = Random.Range(minX, maxX);
        // y와 z는 현재 위치 유지
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
    }
}
