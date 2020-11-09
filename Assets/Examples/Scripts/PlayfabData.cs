using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabData : MonoBehaviour
{
    public static PlayfabData ins;

    public int playerLevel;
    public int gameLevel;
    public int playerHealth;
    public int playerHighScore;

    void Awake()
    {
        ins = this;
    }

    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "playerLevel", Value = 5 },
                new StatisticUpdate { StatisticName = "gameLevel", Value = 10 },
                new StatisticUpdate { StatisticName = "playerHealth", Value = 100 },
                new StatisticUpdate { StatisticName = "playerHighScore", Value = 1000 },
            }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);

            if (eachStat.StatisticName == "playerLevel")
                playerLevel = eachStat.Value;
            if (eachStat.StatisticName == "gameLevel")
                gameLevel = eachStat.Value;
            if (eachStat.StatisticName == "playerHealth")
                playerHealth = eachStat.Value;
            if (eachStat.StatisticName == "playerHighScore")
                playerHighScore = eachStat.Value;
        }
    }
}
