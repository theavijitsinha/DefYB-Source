using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building {
    protected override void PauseBuildingBehaviour() {}

    protected override void ResumeBuildingBehaviour() {}

    protected override BuildSiteState ColliderBuildState(Collider2D collider2D) {
        return BuildSiteState.OccupiedIrreplacable;
    }
}
