using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [Header("Player Attributes")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpHeight;
    public float gravity;
    public float snap;
    public float turnSpeed;
    public float downForce;

    float verticalSpeed;
    CharacterController controller;

    Animator animator;
    public Transform child;

    public bool stick;

    GameObject platform;

    Vector3 lastPlatformPos = Vector3.zero;

    public float dist=1.1f;
    bool jumping;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        verticalSpeed = 0f;


        animator = GetComponentInChildren<Animator>();

        stick = false;
        jumping = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = transform.forward * Input.GetAxisRaw("Vertical") +
                        transform.right * Input.GetAxisRaw("Horizontal");



        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        //look for platform below
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, dist))
        {
            if (!stick && hit.collider.gameObject.tag == "Platform")
            {
                stick = true;
                platform = hit.collider.gameObject;
                // Initialize lastPlatformPos when starting to stick
                lastPlatformPos = platform.transform.position;
            }

        }

        else
        {
            stick = false;
            platform = null;
        }

        // Calculate platform delta movement if sticking
        Vector3 platMove = Vector3.zero;
        if (stick && platform != null)
        {
            Vector3 currentPlatformPos = platform.transform.position;
            platMove = currentPlatformPos - lastPlatformPos;
            lastPlatformPos = currentPlatformPos;
        }


        //Vector3 platMove = stick ? platform.GetComponentInParent<PlatformMovement>().deltaPos : Vector3.zero;
        //controller.Move(platMove);




        if ((controller.isGrounded || stick) && !jumping)
        {

            verticalSpeed = -downForce;


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
                verticalSpeed = jumpHeight;
                animator.SetTrigger("Jump");
                StartCoroutine(KeepJump());
            }

        }
        else
        {
            Debug.Log("Falling");
            verticalSpeed -= gravity * Time.deltaTime;
            animator.SetTrigger("Falling");

        }



        if (Physics.SphereCast(transform.position,0.5f, Vector3.up, out hit, dist-0.5f))
        {
            verticalSpeed = -5f;

        }


        if (input != Vector3.zero)
            child.rotation = Quaternion.Slerp(child.rotation, Quaternion.LookRotation(input), snap * Time.deltaTime);

        animator.SetBool("isGrounded", controller.isGrounded || stick);


        /*
        if (stick && platform != null && controller.isGrounded)
        {
            //Debug.Log(platform.GetComponent<PlatformMovement>().move);
            Debug.Log("Plat");
            controller.Move((platform.GetComponentInParent<PlatformMovement>().move + input.normalized * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
            
        }
        else
        {

            controller.Move((input.normalized * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
            Debug.Log("OFF");
        }
        */
        controller.Move(platMove);
        controller.Move((input.normalized * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime);

        //ignore ISGROUNDED WHEN STICKING TO PLAT



        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + dist * Vector3.down);


        Gizmos.DrawSphere(transform.position + (dist - 0.5f) * Vector3.down, 0.5f);
    }

    IEnumerator KeepJump()
    {
        jumping = true;
        yield return new WaitForSeconds(0.2f);
        jumping = false;
        yield return null;

    }
}
