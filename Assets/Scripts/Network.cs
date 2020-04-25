using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using UnityEngine;

public class Network : MonoBehaviour {

    private string host = "127.0.0.1";
    private int port = 9800;

    private TcpClient connection;
    private Thread receivedThread;

    private bool threadRunning = true;
    private GameHandler gameHandler;

    // Start is called before the first frame update
    void Awake() {
        Connect();
    }

    void OnDestroy() {
        threadRunning = false;

        connection.GetStream().Close();
        connection.Close();
    }

    public void SendScene(string sceneId) {
        if (connection == null)
            return;

        ScenePacket scenePacket = new ScenePacket();

        scenePacket.scene = sceneId;

        Send(JsonUtility.ToJson(scenePacket));
    }

    /**
     * Starts a thread specifically for listening to incoming connections.
     */

    private void Connect() {
        try {
            receivedThread = new Thread(new ThreadStart(Listen));
            receivedThread.IsBackground = true;
            receivedThread.Start();
        } catch (Exception e) {
            Debug.Log("An error has occurred while trying to connect.");
            Debug.Log(e);
        }
    }

    private void Listen() {
        try {

            connection = new TcpClient(host, port);

            Receive();

        } catch (SocketException socketException) {
            Debug.Log("A socket exception has occurred during listening.");
            Debug.Log(socketException);
        }
    }

    /**
     * Handles the receiving of messages from the server. We then
     * relay it through the `gameHandler`.
     */

    private void Receive() {
        Byte[] bytes = new Byte[4096]; //4096 bytes is more than enough


        while (threadRunning) {
            using (NetworkStream stream = connection.GetStream()) {
                int length;

                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                    Byte[] incomingData = new byte[length];

                    Array.Copy(bytes, 0, incomingData, 0, length);

                    string receivedMessage = Encoding.ASCII.GetString(incomingData);

                    if (gameHandler != null)
                        gameHandler.ReceiveNetworkData(receivedMessage);

                }
            }
        }
    }

    /**
     * Takes the `message` string and sends an encoded ASCII to the server.
     */

    public void Send(string message) {
        if (connection == null)
            return;

        try {

            NetworkStream stream = connection.GetStream();

            if (!stream.CanWrite) {
                Debug.Log("Cannot write to the current connection stream.");
                return;
            }

            byte[] byteEncoding = Encoding.ASCII.GetBytes(message);

            stream.Write(byteEncoding, 0, byteEncoding.Length);

        } catch (SocketException socketException) {
            Debug.Log("A socket exception has occurred during sending.");
            Debug.Log(socketException);
        }
    }

    /**
     * Grabs the GameHandler from the current scene and sets it as a
     * global variable. This allows us to interface with each scene.
     */

    public void SyncGameHandler() {
        GameObject gameHandlerObject = GameObject.Find("GameHandler");

        gameHandler = (GameHandler) gameHandlerObject.GetComponent(typeof(GameHandler));
    }

    public bool IsConnected() {
        return connection != null;
    }
}
