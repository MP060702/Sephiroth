using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rigid2D;
    public SpriteRenderer SR;
    public Animator Animator;
    public float MoveSpeed;
    public float v;

    private void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveInput();

    }

    public void MoveInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX < 0) SR.flipX = true;
        else if (moveX > 0) SR.flipX = false;

        Vector3 movePos = new Vector3(moveX, moveY, 0);
        Rigid2D.velocity = movePos * MoveSpeed;

        v = Rigid2D.velocity.magnitude;
        Animator.SetFloat("Velocity", v);
    }
}
