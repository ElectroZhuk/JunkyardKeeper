using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField][Min(1)] private int _amount;

    public int Amount { get; private set; }

    private void Awake()
    {
        Amount = _amount;
    }

    public void Init(int amount)
    {
        Amount = 0;

        if (amount > 0)
            Amount = amount;
    }

    public void Collect()
    {
        Destroy(gameObject);
    }
}
