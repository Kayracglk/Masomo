using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerName;
    public Player Player { get; set; }
    public bool Ready = false;

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        SetPlayerText(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer != null && targetPlayer == Player)
        {
            if(changedProps.ContainsKey("RandomNumber"))
            {
                SetPlayerText(targetPlayer);
            }
        }
    }

    private void SetPlayerText(Player player)
    {
        int result = -1;
        if (Player.CustomProperties.ContainsKey("RandomNumber"))
            result = (int)Player.CustomProperties["RandomNumber"];
        playerName.text = result.ToString() + " - " + player.NickName;
    }
}
