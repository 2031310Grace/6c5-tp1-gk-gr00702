using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScene2 : AIPlayer
{
    private float speedWalk = 1.5f;
    private float speedRun = 3.5f;
    private float slowRadius= 4f;

    private bool isCrawling = false;
    private float crawlSpeed = 0.8f;

    //vient dajouter
    private bool isSlowed = false;
    private float slowZoneSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!player.isOnNavMesh || isCelebrate) return;
        //if (isCelebrate) return;

        if (player.isStopped)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (isCrawling)
        {
            player.speed = crawlSpeed;
            animator.SetFloat("Speed", 0.2f);
            return;
        }

        //vient dajouter
        if (isSlowed)
        {
            player.speed = slowZoneSpeed;
            //animator.SetFloat("Speed", player.velocity.magnitude / speedRun, 0.2f, Time.deltaTime);
        }
        else
        {
            float distance = Vector3.Distance(transform.position, player.destination);
            player.speed = (distance < slowRadius) ? speedWalk : speedRun;
        }

        //float distance = Vector3.Distance(transform.position, player.destination);
        //player.speed = (distance < slowRadius) ? speedWalk : speedRun;

        //if (!isCrawling)
        //{
        //    float distance = Vector3.Distance(transform.position, player.destination);
        //    player.speed = (distance < slowRadius ) ? speedWalk : speedRun;
        //}

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
        //vien dajouter
        if(isCrawling == b) return;

        isCrawling = b;
        animator.SetBool("isCrawling", b);

        if (b)
        {
            //player.speed = speedWalk * 0.5f;
            player.speed = crawlSpeed;

            //vien dajouter
            //StartCoroutine(SmoothSpeedTransition(speedWalk));
        }
    }


    public void Wait(bool pause)
    {
        player.isStopped = pause;
        if (!pause) this.choseNextGoal();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);


        if (other.CompareTag("SlowZone"))
        {
            isSlowed = true;
        }

        if (other.CompareTag("CrawlZone"))
        {
            SetCrawling(true);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("SlowZone"))
        {
            isSlowed = false;
        }

        if (other.CompareTag("CrawlZone"))
        {
            SetCrawling(false);
        }
    }
}
