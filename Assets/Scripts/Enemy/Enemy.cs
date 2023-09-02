using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int gems;
    [SerializeField]
    protected Transform pointA, pointB;
    [SerializeField]
    protected float combatDistanceTrigger = 5.0f;
    [SerializeField]
    protected float attackDistanceTrigger = 1.5f;
    [SerializeField]
    protected float attackCooldown = 2.0f;

    protected bool inCombat = false;
    protected bool shouldAttack = false;
    protected bool attackBreak = false;

    protected GameObject playerObj;

    protected GameObject enemyObj;
    protected Animator enemyAnimator;
    protected SpriteRenderer enemyRenderer;
    private bool shouldMoveToB = false;
    private bool waiting = false;
    protected bool hit = false;
    protected bool dead = false;

    public virtual void Init()
    {
        enemyObj = transform.GetChild(0).gameObject;
        enemyAnimator = enemyObj.GetComponent<Animator>();
        enemyRenderer = enemyObj.GetComponent<SpriteRenderer>();
        playerObj = GameObject.FindWithTag("Player");
    }

    protected void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        CombatAndAttackCheck();
        if (!hit && !dead && !shouldAttack) MoveAround();
        else if (shouldAttack && !attackBreak && !hit && !dead) Attack();
    }

    protected virtual void Attack()
    {
        StartCoroutine(AttackAndWait());
    }

    protected IEnumerator AttackAndWait()
    {
        enemyAnimator.SetTrigger("Attack");
        attackBreak = true;
        waiting = true;
        enemyAnimator.SetTrigger("Idle");
        enemyAnimator.SetBool("Walk", false);
        yield return new WaitForSeconds(attackCooldown);

        attackBreak = false;
        waiting = false;
        StopCoroutine(AttackAndWait());
    }

    protected virtual void CombatAndAttackCheck()
    {
        inCombat = false;
        shouldAttack = false;

        float distanceFromPlayer = Vector3.Distance(transform.position, playerObj.transform.position);
        if (
            // player is close
            distanceFromPlayer <= combatDistanceTrigger
            // and player is in between waypoints
            && playerObj.transform.position.x > pointA.position.x
            && playerObj.transform.position.x < pointB.position.x
        )
        {
            inCombat = true;
            if (distanceFromPlayer < attackDistanceTrigger)
            {
                shouldAttack = true;
            }
        }
    }

    private void MoveAround()
    {
        Vector3 targetPos = shouldMoveToB ? pointB.transform.position : pointA.transform.position;
        if (inCombat)
        {
            targetPos = playerObj.transform.position;
        }

        enemyRenderer.flipX = false;
        if (targetPos.x < transform.position.x)
        {
            enemyRenderer.flipX = true;
        }

        float distanceFromTarget = Vector3.Distance(transform.position, targetPos);

        // Debug.Log(string.Format("{0} : towards {1}, distance now {2}", enemyObj.name, shouldMoveToB ? "right" : "left", distanceFromTarget));
        if (waiting)
        {
            // do nothing
        }
        else if (distanceFromTarget <= 0.1f)
        {
            StartCoroutine(IdleWaitThenContinue());
        }
        else
        {
            // keep moving        
            enemyAnimator.SetBool("Walk", true);
            Vector3 direction = Vector3.right;
            if (targetPos.x < transform.position.x)
            {
                direction = Vector3.left;
            }
            transform.Translate(speed * Time.deltaTime * direction);
        }
    }

    private IEnumerator IdleWaitThenContinue()
    {
        waiting = true;
        // Debug.Log("Stop Moving");
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetTrigger("Idle");
        yield return new WaitForSeconds(3f);

        // Debug.Log("Start Moving");
        enemyAnimator.SetBool("Walk", true);
        shouldMoveToB = !shouldMoveToB;
        StopCoroutine(IdleWaitThenContinue());
        waiting = false;
    }
   
    protected IEnumerator HitWaitThenContinue()
    {
        enemyAnimator.SetTrigger("Got_Hit");
        hit = true;
        yield return new WaitForSeconds(0.5f);

        hit = false;
        enemyAnimator.SetTrigger("Idle");
        StopCoroutine(HitWaitThenContinue());
    }

    protected IEnumerator DieWaitThenDestroy()
    {
        dead = true;
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);

        StopCoroutine(DieWaitThenDestroy());
        Destroy(gameObject);
    }
}
