using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomsCanvases roomsCanvas;

    public void FirstInitialize(RoomsCanvases canvasses)
    {
        roomsCanvas = canvasses;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        roomsCanvas.CurrentRoomCanvas.Hide();
    }

    
}
