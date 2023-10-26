using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook ,joystickLook;
    private Vector3 rotationTarget;
    public bool isPC;
    bool isTargeting;
    float x;
    float y;
    PlayerEvent pEven;

    void Start()
    {
        pEven = GetComponent<PlayerEvent>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context) // Left Stick
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnJoystickLook(InputAction.CallbackContext context) // Right Stick
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    void Awake()
    {
        isTargeting = false;
    }

    void Update()
    {
        if (isPC)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            if (Physics.Raycast (ray, out hit))
            {
                rotationTarget = hit.point;
            }

            movePlayerWithAim();
        }
        else
        {
            if (joystickLook.x == 0 && joystickLook.y == 0)
            {
                movePlayer();
            }
            else
            {
                movePlayerWithAim();
            }
            
        }
        Attack();
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }


    public void movePlayerWithAim()
    {
        if (isPC)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);
            
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);

            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);

            } 
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
    private void Attack()
    {
        if (pEven == null)
            return;
        
        x = joystickLook.x;
        y = joystickLook.y;
        if (x!=0 || y != 0)
        {
            isTargeting = true;
        }
        else if (x == 0 && y == 0 && isTargeting == true)
        {
            pEven.Use();
            isTargeting = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Magic")
        {
            //Magic magic = other.GetComponent<Magic>();
            //curHealth -= bullet.damage;
            //Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            //StartCoroutine(OnDamage());
        }
    }
}