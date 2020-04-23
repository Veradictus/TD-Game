using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Packets {
    Handshake = 0,
    Scene = 1,
    Intro = 2,
    Button = 3
}

public enum IntroOpcode {
    Login = 0,
    Register = 1
}
