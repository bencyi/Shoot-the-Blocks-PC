﻿using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame {
    public class GameManager : MonoBehaviourPunCallbacks {

        [Tooltip ("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        public GameObject vrPrefab;

        public static GameManager Instance;

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom (Player other) {
            Debug.LogFormat ("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat ("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena ();
            }
        }

        public override void OnPlayerLeftRoom (Player other) {
            Debug.LogFormat ("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat ("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena ();
            }
        }

        #endregion

        #region Private Methods

        void Start () {
            Instance = this;
            if (playerPrefab == null) {
                Debug.LogError ("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            } else {

                if (characterController.LocalPlayerInstance == null) {

#if UNITY_STANDALONE_WIN
                    PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);

#endif

#if UNITY_STANDALONE_OSX
                    PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
#endif

#if UNITY_ANDROID

                    PhotonNetwork.Instantiate (this.vrPrefab.name, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
#endif
                    Debug.LogFormat ("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                } else {
                    Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        void LoadArena () {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError ("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat ("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel ("Game");

        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom () {
            SceneManager.LoadScene (0);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom () {
            PhotonNetwork.LeaveRoom ();
        }

        #endregion
    }
}