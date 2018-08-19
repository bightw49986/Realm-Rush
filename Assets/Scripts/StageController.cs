using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageController : SceneManagement
{
    public Scene currentScene;
    public string nextScene;

    UIController uIController;

    ObjectPool objectPool;

    EnemySpawner enemySpawner;
    int enemiesToKill;
    public int EnemiesToKill { get { return enemiesToKill; } set { enemiesToKill = value; OnEnemiesCountChanged(); } }

    Player player;
    List<int> PlayerStates;

    public enum Stage { Build, Defense, Pause, Win, Die };
    Stage currentStage;
    public Stage CurrentStage { get { return currentStage; } private set { currentStage = value; OnStageChanged(currentStage); } }

    void Awake()
    {
        if (FindObjectsOfType<StageController>().Length > 1)
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SubscribeToSceneManager();
            player = FindObjectOfType<Player>();
            player.Died += OnPlayerDied;
            objectPool = FindObjectOfType<ObjectPool>();
        }
    }

    void OnPlayerDied()
    {
        TransStageTo(Stage.Die);
        print("Player died.");
    }

    protected override void OnSceneLoaded(Scene scene)
    {
        currentScene = scene;
        nextScene = "Level " + (int.Parse((currentScene.name.Split(' '))[1]) + 1).ToString();

        objectPool.UpdateLevelEnemy();

        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner == null) return;
        EnemiesToKill = enemySpawner.EnemyNumbers;
        enemySpawner.EnemySpawned += OnEnemySpawned;
        if (player.hasInit == false)
        {
            player.SetPlayerStates(enemySpawner.GetDefaultPlayerData());
            player.hasInit = true;
        }
        PlayerStates = player.GetPlayerStates();
        player.isAlive = true;

        uIController = FindObjectOfType<UIController>();
        if (uIController == null) return;
        uIController.UpdateLevelInfo();

        print("Detected scene loaded,trans to build stage.");
        TransStageTo(Stage.Build);
    }

    protected override void OnSceneReLoaded(Scene scene)
    {
        currentScene = scene;
        player.SetPlayerStates(PlayerStates);
        enemySpawner = FindObjectOfType<EnemySpawner>();
        EnemiesToKill = enemySpawner.EnemyNumbers;
        enemySpawner.EnemySpawned += OnEnemySpawned;
        player.isAlive = true;
        uIController = FindObjectOfType<UIController>();
        uIController.UpdateLevelInfo();
        TransStageTo(Stage.Build);
        print("Detected scene reloaded,level restart");
    }

    protected override void OnSceneUnLoaded(Scene scene)
    {
        player.Tower = PlayerStates[1];
        enemySpawner.EnemySpawned -= OnEnemySpawned;
        enemySpawner = null;
        uIController = null;
        objectPool.ClearPool();
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
        }
    }

    void OnEnemySpawned(GameObject enemy)
    {
        Enemy enemyData = enemy.GetComponent<Enemy>();

        enemyData.EnemyDied += OnEnemyDied;
        enemyData.EnemyPassed += OnEnemyPassed;

    }

    void OnEnemyDied(Enemy enemy)
    {
        enemy.EnemyDied -= OnEnemyDied;
        enemy.EnemyPassed -= OnEnemyPassed;
        EnemiesToKill -= 1;
        player.EnemyKilled += 1;
    }

    void OnEnemyPassed(Enemy enemy)
    {
        enemy.EnemyPassed -= OnEnemyPassed;
        enemy.EnemyDied -= OnEnemyDied;
        player.HP -= enemy.Damage;
        if (player.HP <= 0) return;
        EnemiesToKill -= 1;
    }
}
