using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStageButton : MonoBehaviour
{
    StageController stageController;
    StageController.Stage targetStage = StageController.Stage.Defense;
    Text ButtonText;

    private void Awake()
    {
        stageController = FindObjectOfType<StageController>();
        ButtonText = GetComponentInChildren<Text>();
    }

    void Start()
    {
        stageController.StageChanged += ChangeButton;
    }

    void ChangeButton(StageController.Stage stage)
    {
        print("Button detected stage changed,current stage is: " + stage);
        switch (stage)
        {
            case (StageController.Stage.Build):
                {
                    targetStage = StageController.Stage.Defense;
                    ButtonText.text = "Play";
                    return;
                }
            case (StageController.Stage.Defense):
                {
                    targetStage = StageController.Stage.Pause;
                    ButtonText.text = "Pause";
                    return;
                }
            case (StageController.Stage.Pause):
                {
                    targetStage = StageController.Stage.Defense;
                    ButtonText.text = "Resume";
                    return;
                }
            case (StageController.Stage.Win):
                {
                    gameObject.SetActive(false);
                    return;
                }
            case (StageController.Stage.Die):
                {
                    gameObject.SetActive(false);
                    return;
                }
        }
    }
    public void OnButtonClick()
    {
        print("Detected button clicked,current stage is :"+stageController.CurrentStage + ",trans to traget stage :" + targetStage);
        switch (stageController.CurrentStage)
        {
            case(StageController.Stage.Build):
                {
                    stageController.TransStageTo(StageController.Stage.Defense);
                    return;
                }
            case (StageController.Stage.Defense):
                {
                    stageController.TransStageTo(StageController.Stage.Pause);
                    return;
                }
            case (StageController.Stage.Pause):
                {
                    stageController.TransStageTo(StageController.Stage.Defense);
                    return;
                }
        }
    }
}
