using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGrave : MonoBehaviour
{
    [SerializeField]
    private LootableObject lootableObject;
    private IEnumerator OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")) {
            if(Input.GetKey(KeyCode.E)) {
                PlayerController playerController = other.GetComponent<PlayerController>();
                playerController.ChangeState(PlayerController.PlayerState.Looting);

                yield return new WaitForSeconds(lootableObject.WaitForSeconds);

                MeshFilter meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = lootableObject.lootedMesh;
                playerController.ChangeState(PlayerController.PlayerState.Idle);
            }
        }
    }
}
