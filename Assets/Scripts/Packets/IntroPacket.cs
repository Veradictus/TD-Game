using System;
using UnityEngine;

[Serializable]
public class IntroPacket : Packet {

    public IntroPacket(int opcode) {
        this.packetId = (int) Packets.Intro;
        this.opcode = opcode;
    }

    public string username;
    public string password;
    public string email;

}
