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
    /// Coroutine d�clench�e � l'arriv�e au but.
    /// Arr�te l'agent, lance les particules et d�clenche l'animation de c�l�bration si disponible.
    /// Si aucun Animator n'est pr�sent (capsules), repart automatiquement apr�s un d�lai.
    /// </summary>
    /// <returns>IEnumerator pour la coroutine Unity.</returns>
    private IEnumerator GoalReached()
    {

        player.isStopped = true;
        player.ResetPath();
        player.velocity = Vector3.zero;
        //animator.SetFloat("Speed", 0f);




        //StartCoroutine(HandleParticles(currentGoal));
        //animator.SetTrigger("ReachedGoal");

        if (animator != null)              
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("ReachedGoal");
        }

        if (currentGoal != null)          
            StartCoroutine(HandleParticles(currentGoal));

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
    /// Appel�e par l'Animator au d�but de l'animation de victoire.
    /// Peut �tre surcharg�e par les classes filles pour ajouter un comportement sp�cifique.
    /// </summary>
    public void OnVictoryStart()
    {
       
    }


    /// <summary>
    /// Appel�e par le StateMachineBehaviour (ReachedGoal) � la fin de l'animation de victoire,
    /// ou automatiquement apr�s un d�lai si aucun Animator n'est pr�sent.
    /// R�initialise l'�tat de c�l�bration et choisit le prochain but.
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
    /// Coroutine qui arr�te temporairement l'effet de particules du but atteint,
    /// attend un d�lai, puis le relance.
    /// </summary>
    /// <param name="currentGoal">Le but dont on veut g�rer les particules.</param>
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
    /// Choisit al�atoirement un nouveau but diff�rent du but actuel,
    /// puis ordonne � l'agent de s'y diriger
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
        Debug.Log("Entr�e dans trigger : " + other.name);

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
