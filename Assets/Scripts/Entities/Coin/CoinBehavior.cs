using System.Collections;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    CoinPool coinPool;

    [SerializeField] float orbitSpeed = 200f;
    [SerializeField] float shrinkSpeed = 2f;
    [SerializeField] float moveSpeed = 6f;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    Transform player;
    bool attracting;

    Color coinColor;

    public void OnSpawn(CoinPool pool)
    {
        coinPool = pool;
        attracting = false;

        if (!ps)
            ps = GetComponentInChildren<ParticleSystem>();

        if (ps != null)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        coinColor = GetComponent<Renderer>().material.color;

        transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (!attracting || player == null || ps == null)
            return;

        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = (player.position - particles[i].position).normalized;

            Vector3 toPlayer = dir * 7f;
            Vector3 swirl = Vector3.Cross(dir, Vector3.up) * 3f;

            particles[i].velocity = toPlayer + swirl;
            particles[i].startColor = coinColor;
        }

        ps.SetParticles(particles, count);
    }

    void OnTriggerEnter(Collider other)
    {
        if (attracting) return;

        if (other.CompareTag("Player"))
        {
            attracting = true;
            player = other.transform;

            if (ps != null)
                ps.Play();

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

            transform.RotateAround(player.position, Vector3.up, orbitSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        coinPool.Return(gameObject);
    }
}