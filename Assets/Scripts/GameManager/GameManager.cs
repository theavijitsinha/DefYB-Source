using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    protected static GameManager _instance = null;
    public static GameManager Instance {
        get => _instance;
    }

    public GameState CurrentGameState = GameState.Invalid;

    // Start is called before the first frame update
    protected virtual void Start() {
        _instance = this;
    }

    public void LoadLevel(LevelData levelData) {
        LevelDataManager.CurrentLevelData = levelData;
        LoadScene("Level");
    }

    public void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public bool IsCurrentGameState(GameState state) {
        return CurrentGameState.IsDescendantOf(state);
    }
}
