﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCatItem : MonoBehaviour
{
    public Animator animator;

    public int index;
    public Text catName;
    public Image catImage;    
    public Text levelText;
    public Image priceImage;
    public Text priceText;    
    public GameObject selectEffect;
    public List<GameObject> buttonList = new List<GameObject>();

    bool isLeader = false;
    MercenaryData mercenaryData;

    public void SetCatItem(int _index, MercenaryData _mercenaryData)
    {
        index = _index;
        mercenaryData = _mercenaryData;

        catName.text = mercenaryData.name;
        catImage.sprite = GameDataManager.Instance.userResourceData.mercenaryResourceDataList[(int)mercenaryData.type].catImage;
        levelText.text = string.Format("Lv.{0}", mercenaryData.level);
        if (mercenaryData.price == 0)
        {
            priceImage.gameObject.SetActive(false);            
        }

        priceText.text = string.Format("{0}", mercenaryData.price);

        animator.runtimeAnimatorController = GameDataManager.Instance.userResourceData.mercenaryResourceDataList[(int)mercenaryData.type].uiRuntimeAnimator;

        if (GameDataManager.Instance.userData.leaderData.name == mercenaryData.name)
        {
            isLeader = true;            
        }
        else
        {
            isLeader = false;
            
        }

        if (isLeader == false)
        {
            selectEffect.SetActive(GameDataManager.Instance.isDeckMercenary(mercenaryData));
        }
        else
        {
            selectEffect.SetActive(true);
        }

        if (_mercenaryData.mercenaryGetType == MercenaryGetType.Video)
        {
            buttonList[0].gameObject.SetActive(false);
            buttonList[1].gameObject.SetActive(false);
            buttonList[2].gameObject.SetActive(true);
        }
        else
        {
            if (isLeader == true)
            {
                buttonList[0].gameObject.SetActive(true);
                buttonList[1].gameObject.SetActive(false);
                buttonList[2].gameObject.SetActive(false);
            }
            else
            {
                if (GameDataManager.Instance.isBuyItem(mercenaryData.price) == true)
                {
                    buttonList[0].gameObject.SetActive(true);
                    buttonList[1].gameObject.SetActive(false);
                    buttonList[2].gameObject.SetActive(false);
                }
                else
                {
                    buttonList[0].gameObject.SetActive(false);
                    buttonList[1].gameObject.SetActive(true);
                    buttonList[2].gameObject.SetActive(false);
                }
            }
        }
    }

    public void RefreshCatItem()
    {
        SetCatItem(index, mercenaryData);
    }

    public void SelectButton()
    {
        if (GameDataManager.Instance.isGetMercenaryCat(mercenaryData.name) == false || isLeader == true)
        {
            return;
        }

        if (GameDataManager.Instance.userData.mercenaryDataList.Count < 2)
        {
            if (selectEffect.gameObject.activeSelf == false)
            {
                GameManager.Instance.SelectMercenary(mercenaryData.name);
            }
            else
            {
                GameManager.Instance.UnSelectMercenary(mercenaryData.name);
            }

            selectEffect.gameObject.SetActive(!selectEffect.gameObject.activeSelf);
        }
        else
        {
            if (GameDataManager.Instance.isDeckMercenary(mercenaryData))
            {
                if (selectEffect.gameObject.activeSelf == false)
                {
                    GameManager.Instance.SelectMercenary(mercenaryData.name);
                }
                else
                {
                    GameManager.Instance.UnSelectMercenary(mercenaryData.name);
                }

                selectEffect.gameObject.SetActive(!selectEffect.gameObject.activeSelf);
            }
        }
    }

    public void UpgrageButton()
    {
        if (isLeader == true)
        {
            UIManager.Instance.lobbyUI.HeroButton();
        }
        else
        {
            GameManager.Instance.BuyMercenary(index);
        }        
    }

    public void VideoButton()
    {

    }
}
