using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private List<EnemySpawnData> enemySpawnDataList = null;
    private HashSet<EnemySpawnData> spawnedEnemyDataSet = null;
    private float startTime = -1.0f;

    // Start is called before the first frame update
    void Start() {
        enemySpawnDataList = LevelDataManager.CurrentLevelData.EnemySpawnDataList;
        spawnedEnemyDataSet = new HashSet<EnemySpawnData>();
        startTime = Time.time;
    }

    void OnEnable() {
        IEnumerator setupEnemySpawnsCouritine() {
            yield return new WaitForEndOfFrame();
            foreach (EnemySpawnData enemySpawnData in enemySpawnDataList) {
                spawnEnemyAfterDelay(enemySpawnData);
            }
            yield return null;
        }
        StartCoroutine(setupEnemySpawnsCouritine());
    }

    void spawnEnemyAfterDelay(EnemySpawnData enemySpawnData) {
        if (spawnedEnemyDataSet.Contains(enemySpawnData)) {
            return;
        }
        float timeSinceStart = Time.time - startTime;
        IEnumerator spawnEnemyCouritine() {
            yield return new WaitForSeconds(enemySpawnData.delay - timeSinceStart);
            spawnEnemy(enemySpawnData.enemyData.Prefab);
            spawnedEnemyDataSet.Add(enemySpawnData);
            yield return null;
        }
        StartCoroutine(spawnEnemyCouritine());
    }

    void spawnEnemy(GameObject enemyPrefab) {
        GameObject enemyObj = Instantiate(enemyPrefab, getRandomPointOutsideScreen(), Quaternion.identity, transform);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (!enemy) {
            Debug.LogError("EnemySpawner.spawnEnemy: enemyObj does not contain Enemy component");
            return;
        }
    }

    Vector3 getRandomPointOutsideScreen() {
        Vector3 minScreenPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 maxScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        List<float> adjustedScreenDimensions = new List<float>() {
            minScreenPoint.x - 1.0f, minScreenPoint.y - 1.0f, maxScreenPoint.x + 1.0f, maxScreenPoint.y + 1.0f,
        };
        int randomChoice = Random.Range(0, 4);
        switch (randomChoice) {
        case 0:
        case 2:
            return new Vector3(adjustedScreenDimensions[randomChoice], Random.Range(adjustedScreenDimensions[1], adjustedScreenDimensions[3]), 0.0f);
        case 1:
        case 3:
            return new Vector3(Random.Range(adjustedScreenDimensions[0], adjustedScreenDimensions[2]), adjustedScreenDimensions[randomChoice], 0.0f);
        default:
            return Vector3.zero;
        }
    }

    public int TotalEnemyCount() {
        return enemySpawnDataList.Count;
    }
}
