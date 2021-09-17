using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBuilder : Builder {
    protected override void BuildAbort() {
        buildInitiated = false;
    }

    protected override void BuildCommit() {
        CompleteBuild();
    }

    protected override void BuildInitiate() {
        buildInitiated = true;
    }

    protected override void BuildUpdate() {
        UpdateTempBuildings(new List<Vector3>() { currentMouseWorldPosition });
    }
}
