using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/LootableData")]
public class LootableObject : ScriptableObject
{
    
    public int value = 10;
    public int WaitForSeconds = 10;
    public bool lootable = true;
    public Mesh lootedMesh;

}
