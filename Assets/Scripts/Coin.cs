using UnityEngine;

public class Coin : MonoBehaviour
{
    private float timeToDestroy = 5f;
    private float xSpawnRange = 8f;
    private float zNitroRange = 5f;
    private float ySpawn = 0.3f;

    void Start()
    {
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        float randomZ = Random.Range(-zNitroRange, zNitroRange);

        Vector3 spawnPos = new Vector3(randomX, ySpawn, randomZ);
        transform.position = spawnPos;
    }

    void Update()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
