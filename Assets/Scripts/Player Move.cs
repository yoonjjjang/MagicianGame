using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 move;


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        movePlayer();
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }



    /*
    private Joystick joystick;

    private Transform movingMarker;

    private Animator animator;
    [SerializeField]
    public float movespeed { get; set; }
    bool Movement;
    float x;
    float z;

    void Awake()
    {

        movespeed = 3;
        joystick = GameObject.FindGameObjectWithTag("M_joystick").GetComponent<FixedJoystick>();
        animator = GetComponent<Animator>();
        movingMarker = transform.Find("movingMarker");
    }

    // Update is called once per frame

    void Update()
    {

            x = joystick.Horizontal;
            z = joystick.Vertical;

            if (x != 0 || z != 0)
            {
                movingMarker.position = new Vector3(x + transform.position.x, transform.position.y, z + transform.position.z);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                transform.LookAt(new Vector3(movingMarker.position.x, movingMarker.position.y, movingMarker.position.z));
                transform.Translate(Vector3.forward * Time.deltaTime * movespeed);
                animator.SetBool("runToIdle", false);
                animator.SetBool("idleToRun", true);

                Movement = true;
            }
            else if (Movement == true)
            {
                animator.SetBool("runToIdle", true);
                animator.SetBool("idleToRun", false);
                Movement = false;
            }
        
    }
    */

}