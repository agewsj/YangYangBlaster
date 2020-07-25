using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class LoginManager : SingleTon<LoginManager>
{
    public bool isWaitLogin = false;
    public UnityEngine.UI.InputField nicknameField = null;
    
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
        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login : " + Social.localUser.userName);
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
                string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken() == null ?
                "" : ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                if (GameDataManager.Instance.isGetLoginData() == false)
                {
                    AchievementManager.Instance.ReportAchievement(AchievementManager.AchievementType.Login);
                }
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
                if (GameDataManager.Instance.isGetLoginData() == false)
                {
                    AchievementManager.Instance.ReportAchievement(AchievementManager.AchievementType.Login);
                }
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

    public void Logout()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.SignOut();
#endif
        Debug.Log("Logout : " + Social.localUser.userName);
    }

}
