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
        public override void OnLeftRoom() //플레이어가 서버를 나갈때 실행
        {
            SceneManager.LoadScene("photon");
        }

        #endregion

        #region Public Methods

        public void LeaveRoom() //이 함수 호출 후 내부적으로 OnLeftRoom()실행
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient) //혹시 모르니 한번더 확인
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount); //현재 무엇을 로딩하는지
            PhotonNetwork.LoadLevel("RoomScenes" + PhotonNetwork.CurrentRoom.PlayerCount); //씬에 로딩하기 
        }

        public override void OnPlayerEnteredRoom(Player other) //다른 클라이언트에서 서버에 연결하면 실행 other은 현재 연결하고 있는 플레이어
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); 

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other) //다른 클라이언트에서 연결을 끊었을때 실행 other은 연결을 끊은 플레이어
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient) //LoadArena()에 있는 PhotonNetwork.LoadLevel()은 현재 클라가 마스터클일때만 호출이 가능하여 
                                              //LoadLevel을 바꿀때는 PhotonNetwork.IsMasterClient로 확인해야 한다
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

    }
}