using System.Collections;
using System.Collections.Generic;
using OnlineSubsystem;
using UnityEngine;
using UnityEngine.UI;

public class ServerBrowser : MonoBehaviour
{
    [SerializeField] private GameObject serverItemPrefab;
    private ScrollRect scrollRect;

    private List<ServerItem> items = new List<ServerItem>();
    
    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    
    // Update is called once per frame
    public void Assign(List<Server> servers,GameGUI gui)
    {
        Clear();
        
        for (int i = 0; i < servers.Count; i++)
        {
            GameObject instance = PoolManager.Instance.spawnObject(serverItemPrefab);
            instance.transform.SetParent(scrollRect.content.transform);
            ServerItem serverItem = instance.GetComponent<ServerItem>();
            serverItem.Assign(servers[i],gui);
            items.Add(serverItem);
        }
        
    }

    private void Clear()
    {
        for (int i = 0; i < items.Count; i++)
        {
            PoolManager.Instance.releaseObject(items[i].gameObject);
        }
        items.Clear();
    }
}
