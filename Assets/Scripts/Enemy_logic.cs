using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0.5f;
    public float attackDistance = 1.5f;

    public float attackActiveTime = 0.5f;
    public float attackCooldownTime = 1.0f;

    private Collider attackCollider;
    private bool isNearTarget = false;
    private float attackTimer = 0f;
    private bool isAttackActive = false;

    void Start()
    {
        attackCollider = GetComponent<Collider>();

        if (attackCollider == null)
        {
            attackCollider = GetComponentInChildren<Collider>();
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        // No necesitamos añadir el EnemyIndicator aquí porque lo hace el spawner
    }

    void Update()
    {
        if (target == null || attackCollider == null) return;

        transform.LookAt(target);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackDistance)
        {
            if (!isNearTarget)
            {
                isNearTarget = true;
                attackTimer = 0f;
            }

            HandleAttackCycle();
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            if (isNearTarget)
            {
                isNearTarget = false;
                isAttackActive = false;
                attackCollider.enabled = false;
            }
        }
    }

    void HandleAttackCycle()
    {
        attackTimer += Time.deltaTime;

        if (isAttackActive)
        {
            if (attackTimer >= attackActiveTime)
            {
                attackCollider.enabled = false;
                isAttackActive = false;
                attackTimer = 0f;
            }
        }
        else
        {
            if (attackTimer >= attackCooldownTime)
            {
                attackCollider.enabled = true;
                isAttackActive = true;
                attackTimer = 0f;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform == target)
        {
            Debug.Log("¡Colisión con el jugador!");
        }
    }
}