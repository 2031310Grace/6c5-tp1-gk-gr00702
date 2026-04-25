using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScene2 : AIPlayer
{
    private float speedWalk = 1.5f;
    private float speedRun = 5f;
    private float slowRadius= 4f;
    private bool isCrawling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!player.isOnNavMesh) return;

        if (isCelebrate) return;

        if(player.isStopped)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (!isCrawling)
        {
            float distance = Vector3.Distance(transform.position, player.destination);
            player.speed = (distance < slowRadius ) ? speedWalk : speedRun;
        }

        float normalSpeed = player.velocity.magnitude / speedRun;
        animator.SetFloat("Speed", normalSpeed, 0.2f, Time.deltaTime);

        //if(!player.pathPending && player.remainingDistance < 0.2f 
        //    && !player.isStopped)
        //{
        //    animator.SetTrigger("ReachedGoal");
        //}
    }

    public void SetCrawling(bool b)
    {
        isCrawling = b;
        animator.SetBool("isCrawling", b);

        if (b)
        {
            player.speed = speedWalk * 0.5f;

            //vien dajouter
            //StartCoroutine(SmoothSpeedTransition(speedWalk));
        }
    }

    //vien dajouter
    //private IEnumerator SmoothSpeedTransition(float targetSpeed)
    //{
    //    float startSpeed = player.speed;
    //    float elapsed = 0f;
    //    float duration = 0.3f;

    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        player.speed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
    //        yield return null;
    //    }

    //    player.speed = targetSpeed;
    //}

    public void Wait(bool pause)
    {
        player.isStopped = pause;
        if (!pause) this.choseNextGoal();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //if (other.CompareTag("CrawlZone"))
        //{
        //    SetCrawling(true);
        //}
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //if (other.CompareTag("CrawlZone"))
        //{
        //    SetCrawling(false);
        //}
    }
}
