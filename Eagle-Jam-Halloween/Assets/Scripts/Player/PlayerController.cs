using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 55;
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float backwardsMoveSpeed = 5f;
    // private Rigidbody rigidbody;
    private Animator animator;

    public enum PlayerState 
    {
        Idle,
        Moving, 
        Looting,
        Attacking
    }

    private PlayerState playerState = PlayerState.Idle;

    private void Start() {
        // rigidbody = GetComponent<Rigidbody>();    
        animator = GetComponent<Animator>();
    }

    private void Update() {
        HandleInput();
        HandleState();
    }

    void HandleInput() {

        if(playerState == PlayerState.Idle) {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
                playerState = PlayerState.Moving;
            } else if(Input.GetMouseButton(0)) {
                playerState = PlayerState.Attacking;
            }
        } else if(playerState == PlayerState.Moving) {
            if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
                playerState = PlayerState.Idle;
            }
        } else if(playerState == PlayerState.Attacking) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5 && !animator.IsInTransition(0)) {
                playerState = PlayerState.Idle;
            }
        }

    }

    public void ChangeState(PlayerState newState) {
        if(newState == PlayerState.Moving) {
            playerState = PlayerState.Moving;
        } else if(newState == PlayerState.Idle) {
            playerState = PlayerState.Idle;
        } else if(newState == PlayerState.Looting) {
            playerState = PlayerState.Looting;
        }
    }

    void HandleState() 
    {
        switch(playerState) 
        {
            case PlayerState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isWalkingBackwards", false);
                animator.SetBool("Looting", false);
                animator.SetBool("isAttacking", false);
                PlayerRotation();
                break;
            case PlayerState.Moving:
                PlayerRotation();
                MovePlayer();
                break;
            case PlayerState.Looting:
                Looting();
                break;
            case PlayerState.Attacking:
                BasicAttack();
                break;
        }
    }

    void MovePlayer() {
        if(Input.GetKeyUp(KeyCode.W)) {
            // playerAnim.SetBool("isWalking", false);
        }
        if(Input.GetKeyUp(KeyCode.S)) {
            // playerAnim.SetBool("isWalkingBackwords", false);
        }

        if(Input.GetKey(KeyCode.W)) {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingBackwards", false);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            // playerAnim.SetBool("isWalking", true);
        } else if(Input.GetKey(KeyCode.S)) {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingBackwards", true);
            transform.Translate(Vector3.back * backwardsMoveSpeed * Time.deltaTime);
            // playerAnim.SetBool("isWalkingBackwords", true);
        }
    }

    void PlayerRotation() {
        // if(Input.GetKey(KeyCode.A)) {
        //     transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        // }
        // if(Input.GetKey(KeyCode.D)) {
        //     transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        // }
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        var rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(rayCast, out var enter)) {
            var lookPoint = rayCast.GetPoint(enter);
            lookPoint.y = transform.position.y;
            // Just here for debug purposes
            // Debug.DrawLine(rayCast.origin, lookPoint, Color.red);
            // Debug.DrawRay(lookPoint, Vector3.up, Color.green);
            transform.LookAt(lookPoint);
        }
    }

    void Looting() {
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingBackwards", false);
        animator.SetBool("Looting", true);
    }

    private void BasicAttack() {
        animator.SetBool("isAttacking", true);
    }

    private void OnTriggerEnter(Collider other) {
        if(playerState == PlayerState.Attacking) {
            if(other.tag == "Enemy") {
                Debug.Log("Enemy");
                other.GetComponent<EnemyController>().TakeDamage(10);
            }
        }
    }
}
