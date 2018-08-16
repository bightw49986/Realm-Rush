using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIController : MonoBehaviour 
{
    [SerializeField]
    Text Level,HP,Tower,EnemyKilled , WinLose , Continue;
    Animator winLoseScreen;
    Player player;
    StageController stageController;
    int towerAvailible;


    void Awake()
    {
        stageController = FindObjectOfType<StageController>();
        player = FindObjectOfType<Player>();
        winLoseScreen = WinLose.GetComponent<Animator>();
    }

    void Start () 
    {
        stageController.SceneLoaded += delegate 
        {
            Level.text = "Current Level : " + stageController.currentScene.name;
            towerAvailible = player.Tower;
        };
        stageController.StageChanged += OnStageChanged;
	}

	void Update () 
    {
        HP.text = "HP : " + player.HP;
        Tower.text = "Tower : " + player.Tower + " / " + towerAvailible;
        EnemyKilled.text = "Enemy Killed : " + player.EnemyKilled;
	}

    void OnStageChanged(StageController.Stage stage)
    {
        if(stage == StageController.Stage.Die)
        {
            winLoseScreen.ResetTrigger("Player is dead");
            WinLose.text = "YOU DIED.";
            winLoseScreen.SetTrigger("Player is dead");
        }
        if (stage == StageController.Stage.Win)
        {
            winLoseScreen.ResetTrigger("Level Complete");
            WinLose.text = "LEVEL COMPLETE.";
            WinLose.rectTransform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            winLoseScreen.SetTrigger("Level Complete");
        }
    }
}
