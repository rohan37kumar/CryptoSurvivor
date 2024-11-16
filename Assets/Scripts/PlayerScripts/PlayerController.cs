using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Animator animator;
    private Vector2 moveDirection;

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //stats = GetComponent<PlayerStats>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moveDirection = InputManager.Instance.SwipeInput;
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float currentSpeed = baseSpeed * (1 + stats.agilityModifier);
        rb.velocity = moveDirection.normalized * currentSpeed;
        //Debug.Log("Move Value: "+moveDirection);
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            if (moveDirection != Vector2.zero)
            {
                //move in animation here
                animator.SetBool("isMoving", true);
                //Debug.Log("animator values changed");
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
