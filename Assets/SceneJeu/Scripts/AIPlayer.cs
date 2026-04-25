using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class AIPlayer : MonoBehaviour
{

    public NavMeshAgent player;
    public Transform[] buts;
    private float speedPlayer = 5f;
    private float slowSpeed = 1f;
    public WallMoving wallMoving;
    protected Animator animator;

    private bool waitForTheWall = false;
    private Transform currentGoal;
    protected bool isCelebrate = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<NavMeshAgent>();
        player.speed = speedPlayer;
        choseNextGoal();
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        //float normalSpeed = player.velocity.magnitude;
        //animator.SetFloat("Speed", normalSpeed / player.speed);

        if (!player.isOnNavMesh) return;

        if (!isCelebrate)
        {
            float normalSpeed = player.velocity.magnitude;
            animator.SetFloat("Speed", normalSpeed / player.speed);
        }

        if (waitForTheWall)
        {
            if (wallMoving != null && wallMoving.IsOpen())
            {
                player.isStopped = false;
                waitForTheWall = false;

                if (currentGoal != null)
                {
                    player.SetDestination(currentGoal.position);
                }
            }
            else
            {
                player.isStopped = true;
            }
            return;
        }

        if (!isCelebrate && !player.pathPending && player.hasPath && player.remainingDistance < 0.2f)
        {
            //StartCoroutine(HandleParticles(currentGoal));
            //choseNextGoal();
            isCelebrate = true;
            StartCoroutine(GoalReached());
        }
    }

    private IEnumerator GoalReached()
    {
        //isCelebrate = true;

        //player.isStopped = true;
        //player.ResetPath();
        //animator.SetFloat("Speed", 0f);

        ////vien dajouter
        ////player.velocity = Vector3.zero;
        ////ParticleSystem particules = currentGoal.GetComponentInChildren<ParticleSystem>();


        //StartCoroutine(HandleParticles(currentGoal));
        //animator.SetTrigger("ReachedGoal");

        ////celebration
        //yield return new WaitForSeconds(3f);

        ////vien d'ajouter
        ////if (particules != null)
        ////{
        ////    particules.Play();
        ////}

        //choseNextGoal();

        //isCelebrate = true;
        player.isStopped = true;
        player.ResetPath();
        player.velocity = Vector3.zero;
        animator.SetFloat("Speed", 0f);

        StartCoroutine(HandleParticles(currentGoal));
        animator.SetTrigger("ReachedGoal");

        yield break;

        //player.isStopped = false;
        //isCelebrate = false;
    }

    public void OnVictoryStart()
    {
       
    }

    //viens dajouter
    public void OnVictoryEnd()
    {
        isCelebrate = false;
        player.velocity = Vector3.zero;
        player.isStopped = false;
        //isCelebrate = false;
        choseNextGoal();
    }


    private IEnumerator HandleParticles(Transform currentGoal)
    {
        ParticleSystem particules = currentGoal.GetComponentInChildren<ParticleSystem>();

        if (particules != null)
        {
            particules.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // attendre 2s
            yield return new WaitForSeconds(5f);

            particules.Play();
        }
    }

    protected void choseNextGoal()
    {
        Transform newGoal;

        do
        {
            newGoal = buts[Random.Range(0, buts.Length)];
        }
        while (newGoal == currentGoal);

        currentGoal = newGoal;
        player.SetDestination(currentGoal.position);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrée dans trigger : " + other.name);

        if (other.CompareTag("SlowZone"))
        {
            player.speed = slowSpeed;
        }

        if (other.CompareTag("WaitZone"))
        {
            if (wallMoving != null && !wallMoving.IsOpen())
            {
                waitForTheWall = true;
                player.isStopped = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaitZone"))
        {
            if (wallMoving != null && !wallMoving.IsOpen())
            {
                waitForTheWall = true;
                player.isStopped = true;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            player.speed = speedPlayer;
        }
    }



}
