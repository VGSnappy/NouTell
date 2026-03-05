using System.Collections;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private float orbitSpeed = 200f;
    [SerializeField] private float shrinkSpeed = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float particleSpiralSpeed = 5f; // швидкість закрутки частинок

    private ParticleSystem particleSys;
    private ParticleSystem.Particle[] particles;

    private Transform player;
    private bool isAttracting = false;

    void Start()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();

        if (particleSys != null)
            particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
        else
            Debug.LogWarning($"ParticleSystem не знайдено на {gameObject.name}");

        if (coinPool == null)
            Debug.LogError("CoinPool не прив'язаний!");
    }

    void Update()
    {
        if (!isAttracting || player == null || particleSys == null || particles == null)
            return;

        int count = particleSys.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            // напрямок до гравця
            Vector3 dir = (player.position - particles[i].position).normalized;

            // базова швидкість до гравця
            Vector3 toPlayer = dir * 6f;

            // обертальна спіраль навколо гравця
            Vector3 axis = Vector3.up; // обертаємо навколо вертикальної осі
            float angle = particleSpiralSpeed * Time.deltaTime;

            Vector3 offset = particles[i].position - player.position;
            offset = Quaternion.AngleAxis(angle, axis) * offset;
            Vector3 swirl = (offset - (particles[i].position - player.position)) / Time.deltaTime;

            // сумуємо рух до гравця і спіраль
            particles[i].velocity = toPlayer + swirl;
        }

        particleSys.SetParticles(particles, count);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAttracting) return;

        if (other.CompareTag("Player"))
        {
            isAttracting = true;
            player = other.transform;

            if (particleSys != null)
                particleSys.Play();

            StartCoroutine(ShrinkAndOrbit());
        }
    }

    IEnumerator ShrinkAndOrbit()
    {
        Vector3 startScale = transform.localScale;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * shrinkSpeed;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            if (player != null)
            {
                transform.RotateAround(player.position, Vector3.up, orbitSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }

            yield return null;
        }

        coinPool.Return(gameObject);
    }
}