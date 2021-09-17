using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject {
    public string ID;
    public string Name;
    public int StartingGold;
    public List<EnemySpawnData> EnemySpawnDataList;
}
