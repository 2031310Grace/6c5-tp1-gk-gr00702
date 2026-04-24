using UnityEngine;

public class CelebrateBehaviour : StateMachineBehaviour
{
    private AIPlayer aiPlayer;

    override public void OnStateEnter(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Récupère le AIPlayer sur le même GameObject
        if (aiPlayer == null)
            aiPlayer = animator.GetComponentInParent<AIPlayer>();

        aiPlayer.OnVictoryStart();
    }

    override public void OnStateExit(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("ReachedGoal");
        aiPlayer.OnVictoryEnd();
    }
}
