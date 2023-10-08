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
        Looting
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
            }
        } else if(playerState == PlayerState.Moving) {
            if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
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
                PlayerRotation();
                break;
            case PlayerState.Moving:
                PlayerRotation();
                MovePlayer();
                break;
            case PlayerState.Looting:
                Looting();
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
        if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    void Looting() {
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingBackwards", false);
        animator.SetBool("Looting", true);
    }
}
