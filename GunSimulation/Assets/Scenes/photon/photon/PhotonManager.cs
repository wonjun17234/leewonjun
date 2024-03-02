using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom() //�÷��̾ ������ ������ ����
        {
            SceneManager.LoadScene("photon");
        }

        #endregion

        #region Public Methods

        public void LeaveRoom() //�� �Լ� ȣ�� �� ���������� OnLeftRoom()����
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient) //Ȥ�� �𸣴� �ѹ��� Ȯ��
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount); //���� ������ �ε��ϴ���
            PhotonNetwork.LoadLevel("RoomScenes" + PhotonNetwork.CurrentRoom.PlayerCount); //���� �ε��ϱ� 
        }

        public override void OnPlayerEnteredRoom(Player other) //�ٸ� Ŭ���̾�Ʈ���� ������ �����ϸ� ���� other�� ���� �����ϰ� �ִ� �÷��̾�
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); 

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other) //�ٸ� Ŭ���̾�Ʈ���� ������ �������� ���� other�� ������ ���� �÷��̾�
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient) //LoadArena()�� �ִ� PhotonNetwork.LoadLevel()�� ���� Ŭ�� ������Ŭ�϶��� ȣ���� �����Ͽ� 
                                              //LoadLevel�� �ٲܶ��� PhotonNetwork.IsMasterClient�� Ȯ���ؾ� �Ѵ�
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

    }
}