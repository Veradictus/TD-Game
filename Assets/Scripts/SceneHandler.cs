using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    private GameObject network;

    public string sceneId;

    void Start() {
        network = GameObject.Find("Network");

        Network networkScript = (Network) network.GetComponent(typeof(Network));

        networkScript.SendScene(sceneId);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadLoginScene() {
        SceneManager.LoadScene(1);
    }

}
