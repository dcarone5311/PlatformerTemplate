using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    //public CharacterGroundingReport characterState;
    public float dist;
    bool isGrounded;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = transform.forward * Input.GetAxisRaw("Vertical") +
                transform.right * Input.GetAxisRaw("Horizontal");


        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {

            isGrounded = true;

        }
        else
        {
            isGrounded= false;
        }

        if (isGrounded)
        {

            if (input == Vector3.zero)
                animator.SetInteger("State", 0);
            else
            {


                if (Input.GetKey(KeyCode.LeftShift))
                    animator.SetInteger("State", 2);
                else
                    animator.SetInteger("State", 1);

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
            }

        }
        else
        {
            animator.SetTrigger("Falling");

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + dist * Vector3.down);


        Gizmos.DrawSphere(transform.position + (dist - 0.5f) * Vector3.down, 0.5f);
    }
}
