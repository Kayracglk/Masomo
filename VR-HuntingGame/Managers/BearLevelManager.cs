using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BearLevelManager : MonoBehaviour
{
    public static BearLevelManager instance;
    public GameObject player;
    public GameObject gameUI;
    public int count = 3;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator WaitToLoadScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Score");
    }

    public void LevelFinished()
    {
        gameUI.SetActive(true);
        StartCoroutine(WaitToLoadScene());
    }

    public void SpawnController()
    {
        if(count > 0)
        {
            Spawn();
            count--;
        }
        else
        {
            LevelFinished();
        }
    }

    private void Spawn()
    {
        float spawnDistance = Random.Range(20f, 30f);
        Vector3 playerDirection = Camera.main.transform.forward;
        Vector3 spawnPosition = player.transform.position - playerDirection * spawnDistance + new Vector3(0, 0, Random.Range(0f, 5f));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 100.0f, NavMesh.AllAreas))
        {
            // Eğer geçerli bir NavMesh konumu bulunursa, objeyi bu konuma spawn et
            GameObject bear = Instantiate(Resources.Load("Pooling/" + "Bear") as GameObject, hit.position, Quaternion.identity);
            bear.transform.LookAt(player.transform);
            bear.GetComponent<BearHealthManager>().GiveDamage(0, Vector3.zero);
        }
        else
        {
            // Geçerli bir NavMesh konumu bulunamazsa burada gerekli hata işlemlerini yapabilirsiniz.
            Debug.LogError("Geçerli bir NavMesh konumu bulunamadı!");
        }
    }
}
