using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabLeaderboard : MonoBehaviour
{
    public string leaderBoardName = "playerHighScore";

    //Get the players with the top 10 high scores in the game
    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderBoardName,
            StartPosition = 0,
            MaxResultsCount = 10
        }, DisplayLeaderboard, FailureCallback);
    }

    private void DisplayLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard version: " + result.Version);
        foreach (var entry in result.Leaderboard)
        {
            Debug.Log(entry.DisplayName + " " + entry.StatValue);
        }
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
