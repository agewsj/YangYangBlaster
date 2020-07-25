using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTestManager : SingleTon<ServerTestManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Q))
        {
            GameManager.Instance.StageClear();
        }
#endif
    }

    public void ServerTest()
    {
        Debug.Log("Server Test Button");

        GameDataManager.Instance.DeleteGameData();
    }
}
