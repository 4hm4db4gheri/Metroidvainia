using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Movement : StateMachineBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float y_offset = 10f;
    private Transform playerPos;
    private Rigidbody2D rb;
    private Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(playerPos.position.x, playerPos.position.y + y_offset);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        boss.lookAtPlayer();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
