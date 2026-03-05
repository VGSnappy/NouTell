using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] CoinPool coinPool;
    [SerializeField] Transform player;

    [Header("Spawn Settings")]
    [SerializeField] int coinsCount = 40;
    [SerializeField] float spawnRadius = 25f;
    [SerializeField] float respawnDistance = 30f;

    private List<GameObject> spawnedCoins = new List<GameObject>();

    void Start()
    {
        SpawnInitialCoins();
    }

    void Update()
    {
        CheckCoinsDistance();
    }

    void SpawnInitialCoins()
    {
        for (int i = 0; i < coinsCount; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        Vector3 pos = GetRandomPointAroundPlayer();
        GameObject coin = coinPool.Get(pos);

        spawnedCoins.Add(coin);
    }

    Vector3 GetRandomPointAroundPlayer()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;

        Vector3 position = new Vector3(
            player.position.x + circle.x,
            player.position.y + 20f,
            player.position.z + circle.y
        );

        // кидаємо Raycast вниз на terrain
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 50f))
        {
            position.y = hit.point.y;
        }

        return position;
    }

    void CheckCoinsDistance()
    {
        foreach (var coin in spawnedCoins)
        {
            if (!coin.activeSelf) continue;

            float distance = Vector3.Distance(player.position, coin.transform.position);

            // якщо монета далеко — переносимо її
            if (distance > respawnDistance)
            {
                coinPool.Return(coin);

                Vector3 newPos = GetRandomPointAroundPlayer();
                coinPool.Get(newPos);
            }
        }
    }
}