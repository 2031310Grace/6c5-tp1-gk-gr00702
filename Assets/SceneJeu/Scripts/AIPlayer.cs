using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIPlayer : MonoBehaviour
{

    public NavMeshAgent player;
    public Transform[] buts;

    private Transform currentGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player= GetComponent<NavMeshAgent>();
        choseNextGoal();
    }

 

    // Update is called once per frame
    void Update()
    {
        if(!player.pathPending && player.remainingDistance < 0.5f)
        {
            choseNextGoal();
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
}
