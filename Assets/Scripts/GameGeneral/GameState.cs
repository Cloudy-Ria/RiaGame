using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

public class GameState : MonoBehaviour
{
    GameObject bgMusic;
    GameObject battleMusic;
    public float masterVolume = 1f;
    public Dictionary<string, int> itemsRiaMemory;
    public Dictionary<string, int> sceneIterations;
    public Dictionary<string, int> targetSceneIteration;
    public Dictionary<string, bool> scenePhotosTaken;
    public Dictionary<string, bool> objectPhotosTaken;
    public string respawnScene = "LeftRuins_Tutorial";
    public bool fixedRiaMemory = false;

    public int maxHealth;
    public int currentHealth;
    public bool flipX;

    public string spawnPointName;
    public string transitionType;
    
    public bool SceneLoadInProgress = false;
    
    void Awake()
    {
        masterVolume = 1f;
        itemsRiaMemory = new Dictionary<string, int>()
        {
            { "Key", -1 },
            { "Sofa", -1 }
        };

        sceneIterations = new Dictionary<string, int>();
        targetSceneIteration = new Dictionary<string, int>();
        scenePhotosTaken = new Dictionary<string, bool>();
        objectPhotosTaken = new Dictionary<string, bool>();
        var keys = itemsRiaMemory.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            objectPhotosTaken[keys[i]] = false;
        }
        //Count the number of scene iterations with the same name, i.e. Ruins_01 and Ruins_02 are two scenes with the name Ruins.
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            scenes[i] = scenes[i].Substring(0, scenes[i].Length - 3);
            if (!sceneIterations.ContainsKey(scenes[i]))
            {
                sceneIterations[scenes[i]] = 1;
                targetSceneIteration[scenes[i]] = 1;
                scenePhotosTaken[scenes[i]] = false;
            }
            else
            {
                sceneIterations[scenes[i]] ++;
            }
        }
        bgMusic = this.gameObject.transform.Find("BackgroundMusic").gameObject;
        battleMusic = this.gameObject.transform.Find("BattleMusic").gameObject;

        
    }

    public void Start()
    {
        LoadFromJson();
        //VolumeSlider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<VolumeSlider>();
        //volumeSlider.SetVolume(masterVolume);
    }

    public void LoadFromJson()
    {
        string path = Application.persistentDataPath + '\\'+"data.muahaha";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fileStream = new FileStream(path, FileMode.Open);
            string json = formatter.Deserialize(fileStream) as string;
            fileStream.Close();
            JsonUtility.FromJsonOverwrite(json, this);

            string path2 = Application.persistentDataPath + '\\' + "itemsRiaMemory.muahaha";
            FileStream fileStream2 = new FileStream(path2, FileMode.Open);
            fileStream2.Close();
            JsonUtility.FromJsonOverwrite(json, this.itemsRiaMemory);

            string path3 = Application.persistentDataPath + '\\' + "sceneIterations.muahaha";
            FileStream fileStream3 = new FileStream(path3, FileMode.Open);
            fileStream3.Close();
            JsonUtility.FromJsonOverwrite(json, this.sceneIterations);

            string path4 = Application.persistentDataPath + '\\' + "targetSceneIteration.muahaha";
            FileStream fileStream4 = new FileStream(path4, FileMode.Open);
            fileStream4.Close();
            JsonUtility.FromJsonOverwrite(json, this.targetSceneIteration);

            string path5 = Application.persistentDataPath + '\\' + "scenePhotosTaken.muahaha";
            FileStream fileStream5 = new FileStream(path5, FileMode.Open);
            fileStream5.Close();
            JsonUtility.FromJsonOverwrite(json, this.scenePhotosTaken);

            string path6 = Application.persistentDataPath + '\\' + "scenePhotosTaken.muahaha";
            FileStream fileStream6 = new FileStream(path6, FileMode.Open);
            fileStream6.Close();
            JsonUtility.FromJsonOverwrite(json, this.objectPhotosTaken);

            string currentSceneName = SceneManager.GetActiveScene().name;
            string targetScene = GetSceneIteration(respawnScene);
            if (currentSceneName != targetScene)
            {
                SceneManager.LoadScene(targetScene);
            }
            
        }



    }
    public void SaveToJson()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string json1 = JsonUtility.ToJson(this);
        string path1 = Application.persistentDataPath + '\\' + "data.muahaha";
        FileStream fileStream1 = new FileStream(path1, FileMode.Create);
        formatter.Serialize(fileStream1, json1);
        fileStream1.Close();

        string json2 = JsonUtility.ToJson(itemsRiaMemory);
        string path2 = Application.persistentDataPath + '\\' + "itemsRiaMemory.muahaha";
        FileStream fileStream2 = new FileStream(path2, FileMode.Create);
        formatter.Serialize(fileStream2, json2);
        fileStream2.Close();

        string json3 = JsonUtility.ToJson(sceneIterations);
        string path3 = Application.persistentDataPath + '\\' + "sceneIterations.muahaha";
        FileStream fileStream3 = new FileStream(path3, FileMode.Create);
        formatter.Serialize(fileStream3, json3);
        fileStream3.Close();

        string json4 = JsonUtility.ToJson(targetSceneIteration);
        string path4 = Application.persistentDataPath + '\\' + "targetSceneIteration.muahaha";
        FileStream fileStream4 = new FileStream(path4, FileMode.Create);
        formatter.Serialize(fileStream4, json4);
        fileStream4.Close();

        string json5 = JsonUtility.ToJson(targetSceneIteration);
        string path5 = Application.persistentDataPath + '\\' + "scenePhotosTaken.muahaha";
        FileStream fileStream5 = new FileStream(path5, FileMode.Create);
        formatter.Serialize(fileStream5, json5);
        fileStream5.Close();

        string json6 = JsonUtility.ToJson(targetSceneIteration);
        string path6 = Application.persistentDataPath + '\\' + "objectPhotosTaken.muahaha";
        FileStream fileStream6 = new FileStream(path6, FileMode.Create);
        formatter.Serialize(fileStream6, json6);
        fileStream6.Close();

        /*
        public Dictionary<string, int> itemsRiaMemory;
        public Dictionary<string, int> sceneIterations;
        public Dictionary<string, int> targetSceneIteration;
        public Dictionary<string, bool> scenePhotosTaken;
        */
    }

    public string GetSceneIteration(string sceneName)
    {
        return sceneName + "_" + targetSceneIteration[sceneName].ToString("D2");
    }

    public void OnTransition(string sceneName)
    {
        var player = GameObject.Find("Player");
        maxHealth = player.GetComponent<HealthManager>().maxHealth;
        currentHealth = player.GetComponent<HealthManager>().currentHealth;
        flipX = player.GetComponent<SpriteRenderer>().flipX;

        if (!fixedRiaMemory)
        {
            var keys = itemsRiaMemory.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                itemsRiaMemory[keys[i]] -=1;
            }
        }

        if (!fixedRiaMemory)
        {
            if (sceneIterations[sceneName] == targetSceneIteration[sceneName])
            {
                targetSceneIteration[sceneName] = 1;
            }
            else
            {
                targetSceneIteration[sceneName]++;
            }
        }

    }
    public IEnumerator RespawnPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        string currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneName = currentSceneName.Substring(0, currentSceneName.Length - 3);
        OnTransition(currentSceneName);
        if (!fixedRiaMemory)
        {
            var keys = itemsRiaMemory.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                itemsRiaMemory[keys[i]] = -1;
            }
        }
        currentHealth = maxHealth;
        SceneManager.LoadScene(GetSceneIteration(respawnScene), LoadSceneMode.Single);
    }

    public IEnumerator SceneFadeIn()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneName = currentSceneName.Substring(0, currentSceneName.Length - 3);
        var gameState = GameObject.Find("GameState").GetComponent<GameState>();
        var player = GameObject.Find("Player");
        var sceneBlackout = GameObject.Find("Blackout").GetComponent<SceneBlackout>();

        bgMusic.GetComponent<BackgroundMusic>().UpdatePlayer();
        battleMusic.GetComponent<BackgroundMusic>().UpdatePlayer();
        if (currentSceneName.Contains("Arena"))
        {
            bgMusic.SetActive(false);
            battleMusic.SetActive(true);
        }
        else if (!bgMusic.activeSelf){
            bgMusic.SetActive(true);
            battleMusic.SetActive(false);
        }
        else
        {
            battleMusic.SetActive(false);
        }

            sceneBlackout.SetTo(1);
        sceneBlackout.FadeTo(0);
        yield return new WaitForSeconds(SceneBlackout.immovableTime);

       //Enable player movement
        player.GetComponent<Animator>().enabled = true;
        if (fixedRiaMemory)
        {
            if (!scenePhotosTaken[currentSceneName])
            {
                player.GetComponent<Animator>().SetTrigger("Camera");
                scenePhotosTaken[currentSceneName] = true;
            }
        }

        player.GetComponent<PlayerController>().DoMove();


    }

    public void MemoriseItem(string item)
    {
        if (itemsRiaMemory.ContainsKey(item))
        {
            itemsRiaMemory[item] = 1;
        }
    }

    public void FixRiaMemory()
    {
        fixedRiaMemory = true;
    }
}
