using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LanguageController : MonoBehaviour
{
    private static Dictionary<string, List<string>> Languages;

    [SerializeField] private TextAsset _locolizeXml;
    private void Awake()
    {
        LoadLocalization();
    }
    private void LoadLocalization()
    {
        Languages = new Dictionary<string, List<string>>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(_locolizeXml.text);

        foreach (XmlNode key in xmlDocument["Keys"].ChildNodes)
        {
            string keyString = key.Attributes["name"].Value;
            var values = new List<string>();
            foreach (XmlNode translate in key["translate"].ChildNodes)
            {
                values.Add(translate.InnerText);
            }

            Languages[keyString] = values;
        }
    }

    /// <summary>
    /// *0 - Kyrgyz* ///
    /// *1 - Russia* ///
    /// *2 - English* ///
    /// *3 - German* /// 
    /// *4 - Korean* ///
    /// *5 - Japan* 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public static string GetTranslate(string key, int id)
    {
        if (Languages.ContainsKey(key))
        {
            return Languages[key][id];
        }
        return key;
    }
    public static string GetTowerNameKey(TowerType tower)
    {
        switch (tower)
        {
            case TowerType.Collector: return TranslateKeys.TowerCollectorName;
            case TowerType.Hammer: return TranslateKeys.TowerHammerName;
            case TowerType.Laser: return TranslateKeys.TowerLaserName;
            case TowerType.Mortar: return TranslateKeys.TowerMortarName;
            case TowerType.Pistol: return TranslateKeys.TowerPistolName;
        }
        return null;
    }
    public static string GetResourcesNameKey(ResourcesType type)
    {
        switch (type)
        {
            case ResourcesType.BombaBoost: return TranslateKeys.GrenadeName;
            case ResourcesType.ExtraLife: return TranslateKeys.ExtraLifeName;
            case ResourcesType.StartGems: return TranslateKeys.StartGemsName;
            case ResourcesType.FreezBoost: return TranslateKeys.FreezName;
            case ResourcesType.LavaBoost: return TranslateKeys.LavaName;
            case ResourcesType.Mina: return TranslateKeys.MinaName;
            case ResourcesType.Gold: return TranslateKeys.GoldName;
            default: return type.ToString();
        }
    }
    public static string GetEnemyNameKey(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Boss_1: return TranslateKeys.Bos_1;
            case EnemyType.Boss_2: return TranslateKeys.Bos_2;
            case EnemyType.Boss_3: return TranslateKeys.Bos_3;
            case EnemyType.Boss_4: return TranslateKeys.Bos_4;
            case EnemyType.Boss_5: return TranslateKeys.Bos_5;

            case EnemyType.Enemy_1: return TranslateKeys.Boximon_1;
            case EnemyType.Enemy_2: return TranslateKeys.Boximon_2;
            case EnemyType.Enemy_3: return TranslateKeys.Boximon_3;
            case EnemyType.Enemy_4: return TranslateKeys.Boximon_4;
            case EnemyType.Enemy_5: return TranslateKeys.Boximon_5;

            case EnemyType.BossSpider_1: return TranslateKeys.BossSpider_1;
            case EnemyType.BossSpider_2: return TranslateKeys.BossSpider_2;
            case EnemyType.BossSpider_3: return TranslateKeys.BossSpider_3;

            case EnemyType.Spider_1: return TranslateKeys.Spider_1;
            case EnemyType.Spider_2: return TranslateKeys.Spider_2;
            case EnemyType.Spider_3: return TranslateKeys.Spider_3;

            case EnemyType.MBat: return TranslateKeys.MBat;
            case EnemyType.MDragon: return TranslateKeys.MDragon;
            case EnemyType.MGolem: return TranslateKeys.MGolem;
            case EnemyType.MGolemChild: return TranslateKeys.MGolemChild;
            case EnemyType.MPlant: return TranslateKeys.MPlant;
            case EnemyType.MSkeleton: return TranslateKeys.MSkeleton;
            case EnemyType.MSlime: return TranslateKeys.MSlime;
            case EnemyType.MSpider: return TranslateKeys.MSpider;
            case EnemyType.MTurtle: return TranslateKeys.MTurtle;
        }
        return TranslateKeys.MBat;
    }
    public static string GetEnemyDescriptionKey(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Boss_1: return TranslateKeys.Bos_Description_1;
            case EnemyType.Boss_2: return TranslateKeys.Bos_Description_2;
            case EnemyType.Boss_3: return TranslateKeys.Bos_Description_3;
            case EnemyType.Boss_4: return TranslateKeys.Bos_Description_4;
            case EnemyType.Boss_5: return TranslateKeys.Bos_Description_5;

            case EnemyType.Enemy_1: return TranslateKeys.Boximon_Description_1;
            case EnemyType.Enemy_2: return TranslateKeys.Boximon_Description_2;
            case EnemyType.Enemy_3: return TranslateKeys.Boximon_Description_3;
            case EnemyType.Enemy_4: return TranslateKeys.Boximon_Description_4;
            case EnemyType.Enemy_5: return TranslateKeys.Boximon_Description_5;

            case EnemyType.BossSpider_1: return TranslateKeys.BossSpider_Description_1;
            case EnemyType.BossSpider_2: return TranslateKeys.BossSpider_Description_2;
            case EnemyType.BossSpider_3: return TranslateKeys.BossSpider_Description_3;

            case EnemyType.Spider_1: return TranslateKeys.Spider_Description_1;
            case EnemyType.Spider_2: return TranslateKeys.Spider_Description_2;
            case EnemyType.Spider_3: return TranslateKeys.Spider_Description_3;

            case EnemyType.MBat: return TranslateKeys.MBatDescription;
            case EnemyType.MDragon: return TranslateKeys.MDragonDescription;
            case EnemyType.MGolem: return TranslateKeys.MGolemDescription;
            case EnemyType.MGolemChild: return TranslateKeys.MGolemChildDescription;
            case EnemyType.MPlant: return TranslateKeys.MPlantDescription;
            case EnemyType.MSkeleton: return TranslateKeys.MSkeletonDescription;
            case EnemyType.MSlime: return TranslateKeys.MSlimeDescription;
            case EnemyType.MSpider: return TranslateKeys.MSpiderDescription;
            case EnemyType.MTurtle: return TranslateKeys.MTurtleDescription;
        }
        return TranslateKeys.MBat;
    }
    public static string GetEnemyHealthKey(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Boss_1: return TranslateKeys.Boss_1_Health;
            case EnemyType.Boss_2: return TranslateKeys.Boss_2_Health;
            case EnemyType.Boss_3: return TranslateKeys.Boss_3_Health;
            case EnemyType.Boss_4: return TranslateKeys.Boss_4_Health;
            case EnemyType.Boss_5: return TranslateKeys.Boss_5_Health;

            case EnemyType.Enemy_1: return TranslateKeys.Boximon_1_Health;
            case EnemyType.Enemy_2: return TranslateKeys.Boximon_2_Health;
            case EnemyType.Enemy_3: return TranslateKeys.Boximon_3_Health;
            case EnemyType.Enemy_4: return TranslateKeys.Boximon_4_Health;
            case EnemyType.Enemy_5: return TranslateKeys.Boximon_5_Health;

            case EnemyType.BossSpider_1: return TranslateKeys.BossSpider_1_Health;
            case EnemyType.BossSpider_2: return TranslateKeys.BossSpider_2_Health;
            case EnemyType.BossSpider_3: return TranslateKeys.BossSpider_3_Health;

            case EnemyType.Spider_1: return TranslateKeys.Spider_1_Health;
            case EnemyType.Spider_2: return TranslateKeys.Spider_2_Health;
            case EnemyType.Spider_3: return TranslateKeys.Spider_3_Health;

            case EnemyType.MBat: return TranslateKeys.MBatHealth;
            case EnemyType.MDragon: return TranslateKeys.MDragonHealth;
            case EnemyType.MGolem: return TranslateKeys.MGolemHealth;
            case EnemyType.MGolemChild: return TranslateKeys.MGolemChildHealth;
            case EnemyType.MPlant: return TranslateKeys.MPlantHealth;
            case EnemyType.MSkeleton: return TranslateKeys.MSkeletonHealth;
            case EnemyType.MSlime: return TranslateKeys.MSlimeHealth;
            case EnemyType.MSpider: return TranslateKeys.MSpiderHealth;
            case EnemyType.MTurtle: return TranslateKeys.MTurtleHealth;
        }
        return TranslateKeys.MBat;
    }
    public static string GetEnemySpeedKey(EnemySpeed type)
    {
        switch (type)
        {
            case EnemySpeed.Fast: return TranslateKeys.Enemy_Fast_Speed;
            case EnemySpeed.Medium: return TranslateKeys.Enemy_Medium_Speed;
            case EnemySpeed.Low: return TranslateKeys.Enemy_Low_Speed;
        }
        return TranslateKeys.Enemy_Medium_Speed;
    }
    public static string GetEnemySuperKnowlegeKey(SuperEnemies type)
    {
        switch (type)
        {
            case SuperEnemies.Bird: return TranslateKeys.Bird;
            case SuperEnemies.Gidra: return TranslateKeys.Gidra;
            case SuperEnemies.Shield: return TranslateKeys.Shield;
            case SuperEnemies.Healer: return TranslateKeys.Healer;
            case SuperEnemies.Fat: return TranslateKeys.Fat;
            case SuperEnemies.Leader: return TranslateKeys.Leader;
        }
        return TranslateKeys.Fat;
    }
}

public static class TranslateKeys
{
    public static readonly string Enemy_Low_Speed = "Low";
    public static readonly string Enemy_Medium_Speed = "Medium";
    public static readonly string Enemy_Fast_Speed = "Fast";

    public static readonly string Bird = "Bird";
    public static readonly string Gidra = "Gidra";
    public static readonly string Shield = "Shield";
    public static readonly string Healer = "Healer";
    public static readonly string Fat = "Fat";
    public static readonly string Leader = "Leader";

    public static readonly string Boss_1_Health = "Bos1Health";
    public static readonly string Boss_2_Health = "Bos2Health";
    public static readonly string Boss_3_Health = "Bos3Health";
    public static readonly string Boss_4_Health = "Bos4Health";
    public static readonly string Boss_5_Health = "Bos5Health";

    public static readonly string Boximon_1_Health = "Box1Health";
    public static readonly string Boximon_2_Health = "Box2Health";
    public static readonly string Boximon_3_Health = "Box3Health";
    public static readonly string Boximon_4_Health = "Box4Health";
    public static readonly string Boximon_5_Health = "Box5Health";

    public static readonly string BossSpider_1_Health = "BSpider1Health";
    public static readonly string BossSpider_2_Health = "BSpider2Health";
    public static readonly string BossSpider_3_Health = "BSpider3Health";

    public static readonly string Spider_1_Health = "Spider1Health";
    public static readonly string Spider_2_Health = "Spider2Health";
    public static readonly string Spider_3_Health = "Spider3Health";

    public static readonly string MSlimeHealth = "MslimeHealth";
    public static readonly string MTurtleHealth = "MTurtleHealth";
    public static readonly string MSpiderHealth = "MSpiderHealth";
    public static readonly string MPlantHealth = "MPlantHealth";
    public static readonly string MSkeletonHealth = "MSkeletonHealth";
    public static readonly string MBatHealth = "MBatHealth";
    public static readonly string MDragonHealth = "MDragonHealth";
    public static readonly string MGolemHealth = "MGolemHealth";
    public static readonly string MGolemChildHealth = "MGolemChildHealth";

    public static readonly string Bos_1 = "Bos1";
    public static readonly string Bos_2 = "Bos2";
    public static readonly string Bos_3 = "Bos3";
    public static readonly string Bos_4 = "Bos4";
    public static readonly string Bos_5 = "Bos5";
    public static readonly string Bos_Description_1 = "BosDescription1";
    public static readonly string Bos_Description_2 = "BosDescription2";
    public static readonly string Bos_Description_3 = "BosDescription3";
    public static readonly string Bos_Description_4 = "BosDescription4";
    public static readonly string Bos_Description_5 = "BosDescription5";

    public static readonly string Boximon_1 = "Boximon1";
    public static readonly string Boximon_2 = "Boximon2";
    public static readonly string Boximon_3 = "Boximon3";
    public static readonly string Boximon_4 = "Boximon4";
    public static readonly string Boximon_5 = "Boximon5";
    public static readonly string Boximon_Description_1 = "BoximonDescription1";
    public static readonly string Boximon_Description_2 = "BoximonDescription2";
    public static readonly string Boximon_Description_3 = "BoximonDescription3";
    public static readonly string Boximon_Description_4 = "BoximonDescription4";
    public static readonly string Boximon_Description_5 = "BoximonDescription5";

    public static readonly string BossSpider_1 = "BossSpider1";
    public static readonly string BossSpider_2 = "BossSpider2";
    public static readonly string BossSpider_3 = "BossSpider3";
    public static readonly string BossSpider_Description_1 = "BossSpiderDescription1";
    public static readonly string BossSpider_Description_2 = "BossSpiderDescription2";
    public static readonly string BossSpider_Description_3 = "BossSpiderDescription3";

    public static readonly string Spider_1 = "Spider1";
    public static readonly string Spider_2 = "Spider2";
    public static readonly string Spider_3 = "Spider3";
    public static readonly string Spider_Description_1 = "SpiderDescription1";
    public static readonly string Spider_Description_2 = "SpiderDescription2";
    public static readonly string Spider_Description_3 = "SpiderDescription3";

    public static readonly string MSlime = "Mslime";
    public static readonly string MTurtle = "MTurtle";
    public static readonly string MSpider = "MSpider";
    public static readonly string MPlant = "MPlant";
    public static readonly string MSkeleton = "MSkeleton";
    public static readonly string MBat = "MBat";
    public static readonly string MDragon = "MDragon";
    public static readonly string MGolem = "MGolem";
    public static readonly string MGolemChild = "MGolemChild";
    public static readonly string MSlimeDescription = "MslimeDescription";
    public static readonly string MTurtleDescription = "MTurtleDescription";
    public static readonly string MSpiderDescription = "MSpiderDescription";
    public static readonly string MPlantDescription = "MPlantDescription";
    public static readonly string MSkeletonDescription = "MSkeletonDescription";
    public static readonly string MBatDescription = "MBatDescription";
    public static readonly string MDragonDescription = "MDragonDescription";
    public static readonly string MGolemDescription = "MGolemDescription";
    public static readonly string MGolemChildDescription = "MGolemChildDescription";

    public static readonly string StarterPack = "StarterPack";
    public static readonly string BigPack = "BigPack";
    public static readonly string LegendaryPack = "LegendaryPack";
    public static readonly string SecretSmallPack = "SecretSmallPack";
    public static readonly string SecretBigPack = "SecretBigPack";
    public static readonly string SecretBonusesPack = "SecretBonusesPack";

    public static readonly string TowerPistolName = "TowerPistolName";
    public static readonly string TowerLaserName = "TowerLaserName";
    public static readonly string TowerMortarName = "TowerMortarName";
    public static readonly string TowerHammerName = "TowerHammerName";
    public static readonly string TowerCollectorName = "TowerCollectorName";

    public static readonly string TowerPistolDescription = "TowerPistolDescription";
    public static readonly string TowerLaserDescription = "TowerLaserDescription";
    public static readonly string TowerMortarDescription = "TowerMortarDescription";
    public static readonly string TowerHammerDescription = "TowerHammerDescription";
    public static readonly string TowerCollectorDescription = "TowerCollectorDescription";

    public static readonly string ExtraLifeName = "ExtraLifeName";
    public static readonly string StartGemsName = "StartGemsName";
    public static readonly string MinaName = "MinaName";
    public static readonly string GrenadeName = "GrenadeName";
    public static readonly string LavaName = "LavaName";
    public static readonly string FreezName = "FreezName";
    public static readonly string GoldName = "GoldName";

    public static readonly string ExtraLifeDescription = "ExtraLifeDescription";
    public static readonly string StartGemsDescription = "StartGemsDescription";
    public static readonly string MinaDescription = "MinaDescription";
    public static readonly string GrenadeDescription = "GrenadeDescription";
    public static readonly string LavaDescription = "LavaDescription";
    public static readonly string FreezDescription = "FreezDescription";

    public static readonly string CompleteSomeLevelsDescription = "CompleteSomeLevelsDescription";
    public static readonly string WinBossesDescription = "WinBossesDescription";
    public static readonly string WinUnderGroundEnemiesDescription = "WinUnderGroundEnemiesDescription";
    public static readonly string UseBonusesDescription = "UseBonusesDescription";
    public static readonly string StartWheelDescription = "StartWheelDescription";

    public static readonly string Languages = "Languages";
    public static readonly string Settings = "Settings";
    public static readonly string Popular = "Popular";
    public static readonly string NeedMoreGold = "NeedMoreGold";
    public static readonly string Loading = "Loading";
    public static readonly string InternetError = "InternetError";
    public static readonly string PleaseTryAgainLater = "PleaseTryLater";
    public static readonly string IWon = "IWon";
    public static readonly string TakeReward = "TakeReward";

    public static readonly string TutorialEvilMag_1 = "evilMag_1";
    public static readonly string TutorialEvilMag_2 = "evilMag_2";
    public static readonly string TutorialEvilMag_3 = "evilMag_3";
    public static readonly string TutorialEvilMag_4 = "evilMag_4";

    public static readonly string TutorialBuildTower = "TutorialBuildTower";
    public static readonly string TutorialSelectTower = "TutorialSelectTower";
    public static readonly string TutorialSelectImproveTower = "TutorialSelectImproveTower";
    public static readonly string TutorialImproveTower = "TutorialImproveTower";
    public static readonly string TutorialUseBonus = "TutorialUseBonus";
    
    public static readonly string ClickToContinue = "clickToContinue";

    public static readonly string UnderGroundRewardInfo = "UnderGroundRewardInfo";

    public static readonly string WaveCome = "WaveCome";

    public static readonly string UndergroundName = "UndergroundName";
    public static readonly string UndergroundDiscription = "UndergroundDiscription";

    public static readonly string Victory = "Victory";
    public static readonly string Rate = "Rate";

    public static readonly string Watch = "WatchAd";
    public static readonly string SpinWheelCompleted = "SpinWheelCompleted";
    public static readonly string SpinWheelLater = "SpinWheelLater";
    public static readonly string SpinWheelAvailable = "SpinWheelAvailable";
}
