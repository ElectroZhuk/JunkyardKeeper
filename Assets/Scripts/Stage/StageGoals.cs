using UnityEngine;
using UnityEngine.Events;

public class StageGoals : MonoBehaviour
{
    [SerializeField] private Goal[] _mandotaryGoals;
    [SerializeField] private Goal[] _optionalGoals;

    public event UnityAction MandatoryGoalAchieved;

    public int NotAchievedMandatoryGoalsAmount => _notAchievedMandatoryGoalsAmount;

    private int _notAchievedMandatoryGoalsAmount;

    private void Awake()
    {
        _notAchievedMandatoryGoalsAmount = _mandotaryGoals.Length;
    }

    private void OnEnable()
    {
        foreach (Goal goal in _mandotaryGoals)
            goal.Achieved += OnGoalAchieved;

        foreach (Goal goal in _optionalGoals)
            goal.Achieved += OnGoalAchieved;
    }

    private void OnDisable()
    {
        foreach (Goal goal in _mandotaryGoals)
            goal.Achieved -= OnGoalAchieved;

        foreach (Goal goal in _optionalGoals)
            goal.Achieved -= OnGoalAchieved;
    }

    private void OnGoalAchieved(Goal goal)
    {
        Debug.Log($"Goal \"{goal.Desctiption}\" achieved!");
        _notAchievedMandatoryGoalsAmount--;
        MandatoryGoalAchieved?.Invoke();
    }
}
