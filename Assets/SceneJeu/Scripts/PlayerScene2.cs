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



    /// <summary>
    /// Appelle le Update du parent puis gère :
    /// la vitesse adaptative selon la distance au but,
    /// la vitesse réduite en zone lente,
    /// et la mise à jour du paramètre Speed de l'Animator.
    /// Ne fait rien si le personnage célèbre ou est arrêté.
    /// </summary>

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

        //
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


    /// <summary>
    /// Active/désactive le mode ramper du personnage.
    /// Met à jour le paramètre "isCrawling" de l'Animator et ajuste la vitesse.
    /// Ignore l'appel si l'état demandé est déjà l'état actuel.
    /// </summary>
    /// <param name="b">True pour activer le ramper, false pour le désactiver.</param>
    public void SetCrawling(bool b)
    {
        //
        if(isCrawling == b) return;

        isCrawling = b;
        animator.SetBool("isCrawling", b);

        if (b)
        {
            //player.speed = speedWalk * 0.5f;
            player.speed = crawlSpeed;

            //
            //StartCoroutine(SmoothSpeedTransition(speedWalk));
        }
    }



    /// <summary>
    /// Relance le déplacement de l'agent.
    /// Appelée par le StateMachineBehaviour ReachedGoal à l'entrée et à la sortie
    /// de l'animation de célébration.
    /// </summary>
    /// <param name="pause">True pour arrêter l'agent, false pour le relancer vers le prochain but.</param>
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
