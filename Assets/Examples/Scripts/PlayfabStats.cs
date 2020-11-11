using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.Json;
using UnityEngine;

public class PlayfabStats : MonoBehaviour
{
    public int playerLevel;
    public int gameLevel;
    public int playerHealth;
    public int playerHighScore;

    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "playerLevel", Value = playerLevel },
                new StatisticUpdate { StatisticName = "gameLevel", Value = gameLevel },
                new StatisticUpdate { StatisticName = "playerHealth", Value = playerHealth },
                new StatisticUpdate { StatisticName = "playerHighScore", Value = playerHighScore },
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

    public void Cloud_SetStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SetPlayerLevel",
            FunctionParameter = new { playerLevel = playerLevel, gameLevel = gameLevel, playerHealth = playerHealth, playerHighScore = playerHighScore },
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloud_SetStats, OnErrorShared);
    }

    private static void OnCloud_SetStats(ExecuteCloudScriptResult result)
    {
        // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
        //JsonObject jsonResult = (JsonObject)result.FunctionResult;
        //object messageValue;
        //jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
        //Debug.Log((string)messageValue);
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void CloudIncrement()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "IncrementReadOnlyUserData"
        },
        result => Debug.Log("CloudScript call successful"),
        error => {
            Debug.Log("CloudScript call failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
