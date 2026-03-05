using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private Transform parentTarget;

    [Header("Pool Settings")]
    [SerializeField] private int maxPoolSize = 150;
    [SerializeField] private int maxActiveObjects = 100;

    Queue<GameObject> pool = new Queue<GameObject>();
    Queue<GameObject> activeObjects = new Queue<GameObject>();

    public void InitializePool(int requiredActiveObjects)
    {
        int targetPoolSize = Mathf.Max(requiredActiveObjects, maxActiveObjects);

        while (pool.Count + activeObjects.Count < targetPoolSize)
            InstantiateNewObject();
    }

    public GameObject Get(Vector3 spawnPosition)
    {
        if (pool.Count == 0)
            InstantiateNewObject();

        GameObject obj = pool.Dequeue();
        obj.transform.position = spawnPosition;
        obj.SetActive(true);

        activeObjects.Enqueue(obj);

        if (activeObjects.Count > maxActiveObjects)
        {
            GameObject oldest = activeObjects.Dequeue();
            Return(oldest);
        }

        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);

        if (pool.Count < maxPoolSize)
            pool.Enqueue(obj);
        else
        {
            GameObject oldest = pool.Dequeue();
            Destroy(oldest);
            pool.Enqueue(obj);
        }
    }

    private void InstantiateNewObject()
    {
        GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
        GameObject obj = Instantiate(prefab, parentTarget);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}