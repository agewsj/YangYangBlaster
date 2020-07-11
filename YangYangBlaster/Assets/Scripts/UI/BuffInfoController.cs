﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfoController : MonoBehaviour
{
    [Header("CreatePrefab")]
    public BuffInfoItem buffInfoGo;
    public List<BuffInfoItem> buffInfoItemList = new List<BuffInfoItem>();    

    public void SetBuffInfo()
    {
        if (buffInfoItemList.Count == 0)
        {
            foreach (MilkItem item in GameDataManager.Instance.userData.milkItemList)
            //for (int i = 0; i < GameDataManager.Instance.userData.milkItemList.Count; i++)
            {
                BuffInfoItem bf = Instantiate(buffInfoGo, transform);
                Sprite sprite = GameDataManager.Instance.userResourceData.milkSpriteList[(int)item.type];

                bf.SetBuffItem(item, sprite);
                bf.gameObject.SetActive(false);

                buffInfoItemList.Add(bf);
            }
        }
        else
        {
            for (int i = 0; i < buffInfoItemList.Count; i++)
            {
                buffInfoItemList[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateBuffUI(float _now, float _max, MilkType _type)
    {
        buffInfoItemList[(int)_type].UpdateMilkGauge(_now, _max);        
    }

    public void DisableBuffInfo(MilkType _type)
    {
        buffInfoItemList[(int)_type].gameObject.SetActive(false);
    }
}
