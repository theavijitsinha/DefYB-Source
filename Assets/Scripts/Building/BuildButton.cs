using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour {
    public UnityEngine.UI.Image icon = null;
    public BuildingData buildingData = null;

    // Start is called before the first frame update
    void Start() {
        if (icon == null) {
            Debug.LogError("BuildButton.Start: icon not set");
        }
    }

    public void SetIconSprite(Sprite sprite) {
        icon.sprite = sprite;
    }

    public void Click() {
        if (!GameManager.Instance.IsCurrentGameState(GameState.PlayMode) &&
            !GameManager.Instance.IsCurrentGameState(GameState.PlaceMode)) {
            return;
        }
        GameObject builderObject = Instantiate(buildingData.BuilderPrefab);
        Builder builder = builderObject.GetComponent<Builder>();
        if (builder == null) {
            Debug.LogError("BuildButton.Click: builderObject does not have Builder component");
            return;
        }
        builder.InitiateBuilder(buildingData);
    }
}
