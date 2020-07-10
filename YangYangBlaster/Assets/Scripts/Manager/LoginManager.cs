﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Msg;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class LoginManager : SingleTon<LoginManager>
{
    public bool isWaitLogin = false;
    public UnityEngine.UI.InputField nicknameField = null;

    //private LoginRequest.Types.LOGIN_TYPE loginType_ = 0;
    //private string loginKey_ = "";
    //private string accessKey_ = "";
    //private string nickName_ = "";
    //private int usn_ = 0;

    //private const string PREFIX_PREFS = "yyb_";
    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        LoginInitalize();

#if UNITY_ANDROID
        GoogleLogin();
#elif UNITY_IOS
        IOSLogin();
#endif
        //DoAutoLogin();
    }

    //public void LoadPlayerPrefs()
    //{
    //    loginKey_ = PlayerPrefs.GetString(PREFIX_PREFS + "login_key", "");
    //    loginType_ = (LoginRequest.Types.LOGIN_TYPE)PlayerPrefs.GetInt(
    //        PREFIX_PREFS + "login_type",
    //        (int)LoginRequest.Types.LOGIN_TYPE.NonCert);
    //    nickName_ = PlayerPrefs.GetString(PREFIX_PREFS + "nick_name", "");
    //    GameDataManager.Instance.userData.nickName = 
    //        PlayerPrefs.GetString(PREFIX_PREFS + "nick_name", "");

    //    //nicknameField.text = nickName_;
    //}

    //public void SavePlayerPrefs()
    //{
    //    PlayerPrefs.SetString(PREFIX_PREFS + "login_key", loginKey_);
    //    PlayerPrefs.SetInt(PREFIX_PREFS + "login_type", (int)loginType_);
    //    PlayerPrefs.SetString(PREFIX_PREFS + "nick_name", nickName_);
    //}

    public void LoginInitalize()
    {
#if UNITY_ANDROID
       GoogleInit();
 
#elif UNITY_IOS
        IOSInit();
#endif
    }

    public void LoginDo()
    {
#if UNITY_ANDROID
        GoogleLogin();
#elif UNITY_IOS
        IOSLogin();
#endif
    }

#if UNITY_ANDROID
    void GoogleInit()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
           .EnableSavedGames()
           .Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
    }

    public void GoogleLogin()
    {
        GameDataManager.Instance.userData.loginType = LoginRequest.Types.LOGIN_TYPE.Google;

        var loginType = GameDataManager.Instance.userData.loginType;
        var loginKey = GameDataManager.Instance.userData.loginKey;
        var nickName = GameDataManager.Instance.userData.nickName;

        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login : " + Social.localUser.userName);
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
                string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken() == null ?
                "" : ((PlayGamesLocalUser)Social.localUser).GetIdToken();

                AchievementManager.Instance.ReportAchievement(AchievementManager.AchievementType.Login);

                RpcLogin(loginType, loginKey, nickName, idToken);
            }
            else
            {
                // to do ...
                // 구글 플레이 게임 서비스 로그인 실패 처리
                Debug.Log("Fail");
            }
        });
    }
#endif

#if UNITY_IOS
    void IOSInit()
    {
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
    }

    void IOSLogin()
    {
        GameDataManager.Instance.userData.loginType = LoginRequest.Types.LOGIN_TYPE.Google;

        var loginType = GameDataManager.Instance.userData.loginType;
        var loginKey = GameDataManager.Instance.userData.loginKey;
        var nickName = GameDataManager.Instance.userData.nickName;

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login : " + Social.localUser.userName);
                // to do ...
                // 로그인 성공 처리
                NonCertLogin();
            }
            else
            {
                // to do ...
                // 로그인 실패 처리
                Debug.Log("Fail");
            }
        });
    }
#endif


    public void Login()
    {
#if UNITY_ANDROID
        
#elif UNITY_IOS
        
#endif
        

        
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void DoAutoLogin()
    {
        var loginType = GameDataManager.Instance.userData.loginType;
        var loginKey = GameDataManager.Instance.userData.loginKey;

        if (LoginRequest.Types.LOGIN_TYPE.Google == loginType)
        {
            if (loginKey.Equals(""))
            {
                NonCertLogin();
                return;
            }
#if UNITY_ANDROID
            GoogleLogin();
#endif
        }
        else if (LoginRequest.Types.LOGIN_TYPE.Facebook == loginType)
        {
            NonCertLogin();
        }
        else
        {
            NonCertLogin();
        }
    }

    public void NonCertLogin()
    {
        GameDataManager.Instance.userData.loginType = LoginRequest.Types.LOGIN_TYPE.NonCert;

        var loginType = GameDataManager.Instance.userData.loginType;
        var loginKey = GameDataManager.Instance.userData.loginKey;
        var nickName = GameDataManager.Instance.userData.nickName;
        
        RpcLogin(loginType, loginKey, nickName, "");
    }


    public void RpcLogin(LoginRequest.Types.LOGIN_TYPE loginType,
        string loginKey, string nickName, string idToken)
    {
        LoginModel.Instance.CTosLoginRequest(loginType, loginKey, nickName, idToken);

        return;

        //// 로그인
        //LoginRequest request = new LoginRequest();
        //request.LoginType = loginType;
        ////request.Usn = usn;//Social.localUser.userName == null ? "null" : Social.localUser.userName;
        //request.LoginKey = loginKey;
        //request.NickName = nickName;
        //request.IdToken = idToken;

        //// 요청
        //RpcServiceManager.Instance.Login(request, (LoginReply reply) =>
        //{
        //    // 응답
        //    Debug.Log("LoginReply : " + reply.ToString());

        //    if (ERROR_CODE.Ok == reply.Error)
        //    {
        //        Debug.Log("Ok");

        //        GameDataManager.Instance.userData.loginKey = reply.LoginKey;
        //        GameDataManager.Instance.userData.accessKey = reply.AccessKey;
        //        GameDataManager.Instance.userData.nickName = reply.NickName;
        //        //usn_ = reply.Usn;

        //        GameDataManager.Instance.SaveUserDataLoginParts();
        //        GameDataManager.Instance.LoadGameData();

        //        //// 헬스체크 시작
        //        //HealthCheckRequest healthCheckRequest = new HealthCheckRequest();
        //        //healthCheckRequest.Service = "yyb";

        //        //// 요청
        //        //RpcServiceManager.Instance.Check(healthCheckRequest, 
        //        //    (HealthCheckResponse HealthCheckReply) =>
        //        //{
        //        //    // 응답
        //        //    Debug.Log("Check Response : " + HealthCheckReply.ToString());
        //        //});

        //        //// 요청
        //        //RpcServiceManager.Instance.Watch(healthCheckRequest, 
        //        //    (HealthCheckResponse HealthCheckReply) =>
        //        //{
        //        //    // 응답
        //        //    Debug.Log("Watch Response : " + HealthCheckReply.ToString());
        //        //});
        //    }
        //    else
        //    {
        //        Debug.Log(reply.Error);

        //        DeletePlayerPrefs();
        //    }
        //});
    }

    public void Logout()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.SignOut();
#endif
        Debug.Log("Logout : " + Social.localUser.userName);
    }

}
