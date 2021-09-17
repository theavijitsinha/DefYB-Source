using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState {
    private GameState parent = null;
    public GameState(GameState parent = null) {
        this.parent = parent;
    }

    public static GameState Invalid = new GameState(null);
    public static GameState GameRunning = new GameState(null);
    public static GameState MainMenu = new GameState(GameRunning);
    public static GameState Level = new GameState(GameRunning);
    public static GameState PlayMode = new GameState(Level);
    public static GameState PlaceMode = new GameState(Level);
    public static GameState PauseMenu = new GameState(Level);

    public bool IsDescendantOf(GameState state) {
        GameState cur = this;
        while (cur != null) {
            if (cur == state) {
                return true;
            }
            cur = cur.parent;
        }
        return false;
    }
}
