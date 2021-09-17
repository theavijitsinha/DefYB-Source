using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Slider healthSlider = null;

    private int maxHealth = 0;
    private int minHealth = 0;
    private int currentHealth = 0;

    public int CurrentHealth {
        get => currentHealth;
        set {
            currentHealth = Mathf.Clamp(value, minHealth, maxHealth);
            healthSlider.value = currentHealth;
        }
    }

    private void Start() {
        Canvas canvas = GetComponent<Canvas>();
        if (!canvas) {
            Debug.LogError("Health.Start: gameObject does not have a Canvas component");
            return;
        }
        canvas.worldCamera = Camera.main;
    }

    public void Initialize(GameObject obj, int maxHealth) {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (!rectTransform) {
            Debug.LogError("Health.Initialize: gameObject does not have a RectTransform component");
            return;
        }
        SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (!objSpriteRenderer) {
            Debug.LogError("Health.Initiliaze: obj does not have a SpriteRenderer component");
            return;
        }
        rectTransform.sizeDelta = objSpriteRenderer.size;
        RectTransform sliderRectTransform = healthSlider.GetComponent<RectTransform>();
        if (!sliderRectTransform) {
            Debug.LogError("Health.Initialize: healthSlider does not have a RectTransform " +
                "component");
            return;
        }
        sliderRectTransform.offsetMax = new Vector2(sliderRectTransform.offsetMax.x,
            objSpriteRenderer.size.y * -0.9f);

        this.maxHealth = maxHealth;
        this.minHealth = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;

        CurrentHealth = maxHealth;
    }

    public void AddHealth(int amount) {
        CurrentHealth += amount;
    }

    public void SubtractHealth(int amount) {
        CurrentHealth -= amount;
    }
}
