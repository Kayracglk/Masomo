using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private RoomListing roomListing;

    private List<RoomListing> roomListings = new List<RoomListing>();

    private RoomsCanvases roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    public override void OnJoinedRoom()
    {
        roomsCanvases.CurrentRoomCanvas.Show();
        content.DestroyChildren();
        roomListings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList) 
        {
            // Removed from roomList
            if(roomInfo.RemovedFromList)
            {
                int index = roomListings.FindIndex(x => x.Room.Name == roomInfo.Name);
                if(index != -1)
                {
                    Destroy(roomListings[index].gameObject);
                    roomListings.RemoveAt(index);
                }
            }
            else // Added to roomList
            {
                int index = roomListings.FindIndex(x => x.Room.Name == roomInfo.Name);
                if( index == -1)
                {
                    RoomListing listing = Instantiate(roomListing, content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(roomInfo);
                        roomListings.Add(listing);
                    }
                }
                else
                {

                }
                
            }
        }
    }
}
