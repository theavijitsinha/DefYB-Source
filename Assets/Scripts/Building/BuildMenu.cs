using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour {
    public List<BuildingData> buildingDataList = new List<BuildingData>();
    public GameObject buildMenuButtonPrefab = null;
    public float padding = 0.0f;

    // Start is called before the first frame update
    void Start() {
        if (buildMenuButtonPrefab == null) {
            Debug.LogError("BuildMenu.Start: buildMenuButtonPrefab not set");
        }

        for (int buttonIndex = 0; buttonIndex < buildingDataList.Count; buttonIndex++) {
            BuildingData buildingData = buildingDataList[buttonIndex];
            GameObject newButton = Instantiate(buildMenuButtonPrefab, transform);
            RectTransform rect = newButton.GetComponent<RectTransform>();
            if (rect == null) {
                Debug.LogError("BuildMenu.Start: newButton does not contain RectTransform component");
            }
            rect.localPosition += Vector3.right * (buttonIndex * (padding + rect.sizeDelta.x));
            BuildButton buildButton = newButton.GetComponent<BuildButton>();
            if (buildButton == null) {
                Debug.LogError("BuildMenu.Start: newButton does not contain ButtonIcon component");
            }
            buildButton.SetIconSprite(buildingData.MenuIcon);
            buildButton.buildingData = buildingData;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
