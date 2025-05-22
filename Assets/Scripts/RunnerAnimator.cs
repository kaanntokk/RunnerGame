using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private Animator animator;   
    [SerializeField] RunnerController runnerController;



    private void Awake() {
        animator = GetComponent<Animator>();

    }


    void Update()
    {
        animator.SetBool(IS_WALKING, runnerController.IsWalking());
        animator.SetBool(IS_RUNNING, runnerController.IsRunning());
        animator.SetBool(IS_JUMPING, runnerController.IsJumping());

    }

    public void JumpForce() {
        runnerController.GetRigidbody().AddForce(Vector3.up * runnerController.jumpForce, ForceMode.Impulse);
    }
}
