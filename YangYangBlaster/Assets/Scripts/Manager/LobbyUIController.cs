﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [Header("Center Popup")]
    public LeaderCatShopController leaderCatShopController;
    public ShopController shopController;
    public DeckUIController deckUIController;
    public MilkShopController milkShopController;
    public OptionUIController optionUIController;

    [Header("TOP UI")]
    public Text scoreText;
    public Text coinText;

    [Header("CENTER UI")]
    public Image freeCoinGauge;
    public Text freeCoinText;
    public Text stageText;

    public void OnInitialized()
    {
        deckUIController.SetDeckUI();
        deckUIController.SetStageGauge();

        UpdateScoreText();
        UpdateCoinText();
        UpdateFreeCoinText();
        UpdateStageText();
    }

    public void LobbyUIUpdate()
    {
        TimeSpan sp = GameDataManager.Instance.userData.freeCoinUpdateTime - GameDataManager.Instance.userData.freeCoinGetTime;

        TimeSpan nsp = GameDataManager.Instance.userData.freeCoinUpdateTime - DateTime.Now;

        freeCoinGauge.fillAmount = (float)((sp.TotalSeconds - nsp.TotalSeconds) / sp.TotalSeconds);
    }

    public void AllClosePopup()
    {
        leaderCatShopController.QuitButton();
        shopController.QuitButton();
    }

    public void HeroButton()
    {
        AllClosePopup();

        if (leaderCatShopController.gameObject.activeInHierarchy == false)
        {
            leaderCatShopController.gameObject.SetActive(true);
        }

        leaderCatShopController.SetLeaderCatShop();
    }

    public void FriendButton()
    {
        AllClosePopup();

        if (shopController.gameObject.activeInHierarchy == false)
        {
            shopController.gameObject.SetActive(true);
        }

        shopController.SetCatShopList();
    }

    public void MilkSkillButton()
    {
        AllClosePopup();

        if (milkShopController.gameObject.activeInHierarchy == false)
        {
            milkShopController.gameObject.SetActive(true);
        }

        milkShopController.SetMilkShopList();
    }

    public void ShopButton()
    {
        AllClosePopup();
    }

    public void LoginButton()
    {
#if UNITY_ANDROID
        LoginManager.Instance.GoogleLogin();
#endif
    }

    public void GameStartButton()
    {
        GameManager.Instance.nowStageScore = 0;
        GameManager.Instance.nowStageCoin = 0;
        GameManager.Instance.ChangeGameState(GameManager.GameState.InGame);
    }

    public void UpdateScoreText()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is Null");
            return;
        }

        scoreText.text = GameDataManager.Instance.userData.score.ToString();
    }

    public void UpdateCoinText()
    {
        if (coinText == null)
        {
            Debug.LogError("Coint Text is Null");
            return;
        }

        coinText.text = GameDataManager.Instance.userData.userCurrency.userCoin.ToString();
    }

    public void UpdateFreeCoinText()
    {
        freeCoinText.text = GameDataManager.Instance.GetPlayerUpgradeFreeCoin().ToString();
    }

    public void UpdateStageText()
    {
        stageText.text = string.Format("STAGE {0}", GameDataManager.Instance.userData.stageNum.ToString());
    }

    public void FreeCoinButton()
    {
        TimeSpan sp = GameDataManager.Instance.userData.freeCoinUpdateTime - DateTime.Now;

        if (sp.TotalSeconds <= 0)
        {
            GameDataManager.Instance.userData.userCurrency.userCoin = GameDataManager.Instance.userData.userCurrency.userCoin + GameDataManager.Instance.GetPlayerUpgradeFreeCoin();
            GameDataManager.Instance.SetFreeCoinInfo();

            if (0 != GameDataManager.Instance.freeCoin)
            {
                //GameDataManager.Instance.SaveGameData(Msg.ITEM_TYPE.Gold, GameDataManager.Instance.userData.userCurrency.userCoin);

                GameDataManager.Instance.SaveGameData();
            }

            UpdateFreeCoinText();
            UpdateCoinText();
        }
    }

    public void OptionButton()
    {
        optionUIController.gameObject.SetActive(true);
    }

    public void ShowRewardButton()
    {
        GoogleAdmobManager.Instance.ShowReward();
    }

    public void ServerTestButton()
    {
        ServerTestManager.Instance.ServerTest();
    }

    public void ShowLeaderBoardUI()
    {
        AchievementManager.Instance.ShowLeaderboard();
    }

    public void ShowAchievementUI()
    {
        AchievementManager.Instance.ShowAchievement();
    }
}
