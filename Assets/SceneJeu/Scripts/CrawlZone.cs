using System.Collections;
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
        {
            player.StartCoroutine(StartCraw(player));
        }
            //player.SetCrawling(true);
    }

    void OnTriggerExit(Collider other)
    {
        var agent = other.GetComponentInParent<PlayerScene2>();
        if (agent != null)
        {
             agent.StartCoroutine(StopCraw(agent));
        }
            //agent.SetCrawling(false);
    }


    IEnumerator StartCraw(PlayerScene2 player)
    {
        player.player.speed = 1f; // ralentit avant
        yield return new WaitForSeconds(0.2f);

        player.SetCrawling(true);
    }

    IEnumerator StopCraw(PlayerScene2 player)
    {
        yield return new WaitForSeconds(0.1f);

        player.SetCrawling(false);
    }


}
