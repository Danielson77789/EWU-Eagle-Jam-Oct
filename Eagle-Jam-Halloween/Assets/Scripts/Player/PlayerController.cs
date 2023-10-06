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
    private Rigidbody rigidbody;

    private enum PlayerState 
    {
        Idle,
        Moving
    }

    private PlayerState playerState = PlayerState.Idle;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();    
    }

    private void Update() {
        // MovePlayer();
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

    void HandleState() 
    {
        switch(playerState) 
        {
            case PlayerState.Idle:
                PlayerRotation();
                break;
            case PlayerState.Moving:
                PlayerRotation();
                MovePlayer();
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
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            // playerAnim.SetBool("isWalking", true);
        } else if(Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
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
}
