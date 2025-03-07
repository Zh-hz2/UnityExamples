using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] platformPrefabs; // 平台预制体数组
    public Transform startLine; // 生成基准线
    public float spawnInterval = 2f; // 平台生成间隔
    public float minX = -3f, maxX = 3f; // 平台 X 坐标范围
    public float yOffset = 5f; // 每个平台的 Y 轴间距
    public float minDistanceX = 2f; // 两个平台之间的最小间距

    void Start()
    {
        if (startLine == null)
        {
            Debug.LogError("❌ StartLine is NOT assigned in Spawner! Please drag it in the Inspector.");
            return;
        }

        Debug.Log("✅ Spawner started...");
        Debug.Log("Platform Prefabs Count: " + platformPrefabs.Length);

        StartCoroutine(SpawnPlatforms());
    }

    IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (platformPrefabs.Length == 0)
            {
                Debug.LogError("❌ No platform prefabs assigned in Spawner!");
                yield break;
            }

            // **获取 StartLine 当前 Y 位置**
            float currentY = startLine.position.y;
            Debug.Log("📍 StartLine Y Position: " + currentY);

            // **随机生成两个平台**
            Vector3 spawnPosition1 = GetRandomPosition(currentY);
            Vector3 spawnPosition2 = GetRandomPosition(currentY);

            // **确保两个平台不会太靠近**
            while (Mathf.Abs(spawnPosition1.x - spawnPosition2.x) < minDistanceX)
            {
                spawnPosition2 = GetRandomPosition(currentY);
            }

            // **随机选择平台预制体**
            GameObject platformPrefab1 = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            GameObject platformPrefab2 = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

            // **生成两个平台**
            Instantiate(platformPrefab1, spawnPosition1, Quaternion.identity);
            Instantiate(platformPrefab2, spawnPosition2, Quaternion.identity);

            Debug.Log($"✅ Spawned two platforms at {spawnPosition1} and {spawnPosition2}");
        }
    }

    // **随机生成一个平台的位置**
    Vector3 GetRandomPosition(float currentY)
    {
        float randomX = Random.Range(minX, maxX);
        float newY = currentY + yOffset; // 让平台随 StartLine 上移
        return new Vector3(randomX, newY, 0);
    }
}
