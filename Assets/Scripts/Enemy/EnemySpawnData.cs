using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnData {
    public float delay = 0.0f;
    public EnemyData enemyData = null;
    public EnemySpawnData(float delay, EnemyData enemyData) {
        this.delay = delay;
        this.enemyData = enemyData;
    }
}
