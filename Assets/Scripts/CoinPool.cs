using System;
using System.Collections.Generic;
using UnityEngine;


 class CoinPool : MonoBehaviour
{
    [SerializeField] GameObject[] objectPrefab; // Префаби об'єктів у пулі
    [SerializeField] Transform parentTarget;
    [SerializeField] int initialPoolSize = 50; // Початкова кількість об'єктів у пулі
    [SerializeField] int maxPoolSize = 150; // Максимальна кількість об'єктів у пулі
    [SerializeField] int maxActiveObjects = 100; // Максимальна кількість активних об'єктів

    private Queue<GameObject> pool; // Черга для об'єктів у пулі
    private Queue<GameObject> activeObjects; // Черга для активних об'єктів

    void Start()
    {
        pool = new Queue<GameObject>();
        activeObjects = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
            AddQueue();
    }

    public GameObject Get(Vector3 spawnPosition)
    {
        if (pool.Count == 0)
            AddQueue();

        GameObject pooledObject = pool.Dequeue();
        pooledObject.transform.position = spawnPosition;
        pooledObject.SetActive(true);

        // Додаємо об'єкт до черги активних об'єктів
        activeObjects.Enqueue(pooledObject);

        // Якщо кількість активних об'єктів перевищує максимум, повертаємо найстаріший активний об'єкт у пул
        if (activeObjects.Count > maxActiveObjects)
        {
            GameObject oldestActiveObject = activeObjects.Dequeue();
            Return(oldestActiveObject);
        }

        return pooledObject;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);

        // Якщо кількість об'єктів у пулі менша за максимум, додаємо його назад у пул
        if (pool.Count < maxPoolSize)
        {
            pool.Enqueue(obj);
        }
        else
        {
            // Інакше видаляємо найстаріший об'єкт з пулу та додаємо новий
            GameObject oldestInPool = pool.Dequeue();
            Destroy(oldestInPool);
            pool.Enqueue(obj);
        }
    }

    private void AddQueue()
    {
        GameObject tilePrefab = objectPrefab[UnityEngine.Random.Range(0, objectPrefab.Length)];
        GameObject obj = Instantiate(tilePrefab, parentTarget);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}