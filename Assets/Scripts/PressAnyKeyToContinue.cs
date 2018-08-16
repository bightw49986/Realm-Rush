using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKeyToContinue : MonoBehaviour 
{
    StageController stageController;

    void Awake()
    {
        stageController = FindObjectOfType<StageController>();
    }

    void Update () 
    {
        if (Input.anyKey)
        {
            if(stageController.CurrentStage == StageController.Stage.Die)
            {
                SceneManager.LoadScene(stageController.currentScene.name);
                return;
            }
            if (stageController.CurrentStage == StageController.Stage.Win)
            {
                SceneManager.LoadScene(stageController.nextScene);
                return;
            }
            return;
        }
	}
}
