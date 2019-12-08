using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace Photon.Pun.Demo.PunBasics
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private GameObject feedbackText;

        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        bool isConnecting;

        string gameVersion = "1";

        public InputField playerNameField;
        public InputField roomNameField;

        public Text playerStatus;
        public Text connectionStatus;

        public GameObject roomJoinUI;
        public GameObject buttonLoadArena;
        public GameObject buttonJoinRoom;

        string playerName = "";
        string roomName = "";
        // Start is called before the first frame update
        void Start()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Connecting to Photon Network");
            roomJoinUI.SetActive(false);
            buttonLoadArena.SetActive(false);

            ConnectToPhoton();
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void SetPlayerName(string name)
        {
            playerName = name;
        }

        public void SetRoomName(string name)
        {
            roomName = name;
        }

        private void ConnectToPhoton()
        {
            connectionStatus.text = "Connecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                SetPlayerName(playerNameField.text);
                SetRoomName(roomNameField.text);
                PhotonNetwork.LocalPlayer.NickName = playerName;
                Debug.Log("PhotonNetwork.IsConnected | Trying to Create/Join Room" + roomNameField.text);
                RoomOptions roomOptions = new RoomOptions();
                TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);

            }
        }

        public void LoadArena()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.LoadLevel("ChessArena");
            }
            else
            {
                playerStatus.text = "2 Players required to Load Arena";
            }
        }

        public override void OnConnected()
        {
            base.OnConnected();
            connectionStatus.text = "Connected to Photon";
            connectionStatus.color = Color.green;
            roomJoinUI.SetActive(true);
            buttonLoadArena.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            isConnecting = false;
            controlPanel.SetActive(true);
            Debug.LogError("Disconnected. Please check your Internet connection");
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                buttonLoadArena.SetActive(true);
                buttonJoinRoom.SetActive(false);
                playerStatus.text = "You are Lobby Leader";
            }
            else {
                playerStatus.text = "Connected to Lobby";
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
