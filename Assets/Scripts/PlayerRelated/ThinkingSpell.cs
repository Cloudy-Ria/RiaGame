using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThinkingSpell : MonoBehaviour
{
    [SerializeField] Transform ItemHolder;
    [SerializeField] GameObject Cloud;
    [SerializeField] Transform SpellPoint;
    [SerializeField] GameObject StandIn, KeyMemory, SofaMemory, KeyMagic, SofaMagic;
    [SerializeField] List<GameObject> SpawnedItems;
    public readonly int MaxSpawnedItems = 2;
    private int spawnedItemIndex = 0;
    Dictionary<string, GameObject> memories;
    Dictionary<string, GameObject> magic;
    List<string> items = new List<string>();
    private int itemsCount;
    private int currentIndex;
    private bool UnSpawnable = false;
    GameState gameState;
    GameObject currentMemory = null;
    void Start()
    {
        SpawnedItems = new List<GameObject>();
        for (int i = 0; i < MaxSpawnedItems; i++)
        {
            SpawnedItems.Add(null);
        }
    }
    private void OnEnable()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        memories = new Dictionary<string, GameObject>()
        {
            { "Key", KeyMemory },
            { "Sofa", SofaMemory }
        };
        magic = new Dictionary<string, GameObject>()
        {
            { "Key", KeyMagic },
            { "Sofa", SofaMagic }
        };
    }
    public void Enable()
    {
        UnSpawnable = false;
        gameObject.SetActive(true);
        Cloud.SetActive(true);
        currentIndex = -1;
        items = new List<string>();
        itemsCount = 0;
        var keys = gameState.itemsRiaMemory.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            if (gameState.itemsRiaMemory[keys[i]] > -1)
            {
                items.Add(keys[i]);
                itemsCount++;
            }
        }
        GetComponent<Animator>().Play("CloudAnim", 0, 0);
    }

    void Update()
    {
        
    }

    public bool HasItems()
    {
        return itemsCount > 0;
    }

    public void ChangeItem()
    {
        if (UnSpawnable) return;
        if (itemsCount == 0)
        {
            if (currentMemory == null)
            {
                currentMemory = Instantiate(StandIn);
                currentMemory.transform.SetParent(ItemHolder);
                currentMemory.transform.position = ItemHolder.position;
            }
            return;
        }

        int index;
        currentIndex++;
        if (currentIndex >= itemsCount)
        {
            currentIndex = 0;
        }
        index = currentIndex;
        
        if (currentMemory != null)
        {
            Destroy(currentMemory);
        }

        currentMemory = Instantiate(memories[items[index]]);
        currentMemory.transform.SetParent(ItemHolder);
        currentMemory.transform.position = ItemHolder.position;
    }

    public void Fire()
    {
        Cloud.SetActive(false);
        UnSpawnable = true;
        Destroy(currentMemory);
    }
    public void SpawnItem()
    {
        if (SpawnedItems.Count == MaxSpawnedItems)
        {
            Destroy(SpawnedItems[spawnedItemIndex]);
        }
        GameObject newItem = Instantiate(magic[items[currentIndex]]);
        newItem.transform.position = SpellPoint.position;
        SpawnedItems[spawnedItemIndex] = newItem;
        spawnedItemIndex = (spawnedItemIndex + 1) % MaxSpawnedItems;
        gameObject.SetActive(false);

    }
}
