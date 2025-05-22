using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float obstacleCheckDistance = 0.6f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private int jumpCount = 0;
    private int maxJumpCount = 2;

    private bool isGrounded;
    private bool isJumping;

    private Rigidbody rb;
    private Vector2 inputVector;
    private Vector3 moveDirection;

    private bool isWalking;
    private bool isRunning;

    public bool IsWalking() => isWalking;
    public bool IsRunning() => isRunning;
    public bool IsJumping() => isJumping;
    public Rigidbody GetRigidbody() => rb;
    public float jumpForce => _jumpForce;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        inputVector = gameInput.GetMovementVectorNormalized();
        moveDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
        if (isGrounded) {
            Debug.Log("Yere basıyorum!");
        }

        if (moveDirection != Vector3.zero && !IsObstacleAhead()) {
            RotateTowardsMoveDirection();
            isWalking = true;
        }
        else {
            isWalking = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero) {
            moveSpeed = 10f;
            isRunning = true;
            isWalking = false;
        }
        else {
            moveSpeed = 5f;
            isRunning = false;
        }


        if (isGrounded) {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount) {
            jumpCount++;
            isJumping = true;
        }

        else {
            isJumping = false;
 
        }
    }





    private void FixedUpdate() {
        if (moveDirection != Vector3.zero && !IsObstacleAhead()) {
            Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }


    private bool IsObstacleAhead() {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        bool hit = Physics.Raycast(rayOrigin, moveDirection, obstacleCheckDistance, obstacleLayer);

        if (hit) {
            Debug.DrawRay(rayOrigin, moveDirection * obstacleCheckDistance, Color.red, 1f);
            //Debug.Log("Engel var!");
        }
        else {
            Debug.DrawRay(rayOrigin, moveDirection * obstacleCheckDistance, Color.green, 1f);
            // Debug.Log("Engel yok!");
        }

        return hit;
    }



    private void RotateTowardsMoveDirection() {
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (moveDirection != Vector3.zero) {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Gizmos.DrawLine(origin, origin + moveDirection * obstacleCheckDistance);
        }
    }
}
