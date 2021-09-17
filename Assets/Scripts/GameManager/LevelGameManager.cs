using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelGameManager : GameManager {
    public new static LevelGameManager Instance {
        get => (LevelGameManager) _instance;
    }

    [SerializeField] private GameObject gameRoot = null;
    [SerializeField] private GameObject gameUIRoot = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject gameOverMenu = null;
    [SerializeField] private TextMeshProUGUI goldText = null;

    private int totalEnemyCount = LevelDataManager.CurrentLevelData.EnemySpawnDataList.Count;
    private int enemyKillCount = 0;

    private bool _gamePaused = false;
    private int _gold;

    public GameObject GameRoot {
        get => gameRoot;
    }

    public bool GamePaused {
        get => _gamePaused;
        set {
            _gamePaused = value;
            PauseGameObjects(value);
            pauseMenu.SetActive(value);
        }
    }

    public int Gold {
        get => _gold;
        set {
            _gold = value;
            if (goldText) {
                goldText.SetText(_gold.ToString());
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        GamePaused = false;
        CurrentGameState = GameState.PlayMode;

        Gold = LevelDataManager.CurrentLevelData.StartingGold;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) && !GamePaused) {
            PauseGame();
        }
    }

    public void PauseGame() {
        GamePaused = true;
    }

    public void UnpauseGame() {
        GamePaused = false;
    }

    protected void PauseGameObjects(bool value) {
        GameRoot.SetActive(!value);
        gameUIRoot.SetActive(!value);
        Time.timeScale = value ? 0.0f : 1.0f;
    }

    public void AddGold(int amount) {
        Gold += amount;
    }

    public void SubtractGold(int amount) {
        Gold -= amount;
    }

    public void EndLevel(bool won) {
        if (won) {
            GameData.TotalGold += Gold;
            LoadScene("MainMenu");
        }
        else {
            PauseGameObjects(true);
            gameOverMenu.SetActive(true);
        }
    }

    public void IncrementEnemyKillCount() {
        enemyKillCount++;
        if (enemyKillCount < totalEnemyCount) {
            return;
        }
        EndLevel(true);
    }
}
