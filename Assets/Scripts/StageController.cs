using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageController : SceneManagement
{
    public Scene currentScene;
    public string nextScene;

    EnemySpawner enemySpawner;
    int _enemiesToKill;
    public int EnemiesToKill { get { return _enemiesToKill; } set { _enemiesToKill = value; OnEnemiesCountChanged(); } }

    Player player;
    List<int> PlayerStates;

    public enum Stage { Build, Defense, Pause, Win, Die };
    Stage _currentStage;
    public Stage CurrentStage { get { return _currentStage; } private set { _currentStage = value; OnStageChanged(_currentStage); } }

    void Awake()
    {
        if (FindObjectsOfType<StageController>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        player = FindObjectOfType<Player>();
    }
    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene());
        SceneLoaded += OnSceneLoaded;
        SceneReLoaded += OnSceneReLoaded;
        player.Died += OnPlayerDied;
    }

    void OnPlayerDied()
    {
        TransStageTo(Stage.Die);
        print("Player died.");
        //retry or back to main menu
    }

    protected override void OnSceneLoaded(Scene scene)
    {
        currentScene = scene;
        nextScene = "Level " + (int.Parse((currentScene.name.Split(' '))[1]) + 1).ToString();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner == null) return;
        else
        {
            EnemiesToKill = enemySpawner.EnemyNumbers;
            enemySpawner.EnemySpawned += OnEnemySpawned;
        }
        if (player.hasInit == false)
        {
            player.SetPlayerStates(enemySpawner.GetDefaultPlayerData());
            player.hasInit = true;
        }
        PlayerStates = player.GetPlayerStates();
        player.isAlive = true;
        print("Detected scene loaded,trans to Build stage.");
        TransStageTo(Stage.Build);
    }

    protected override void OnSceneReLoaded(Scene scene)
    {
        player.SetPlayerStates(PlayerStates);
        enemySpawner = FindObjectOfType<EnemySpawner>();
        EnemiesToKill = enemySpawner.EnemyNumbers;
        enemySpawner.EnemySpawned += OnEnemySpawned;
        player.isAlive = true;
        TransStageTo(Stage.Build);
        print("Detected scene reloaded,trans to Build stage.");
    }

    public void TransStageTo(Stage stage)
    {
        CurrentStage = stage;
    }

    public event Action<Stage> StageChanged;
    protected virtual void OnStageChanged(Stage stage)
    {
        if (StageChanged != null)
            StageChanged(stage);
        print("Stage has change to " + stage);

        if (stage == Stage.Build || stage == Stage.Defense)
        {
            Time.timeScale = 1;
            return;
        }
        if (stage == Stage.Pause || stage == Stage.Die || stage == Stage.Win)
        {
            Time.timeScale = 0;
            return;
        }
    }

    void OnEnemiesCountChanged()
    {
        if (EnemiesToKill <= 0)
        {
            TransStageTo(Stage.Win);
            print("Level complete.");
            hasLoaded = false;
            //continue or back to main menu. 
        }
    }

    void OnEnemySpawned(GameObject enemy)
    {
        Enemy enemyData = enemy.GetComponent<Enemy>();

        enemyData.EnemyDied += delegate
        {
            EnemiesToKill -= 1;
            player.EnemyKilled += 1;
        };
        enemyData.EnemyPassed += delegate
        {
            player.HP -= enemyData.Damage;
            if (player.HP <= 0) return;
            EnemiesToKill -= 1;
        };
    }
}
