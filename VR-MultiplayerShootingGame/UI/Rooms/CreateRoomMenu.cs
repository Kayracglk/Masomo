using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomName;

    private RoomsCanvases roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    public void OnClickCreateRoom()
    {
        // CreateRoom 
        // JoinOrCreateRoom
        if(!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photona baglanmadi");
            return;
        }    
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.PublishUserId = true;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully", this);
        roomsCanvases.CurrentRoomCanvas.Show();

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed " + message, this);
    }
}
