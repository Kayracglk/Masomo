using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    public RoomInfo Room { get; set; }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        Room = roomInfo;
        roomName.text = roomInfo.MaxPlayers + " , " + roomInfo.Name;
    }

    public void OnClickJoinRoomButton()
    {
        PhotonNetwork.JoinRoom(Room.Name);
    }
}
