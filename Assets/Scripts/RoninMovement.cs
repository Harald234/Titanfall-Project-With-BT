using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoninMovement : MonoBehaviour
{
    bool isMoving;
    bool isDead;
    bool shouldBeStopped;

    public NavMeshAgent agent;
    public Transform player;

    public int Health;

    public Animator roninAnimator;
    public EnterVanguardTitan evt;

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            player = GetClosestPlayer();
            if (player == null) return;
            
            agent.SetDestination(player.position);
            HandleAnimation();
            if (agent.isStopped == false)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            /*if (evt.isEmbarking && !alreadyStopped)
            {
                StartCoroutine(Stop());
                alreadyStopped = true;
            }*/

            if (GetComponentInChildren<Renderer>().isVisible == false)
            {
                agent.isStopped = true;
            }
            else if (GetComponentInChildren<Renderer>().isVisible && !shouldBeStopped)
            {
                agent.isStopped = false;
            }
        }
    }

    public Transform GetClosestPlayer()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in GameObject.FindGameObjectsWithTag("Player"))
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

    IEnumerator Stop()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(2.4f);
        agent.isStopped = false;
    }

    public IEnumerator TakeDamage(int damage)
    {
        Health -= damage;
        agent.isStopped = true;
        shouldBeStopped = true;
        if (Health <= 0)
        {
            isDead = true;
            roninAnimator.SetTrigger("die");
            Destroy(this, 10f);
        }
        else
        {
            roninAnimator.SetTrigger("getHit");
        }


        yield return new WaitForSeconds(1.5f);

        if (!isDead)
            agent.isStopped = false;
        shouldBeStopped = false;
    }

    void HandleAnimation()
    {
        roninAnimator.SetBool("isMoving", isMoving);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            agent.isStopped = true;
        }
    }
}