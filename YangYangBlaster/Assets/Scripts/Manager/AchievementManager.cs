using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class AchievementManager : SingleTon<AchievementManager>
{
    public enum AchievementType
    {
        Login
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ShowAchievement()
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 업적 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 업적 UI 표시 요청
                    Social.ShowAchievementsUI();
                    return;
                }
                else
                {
                    // Sign In 실패 처리
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    public void ShowLeaderboard()
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 리더보드 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 리더보드 UI 표시 요청
                    Social.ShowLeaderboardUI();
                    return;
                }
                else
                {
                    // Sign In 실패 
                    // 그에 따른 처리
                    return;
                }
            });
        }

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#elif UNITY_IOS
        UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
    }

    public void ReportLeaderBoard(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard, (bool bSuccess) =>
        {
            if (bSuccess)
            {
                Debug.Log("ReportLeaderBoard Success");                
            }
            else
            {
                Debug.Log("ReportLeaderBoard Fall");                
            }
        });        
    }

    string GetAchievementName(AchievementType _type)
    {
        string result = null;

        if (_type == AchievementType.Login)
        {
            result = GPGSIds.achievement;
        }

        return result;
    }

    public void ReportAchievement(AchievementType _type)
    {
        string achieveName = GetAchievementName(_type);

        if (achieveName == null)
        {
            Debug.LogError("AchieveMent Null");
            return;
        }
            
        Social.ReportProgress(achieveName, 100f, (bool success) =>
        {
            if (success)
            {
                // Report 성공
                // 그에 따른 처리
                Debug.Log("AddAchievement Success");
            }
            else
            {
                // Report 실패
                // 그에 따른 처리
                Debug.Log("AddAchievement Fail");
            }
        });
    }
}
