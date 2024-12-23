using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public int maxPlayer = 3;
    public float resetDelay = 3f;
    
    [Header("Prefabs")]
    public GameObject warriorPrefab;
    public Transform spawnPoint;
    
    [Header("UI References")]
    public Text birdsLeftText;
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;
    
    private int playerLeft;
    private int enemyLeft;
    private bool isGameActive = false;
    
    public GameObject barrel;
    public BarrelController barrelController;

    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
    }
    
    void InitializeGame()
    {
        playerLeft = maxPlayer;
        enemyLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        isGameActive = true;
        
        UpdateUI();
        SpawnNewWarrior();
    }

    public void OnEnemyDestroyed()
    {
        enemyLeft--;
        CheckWinCondition();
    }
    
    void CheckWinCondition()
    {
        if (enemyLeft <= 0)
        {
            StartCoroutine(LevelComplete());
        }
        else if (playerLeft <= 0 && GameObject.FindGameObjectWithTag("Player") == null)
        {
            StartCoroutine(GameOver());
        }
    }
    
    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(resetDelay);
        levelCompletePanel.SetActive(true);
        isGameActive = false;
    }
    
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(resetDelay);
        gameOverPanel.SetActive(true);
        isGameActive = false;
    }
    
    void SpawnNewWarrior()
    {
        if (playerLeft > 0 && isGameActive)
        {
            GameObject newWarrior = Instantiate(warriorPrefab, spawnPoint.position, Quaternion.identity);
            playerLeft--;
            UpdateUI();
        }
    }
    
    void UpdateUI()
    {
        if (birdsLeftText != null)
        {
            birdsLeftText.text = $"Warriors Left: {playerLeft}";
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (barrelController.isDone)
        {
            Destroy(barrel.gameObject);
        }
    }
}
