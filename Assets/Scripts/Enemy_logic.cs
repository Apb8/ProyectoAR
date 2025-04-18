using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0.5f;
    public float attackDistance = 1.5f;

    public float attackActiveTime = 0.5f;
    public float attackCooldownTime = 1.0f;
    public int damageAmount = 10;

    public HealthController playerHealthController;
    public string healthControllerTag = "Player";

    private Collider attackCollider;
    private bool isNearTarget = false;
    private float attackTimer = 0f;
    private bool isAttackActive = false;

    private Animator animator;

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
        animator = GetComponent<Animator>();
        if (playerHealthController == null)
        {
            GameObject healthObject = GameObject.FindGameObjectWithTag(healthControllerTag);
            if (healthObject != null)
            {
                playerHealthController = healthObject.GetComponent<HealthController>();
            }
            if (playerHealthController == null)
            {
                playerHealthController = FindObjectOfType<HealthController>();
            }
            if (playerHealthController == null)
            {
                Debug.LogWarning("no se encontro healthcontroller");
            }
        }
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
                animator.SetTrigger("Attack");
                isNearTarget = true;
                attackTimer = 0f;
                ApplyDamageToPlayer();
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
                animator.SetTrigger("Attack");
                ApplyDamageToPlayer();
            }
        }
    }

    void ApplyDamageToPlayer()
    {
        if (playerHealthController != null && isNearTarget)
        {
            playerHealthController.TakeDamage(damageAmount);
            Debug.Log("-" + damageAmount + " de vida");
        }
    }
}