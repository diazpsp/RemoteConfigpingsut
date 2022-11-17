using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_Text feedbackText;

    public void ClickConnect(){
        feedbackText.text = "";
        if(usernameInput.text.Length < 3){
            feedbackText.text = "Username Min 3 characters";

            return;
        }
        //simpan usenrame
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        //connect ke server
        PhotonNetwork.ConnectUsingSettings();
        feedbackText.text = "Connecting...";
   }

    //dijalankan ketika sudha connect
   public override void OnConnectedToMaster(){
        Debug.Log("Connected to master");
        feedbackText.text = "Connected to master";
        StartCoroutine(LoadLevelAfterConnectedAndReady());  
   }

    public void LoadSceneTwo(int sceneIndex2)
    {
        SceneManager.LoadScene(sceneIndex2);
    }

    IEnumerator LoadLevelAfterConnectedAndReady(){
        while(PhotonNetwork.IsConnectedAndReady == false){
            yield return null;
        }
        SceneManager.LoadScene("Lobby");
    }
}
