using UnityEngine;

public class PlayerLevelElement : MonoBehaviour
{
    public int Amount => _amount;

    private int _amount = 1;

    public void Collect()
    {
        Destroy(gameObject);
    }
}
