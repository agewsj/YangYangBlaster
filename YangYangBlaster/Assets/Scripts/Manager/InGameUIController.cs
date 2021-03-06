﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class InGameUIController : MonoBehaviour
{
    [Header ("TOP")]
    public Text coinText;
    public Text stageText;
    public Text nextStageText;
    public Image stageGauge;
    public BuffInfoController buffInfoController;

    [Header("CENTER")]
    public GameObject continueUI;
    public Image continueGauge;
    public Text continueCountText;
    float nowContinueTime = 0.0f;

    public CanvasGroup bossWarringUI;    

    public GameObject resultUI;
    public GameObject resultOkButton;
    public GameObject resultHomeButton;
    public Text resultScoreText;
    public Text resultCoinText;

    public OptionUIController optionUIController;

    public void OnInitialized(bool _isContinue = false)
    {
        continueUI.SetActive(false);
        resultUI.SetActive(false);

        nowContinueTime = 0;

        SetCoinUI();
        StageUI();

        buffInfoController.SetBuffInfo();

        if (GameManager.Instance.isBossStage() == true)
        {
            if (_isContinue == false)
            {
                BossWarringUI();
            }            
            SetBossStageGaugeUI();
        }
        else
        {
            SetStageGaugeUI();
        }
    }

    private void Update()
    {
        if (continueUI.activeInHierarchy == true)
        {
            nowContinueTime += Time.deltaTime;
            
            if (nowContinueTime >= 10)
            {
                nowContinueTime = 0;

                GameManager.Instance.GameOver();
            }

            continueCountText.text = (10 - nowContinueTime).ToString("N0");
            continueGauge.fillAmount = (10 - nowContinueTime) / 10;
        }
    }

    public void GameOverUI()
    {
        continueUI.SetActive(false);
        resultUI.SetActive(false);

        if (GameManager.Instance.gameOverCount > 1)
        {
            StageClearUI();            
        }
        else
        {
            continueUI.SetActive(true);
        }

        resultHomeButton.SetActive(true);
        resultOkButton.SetActive(false);
    }

    public void StageClearUI()
    {
        resultUI.SetActive(true);
        resultScoreText.text = GameManager.Instance.nowStageScore.ToString();

        if (GameManager.Instance.isStageClear)
        {
            resultHomeButton.SetActive(false);
            resultOkButton.SetActive(true);
        }
        
        //resultCoinText.text = GameManager.Instance.nowStageCoin.ToString();
    }

    public void SetCoinUI()
    {
        coinText.text = GameDataManager.Instance.userData.userCurrency.userCoin.ToString();
        resultCoinText.text = GameManager.Instance.nowStageCoin.ToString();
    }

    public void StageUI()
    {
        stageText.text = GameDataManager.Instance.userData.stageNum.ToString();
        nextStageText.text = (GameDataManager.Instance.userData.stageNum + 1).ToString();
    }

    public void SetStageGaugeUI()
    {
        stageGauge.fillAmount = (float)MonsterManager.Instance.nowMonsterCount / (float)MonsterManager.Instance.monsterStageCount;
    }

    public void SetBossStageGaugeUI()
    {
        stageGauge.fillAmount = 1;
    }

    public void ContinueVideoButton()
    {        
        GoogleAdmobManager.Instance.ShowReward(GameManager.Instance.SetContinueInGame);
    }

    public void ResultVideoButton()
    {
        GoogleAdmobManager.Instance.ShowReward(ResultVideo);
    }

    void ResultVideo()
    {
        if (GameManager.Instance.isStageClear)
        {
            GameManager.Instance.ResultVideoReward();
            GameManager.Instance.SetInGame();
        }
        else
        {
            GameManager.Instance.ResultVideoReward();
            GameManager.Instance.ChangeGameState(GameManager.GameState.Lobby);
        }        
    }

    public void ResultOkButton()
    {
        GameManager.Instance.ResultReward();
        GameManager.Instance.SetInGame();
    }

    public void ExitLobby()
    {
        GameManager.Instance.ResultReward();
        GameManager.Instance.ChangeGameState(GameManager.GameState.Lobby);
    }

    public void OptionButton()
    {
        optionUIController.gameObject.SetActive(true);
        GameManager.Instance.isPause = true;
    }

    public void BossWarringUI()
    {
        bossWarringUI.gameObject.SetActive(true);

        bossWarringUI.DOFade(0, 0.6f).SetLoops(6, LoopType.Yoyo).OnComplete(
            () =>
            {
                bossWarringUI.gameObject.SetActive(false);
                GameManager.Instance.isBossReady = true;
            });
    }
}
