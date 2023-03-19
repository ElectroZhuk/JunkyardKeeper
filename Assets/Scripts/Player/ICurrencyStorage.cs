using UnityEngine.Events;

public interface ICurrencyStorage
{
    public event UnityAction CurrencyCollected;

    public void Spend(int amount);

    public bool CanSpend(int amount);
}
