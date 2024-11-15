using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    
    private Rigidbody2D rb;
    private PlayerStats stats;
    private Vector2 moveDirection;
    private Animator animator;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        moveDirection = InputManager.Instance.MovementInput;
        //UpdateAnimation();
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        float currentSpeed = baseSpeed * (1 + stats.agilityModifier);
        rb.velocity = moveDirection.normalized * currentSpeed;
    }
    
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
            if (moveDirection != Vector2.zero)
            {
                animator.SetFloat("Horizontal", moveDirection.x);
                animator.SetFloat("Vertical", moveDirection.y);
            }
        }
    }
}
