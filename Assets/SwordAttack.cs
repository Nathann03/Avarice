using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 3;
    Vector2 rightAttackOffset;

    private void Start() {
        swordCollider = GetComponent<Collider2D>();
        rightAttackOffset = transform.position;
    }

    public void AttackRight() {
        print("Attacking right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        print("Attacking left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            // Deal damage to the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            // knockback enemy
            enemy.Knockback(transform.position);

            if(enemy != null) {
                enemy.Health -= damage;
            }
        }
    }
}
