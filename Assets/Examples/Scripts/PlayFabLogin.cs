using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    public static PlayFabLogin ins;
    public string playfabID;
    public string entityId;
    public string entityType;

    [SerializeField] InputField userName;
    [SerializeField] InputField userEmail;
    [SerializeField] InputField userPassword;

    [SerializeField] GameObject authPanel;
    [SerializeField] GameObject userPanel;
    [SerializeField] Text txtUserName;

    string email, pass;

    private void Start()
    {
        ins = this;
        if (PlayerPrefs.HasKey("Email"))
        {
            email = PlayerPrefs.GetString("Email");
            pass = PlayerPrefs.GetString("Pass");
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
                Login();
        }
        else
        {
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginSuccessMobile, OnLoginFailure);
#endif
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginSuccessMobile, OnLoginFailure);
#endif
        }
    }

    private string ReturnMobileID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    public void BtnRegister()
    {
        email = userEmail.text;
        pass = userPassword.text;
        var request = new RegisterPlayFabUserRequest { Email = email, Password = pass, Username = userName.text, DisplayName = userName.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    public void BtnLogin()
    {
        email = userEmail.text;
        pass = userPassword.text;

        Login();
    }
    public void BtnAddLogin() //For mobile recovery account
    {
        email = userEmail.text;
        pass = userPassword.text;

        var request = new AddUsernamePasswordRequest { Email = email, Password = pass, Username = userName.text };
        PlayFabClientAPI.AddUsernamePassword(request, OnAddLoginSuccess, OnRegisterFailure);
    }

    public void BtnLogOut()
    {
        email = string.Empty;
        pass = string.Empty;
        PlayerPrefs.DeleteKey("Email");
        PlayerPrefs.DeleteKey("Pass");
        authPanel.SetActive(true);
        userPanel.SetActive(false);
        PlayFabClientAPI.ForgetAllCredentials();
    }

    void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = pass,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowLastLogin = true,
                    ShowDisplayName = true
                }
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        SaveUserData();
        authPanel.SetActive(false);
        userPanel.SetActive(true);
        txtUserName.text = obj.Username;
        playfabID = obj.PlayFabId;

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName.text }, result=> { }, OnError);
        Debug.Log("Congratulations, you registered!");
    }
    void OnAddLoginSuccess(AddUsernamePasswordResult obj)
    {
        SaveUserData();
        authPanel.SetActive(false);
        userPanel.SetActive(true);
        txtUserName.text = obj.Username;

        Debug.Log("Congratulations, you registered!");
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {
        SaveUserData();
        authPanel.SetActive(false);
        userPanel.SetActive(true);
        txtUserName.text = 
            "Username: " + result.InfoResultPayload.PlayerProfile.DisplayName + "\n" + 
            "Last Login: " + result.InfoResultPayload.PlayerProfile.LastLogin.ToString();

        playfabID = result.PlayFabId;
        entityId = result.EntityToken.Entity.Id;
        // The expected entity type is title_player_account.
        entityType = result.EntityToken.Entity.Type;

        Debug.Log("Congratulations, you made your first successful API call!");
    }
    void OnLoginSuccessMobile(LoginResult result)
    {
        authPanel.SetActive(false);
        userPanel.SetActive(true);
        txtUserName.text = "Welcome Android";

        Debug.Log("Congratulations, you made your first successful API call!");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void SaveUserData()
    {
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Pass", pass);
    }
}
