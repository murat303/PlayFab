using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabFriends : MonoBehaviour
{
    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    List<FriendInfo> _friends = null;

    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends = result.Friends;

            Debug.Log("My Friends;");
            _friends.ForEach(f => Debug.Log(f.Username));
        }, DisplayPlayFabError);
    }

    public void AddFriend(string friendId)
    {
        FriendIdType idType = FriendIdType.Username;

        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }

    // unlike AddFriend, RemoveFriend only takes a PlayFab ID
    // you can get this from the FriendInfo object under FriendPlayFabId
    void RemoveFriend(FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }

    // this REPLACES the list of tags on the server
    // for updates, make sure this includes the original tag list
    void SetFriendTags(FriendInfo friend, List<string> newTags)
    {
        // update the tags with the edited list
        PlayFabClientAPI.SetFriendTags(new SetFriendTagsRequest
        {
            FriendPlayFabId = friend.FriendPlayFabId,
            Tags = newTags
        }, tagresult => {
            // Make sure to save new tags locally. That way you do not have to hard-update friendlist
            friend.Tags = newTags;
        }, DisplayPlayFabError);
    }

    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
