using UnityEngine;
using UnityEngine.Events;

public abstract class Goal : MonoBehaviour
{
    [SerializeField] private string _description;

    public string Desctiption => _description;

    public event UnityAction<Goal> Achieved;

    protected void Achieve()
    {
        Achieved?.Invoke(this);
    }
}
