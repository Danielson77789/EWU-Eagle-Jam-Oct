using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool isResting;
    public float restTime;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    int health = 100;

    private void Awake() {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) {
            Patroling();
        }
        if(playerInSightRange && !playerInAttackRange) {
            ChasePlayer();
        }
        if(playerInSightRange && playerInAttackRange) {
            AttackPlayer();
        }
    }
    private void Patroling() {
        if(!walkPointSet) {
            SearchWalkPoint();
        }

        if(walkPointSet) {
            agent.SetDestination(walkPoint);
        }
        transform.LookAt(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f) {
            if(!isResting) {
                walkPointSet = false;
                isResting = true;
                Invoke(nameof(ResetPatrol), restTime);
            }
        }
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }

    private void ChasePlayer() {
        if(!playerInAttackRange) {
            agent.SetDestination(player.position);
        }
        transform.LookAt(player);
    }

    private void AttackPlayer() {
        transform.LookAt(player);

        if(!alreadyAttacked) {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    private void ResetPatrol() {
        isResting = false;
    }

    public void TakeDamage(int damage) { 
        health-=damage;
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    // Used to draw spehere for attack and sight range
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, sightRange);
    // }

}
