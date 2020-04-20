using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Immediatley load a scene upon awakening
    void Awake() {
        SceneManager.LoadScene(0);
    }
}
