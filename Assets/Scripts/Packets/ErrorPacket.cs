using System;
using UnityEngine;

[Serializable]
public class ErrorPacket : Packet {

    public ErrorPacket() {
        this.packetId = (int) Packets.Error;
    }

    public string message;

}
