using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private int firstGameScene = 4;
    [SerializeField] private Transform content;
    [SerializeField] private PlayerListing playerListing;
    [SerializeField] private TMP_Text readyUpText;


    private List<PlayerListing> playerListings = new List<PlayerListing>();
    private RoomsCanvases roomsCanvases;
    private bool ready = false;

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < playerListings.Count; i++)
        {
            Destroy(playerListings[i]);
        }
        playerListings.Clear();
        content.DestroyChildren();

    }

    public void FirstInitialize(RoomsCanvases _roomsCanvases)
    {
        roomsCanvases = _roomsCanvases;
    }
    private void SetReadyUp(bool state)
    {
        ready = state;
        if (ready)
            readyUpText.text = "Ready";
        else
            readyUpText.text = "Not Ready !";
    }
    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player newPlayer)
    {
        int index = playerListings.FindIndex(x => x.Player == newPlayer);
        if (index != -1)
        {
            playerListings[index].SetPlayerInfo(newPlayer);
        }
        else
        {
            PlayerListing listing = Instantiate(playerListing, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(newPlayer);
                playerListings.Add(listing);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClickLeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerListings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(playerListings[index].gameObject);
            playerListings.RemoveAt(index);
        }
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerListings.Count; i++)
            {
                if (playerListings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!playerListings[i].Ready)
                    {
                        return;
                    }
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(firstGameScene);
        }
    }

    public void OnClickReadyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = playerListings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            playerListings[index].Ready = ready;
        }
    }
}
