using System.Collections.Generic;

[System.Serializable]
public class Data 
{
    public int StageBuildIndex;
    public int StageMoney;
    public int GlobalMoney;
    public Dictionary<ShopItem, int> StageShopItemsLevels;
    public Dictionary<Goal, bool> StageGoalsReachedStatuses;
    public Dictionary<Venicle, Dictionary<Upgradeable, int>> VenicleUpgradeableLevels;
}
