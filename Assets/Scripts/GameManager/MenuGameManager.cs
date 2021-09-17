using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuGameManager : GameManager {
    public new static MenuGameManager Instance {
        get => (MenuGameManager) _instance;
    }

    [SerializeField] private TextMeshProUGUI goldText = null;

    public int Gold {
        get => GameData.TotalGold;
        set {
            GameData.TotalGold = value;
            if (goldText) {
                goldText.SetText(GameData.TotalGold.ToString());
            }
        }
    }

    protected override void Start() {
        base.Start();

        Gold = GameData.TotalGold;
    }
}
