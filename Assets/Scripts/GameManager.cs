using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highestScoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI guidText;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject coin;
    [SerializeField] PlayerController player;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] AudioClip crushSound;
    private AudioSource playerAudio;

    public float score = 0;
    public bool isGameActive = true;
    public float lives;
    private float spawnRate = 4;
    private int spawnCoinRate = 7;
    private bool isPaused;

   /* private void Awake()
    {
        LoadScore();
        highestScoreText.text = "Highest Score: " + score;
    }*/
    void Start()
    {
        if (MainManager.Instance != null)
        {
            highestScoreText.text = "Highest Score: " + MainManager.Instance.hScore;
        }
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGameActive)
        {
            CheckForPause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainManager.Instance.SaveScore();
            Application.Quit();
        }
    }

    private IEnumerator AddScore()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);
            score++;
            scoreText.text = "Score: " + score;
        }
    }

    private IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            if (!isGameActive)
                yield break;
            int index = Random.Range(0, enemies.Count);
            Instantiate(enemies[index]);
        }
    }
    private IEnumerator SpawnCoin()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnCoinRate);
            if (!isGameActive)
                yield break;
            Instantiate(coin);
        }
    }

    public void UpdateLives()
    {
        if (isGameActive)
        {
            lives--;
            livesText.text = "Lives: " + lives;
        }
    }

    public void StartGame(int diffuclty)
    {
        isGameActive = true;
        score = 0;
        lives = 4;
        spawnRate /= diffuclty;
        titleScreen.gameObject.SetActive(false);
        guidText.gameObject.SetActive(true);
        StartCoroutine(AddScore());
        StartCoroutine(SpawnTarget());
        StartCoroutine(SpawnCoin());
        UpdateLives();
    }

    public void GameOver()
    {
        if (score > MainManager.Instance.hScore)
        {
            MainManager.Instance.hScore = score;
        }
        playerAudio.PlayOneShot(crushSound, 1.0f);
        isGameActive = false;
        gameOver.SetActive(true);
        Instantiate(player.explosionParticle, player.transform.localPosition, player.explosionParticle.transform.rotation);
        finalScoreText.text = "Final Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckForPause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    /*[System.Serializable]
    class SaveData
    {
        public float Score;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.Score = highestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data.Score > highestScore)
            {
                highestScore = data.Score;
            }
        }
    }*/
}
