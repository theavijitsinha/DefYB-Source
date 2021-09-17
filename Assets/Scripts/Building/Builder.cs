using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildSiteState {
    Invalid,
    Free,
    OccupiedReplacable,
    OccupiedIrreplacable,
}

public abstract class Builder : MonoBehaviour {
    private BuildingData buildingData = null;

    protected List<Building> tempBuildingList = new List<Building>();
    protected bool buildInitiated = false;
    protected Vector3 currentMouseWorldPosition = Vector3.zero;

    // Update is called once per frame
    void Update() {
        currentMouseWorldPosition = snapWorldPositionToGrid(worldMousePosition());
        if (buildInitiated) {
            BuildUpdate();
        }
        else {
            UpdateTempBuildings(new List<Vector3>() { currentMouseWorldPosition });
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !buildInitiated) {
            BuildInitiate();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && buildInitiated) {
            BuildCommit();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && buildInitiated) {
            BuildAbort();
        }
    }

    public void InitiateBuilder(BuildingData buildingData) {
        this.buildingData = buildingData;
    }

    protected abstract void BuildInitiate();
    protected abstract void BuildUpdate();
    protected abstract void BuildCommit();
    protected abstract void BuildAbort();

    protected void UpdateTempBuildings(List<Vector3> positions) {
        List<Building> newTempBuildingList = new List<Building>();
        for (int i = 0; i < tempBuildingList.Count && i < positions.Count; i++) {
            tempBuildingList[i].transform.position = positions[i];
            newTempBuildingList.Add(tempBuildingList[i]);
        }
        for (int i = tempBuildingList.Count; i < positions.Count; i++) {
            Building newTempBuilding = AddBuildingAtPosition(positions[i]);
            if (!newTempBuilding) {
                Debug.LogError("Builder.UpdateTempBuildings: newTempBuilding not set");
                return;
            }
            newTempBuildingList.Add(newTempBuilding);
        }
        for (int i = positions.Count; i < tempBuildingList.Count; i++) {
            Destroy(tempBuildingList[i].gameObject);
        }
        tempBuildingList = newTempBuildingList;
    }

    protected Building AddBuildingAtPosition(Vector3 position) {
        GameObject newTempBuildingObject = Instantiate(buildingData.Prefab, position,
            Quaternion.identity, LevelGameManager.Instance.GameRoot.transform);
        Building newTempBuilding = newTempBuildingObject.GetComponent<Building>();
        if (!newTempBuilding) {
            Debug.LogError("Builder.Update: tempBuilding does not have Building component");
            return null;
        }
        return newTempBuilding;
    }

    protected void ClearTempBuildingList() {
        foreach (Building tempBuilding in tempBuildingList) {
            Destroy(tempBuilding.gameObject);
        }
        tempBuildingList.Clear();
    }

    protected void CompleteBuild() {
        foreach (Building building in tempBuildingList) {
            BuildSiteState siteState = building.SiteState();
            if (siteState == BuildSiteState.Free ||
                siteState == BuildSiteState.OccupiedReplacable) {
                building.Built = true;
                LevelGameManager.Instance.SubtractGold(buildingData.BuildCost);
            }
            else {
                Destroy(building.gameObject);
            }
        }
        tempBuildingList.Clear();
        QuitBuilder();
    }

    private void QuitBuilder() {
        ClearTempBuildingList();
        Destroy(gameObject);
    }

    protected Vector3 worldMousePosition() {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0.0f;
        return position;
    }

    protected Vector3 snapWorldPositionToGrid(Vector3 position) {
        return new Vector3(
            Mathf.Floor(position.x) + 0.5f,
            Mathf.Floor(position.y) + 0.5f,
            0.0f
        );
    }
}
