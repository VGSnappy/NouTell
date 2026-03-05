using System.Collections;
using UnityEngine;


public class CoinSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CoinPool coinPool; // Пул монет
    [SerializeField] private Transform player;  // Гравець

    [Header("Spawn Settings")]
    [SerializeField] private int coinsCount = 40;
    [SerializeField] private float spawnRadius = 25f;
    [SerializeField] private float respawnDistance = 30f;
    [SerializeField] private float spawnDelay = 0.1f;

    private GameObject[] spawnedCoins;
    private float respawnDistanceSqr;

    void Start()
    {
        // Перевірка налаштувань
        if (coinPool == null || player == null)
        {
            Debug.LogError("CoinPool або Player не прив'язані!");
            return;
        }

        coinPool.InitializePool(); // Ініціалізація пулу

        spawnedCoins = new GameObject[coinsCount];
        respawnDistanceSqr = respawnDistance * respawnDistance;

        StartCoroutine(SpawnCoinsWithDelay());
    }

    void Update()
    {
        // Переспавн монет, які відійшли далеко від гравця
        for (int i = 0; i < spawnedCoins.Length; i++)
        {
            GameObject coin = spawnedCoins[i];
            if (coin == null || !coin.activeInHierarchy) continue;

            float sqrDistance = (player.position - coin.transform.position).sqrMagnitude;
            if (sqrDistance > respawnDistanceSqr)
            {
                coin.transform.position = GetRandomPointAroundPlayer();
            }
        }
    }

    /// <summary>
    /// Спавн монет із затримкою
    /// </summary>
    IEnumerator SpawnCoinsWithDelay()
    {
        for (int i = 0; i < coinsCount; i++)
        {
            Vector3 pos = GetRandomPointAroundPlayer();
            spawnedCoins[i] = coinPool.Get(pos);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    /// <summary>
    /// Вибір випадкової точки навколо гравця з урахуванням рельєфу
    /// </summary>
    private Vector3 GetRandomPointAroundPlayer()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;
        Vector3 position = new Vector3(player.position.x + circle.x, player.position.y + 50f, player.position.z + circle.y);

        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 100f))
            position.y = hit.point.y;

        return position;
    }
}