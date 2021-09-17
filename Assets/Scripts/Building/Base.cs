using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Building {
    public static Base instance = null;

    public static Base Instance() {
        return instance;
    }

    protected override BuildSiteState ColliderBuildState(Collider2D collider2D) {
        return BuildSiteState.Free;
    }

    protected override void PauseBuildingBehaviour() {}

    protected override void ResumeBuildingBehaviour() {}

    // Start is called before the first frame update
    protected override void Start() {
        Built = true;
        instance = this;
    }

    protected override void Kill() {
        LevelGameManager.Instance.EndLevel(false);
        base.Kill();
    }
}
