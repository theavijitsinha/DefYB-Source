using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBuilder : Builder {
    private Vector3 lineStartPosition = Vector3.zero;
    protected override void BuildAbort() {
        buildInitiated = false;
    }

    protected override void BuildCommit() {
        CompleteBuild();
    }

    protected override void BuildInitiate() {
        buildInitiated = true;
        lineStartPosition = currentMouseWorldPosition;
    }

    protected override void BuildUpdate() {
        UpdateTempBuildings(GetLineIndices());
    }

    private List<Vector3> GetLineIndices() {
        List<Vector3> lineIndices = new List<Vector3>();
        Vector3 lineDir = new Vector3(
            lineStartPosition.x < currentMouseWorldPosition.x ? 1.0f : -1.0f,
            lineStartPosition.y < currentMouseWorldPosition.y ? 1.0f : -1.0f,
            0.0f);
        List<Vector3> lineDirOptions = new List<Vector3>() {
            new Vector3(lineDir.x, 0.0f, 0.0f),
            new Vector3(0.0f, lineDir.y, 0.0f),
            lineDir,
        };
        Vector3 curIndex = lineStartPosition;
        lineIndices.Add(curIndex);
        while (curIndex != currentMouseWorldPosition) {
            float minError = Mathf.Infinity;
            Vector3 minErrorIndex = curIndex;
            foreach (Vector3 nextIndexDir in lineDirOptions) {
                Vector3 nextIndex = curIndex + nextIndexDir;
                float error = GetLineError(nextIndex);
                if (error < minError) {
                    minError = error;
                    minErrorIndex = nextIndex;
                }
            }
            curIndex = minErrorIndex;
            lineIndices.Add(curIndex);
        }
        return lineIndices;
    }

    private float GetLineError(Vector3 point) {
        Vector3 a = currentMouseWorldPosition - lineStartPosition;
        Vector3 b = point - lineStartPosition;
        return Vector3.Cross(a, b).magnitude / a.magnitude;
    }
}
