using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

    /**
     * The interface between each scene and the network
     * game object. We filter all data through this file.
     */

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

    public void ClickPlayButton(string button) {
        network.SendButton(button);
    }

    public void ReceiveNetworkData(string message) {
        Debug.Log("Received network data!!!");
        Debug.Log(message);
    }

}
