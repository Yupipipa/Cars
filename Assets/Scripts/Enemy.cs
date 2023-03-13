using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody objectRb;
    private GameManager gameManager;
    private AudioSource playerAudio;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] AudioClip bumpSound;

    [SerializeField] float speed = 5f;
    private float zBound = -12.0f;
    private float zEnemySpawn = 12f;
    private float xSpawnRange = 8f;
    private float ySpawn = 0.4f;

    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        Vector3 spawnPos = new Vector3(randomX, ySpawn, zEnemySpawn);

        transform.position = spawnPos;
    }

    void Update()
    {
        objectRb.AddForce(Vector3.back * speed);
        if (objectRb.transform.position.z < zBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateLives();
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            playerAudio.PlayOneShot(bumpSound, 1.0f);
            Destroy(gameObject);
            if (gameManager.lives == 0)
            {
                gameManager.GameOver();
            }
        }
    }
}
