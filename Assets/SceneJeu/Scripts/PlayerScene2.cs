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

        if (!isCrawling)
        {
            float distance = Vector3.Distance(transform.position, player.destination);
            player.speed = (distance < slowRadius ) ? speedWalk : speedRun;
        }

        float normalSpeed = player.velocity.magnitude / speedRun;
        animator.SetFloat("Speed", normalSpeed, 0.1f, Time.deltaTime);
    }

    public void SetCrawling(bool b)
    {
        isCrawling = b;
        animator.SetBool("isCrawling", b);

        if (b) player.speed = speedWalk;
    }

    public void Wait(bool pause)
    {
        player.isStopped = pause;
        if (!pause) this.choseNextGoal();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("CrawlZone"))
        {
            SetCrawling(true);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("CrawlZone"))
        {
            SetCrawling(false);
        }
    }
}
