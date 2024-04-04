using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


private void FixedUpdate()
{
    if (canMove) {
  
        if (movementInput != Vector2.zero)
        {
            TryMove(movementInput * moveSpeed * Time.fixedDeltaTime);

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}

private void TryMove(Vector2 movementVector)
{
    // Perform a cast without the collisionOffset to detect collisions ahead
    RaycastHit2D hit = Physics2D.Raycast(rb.position + movementVector.normalized * collisionOffset, movementVector.normalized, movementVector.magnitude - collisionOffset, movementFilter.layerMask);

    if (hit.collider == null)
    {
        // No collision detected, move without collision
        rb.MovePosition(rb.position + movementVector);
        
    }
    else
    {
        // Adjust movement vector to stop at the collision point
        Vector2 adjustedMovement = movementVector.normalized * (hit.distance - collisionOffset);
        rb.MovePosition(rb.position + adjustedMovement);
    }
}


private void SlideAlongSurface(Vector2 surfaceNormal, Vector2 movementVector)
{
    // Calculate sliding direction
    Vector2 slideDirection = Vector2.zero;

    if (Mathf.Abs(surfaceNormal.x) > Mathf.Abs(surfaceNormal.y))
    {
        slideDirection = new Vector2(surfaceNormal.x, 0f).normalized;
    }
    else
    {
        slideDirection = new Vector2(0f, surfaceNormal.y).normalized;
    }

    // Check if the movement vector is colliding with the surface normal within a tolerance
    if (Vector2.Dot(movementVector.normalized, slideDirection) < 0.1f)
    {
        // Move without sliding if the angle between movement vector and surface normal is small
        rb.MovePosition(rb.position + movementVector);
    }
    else
    {
        // Calculate sliding movement
        Vector2 slideMovement = slideDirection * moveSpeed * Time.fixedDeltaTime;

        // Move with sliding
        rb.MovePosition(rb.position + slideMovement);
    }
}



void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
        
    }

void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

public void SwordAttack() {
    LockMovement();

    if(spriteRenderer.flipX == true){
        swordAttack.AttackLeft();
    } else {
        swordAttack.AttackRight();
    }
}

public void EndSwordAttack() {
    UnlockMovement();
    swordAttack.StopAttack();
}

public void LockMovement() {
    canMove = false;
}

public void UnlockMovement() {
    canMove = true;
}




}