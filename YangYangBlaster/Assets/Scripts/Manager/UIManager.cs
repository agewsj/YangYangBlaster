﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("Create Prefab")]
    public GameObject createLoadingUI;
    public GameObject createlobbyUI;
    public GameObject createIngameUI;

    public LoadingUIController loadingUI;
    public LobbyUIController lobbyUI;
    public InGameUIController inGameUI;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LobbyUIUpdate()
    {
        lobbyUI.LobbyUIUpdate();
    }

    public void InGameUIUpdate()
    {
        
    }

    public void SetLoadingUI()
    {
        if (createLoadingUI == null)
        {
            Debug.LogError("UIManager CreateIngameUI is Null !!");
            return;
        }

        if (loadingUI == null)
        {
            GameObject go = Instantiate(createLoadingUI, transform);
            loadingUI = go.GetComponent<LoadingUIController>();
        }

        if (lobbyUI != null)
        {
            lobbyUI.gameObject.SetActive(false);
        }

        if (inGameUI != null)
        {
            inGameUI.gameObject.SetActive(false);
        }

        loadingUI.SetLoadingUI();

        SetMainCanvasScale(loadingUI.GetComponent<CanvasScaler>());
    }


    public void SetLobbyUI()
    {
        if (createlobbyUI == null)
        {
            Debug.LogError("UIManager CreateIngameUI is Null !!");
            return;
        }

        if (lobbyUI == null)
        {
            GameObject go = Instantiate(createlobbyUI, transform);
            lobbyUI = go.GetComponent<LobbyUIController>();
        }

        if (inGameUI != null)
        {
            inGameUI.gameObject.SetActive(false);
        }

        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(false);
        }        

        lobbyUI.gameObject.SetActive(true);
        lobbyUI.OnInitialized();

        SetMainCanvasScale(lobbyUI.GetComponent<CanvasScaler>());
    }

    public void SetInGameUI(bool _isContinue = false)
    {        
        if (createIngameUI == null)
        {
            Debug.LogError("UIManager CreateIngameUI is Null !!");
            return;
        }

        if (inGameUI == null)
        {
            GameObject go = Instantiate(createIngameUI, transform);
            inGameUI = go.GetComponent<InGameUIController>();
        }

        if (lobbyUI != null)
        {
            lobbyUI.gameObject.SetActive(false);
        }

        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(false);
        }

        inGameUI.gameObject.SetActive(true);
        inGameUI.OnInitialized(_isContinue);

        SetMainCanvasScale(inGameUI.GetComponent<CanvasScaler>());
    }

    public void SetMainCanvasScale(CanvasScaler _canvasScaler)
    {
        float mobile_w = Screen.width - 720;
        float mobile_h = Screen.height - 1280;

        if (mobile_w <= 0 && mobile_h <= 0)
        {
            _canvasScaler.matchWidthOrHeight = 0.5f;          
        }
        else
        {
            if (mobile_w > mobile_h)
            {
                //가로가 길어짐
                _canvasScaler.matchWidthOrHeight = 1;
            }
            else
            {
                //세로가 길어짐
                _canvasScaler.matchWidthOrHeight = 0f;
            }
        }
    }
}
