using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

public class GameState : MonoBehaviour
{
    public Dictionary<string, int> itemsRiaMemory;
    public Dictionary<string, int> sceneIterations;
    public Dictionary<string, int> targetSceneIteration;
    public string respawnScene = "LeftRuins_Tutorial";
    public bool fixedRiaMemory = false;

    public int maxHealth;
    public int currentHealth;

    public string spawnPointName;
    public string transitionType;
    
    void Awake()
    {
        itemsRiaMemory = new Dictionary<string, int>()
        {
            { "key", -1 },
            { "sofa", -1 }
        };

        sceneIterations = new Dictionary<string, int>();
        targetSceneIteration = new Dictionary<string, int>();
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
            }
            else
            {
                sceneIterations[scenes[i]] ++;
            }
        }
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
    
}
