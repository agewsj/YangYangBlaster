using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

[System.Serializable]
public struct UserData
{
    public int stageNum;
    public int score;
    public UserCurrency userCurrency;
    public DateTime freeCoinGetTime;
    public DateTime freeCoinUpdateTime;
    public UpgradePlayer upgradePlayer;
    public List<MilkItem> milkItemList;   

    public MercenaryData leaderData;
    public List<MercenaryData> mercenaryDataList;
    public Dictionary<string, MercenaryData> getMercenaryDataDic;
}

[System.Serializable]
public struct UserResourceData
{
    public List<Sprite> milkSpriteList;
    public List<MercenaryResourceData> mercenaryResourceDataList;
}

[Serializable]
public struct UpgradePlayer
{
    public int powerLevel;
    public int powerIncrease;
    public int powerPrice;

    public int attackSpeedLevel;
    public float attackSpeedIncrease;
    public int attackSpeedPrice;

    public int criticalLevel;
    public float criticalIncrease;
    public int criticalPrice;

    public int buffDurationLevel;
    public float buffDurationIncrease;
    public int buffDurationPrice;

    public int freeCoinLevel;
    public int freeCoinIncrease;
    public int freeCoinPrice;
}

public enum MilkType
{
    SLOW,
    FREZE,
    SPEED,
    POWER,
    MONEY,
    GIANT,
    SHIELD
}

public enum MercenaryType
{
    MIA,
    MEOWMEOW,
    BABA,
    STAR,
    YAKYAK,
    STUDY,
    COWBOY,
    FISHERMAN
}

[Serializable]
public struct MilkItem //
{
    public MilkType type;
    //public Sprite milkSprite;
    public int milkLevel;
    public float milkDuration;
    public string milkInfo;
}

public enum MercenaryGetType
{
    Gold,
    Ruby,
    Video
}


[System.Serializable]
public struct MercenaryData
{
    public MercenaryType type;
    public string name;
    public int damage;
    public float moveSpeed;
    public float attackSpeed;
    //public Sprite catImage;
    public int level;
    public int price;
    public MercenaryGetType mercenaryGetType;
    //public Sprite bulletImage;
    //public RuntimeAnimatorController runtimeAnimator;
    //public RuntimeAnimatorController uiRuntimeAnimator;
}

[System.Serializable]
public struct MercenaryResourceData
{
    public Sprite catImage;
    public Sprite bulletImage;
    public RuntimeAnimatorController runtimeAnimator;
    public RuntimeAnimatorController uiRuntimeAnimator;
}

[System.Serializable]
public struct BossData
{
    public string name;
    public int pattenType;
    public int hp;
    public float attackTime;
    public Vector2 createPos;
    public Vector2 colliderOffset;
    public float radius;
    public Color deadColor;
    public RuntimeAnimatorController runtimeAnimator;
}

[System.Serializable]
public struct StageData
{
    public int stageNum;
    public int minHp;
    public int maxHp;
    public int divideCount;
    public int spawnTime;
    public int spawnCount;
}

[System.Serializable]
public struct UserCurrency
{
    public int userCoin;
    public int userRubby;

    public int knightPiece;
    public int piratePiece;
    public int starPiece;
    public int scientistPiece;
    public int studentPiece;
    public int cowboyPiece;
    public int fishermanPiece;
}

public class GameDataManager : SingleTon<GameDataManager>
{
    [Header("User Data")]
    [SerializeField]
    public UserData userData;
    public int freeCoin = 0;

    public UserResourceData userResourceData;

    [Header("Mercenary Data")]
    [SerializeField]
    public List<MercenaryData> ReadMercenaryDataList = new List<MercenaryData>();    

    [Header("Stage Data")]
    public List<StageData> stageDataList = new List<StageData>();
    public List<BossData> BossDataList = new List<BossData>();

    public const string PREFIX_PREFS = "yyb_";

    [Header("Version")]
    public string androidVersion;
    public string iosVersion;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadGameData()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/yyb.dat";
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);
                userData = (UserData)bf.Deserialize(file);
                file.Close();

                if (userData.getMercenaryDataDic == null)
                {
                    userData.getMercenaryDataDic = new Dictionary<string, MercenaryData>();
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log("Load game data error");
        }
    }

    public void SaveGameData()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/yyb.dat");
            bf.Serialize(file, userData);
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log("Save game data error");
        }
    }
    public void DeleteGameData()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/yyb.dat");
        }
        catch (IOException e)
        {
            Debug.Log("Delete game data error");
        }
    }


    public void SetUserData()
    {
        userData.userCurrency.userCoin = 0;
        userData.userCurrency.userRubby = 0;
        userData.userCurrency.knightPiece = 0;
        userData.userCurrency.piratePiece = 0;
        userData.userCurrency.starPiece = 0;
        userData.userCurrency.scientistPiece = 0;
        userData.userCurrency.studentPiece = 0;
        userData.stageNum = 1;
        userData.score = 0;
        SetFreeCoinInfo();

        userData.leaderData = ReadMercenaryDataList[0];
        userData.mercenaryDataList = new List<MercenaryData>();
        userData.getMercenaryDataDic = new Dictionary<string, MercenaryData>();
    }

    public void SetFreeCoinInfo()
    {
        userData.freeCoinGetTime = DateTime.Now;
        userData.freeCoinUpdateTime = DateTime.Now.AddMinutes(1);
    }

    public void InsertMercenary(bool isInsert, MercenaryData _mercenaryData)
    {
        if (isInsert == true)
        {
            if (userData.mercenaryDataList.Count > 2)
            {
                return;
            }

            userData.mercenaryDataList.Add(_mercenaryData);
        }
        else
        {
            for (int i = 0; i < userData.mercenaryDataList.Count; i++)
            {
                if (userData.mercenaryDataList[i].name == _mercenaryData.name)
                {
                    userData.mercenaryDataList.Remove(userData.mercenaryDataList[i]);
                }
            }
        }                
    }

    public void BuyMercenary(int _num)
    {
        string key = ReadMercenaryDataList[_num].name;
        int catPrice = 0;
        MercenaryData mercenaryData = ReadMercenaryDataList[_num];

        if (isGetMercenaryCat(key) == true)
        {
            MercenaryData myMercenary = GetMyMercenaryData(key);
            myMercenary.level = myMercenary.level + 1;
            catPrice = myMercenary.price;

            SetMyMercenaryData(key, myMercenary);
        }
        else
        {
            mercenaryData.level = mercenaryData.level + 1;
            catPrice = mercenaryData.price;

            userData.getMercenaryDataDic.Add(key, mercenaryData);
        }

        userData.userCurrency.userCoin = userData.userCurrency.userCoin - catPrice;

        SaveGameData();
    }

    public void SelectMercenary(MercenaryData _mercenaryData)
    {
        if (userData.mercenaryDataList.Count >= 2)
        {
            Debug.LogError("Mercenary Count is Big");
            return;
        }

        userData.mercenaryDataList.Add(_mercenaryData);
    }

    public void RemoveMercenary(MercenaryData _mercenaryData)
    {
        for (int i = 0; i < userData.mercenaryDataList.Count; i++)
        {
            if (userData.mercenaryDataList[i].name == _mercenaryData.name)
            {
                Debug.Log("Mercenary Remove : " + _mercenaryData.name);
                userData.mercenaryDataList.Remove(userData.mercenaryDataList[i]);
                break;
            }
        }        
    }

    public bool isDeckMercenary(MercenaryData _mercenaryData)
    {
        bool result = false;

        for (int i = 0; i < userData.mercenaryDataList.Count; i++)
        {
            if (userData.mercenaryDataList[i].name == _mercenaryData.name)
            {
                result = true;
                break;
            }            
        }

        return result;
    }

    //아이템 살 수 있는 지 여부 확인
    public bool isBuyItem(int _coin)
    {
        bool result = false;

        if (userData.userCurrency.userCoin >= _coin)
        {
            result = true;
        }

        return result;
    }

    #region Data Read
    public bool isGetMercenaryCat(string _name)
    {
        bool isResult = false;

        if (userData.getMercenaryDataDic.ContainsKey(_name) == true)
        {
            isResult = true;
        }

        return isResult;
    }

    public MercenaryData GetReadMercenaryData(string _name)
    {
        MercenaryData data = new MercenaryData();

        for (int i = 0; i < ReadMercenaryDataList.Count; i++)
        {
            if (ReadMercenaryDataList[i].name == _name)
            {
                data = ReadMercenaryDataList[i];
                break;
            }
        }

        return data;
    }

    public MercenaryData GetMyMercenaryData(string _name)
    {
        return userData.getMercenaryDataDic[_name];
    }

    public void SetMyMercenaryData(string _name, MercenaryData _mercenaryData)
    {
        userData.getMercenaryDataDic[_name] = _mercenaryData;
    }

    public float GetPlayerDamage()
    {
        float result = 0;

        result = 1 + ((float)(userData.upgradePlayer.powerLevel * userData.upgradePlayer.powerIncrease) / 100);        

        return result;
    }

    public int GetPlayerUpgradeDamage()
    {
        int result = 0;

        result = 100 + userData.upgradePlayer.powerLevel * userData.upgradePlayer.powerIncrease;

        return result;
    }

    public int GetPlayerUpgradeDamagePrice()
    {
        int result = 0;

        result = userData.upgradePlayer.powerLevel * userData.upgradePlayer.powerPrice;

        return result;
    }

    public float GetPlayerUpgradeAttackSpeed()
    {
        float result = 0;

        result = userData.upgradePlayer.attackSpeedLevel * userData.upgradePlayer.attackSpeedIncrease;

        return result;
    }

    public int GetPlayerUpgradeAttackSpeedPrice()
    {
        int result = 0;

        result = userData.upgradePlayer.attackSpeedLevel * userData.upgradePlayer.attackSpeedPrice;

        return result;
    }
    
    public float getPlayerCritical()
    {
        float result = 0.0f;

        result = (userData.upgradePlayer.criticalLevel * userData.upgradePlayer.criticalIncrease);

        return result;
    }

    public float GetPlayerUpgradeCritical()
    {
        float result = 0;

        result = userData.upgradePlayer.criticalLevel * userData.upgradePlayer.criticalIncrease;

        return result;
    }

    public int GetPlayerUpgradeCriticalPrice()
    {
        int result = 0;

        result = userData.upgradePlayer.criticalLevel * userData.upgradePlayer.criticalPrice;

        return result;
    }

    public float GetPlayerUpgradeBuffSKill()
    {
        float result = 0;

        result = (userData.upgradePlayer.buffDurationLevel * userData.upgradePlayer.buffDurationIncrease);

        return result;
    }

    public float GetPlayerUpgradeMilkSKill(MilkType _type)
    {
        float result = 0;

        result = userData.milkItemList[(int)_type].milkDuration + (userData.upgradePlayer.buffDurationLevel * userData.upgradePlayer.buffDurationIncrease);

        return result;
    }

    public int GetPlayerUpgradeMilkSkillPrice()
    {
        int result = 0;

        result = userData.upgradePlayer.buffDurationLevel * userData.upgradePlayer.buffDurationPrice;

        return result;
    }

    public int GetPlayerUpgradeFreeCoin()
    {
        int result = 0;

        result = freeCoin + (userData.upgradePlayer.freeCoinLevel * userData.upgradePlayer.freeCoinIncrease);

        return result;
    }

    public int GetPlayerUpgradeFreeCoinPrice()
    {
        int result = 0;

        result = userData.upgradePlayer.freeCoinLevel * userData.upgradePlayer.freeCoinPrice;

        return result;
    }

    public void SetUpgradeDamage(int _price)
    {
        userData.userCurrency.userCoin = userData.userCurrency.userCoin - _price;

        userData.upgradePlayer.powerLevel++;

        SaveGameData();
    }

    public void SetUpgradeAttackSpeed(int _price)
    {
        userData.userCurrency.userCoin = userData.userCurrency.userCoin - _price;

        userData.upgradePlayer.attackSpeedLevel++;

        SaveGameData();
    }

    public void SetUpgradeCritical(int _price)
    {
        userData.userCurrency.userCoin = userData.userCurrency.userCoin - _price;

        userData.upgradePlayer.criticalLevel++;

        SaveGameData();
    }

    public void SetUpgradeSkillDamage(int _price)
    {
        userData.userCurrency.userCoin = userData.userCurrency.userCoin - _price;

        userData.upgradePlayer.buffDurationLevel++;

        SaveGameData();
    }

    public void SetUpgradeFreeCoin(int _price)
    {
        userData.userCurrency.userCoin = userData.userCurrency.userCoin - _price;

        userData.upgradePlayer.freeCoinLevel++;

        SaveGameData();
    }

    public bool isUpgrade(int _price)
    {
        bool result = false;

        if (userData.userCurrency.userCoin >= _price)
        {
            result = true;
        }

        return result;
    }
    #endregion

    #region VersionInfo
    [Serializable]
    public class VersionInfo
    {
        [SerializeField]
        public string Android_Live;
        [SerializeField]
        public string Android_Test;
        [SerializeField]
        public string IOS_Live;
        [SerializeField]
        public string IOS_Test;
    }

    public VersionInfo versionInfo;

    public IEnumerator GetServerVersion()
    {
        string uri = "https://raw.githubusercontent.com/agewsj/YangYangBlaster/master/Version.json";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                versionInfo = JsonUtility.FromJson<VersionInfo>(webRequest.downloadHandler.text);

                string ver = string.Format("{0}_{1}", Application.version, GameDataManager.Instance.GetBundleVer());

#if UNITY_ANDROID
                if (ver != versionInfo.Android_Live || ver != versionInfo.Android_Test)
                {
                    CheckUpdateVersion();
                }
#elif UNITY_IOS
                if (ver != versionInfo.IOS_Live || ver != versionInfo.IOS_Test)
                {
                    CheckUpdateVersion();
                }
#endif
            }
        }
    }

    public void CheckUpdateVersion()
    {       
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("market://details?id=com.TongTongStudio.YangYangBlaster");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/id1520506681");
        }
    }

    public string GetBundleVer()
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        return AndroidVersionCode().ToString();
#elif UNITY_IOS
        return GetBundleVersion();
#endif
#endif
        return "1";
    }

    public void GetDeviceVer()
    {
#if UNITY_IOS
        Debug.Log("Device Version : " + GetDeviceVersion());
#endif
    }


#if UNITY_ANDROID
    AndroidJavaObject GetPackageInfo()
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        return packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);
    }

    private int AndroidVersionCode()
    {
        using (var packageInfo = GetPackageInfo())
        {
            return packageInfo.Get<int>("versionCode");
        }
    }
#elif UNITY_IOS
    [DllImport("__Internal")]
    static extern string GetBundleVersion();
    [DllImport("__Internal")]
    static extern string GetDeviceVersion();
#endif

    #endregion

    #region PlayerPrefabsData
    string loginStr = "Login";

    public bool isGetLoginData()
    {
        bool result = false;
        int i = PlayerPrefs.GetInt(loginStr);

        if (i == 0)
        {
            result = false;
            PlayerPrefs.SetInt(loginStr, 1);
        }
        else
        {
            result = true;
        }

        return result;
    }

    string soundStr = "Sound";

    public void SetSoundData(bool _value)
    {
        int i = 0;

        if (_value == false)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }

        PlayerPrefs.SetInt(soundStr, i);
    }

    public bool isGetSoundData()
    {
        bool result = false;
        int i = PlayerPrefs.GetInt(soundStr);

        if (i == 0)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    string vibrationStr = "Vibration";

    public void SetVibrationData(bool _value)
    {
        int i = 0;

        if (_value == false)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }

        PlayerPrefs.SetInt(vibrationStr, i);
    }

    public bool isGetVibrationData()
    {
        bool result = false;
        int i = PlayerPrefs.GetInt(vibrationStr);

        if (i == 0)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }
    #endregion
}
