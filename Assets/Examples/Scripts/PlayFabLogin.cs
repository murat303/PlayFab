using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] InputField userName;
    [SerializeField] InputField userEmail;
    [SerializeField] InputField userPassword;

    [SerializeField] GameObject authPanel;
    [SerializeField] Text txtUserName;

    public void BtnRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = userEmail.text, Password = userPassword.text, Username = userName.text, DisplayName = userName.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    public void BtnLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail.text, Password = userPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        Debug.Log("Congratulations, you registered!");
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {
        authPanel.SetActive(false);
        txtUserName.text = result.InfoResultPayload.PlayerProfile.DisplayName; //TODO: neden null geliyor?
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
