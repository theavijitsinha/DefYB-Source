using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingData", menuName = "BuildingData")]
public class BuildingData : ScriptableObject {
    public string ID;
    public string Name;
    public int BuildCost;
    public GameObject Prefab;
    public GameObject BuilderPrefab;
    public Sprite MenuIcon;
}
