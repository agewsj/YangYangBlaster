using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkShopController : MonoBehaviour
{
    [Header("Create Prefab")]
    public MilkShopItem milkItemPrefab;

    public Transform prefabParent;
    public List<MilkShopItem> milkShopItemList = new List<MilkShopItem>();

    public void SetMilkShopList()
    {
        if (milkShopItemList.Count == 0)
        {
            foreach (MilkItem item in GameDataManager.Instance.userData.milkItemList)
            {
                MilkShopItem ms = Instantiate(milkItemPrefab, prefabParent);
                Sprite sprite = GameDataManager.Instance.userResourceData.milkSpriteList[(int)item.type];
                
                ms.SetMilkShopItem(item, sprite);

                milkShopItemList.Add(ms);
            }
        }
        else
        {
            RefreshMilkShopList();
        }
    }

    public void RefreshMilkShopList()
    {
        for (int i = 0; i < milkShopItemList.Count; i++)
        {
            MilkItem item = GameDataManager.Instance.userData.milkItemList[i];
            Sprite sprite = GameDataManager.Instance.userResourceData.milkSpriteList[(int)item.type];

            milkShopItemList[i].SetMilkShopItem(item, sprite);
        }
    }

    public void QuitButton()
    {
        gameObject.SetActive(false);
    }
}
