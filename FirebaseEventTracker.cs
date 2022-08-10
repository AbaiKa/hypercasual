using Firebase.Analytics;
using UnityEngine;

public class FirebaseEventTracker : MonoBehaviour
{
    public static FirebaseEventTracker main;

    private static readonly string Complete_lvl_1 = "complete_lvl_1";
    private static readonly string Complete_lvl_3 = "complete_lvl_3";
    private static readonly string Complete_lvl_5 = "complete_lvl_5";
    private static readonly string Complete_lvl_7 = "complete_lvl_7";
    private static readonly string Complete_lvl_10 = "complete_lvl_10";
    private static readonly string Complete_lvl_15 = "complete_lvl_15";
    private static readonly string Complete_lvl_20 = "complete_lvl_20";
    private static readonly string Complete_lvl_30 = "complete_lvl_30";
    private static readonly string Complete_lvl_50 = "complete_lvl_50";
    
    private static readonly string Complete_five_ads = "complete_five_ads";

    private static readonly string Ad_SpinWheel = "ad_spin_wheel";
    private static readonly string Ad_CoinsReward = "ad_coins";
    private static readonly string Ad_GetDoubleCoins = "ad_double_coins";

    private static readonly string Buy_Gold_500 = "buy_gold_500";
    private static readonly string Buy_Gold_1100 = "buy_gold_1100";
    private static readonly string Buy_Gold_2300 = "buy_gold_2300";
    private static readonly string Buy_Gold_5000 = "buy_gold_5000";
    private static readonly string Buy_Gold_12000 = "buy_gold_12000";
    private static readonly string Buy_Gold_27000 = "buy_gold_27000";
    private static readonly string Buy_Starter_Pack = "buy_starter_pack";
    private static readonly string Buy_Big_Pack = "buy_big_pack";
    private static readonly string Buy_Legendary_Pack = "buy_legendary_pack";
    private static readonly string Buy_Secret_Small_Pack = "buy_secret_small_pack";
    private static readonly string Buy_Secret_Big_Pack = "buy_secret_big_pack";
    private static readonly string Buy_Secret_Bonus_Pack = "buy_secret_bonus_pack";

    private static readonly string Lose_Game = "lose";
    private static readonly string Win_Game = "win";

    private void Start()
    {
        main = this;
    }
    public void AdImpression() => FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression);
    public void LevelStart() => FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    public void SendEventToFirebase(FirebaseEventType type) => FirebaseAnalytics.LogEvent(GetEventKey(type));
    public void CompleteLvl(int lvl, bool win)
    {
        Debug.Log($"Lvl: {lvl}");
        if (win) FirebaseAnalytics.LogEvent(GetEventKey(FirebaseEventType.WinGame), "Lvl:", lvl);
        else FirebaseAnalytics.LogEvent(GetEventKey(FirebaseEventType.LoseGame), "Lvl:", lvl);

        if (lvl == 1) SendEventToFirebase(FirebaseEventType.Complete_lvl_1);
        else if (lvl == 3) SendEventToFirebase(FirebaseEventType.Complete_lvl_3);
        else if (lvl == 5) SendEventToFirebase(FirebaseEventType.Complete_lvl_5);
        else if (lvl == 7) SendEventToFirebase(FirebaseEventType.Complete_lvl_7);
        else if (lvl == 10) SendEventToFirebase(FirebaseEventType.Complete_lvl_10);
        else if (lvl == 15) SendEventToFirebase(FirebaseEventType.Complete_lvl_15);
        else if (lvl == 20) SendEventToFirebase(FirebaseEventType.Complete_lvl_20);
        else if (lvl == 30) SendEventToFirebase(FirebaseEventType.Complete_lvl_30);
        else if (lvl == 50) SendEventToFirebase(FirebaseEventType.Complete_lvl_50);
    }
    private string GetEventKey(FirebaseEventType type)
    {
        switch (type)
        {
            case FirebaseEventType.Complete_lvl_1: return Complete_lvl_1;
            case FirebaseEventType.Complete_lvl_3: return Complete_lvl_3;
            case FirebaseEventType.Complete_lvl_5: return Complete_lvl_5;
            case FirebaseEventType.Complete_lvl_7: return Complete_lvl_7;
            case FirebaseEventType.Complete_lvl_10: return Complete_lvl_10;
            case FirebaseEventType.Complete_lvl_15: return Complete_lvl_15;
            case FirebaseEventType.Complete_lvl_20: return Complete_lvl_20;
            case FirebaseEventType.Complete_lvl_30: return Complete_lvl_30;
            case FirebaseEventType.Complete_lvl_50: return Complete_lvl_50;
            case FirebaseEventType.Complete_five_ads: return Complete_five_ads;
            case FirebaseEventType.AdSpinWheel: return Ad_SpinWheel;
            case FirebaseEventType.AdCoins: return Ad_CoinsReward;
            case FirebaseEventType.AdGetDoubleCoins: return Ad_GetDoubleCoins;
            case FirebaseEventType.BuyGold_500: return Buy_Gold_500;
            case FirebaseEventType.BuyGold_1100: return Buy_Gold_1100;
            case FirebaseEventType.BuyGold_2300: return Buy_Gold_2300;
            case FirebaseEventType.BuyGold_5000: return Buy_Gold_5000;
            case FirebaseEventType.BuyGold_12000: return Buy_Gold_12000;
            case FirebaseEventType.BuyGold_27000: return Buy_Gold_27000;
            case FirebaseEventType.BuyStarterPack: return Buy_Starter_Pack;
            case FirebaseEventType.BuyBigPack: return Buy_Big_Pack;
            case FirebaseEventType.BuyLegendaryPack: return Buy_Legendary_Pack;
            case FirebaseEventType.BuySecretSmallPack: return Buy_Secret_Small_Pack;
            case FirebaseEventType.BuySecretBigPack: return Buy_Secret_Big_Pack;
            case FirebaseEventType.BuySecretBonusPack: return Buy_Secret_Bonus_Pack;
            case FirebaseEventType.WinGame: return Win_Game;
            case FirebaseEventType.LoseGame: return Lose_Game;

            default:return null;
        }
    }
}

public enum FirebaseEventType
{
    Complete_lvl_1,
    Complete_lvl_3,
    Complete_lvl_5,
    Complete_lvl_7,
    Complete_lvl_10,
    Complete_lvl_15,
    Complete_lvl_20,
    Complete_lvl_30,
    Complete_lvl_50,
   
    Complete_five_ads,

    AdSpinWheel,
    AdCoins,
    AdGetDoubleCoins,
    
    BuyGold_500,
    BuyGold_1100,
    BuyGold_2300,
    BuyGold_5000,
    BuyGold_12000,
    BuyGold_27000,
    BuyStarterPack,
    BuyBigPack,
    BuyLegendaryPack,
    BuySecretSmallPack,
    BuySecretBigPack,
    BuySecretBonusPack,

    LoseGame,
    WinGame
}
