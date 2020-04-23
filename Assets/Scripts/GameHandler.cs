using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

    /**
     * The interface between each scene and the network
     * game object. We filter all data through this file.
     */


    // Login screen variables
    public Text errorMessage;

    private bool rememberMe;
    private string usernameLoginText = "";
    private string passwordLoginText = "";


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

    private void SetErrorMessage(string message) {
        errorMessage.text = message;
    }

    public void ClickPlayButton(string button) {
        network.SendButton(button);
    }

    public void ClickLoginButton() {
        SetErrorMessage("");

        if (usernameLoginText.Length < 4) {
            SetErrorMessage("Your username must be at least 3 character long!");
            return;
        }

        if (passwordLoginText.Length < 5) {
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
        jObject.AddField("username", usernameLoginText);
        jObject.AddField("password", passwordLoginText);

        network.Send(jObject.ToString());
    }

    public void ReceiveNetworkData(string message) {
        Debug.Log("Received network data!!!");
        Debug.Log(message);
    }

    public void SetUsernameLoginText(string usernameLoginText) {
        this.usernameLoginText = usernameLoginText;
    }

    public void SetPasswordLoginText(string passwordLoginText) {
        this.passwordLoginText = passwordLoginText;
    }

    public void SetRememberMe(bool rememberMe) {
        this.rememberMe = rememberMe;
    }

}
