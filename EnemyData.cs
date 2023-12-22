using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public int cost;
    [Range(0f, 1f)] // Ensure the weight is between 0 and 1
    public float weight;
}
