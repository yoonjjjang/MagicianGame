using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public VariableJoystick joy;
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public Camera followCamera;


    public int ammo;
    public int coin;
    public int health;

    float hAxis;
    float vAxis;
    bool wDown;
    bool fDown;


    bool isReload;


    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;



    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


    }


    void FixedUpdate()
    {
        // 1. input value
        float x = joy.Horizontal;
        float z = joy.Vertical;

        // 2. Move Position
        moveVec = new Vector3(x, 0, z) * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec);



        if (moveVec.sqrMagnitude == 0)
            return;


        // 3 Move Rotation
        Quaternion dirQuat = Quaternion.LookRotation(moveVec);
        Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, dirQuat, 0.3f);
        rigid.MoveRotation(moveQuat);


        Debug.Log(moveVec.sqrMagnitude);
       
    }


    void Update()
    {
        GetInput();

        //animation
        anim.SetBool("idleToRun", moveVec != Vector3.zero);
        anim.SetBool("idleToWalk", wDown);
        //Move();
        //Turn();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
    }



}
