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
    public float JumpPower;

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
    }

    public void MoveInput()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX < 0) SR.flipX = true;
        else if (moveX > 0) SR.flipX = false;
        
        Vector3 movePos = new Vector3(moveX, 0, 0);
        Rigid2D.velocity = movePos * MoveSpeed;

        Animator.SetFloat("xVelocity", Mathf.Abs(Rigid2D.velocity.x));
        Animator.SetFloat("yVelocity", Rigid2D.velocity.y);
    }
    
    public void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, JumpPower);
            Animator.SetBool("isJump", true);
        }
    }

    public void RollInput()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            
        }
    }
}
