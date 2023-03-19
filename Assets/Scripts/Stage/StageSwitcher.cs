using UnityEngine;
using UnityEngine.Events;

public class StageSwitcher : MonoBehaviour
{
    [SerializeField] private StageGoals _stageGoals;
    [SerializeField] private SceneTransition _sceneTransition;

    public event UnityAction SwitchConditionCompleted;

    public bool CanSwitch { get; private set; }

    private void OnEnable()
    {
        _stageGoals.MandatoryGoalAchieved += OnMandatoryGoalAchieved;
    }

    private void OnDisable()
    {
        _stageGoals.MandatoryGoalAchieved -= OnMandatoryGoalAchieved;
    }

    public void SwitchStage()
    {
        if (CanSwitch == false)
        {
            Debug.LogError("You can't switch stage, because stage condition not met");
            return;
        }

        if (_sceneTransition.IsCurrentSceneLast)
        {
            Debug.Log("You finished the game!!!");
            return;
        }

        _sceneTransition.SwitchToNextScene();
    }

    private void OnMandatoryGoalAchieved()
    {
        CanSwitch = _stageGoals.NotAchievedMandatoryGoalsAmount <= 0;

        if (CanSwitch)
            SwitchConditionCompleted?.Invoke();
    }
}
