﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState
    {
        Loading,
        Lobby,
        InGame
    }
    [Header("Game State")]
    public GameState state = GameState.Lobby;

    [Header("InGame")]
    [HideInInspector]
    public Vector2 minScreenPos;
    [HideInInspector]
    public Vector2 maxScreenPos;

    public int gameOverCount = 0;

    public int nowStageScore = 0;
    public int nowStageCoin = 0;

    public bool isGameOver = false;
    public bool isStageClear = false;
    public bool isBossReady = false;
    public bool isPause = false;

    [Header("InGameMode")]
    public bool isSlowMode = false;
    public bool isFrezeMode = false;
    public bool isSpeedMode = false;
    public bool isPowerMode = false;
    public bool isMoneyMode = false;
    public bool isShieldMode = false;
    public bool isGiantMode = false;

    #region GameMode Value
    [HideInInspector]
    public float slowTime = 0.0f;

    [HideInInspector]
    public float frezeTime = 0.0f;

    [HideInInspector]
    public float speedTime = 0.0f;

    [HideInInspector]
    public float powerTime = 0.0f;

    [HideInInspector]
    public float moneyTime = 0.0f;

    [HideInInspector]
    public float shieldTime = 0.0f;

    [HideInInspector]
    public float giantTime = 0.0f;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    IEnumerator Start()
    {
        ChangeGameState(GameState.Loading);

        //Net.NetManager.Instance.Connect();
        yield return LoadingManager.Instance.SetLoading();

        yield return GameManagerInit();

        yield return GameDataManager.Instance.GetServerVersion();

        GameDataManager.Instance.GetDeviceVer();
    }

    // Start is called before the first frame update
    IEnumerator GameManagerInit()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Screen.SetResolution(9, 16, true);

        Application.targetFrameRate = 60;

        GameDataManager.Instance.SetUserData();
        if(Application.platform == RuntimePlatform.Android 
            || Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.OSXEditor)
        {            
            LoginManager.Instance.DoAutoLogin();
        }

        GameDataManager.Instance.LoadGameData();

        ChangeGameState(GameState.Lobby);

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause == false)
        {
            Time.timeScale = 1;
        }

        if (isPause == true)
        {
            Time.timeScale = 0;
            return;
        }

        if (state == GameState.InGame)
        {
            if (isBossStage() && isBossReady == false)
                return;

            if (isGameOver == true || isStageClear == true)
            {
                PlayerManager.Instance.ChangeAniState(PlayerState.Idle);
            }
            else
            {
                MonsterManager.Instance.MonsterManagerUpdate();

                if (Input.GetMouseButton(0))
                {
                    Vector2 target = new Vector2(Input.mousePosition.x, 0);
                    target = Camera.main.ScreenToWorldPoint(target);
                    target.x = Mathf.Clamp(target.x, minScreenPos.x, maxScreenPos.x);
                    PlayerManager.Instance.transform.DOMove(new Vector2(target.x, -3.89f), 0.2f);

                    PlayerManager.Instance.PlayerShot();
                    MercenaryManager.Instance.MercenaryShot();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    PlayerManager.Instance.ChangeAniState(PlayerState.Idle);
                }

                PlayerManager.Instance.PlayerUpdate();

                MercenaryManager.Instance.MercenaryMovePoint();

                UIManager.Instance.InGameUIUpdate();

                SetMilkMode();
            }            
        }
        else if (state == GameState.Lobby)
        {
            UIManager.Instance.LobbyUIUpdate();
        }
    }

    public void SetMilkMode()
    {
        if (isGameOver == true)
            return;

        if (isStageClear == true)
            return;        

        if (isSlowMode == true)
        {
            slowTime = slowTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(slowTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SLOW), MilkType.SLOW);

            if (slowTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SLOW))
            {
                Debug.Log("Slow End");
                isSlowMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.SLOW);

                slowTime = 0;
            }
        }

        if (isFrezeMode == true)
        {
            frezeTime = frezeTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(frezeTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.FREZE), MilkType.FREZE);

            if (frezeTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.FREZE))
            {
                Debug.Log("Freze End");
                isFrezeMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.FREZE);

                frezeTime = 0;
            }
        }

        if (isSpeedMode == true)
        {
            speedTime = speedTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(speedTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SPEED), MilkType.SPEED);

            if (speedTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SPEED))
            {
                Debug.Log("Speed End");
                isSpeedMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.SPEED);

                speedTime = 0;
            }
        }

        if (isShieldMode == true)
        {
            shieldTime = shieldTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(shieldTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SHIELD), MilkType.SHIELD);

            if (shieldTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.SHIELD))
            {
                Debug.Log("Shield End");
                isShieldMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.SHIELD);

                shieldTime = 0;
            }
        }

        if (isMoneyMode == true)
        {
            moneyTime = moneyTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(moneyTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.MONEY), MilkType.MONEY);

            if (moneyTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.MONEY))
            {
                Debug.Log("Money End");
                isMoneyMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.MONEY);

                moneyTime = 0;
            }
        }

        if (isGiantMode == true)
        {
            giantTime = giantTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(giantTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.GIANT), MilkType.GIANT);

            if (giantTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.GIANT))
            {
                Debug.Log("Giant End");
                isGiantMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.GIANT);

                giantTime = 0;
            }
        }

        if (isPowerMode == true)
        {
            powerTime = powerTime + Time.deltaTime;

            UIManager.Instance.inGameUI.buffInfoController.UpdateBuffUI(powerTime, GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.POWER), MilkType.POWER);

            if (powerTime >= GameDataManager.Instance.GetPlayerUpgradeMilkSKill(MilkType.POWER))
            {
                Debug.Log("Power End");
                isPowerMode = false;
                UIManager.Instance.inGameUI.buffInfoController.DisableBuffInfo(MilkType.POWER);

                powerTime = 0;
            }
        }
    }

    public void ChangeGameState(GameState _state)
    {
        state = _state;

        switch (state)
        {
            case GameState.Loading:
                SetLoading();
                break;
            case GameState.Lobby:
                SetLobby();
                break;
            case GameState.InGame:
                SetInGame();
                break;
        }
    }

    #region Loading
    public void SetLoading()
    {
        UIManager.Instance.SetLoadingUI();
    }
    #endregion

    #region Lobby
    public void SetLobby()
    {        
        PlayerManager.Instance.SetLobbyInit();
        MonsterManager.Instance.SetLobbyInit();
        BulletManager.Instance.SetLobbyInit();
        RpcServiceManager.Instance.SetLobbyInit();
        MercenaryManager.Instance.SetLobbyInit();
        EffectManager.Instance.SetLobbyInit();

        UIManager.Instance.SetLobbyUI();
        SoundManager.Instance.LobbyBGMSound();
        StageManager.Instance.SetLobbyStage();
    }

    public void BuyMercenary(int _num)
    {
        Debug.Log("BuyMercenary");
        GameDataManager.Instance.BuyMercenary(_num);
        UIManager.Instance.lobbyUI.shopController.RefreshCatShopList();
        UIManager.Instance.lobbyUI.deckUIController.SetDeckUI();
        UIManager.Instance.lobbyUI.UpdateCoinText();
    }

    public void SelectMercenary(string _name)
    {
        GameDataManager.Instance.SelectMercenary(GameDataManager.Instance.GetMyMercenaryData(_name));

        UIManager.Instance.lobbyUI.deckUIController.SetDeckUI();
    }

    public void UnSelectMercenary(string _name)
    {
        GameDataManager.Instance.RemoveMercenary(GameDataManager.Instance.GetMyMercenaryData(_name));

        UIManager.Instance.lobbyUI.deckUIController.SetDeckUI();
    }

    #endregion

    #region INGAME
    public void SetInGame()
    {
        Debug.Log("Set InGame");
        
        if (isStageClear == true)
        {
            GameDataManager.Instance.userData.stageNum = GameDataManager.Instance.userData.stageNum + 1;
            GameDataManager.Instance.SaveGameDataStage();


            MonsterManager.Instance.monsterStageCount = 8;
            MonsterManager.Instance.regenTime = MonsterManager.Instance.regenTime - 0.01f;
            MonsterManager.Instance.monsterHp = MonsterManager.Instance.monsterHp + 2;            
        }

        isGameOver = false;
        isStageClear = false;
        isBossReady = false;
        isPause = false;

        slowTime = 0.0f;
        isSlowMode = false;

        frezeTime = 0.0f;
        isFrezeMode = false;

        speedTime = 0.0f;
        isSpeedMode = false;

        powerTime = 0.0f;
        isPowerMode = false;

        moneyTime = 0.0f;
        isMoneyMode = false;

        shieldTime = 0.0f;
        isShieldMode = false;

        giantTime = 0.0f;
        isGiantMode = false;

        gameOverCount = 0;

        minScreenPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxScreenPos = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        
        PlayerManager.Instance.SetInGameInit();
        MonsterManager.Instance.SetInGameInit();        
        BulletManager.Instance.SetInGameInit();
        MercenaryManager.Instance.SetInGameInit();        
        EffectManager.Instance.SetInGameInit();
        UIManager.Instance.SetInGameUI();

        if (isBossStage() == true)
        {
            SoundManager.Instance.StageBossBGMSound();
        }
        else
        {
            SoundManager.Instance.StageBGMSound();
        }

        float num = ((float)GameDataManager.Instance.userData.stageNum % 6);
        if (num == 1)
        {
            StageManager.Instance.SetIngameStage();
        }
        
    }

    public void SetContinueInGame()
    {
        Debug.Log("Set ContinueInGame");

        isGameOver = false;
        isStageClear = false;
        SetMilkItem(GameDataManager.Instance.userData.milkItemList[6]);

        minScreenPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxScreenPos = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        PlayerManager.Instance.SetInGameInit();
        BulletManager.Instance.SetInGameInit();
        UIManager.Instance.SetInGameUI(true);

        if (isBossStage() == true)
        {
            SoundManager.Instance.StageBossBGMSound();
        }
        else
        {
            SoundManager.Instance.StageBGMSound();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverCount = gameOverCount + 1;

        UIManager.Instance.inGameUI.GameOverUI();        
    }

    public void StageClear()
    {
        isStageClear = true;

        UIManager.Instance.inGameUI.StageClearUI();        
    }

    public void UpdateNowStageScore(int _score)
    {
        nowStageScore = nowStageScore + _score;
    }

    public void UpdateNowGetCoin(int _coin)
    {
        nowStageCoin = nowStageCoin + _coin;

        GameDataManager.Instance.userData.userCurrency.userCoin += _coin;
        // 게임 플레이중에서 먹은 코인이라 DB저장하면 안댐
        //GameDataManager.Instance.SaveGameDataItem();

        UIManager.Instance.inGameUI.SetCoinUI();
    }

    public void ResultVideoReward()
    {
        GetCoin(nowStageCoin * 2);
        UpdateScore(nowStageScore);

        nowStageScore = 0;
        nowStageCoin = 0;
    }

    public void ResultReward()
    {
        GetCoin(nowStageCoin);
        UpdateScore(nowStageScore);

        nowStageScore = 0;
        nowStageCoin = 0;
    }

    public void UpdateScore(int _score)
    {
        GameDataManager.Instance.userData.score = GameDataManager.Instance.userData.score + _score;
        if (0 != _score)
        {
            GameDataManager.Instance.SaveGameDataStage();
            AchievementManager.Instance.ReportLeaderBoard(_score);
        }
    }

    public void GetCoin(int _coin)
    {
        GameDataManager.Instance.userData.userCurrency.userCoin += _coin;
        if (0 != _coin)
        {
            //GameDataManager.Instance.SaveGameData(Msg.ITEM_TYPE.Gold, GameDataManager.Instance.userData.userCurrency.userCoin);
            GameDataManager.Instance.SaveGameData();
        }

        UIManager.Instance.inGameUI.SetCoinUI();
    }

    public bool isBossStage()
    {
        bool result = false;
        float num = ((float)GameDataManager.Instance.userData.stageNum % 6);
        if (num == 3 || num == 5)
        {
            result = true;
        }

        return result;
    }

    public void SetMilkItem(MilkItem _item)
    {
        if (_item.type == MilkType.SLOW)
        {
            isSlowMode = true;
            slowTime = 0;           
        }
        else if (_item.type == MilkType.FREZE)
        {
            isFrezeMode = true;
            frezeTime = 0;
        }
        else if (_item.type == MilkType.GIANT)
        {
            isGiantMode = true;
            giantTime = 0;
        }
        else if (_item.type == MilkType.MONEY)
        {
            isMoneyMode = true;
            moneyTime = 0;
        }
        else if (_item.type == MilkType.POWER)
        {
            isPowerMode = true;
            powerTime = 0;
        }
        else if (_item.type == MilkType.SHIELD)
        {
            isShieldMode = true;
            shieldTime = 0;
        }
        else if (_item.type == MilkType.SPEED)
        {
            isSpeedMode = true;
            speedTime = 0;
        }
    }
    #endregion
}
