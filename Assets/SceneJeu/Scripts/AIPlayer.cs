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
    private float speedPlayer = 3.5f;
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

        //
        if (player == null)
        {
            Debug.LogError("NavMeshAgent manquant sur " + gameObject.name);
            return;
        }

        player.speed = speedPlayer;
        choseNextGoal();
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        //float normalSpeed = player.velocity.magnitude;
        //animator.SetFloat("Speed", normalSpeed / player.speed);

        //
        if (player == null) return;

        if (!player.isOnNavMesh) return;

        if (animator != null && !isCelebrate)
        {
            float normalSpeed = player.velocity.magnitude;
            animator.SetFloat("Speed", normalSpeed / player.speed);
        }


        //if (!isCelebrate)
        //{
        //    float normalSpeed = player.velocity.magnitude;
        //    animator.SetFloat("Speed", normalSpeed / player.speed);
        //}

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


    /// <summary>
    /// Coroutine déclenchée à l'arrivée au but.
    /// Arrête l'agent, lance les particules et déclenche l'animation de célébration si disponible.
    /// Si aucun Animator n'est présent (capsules), repart automatiquement après un délai.
    /// </summary>
    /// <returns>IEnumerator pour la coroutine Unity.</returns>
    private IEnumerator GoalReached()
    {

        player.isStopped = true;
        player.ResetPath();
        player.velocity = Vector3.zero;
        //animator.SetFloat("Speed", 0f);

        //
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("ReachedGoal");
        }

        StartCoroutine(HandleParticles(currentGoal));
        //animator.SetTrigger("ReachedGoal");

        // pas d'animator, repartir immédiatement
        if (animator == null)
        {
            yield return new WaitForSeconds(1f);
            OnVictoryEnd();
        }

        yield break;

        //player.isStopped = false;
        //isCelebrate = false;
    }


    /// <summary>
    /// Appelée par l'Animator au début de l'animation de victoire.
    /// Peut être surchargée par les classes filles pour ajouter un comportement spécifique.
    /// </summary>
    public void OnVictoryStart()
    {
       
    }


    /// <summary>
    /// Appelée par le StateMachineBehaviour (ReachedGoal) à la fin de l'animation de victoire,
    /// ou automatiquement après un délai si aucun Animator n'est présent.
    /// Réinitialise l'état de célébration et choisit le prochain but.
    /// </summary>
    public void OnVictoryEnd()
    {
        isCelebrate = false;
        player.velocity = Vector3.zero;
        player.isStopped = false;
        //isCelebrate = false;
        choseNextGoal();
    }



    /// <summary>
    /// Coroutine qui arrête temporairement l'effet de particules du but atteint,
    /// attend un délai, puis le relance.
    /// </summary>
    /// <param name="currentGoal">Le but dont on veut gérer les particules.</param>
    /// <returns>IEnumerator pour la coroutine Unity.</returns>
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


    /// <summary>
    /// Choisit aléatoirement un nouveau but différent du but actuel,
    /// puis ordonne à l'agent de s'y diriger
    /// </summary>
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

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("Sortie de lazone trigger : " + other.name);

        if (other.CompareTag("SlowZone"))
        {
            player.speed = speedPlayer;
        }
    }

}
