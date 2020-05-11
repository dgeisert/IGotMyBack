using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public PauseMenu pauseMenu;
    public ScoreScreen scoreScreen;
    public InGameUI inGameUI;
    public List<Enemy> enemies;
    public List<Enemy> enemyPrefabs;
    Transform[] spawnPoints;
    public Transform spawnPointParent;
    public float spawnArea = 20;
    public float spawnDistance = 40;
    public bool active = true;
    int wave = 1;
    public static float Score
    {
        get
        {
            if (Instance)
            {
                return Instance.score;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.score = value;
                Instance.inGameUI.UpdateScore(value);
            }
        }
    }
    public float score;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
        enemies = new List<Enemy>();
    }
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        Time.timeScale = 1;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Controls.Pause)
            {
                Pause();
            }
        }
        if (enemies.Count == 0)
        {
            SpawnEnemies();
        }
    }

    int perGroup = 5;
    public void SpawnEnemies()
    {
        wave++;
        for (int i = 0; i <= Mathf.FloorToInt(wave / perGroup); i++)
        {
            SpawnGroup(Mathf.Min(perGroup, wave - perGroup * i));
        }
    }

    public void SpawnGroup(int num)
    {
        if (spawnPoints == null || spawnPoints.Length <= 0)
        {
            spawnPoints = new Transform[spawnPointParent.childCount];
            for (int i = 0; i < spawnPointParent.childCount; i++)
            {
                spawnPoints[i] = spawnPointParent.GetChild(i);
            }
        }
        Transform spawnPoint = spawnPoints[Mathf.FloorToInt(Random.value * spawnPoints.Length)];
        while (Vector3.Distance(spawnPoint.position, Char.Instance.transform.position) < spawnDistance)
        {
            spawnPoint = spawnPoints[Mathf.FloorToInt(Random.value * spawnPoints.Length)];
        }
        Enemy chosenEnemy = enemyPrefabs[Mathf.FloorToInt(enemyPrefabs.Count * Random.value)];
        for (int i = 0; i < num; i++)
        {
            Vector3 shift = new Vector3((Random.value - 0.5f) * spawnArea, 0, (Random.value - 0.5f) * spawnArea);
            enemies.Add(Instantiate(chosenEnemy, spawnPoint.position + shift, Quaternion.identity));
        }
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    public void GameOver(bool victory = false)
    {
        inGameUI.EndGame(victory);
        scoreScreen.EndGame(victory);
        pauseMenu.gameObject.SetActive(false);
    }
}