// UNUSED

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander_NoWeapon : MonoBehaviour
{
    CircleCollider2D circleCollider;

    [SerializeField]
    private float pursuitSpeed;

    [SerializeField]
    private float wanderSpeed;

    private float currentSpeed;

    [SerializeField]
    private float directionChangeInterval;

    [SerializeField]
    private bool followPlayer;

    private Coroutine moveCoroutine;
    private Rigidbody2D rb2d;
    private Animator animator;
    private Transform targetTransform;
    public Vector3 endPosition;
    private float currentAngle;

    private void Start()
    {
        endPosition = transform.position;
        currentSpeed = wanderSpeed;
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
    }

    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius * transform.localScale.x);
        }
    }

    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            circleCollider.radius *= 3;

            targetTransform = collision.gameObject.transform;
            endPosition = targetTransform.position;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            circleCollider.radius /= 3;
            animator.SetBool("isWalking", false);
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
    }

    private IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        var remainingDistance = (transform.position - endPosition).sqrMagnitude;
        
        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
                if (remainingDistance < 3 && !this.name.Equals("Dog"))
                {
                    animator.SetTrigger("Attack");
                } else if (remainingDistance >= 3)
                {
                    animator.ResetTrigger("Attack");
                }
            }
            
            if (rigidBodyToMove != null)
            {
                var newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                animator.SetFloat("xDir", newPosition.x - rigidBodyToMove.position.x);
                animator.SetFloat("yDir", newPosition.y - rigidBodyToMove.position.y);

                animator.SetBool("isWalking", true);

                rb2d.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isWalking", false);
    }

    private void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle) * transform.localScale.x;
    }

    private static Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        var inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }
}
