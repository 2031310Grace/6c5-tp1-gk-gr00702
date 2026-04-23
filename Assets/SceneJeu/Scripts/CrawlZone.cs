using UnityEngine;

public class CrawlZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerScene2>();
        if (player != null)
            player.SetCrawling(true);
    }

    void OnTriggerExit(Collider other)
    {
        var agent = other.GetComponentInParent<PlayerScene2>();
        if (agent != null)
            agent.SetCrawling(false);
    }


}
