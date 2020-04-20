using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    static public bool initialized;

    void Awake() {

        if (initialized)
            Destroy(this.gameObject);
        else {
            initialized = true;
            DontDestroyOnLoad(this.gameObject);
        }

    }

}
