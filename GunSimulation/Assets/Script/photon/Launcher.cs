using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{


    public class Launcher : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        bool isConnecting;

        [SerializeField]
        

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;

            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
        }



        
        public void Connect()
        {
            isConnecting = true;
            controlPanel.SetActive(false);
            progressLabel.SetActive(true);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected) //연결되어 있으면 
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom(); //서버 접속
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();   
            }
        }

        public override void OnConnectedToMaster() //언제 호출되는지 모름 아마 PhotonNetwork.IsConnected이 값이 true면 계속 실행된는듯
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom(); //현재 사용하고 있는 로비에서 사용할 수 있는 룸에 참여 하고 사용할 수 있는 룸이 없으면 실패
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) //joinrandomroom 실패
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });// 유일한 이름으로 룸 생성을 원하지 않으면 null 또는 "" 을 전달하여 서버가 roomName을 할당
        }

        public override void OnJoinedRoom() //연결  ?지금 연결될 때 현재 서버에 이미 마스터클라가 있으면 이 클라는 마스터가 아니게 되는게 어디서 동작하는지
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");

                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("RoomScenes1");
            }
        }
    }
}
