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
            if (PhotonNetwork.IsConnected) //����Ǿ� ������ 
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom(); //���� ����
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();   
            }
        }

        public override void OnConnectedToMaster() //���� ȣ��Ǵ��� �� �Ƹ� PhotonNetwork.IsConnected�� ���� true�� ��� ����ȴµ�
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom(); //���� ����ϰ� �ִ� �κ񿡼� ����� �� �ִ� �뿡 ���� �ϰ� ����� �� �ִ� ���� ������ ����
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) //joinrandomroom ����
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });// ������ �̸����� �� ������ ������ ������ null �Ǵ� "" �� �����Ͽ� ������ roomName�� �Ҵ�
        }

        public override void OnJoinedRoom() //����  ?���� ����� �� ���� ������ �̹� ������Ŭ�� ������ �� Ŭ��� �����Ͱ� �ƴϰ� �Ǵ°� ��� �����ϴ���
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
