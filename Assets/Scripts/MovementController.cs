using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 3.0f;

    public Vector2 CurrentDirection { get; set; }
    private Vector2 previousDirection = Vector2.zero;

    public void Start()
    {
        //cs = GetComponent<CharacterStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (CurrentDirection != Vector2.zero)
        {
            Move(CurrentDirection);
        }

        if (CurrentDirection != previousDirection)
        {
            previousDirection = CurrentDirection;
        }
    }

    public virtual void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction.normalized * (Time.fixedDeltaTime * moveSpeed));
    }
}

public class DirectionChangeEvent
{
    public Vector2 Direction { get; private set; }
    public DirectionChangeEvent(Vector2 direction)
    {
        Direction = direction;
    }
}