using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameplayNetworkManager : MonoBehaviourPunCallbacks
{
    public void BackToMenu(){
       StartCoroutine(BackToMenuCR());
    }

    IEnumerator BackToMenuCR(){

        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToLobby(){
        
        StartCoroutine(BackToLobbyCR());
    }

    IEnumerator BackToLobbyCR(){

        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady == false )
        {
            yield return null;    
        }
        SceneManager.LoadScene("Lobby");
    }

    public void Replay(){
        if (PhotonNetwork.IsMasterClient)
        {
            var scene = SceneManager.GetActiveScene();
            PhotonNetwork.LoadLevel(scene.name);
        }
    }

    public void Quit(){

        StartCoroutine(QuitCR());
    }

        IEnumerator QuitCR(){

        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        Application.Quit();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            BackToLobby();
        }
    }

    public void ClickStartGame(string levelName){

        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel(levelName);
        }
    }
}
