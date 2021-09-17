using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject {
    public string ID;
    public string Name;
    public GameObject Prefab;
}
