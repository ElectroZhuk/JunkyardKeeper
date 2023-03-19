
public class PlayerLevelUpPriceController : PriceController
{
    public override int GetPrice(int itemLevel)
    {
        if (StageSettings.LevelElementsToLevelUpForLevel.Count < itemLevel)
            return 0;

        return StageSettings.LevelElementsToLevelUpForLevel[itemLevel - 1];
    }
}
