using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmerPrefab;
    [SerializeField]
    private GameObject bigSwarmerPrefab;

    [SerializeField]
    private float swarmerInterval = 3.5f;
    [SerializeField]
    private float bigSwarmerInterval = 10f;

    // [SerializeField]
    // private float spawnOffset = 10f; // distance from edge of screen to spawn enemy

    [SerializeField]
    private int maxEnemies = 4; // maximum number of enemies that can spawn at once
    private int enemyCount = 0; // current number of enemies in the scene
    [SerializeField]
    private int maxTotalEnemies = 10; // maximum number of enemies that can spawn in total
    private int totalEnemyCount = 0; // current number of enemies spawned in total

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));
        StartCoroutine(spawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        // only spawn enemy if maximum number of enemies has not been reached
        if (enemyCount < maxEnemies && totalEnemyCount < maxTotalEnemies) {
            float randY = Random.Range(-6f, 6f);
            Vector3 spawnPos = new Vector3(-10f, randY, 0f); // spawn on left side of screen by default

            // randomly spawn on left or right side of screen
            if (Random.value > 0.5f) {
                spawnPos = new Vector3(10f, randY, 0f);
            }

            GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
            enemyCount++;
            totalEnemyCount++;
        }

        StartCoroutine(spawnEnemy(interval, enemy));
    }

    // method to decrement enemy count when an enemy is destroyed
    public void DecrementEnemyCount()
    {
        enemyCount--;
    }
}