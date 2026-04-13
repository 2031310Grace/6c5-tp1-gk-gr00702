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
   

    private Transform currentGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player= GetComponent<NavMeshAgent>();
        player.speed = speedPlayer;
        choseNextGoal();
    }

 

    // Update is called once per frame
    void Update()
    {
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            player.speed = speedPlayer;
        }
    }
}
