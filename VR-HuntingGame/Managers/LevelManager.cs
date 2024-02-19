using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public struct Grid
{
    public AnimalType animalType { get; set; }
    public GameObject image { get; set; }
}
public class LevelManager: MonoBehaviour
{
    public static LevelManager instance;

    [Header("LEVEL OBJECTS")]
    public int currentLevel = -1;
    public MissionList levelLists;
    public Transform UiParent;
    public Transform[] playerSpawnPoints;
    public List<Transform> spawnPointParents;

    [Header("LEVEL COMPLATE PANEL")]
    [SerializeField] private GameObject levelComplatePanel;
    [SerializeField] private GameObject bonusGamePanel;
    [SerializeField] private Image counterImage;
    [SerializeField] private int counter;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private int maxLevel;


    private List<GameObject> animals = new List<GameObject>();
    private Dictionary<AnimalType, int> levelState = new Dictionary<AnimalType, int>();
    private List<Grid> gridBlocks = new List<Grid>();
    private GameObject player;
    private List<Transform> spawnPoints = new List<Transform>();
    private Grid grid = new Grid();
    private List<int> gridState = new List<int>();
    private bool isLevelFinished = false;
    public bool ayi = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartLevel();
    }

    private void StartLevel()
    {
        SetLevel();
        WriteLevelState();
        player.transform.position = playerSpawnPoints[currentLevel].position;
        UiWriter();
        StartCoroutine(SpawnEnum());
    }

    public void WriteLevelState()
    {
        for (int i = 0; i < levelLists.levels[currentLevel].missionsList.Count; i++)
        {
            levelState.Add(levelLists.levels[currentLevel].missionsList[i].type, levelLists.levels[currentLevel].missionsList[i].count);
        }
    }
    public void UiWriter()
    {
        for (int i = 0; i < levelLists.levels[currentLevel].missionsList.Count; i++)
        {
            for (int j = 0; j < levelLists.levels[currentLevel].missionsList[i].count; j++)
            {
                GameObject a = Instantiate(Resources.Load("UI/" + levelLists.levels[currentLevel].missionsList[i].type.ToString()), UiParent) as GameObject;
                grid.image = a;
                grid.animalType = levelLists.levels[currentLevel].missionsList[i].type;
                gridBlocks.Add(grid);
            }
        }
        for (int i = 0; i < gridBlocks.Count; i++)
        {
            gridState.Add(i);
        }
    }

    public void SetLevel()
    {
        if (gridBlocks.Count > 0)
        {
            for (int i = 0; i < gridBlocks.Count; i++)
            {
                if (gridBlocks[i].image != null)
                {
                    Destroy(gridBlocks[i].image);
                }
            }
        }
        if (animals.Count > 0)
        {
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals[i] != null)
                {
                    Destroy(animals[i]);
                }
            }
        }
        animals.Clear();
        gridBlocks.Clear();
        StopCoroutine(SpawnEnum());
    }

    public void CheckLevel(AnimalType type)
    {
        foreach (int i in gridState)
        {
            if (gridBlocks[i].animalType == type)
            {
                string newImagePath = "Animals/" + type.ToString() + "1";
                Sprite newSprite = Resources.Load<Sprite>(newImagePath);
                gridBlocks[i].image.GetComponent<Image>().sprite = newSprite;
                gridState.Remove(i);
                break;
            }
        }
        if (levelState.Count > 0)
        {
            if (levelState[type] > 1)
            {
                levelState[type]--;
            }
            else
            {
                levelState.Remove(type);
            }
        }
        if (levelState.Count <= 0)
        {
            print("Level Bitti");
            NextLevel();
        }
    }
    IEnumerator WaitToLoadScene()
    {   print("Bonus Game");
        bonusGamePanel.SetActive(true);
        yield return new WaitForSeconds(5);
        
        if (ayi == true)
            SceneManager.LoadScene("Ayı");
        else
            SceneManager.LoadScene("Kus");
    }

    private void NextLevel()
    {
        if (currentLevel >= maxLevel){
        SetLevel();
            StartCoroutine(WaitToLoadScene());
        }
        else
        {
            print("Next Level");
            SetLevel(); 
            StartCoroutine(LevelUi());
        }
    }

    private IEnumerator LevelUi()
    {
        levelComplatePanel.SetActive(true);
        int temp = counter;
        while (temp > 0)
        {
            counterImage.fillAmount = (float)temp / counter;
            counterText.text = temp.ToString();
            temp--;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForEndOfFrame();
        levelComplatePanel.SetActive(false);
        yield return new WaitForEndOfFrame();
        currentLevel++;
        UiWriter();
        WriteLevelState();
        player.transform.position = playerSpawnPoints[currentLevel].position;
        StartCoroutine(SpawnEnum());
    }

    public IEnumerator SpawnEnum()
    {
        yield return null;
        for (int k = 0; k < 2; k++)
        {
            spawnPoints.Clear();
            for (int i = 0; i < spawnPointParents[currentLevel].childCount; i++)
            {
                spawnPoints.Add(spawnPointParents[currentLevel].GetChild(i));
            }
            for (int i = 0; i < levelLists.levels[currentLevel].missionsList.Count; i++)
            {
                for (int j = 0; j < levelLists.levels[currentLevel].missionsList[i].count; j++)
                {
                    if (j % 2 == 0)
                        yield return new WaitForSeconds(Random.Range(5, 10));
                    GameObject a = Instantiate(Resources.Load("Pooling/" + levelLists.levels[currentLevel].missionsList[i].type.ToString(), typeof(GameObject)),
                        spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity) as GameObject;
                    animals.Add(a);
                }
            }
            yield return new WaitForSeconds(Random.Range(30, 40));

        }
    }
}