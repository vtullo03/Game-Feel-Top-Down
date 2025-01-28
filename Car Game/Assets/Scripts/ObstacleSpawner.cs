using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    private float maxY = 2.7f;
    private float minY = -2.7f;
    private bool isSpawning;

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning) StartCoroutine(SpawnObstacle());
    }
    
    IEnumerator SpawnObstacle()
    {
        isSpawning = true;
        float spawnY = Random.Range(minY, maxY);
        Instantiate(obstaclePrefab, new Vector3(10.0f, spawnY, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        isSpawning = false;
    }
}
