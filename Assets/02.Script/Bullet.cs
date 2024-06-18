using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb2d;
    public Vector2 div;

    private Transform targetObject;
    public float disappearDistance = 500f; 

    void Start()
    {
        rb2d.velocity = div * speed;
        targetObject = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CheckDistanceAndDisappear();
    }

    void CheckDistanceAndDisappear()
    {
        if (targetObject != null)
        {
            float distance = Vector3.Distance(transform.position, targetObject.position);
            if (distance > disappearDistance)
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Destroy(gameObject);
    }
}
