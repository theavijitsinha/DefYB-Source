using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOverlay : MonoBehaviour {
    [SerializeField] private Color overlappingColor = Color.white;
    [SerializeField] private Color nonOverlappingColor = Color.white;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize(Building building) {
        transform.localPosition = Vector3.zero;
        SpriteRenderer buildingSprite = building.gameObject.GetComponent<SpriteRenderer>();
        if (!buildingSprite) {
            Debug.LogError("Building.Built.set: building.gameObject does not have a " +
                "SpriteRenderer component");
            return;
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer) {
            Debug.LogError("BuildOverlay.InitializeSprite: gameObject does not have a " +
                "SpriteRenderer component");
            return;
        }
        spriteRenderer.size = buildingSprite.size;
    }

    public void UpdateSprite(bool overlapping) {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer) {
            Debug.LogError("BuildOverlay.UpdateSprite: gameObject does not have " +
                "SpriteRenderer component");
            return;
        }
        spriteRenderer.color = overlapping ? overlappingColor : nonOverlappingColor;
    }
}
