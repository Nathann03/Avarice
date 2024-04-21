using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    Animator animator;

    public float Health {
        set {
            health = value;

            if(value > 0) {
                animator.SetTrigger("Hit");
                ShuffleWaypoints(); // Shuffle waypoints when hit
            } 

            if(health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    public float health = 1;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Defeated(){
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }

    // Knockback the enemy 2d smoothly
    public float knockbackDistance = 1f;
    public float knockbackDuration = 0.5f;

    public void Knockback(Vector2 sourcePosition)
    {
        Vector2 knockbackDirection = ((Vector2)transform.position - sourcePosition).normalized;
        StartCoroutine(KnockbackCoroutine(knockbackDirection));

        // Run away from the source position
        Vector2 runAwayDirection = (sourcePosition - (Vector2)transform.position).normalized;
        StartCoroutine(RunAwayCoroutine(runAwayDirection));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDirection)
    {
        float timer = 0f;
        Vector3 originalPosition = transform.position;

        while (timer < knockbackDuration)
        {
            float knockbackMagnitude = Mathf.Lerp(0, knockbackDistance, timer / knockbackDuration);
            Vector3 knockbackVector = new Vector3(knockbackDirection.x, knockbackDirection.y, 0) * knockbackMagnitude;
            transform.Translate(knockbackVector * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition + new Vector3(knockbackDirection.x, knockbackDirection.y, 0) * knockbackDistance * 0.5f;
    }

    private IEnumerator RunAwayCoroutine(Vector2 runAwayDirection)
    {
        float runAwayDuration = 2f; // Adjust as needed
        float timer = 0f;

        while (timer < runAwayDuration)
        {
            transform.Translate(runAwayDirection * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Waypoint Navigation
    public Transform[] waypoints;
    public float moveSpeed = 1f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length > 0)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Reached current waypoint, move to next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private void ShuffleWaypoints()
    {
        // Fisher-Yates shuffle algorithm
        for (int i = waypoints.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = waypoints[i];
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }
}
