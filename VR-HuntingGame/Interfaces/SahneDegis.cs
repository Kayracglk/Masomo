using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class SahneDegis : MonoBehaviour
{
    public async void Hunting()
    {
        string hedefSahneAdi = "Hunting";

        await ChangeSceneAsync(hedefSahneAdi);

        Debug.Log(hedefSahneAdi);
    }

    public async void AnaMenu(){

        string hedefSahneAdi ="AnaMenu";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }

    public async void Bonus1(){

        string hedefSahneAdi ="Bonus1";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }

    public async void Bonus2(){

        string hedefSahneAdi ="Bonus2";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }
    public async void AliTest(){

        string hedefSahneAdi ="AliTEST";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }
    public async void Skore(){

        string hedefSahneAdi ="Score";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }
    public async void ayi(){

        string hedefSahneAdi ="Ayı";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }
    public async void Kus(){

        string hedefSahneAdi ="Kus";
        
        await ChangeSceneAsync(hedefSahneAdi);
        Debug.Log(hedefSahneAdi);
    }
    public async void CiK(){

        Application.Quit();
    }

    

    private async Task ChangeSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    }
}