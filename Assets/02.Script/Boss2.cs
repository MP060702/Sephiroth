using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public float minX = -10f; // �ּ� x ��ġ
    public float maxX = 10f;  // �ִ� x ��ġ
    public float speed = 2f;  // �̵� �ӵ�
    private Vector3 targetPosition; // ��ǥ ��ġ

    void Start()
    {
        // �ʱ� ��ǥ ��ġ ����
        SetNewTargetPosition();
    }

    void Update()
    {
        // ���͸� ��ǥ ��ġ�� �̵�
        MoveToTarget();
    }

    void MoveToTarget()
    {
        // ���� ��ġ�� ��ǥ ��ġ �������� ���� ����
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ��ǥ ��ġ�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ���ο� ��ǥ ��ġ ����
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        // ������ x �� ����
        float randomX = Random.Range(minX, maxX);
        // y�� z�� ���� ��ġ ����
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
    }
}
