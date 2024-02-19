using Photon.Pun;
using TMPro;
using UnityEngine;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private void SetCustomNumber()
    {
        System.Random random = new System.Random();
        int result = random.Next(0,90);

        text.text = result.ToString();

        myCustomProperties["RandomNumber"] = result;
        PhotonNetwork.SetPlayerCustomProperties(myCustomProperties);
        // PhotonNetwork.LocalPlayer.CustomProperties = myCustomProperties;
    }
    public void OnClickGenerateButton()
    {
        SetCustomNumber();
    }
}
