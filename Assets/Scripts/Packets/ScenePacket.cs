using System;
using UnityEngine;

[Serializable]
public class ScenePacket : Packet {

    public ScenePacket() {
        this.packetId = (int) Packets.Scene;
        this.opcode = -1;
    }

    public ScenePacket(int sceneId) {
        this.packetId = (int) Packets.Scene;
        this.opcode = sceneId;
    }

    public string scene;
}
