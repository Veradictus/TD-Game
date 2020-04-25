using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

    /**
     * The interface between each scene and the network
     * game object. We filter all data through this file.
     */


    // ------------------------------------------
    // Login/register screen variables
    public Text errorMessage;

    private bool rememberMe;
    private string usernameText = "";
    private string passwordText = "";

    // Register Variables
    private string passwordConfirmText = "";
    private string emailText = "";
    // ------------------------------------------

    private Network network;

    public string sceneId;

    void Awake() {
        GameObject networkObject = GameObject.Find("Network");

        network = (Network) networkObject.GetComponent(typeof(Network));

        network.SendScene(sceneId);
        network.SyncGameHandler();
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadLoginScene() {
        SceneManager.LoadScene(1);
    }

    public void LoadRegisterScene() {
        SceneManager.LoadScene(3);
    }

    private void SetErrorMessage(string message) {
        errorMessage.text = message;
    }

    public void ClickPlayButton(string button) {
        network.SendButton(button);
    }

    public void ClickLoginButton() {
        SetErrorMessage("");

        if (usernameText.Length < 3) {
            SetErrorMessage("Your username must be at least 3 character long!");
            return;
        }

        if (passwordText.Length < 4) {
            SetErrorMessage("Your password must be at least 4 characters long.");
            return;
        }

        if (network == null) {
            SetErrorMessage("Could not establish a connection to the server.");
            return;
        }

        JSONObject jObject = new JSONObject(JSONObject.Type.OBJECT);

        jObject.AddField("packetId", (int) Packets.Intro);
        jObject.AddField("introOpcode", (int) IntroOpcode.Login);
        jObject.AddField("username", usernameText);
        jObject.AddField("password", passwordText);

        network.Send(jObject.ToString());
    }

    public void ClickRegisterButton() {
        SetErrorMessage("");

        if (usernameText.Length < 3) {
            SetErrorMessage("Your username must be at least 3 character long!");
            return;
        }

        if (passwordText.Length < 4) {
            SetErrorMessage("Your password must be at least 4 characters long.");
            return;
        }

        if (passwordText != passwordConfirmText) {
            SetErrorMessage("Your passwords do not match.");
            return;
        }

        if (!Utils.IsEmailValid(emailText)) {
            SetErrorMessage("The email address you have entered is invalid.");
            return;
        }

        if (network == null) {
            SetErrorMessage("Could not establish a connection to the server.");
            return;
        }

        JSONObject jObject = new JSONObject(JSONObject.Type.OBJECT);

        jObject.AddField("packetId", (int) Packets.Intro);
        jObject.AddField("introOpcode", (int) IntroOpcode.Register);
        jObject.AddField("username", usernameText);
        jObject.AddField("password", passwordText);
        jObject.AddField("email", emailText);

        network.Send(jObject.ToString());
    }

    public void ReceiveNetworkData(string message) {
        Packet packet = JsonUtility.FromJson<Packet>(message);

        SyncContext.RunOnUnityThread(() => {
            switch (packet.packetId) {

                case (int) Packets.Error:
                    HandleError(message);
                    break;
            }
        });

    }

    private void HandleError(string message) {
        ErrorPacket errorPacket = JsonUtility.FromJson<ErrorPacket>(message);

        switch(this.sceneId) {
            case "register":
            case "login":
                SetErrorMessage(errorPacket.message);
                break;
        }
    }

    public void SetUsernameText(string usernameText) {
        this.usernameText = usernameText;
    }

    public void SetPasswordText(string passwordText) {
        this.passwordText = passwordText;
    }

    public void SetPasswordConfirmText(string passwordConfirmText) {
        this.passwordConfirmText = passwordConfirmText;
    }

    public void SetEmailText(string emailText) {
        this.emailText = emailText;
    }

    public void SetRememberMe(bool rememberMe) {
        this.rememberMe = rememberMe;
    }

}
