using Unity.VisualScripting;
using UnityEngine;

public class PlayerScene2 : AIPlayer
{
    private float speedWalk = 1.5f;
    private float speedRun = 5f;
    private float slowRadius= 4f;
    private bool isCrawling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        animator.SetFloat("Speed", normalSpeed);
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
}
