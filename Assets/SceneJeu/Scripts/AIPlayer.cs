using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIPlayer : MonoBehaviour
{

    public NavMeshAgent player;
    public Transform[] buts;
    private float speedPlayer = 5f;
    private float slowSpeed = 1f;
    public WallMoving wallMoving;
    private Animator animator;

    private bool waitForTheWall = false;
    private Transform currentGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player= GetComponent<NavMeshAgent>();
        player.speed = speedPlayer;
        choseNextGoal();
    }

 

    // Update is called once per frame
    void Update()
    {
        float normalSpeed = player.velocity.magnitude;
        animator.SetFloat("Speed", normalSpeed / player.speed);

        if (!player.isOnNavMesh)
            return;

        if(waitForTheWall)
        {
            if(wallMoving !=null && wallMoving.IsOpen())
            {
                player.isStopped = false;
                waitForTheWall = false;

                if(currentGoal != null)
                {
                    player.SetDestination(currentGoal.position);
                }
            }
            else
            {
                player.isStopped=true;
            }
            return;
        }

        if(!player.pathPending && player.remainingDistance < 0.2f)
        {
            StartCoroutine(HandleParticles(currentGoal));

            choseNextGoal();
        }
    }

    private IEnumerator HandleParticles(Transform currentGoal)
    {
        ParticleSystem particules = currentGoal.GetComponentInChildren<ParticleSystem>();

        if (particules != null)
        {
            particules.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // attendre 2 secondes
            yield return new WaitForSeconds(2f);

            // ensuite relancer
            particules.Play();
        }
    }

    private void choseNextGoal()
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrée dans trigger : " + other.name);

        if (other.CompareTag("SlowZone"))
        {
            player.speed = slowSpeed;
        }

        if (other.CompareTag("WaitZone"))
        {
            if(wallMoving != null && !wallMoving.IsOpen())
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
            if(wallMoving != null && !wallMoving.IsOpen())
            {
                waitForTheWall = true;
                player.isStopped = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            player.speed = speedPlayer;
        }
    }
}
