using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private bool isRandomized;
    private float maxY = 2.7f;
    private float minY = -2.7f;
    private bool isSpawning;
    private float spawnY;

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning) StartCoroutine(SpawnObstacle());
    }
    
    IEnumerator SpawnObstacle()
    {
        isSpawning = true;
        float spawnY;
        float spawnZ;
        if (isRandomized)
        {
            spawnY = Random.Range(minY, maxY);
            spawnZ = 0.0f;
        }
        else
        {
            spawnY = 0.0f;
            spawnZ = 1.0f;
        }
        Instantiate(objectPrefab, new Vector3(10.0f, spawnY, spawnZ), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        isSpawning = false;
    }
}
