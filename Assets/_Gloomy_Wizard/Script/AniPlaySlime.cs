﻿using UnityEngine;
using System.Collections;

public class AniPlaySlime : MonoBehaviour
{
    public Transform[] transforms;
    public GUIContent[] GUIContents;
    private Animator[] animator;
    private string currentState = "";

    void Start()
    {
        animator = new Animator[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            animator[i] = transforms[i].GetComponent<Animator>();
        }
    }

    private void OnEventFx(GameObject InEffect)
    {
        GameObject newSpell = Instantiate(InEffect);

        Destroy(newSpell, 1.0f);
    }

    void OnGUI()
    {
        GUILayout.BeginVertical("box");
        for (int i = 0; i < GUIContents.Length; i++)
        {

            if (GUILayout.Button(GUIContents[i]))
            {
                currentState = GUIContents[i].text;
            }

            AnimatorStateInfo stateInfo = animator[0].GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("Base Layer.idle"))
            {
                for (int j = 0; j < animator.Length; j++)
                {
                    animator[j].SetBool("idleToIdle01", false);
                    animator[j].SetBool("idleToRun", false);
                    animator[j].SetBool("idleToDamage", false);
                    animator[j].SetBool("idleToAttack01", false);
                    animator[j].SetBool("idleToWin", false);
                    animator[j].SetBool("idleToDie", false);
                }
            }
            else
            {
                for (int j = 0; j < animator.Length; j++)
                {
                    animator[j].SetBool("runToIdle", false);
                    animator[j].SetBool("dieToIdle", false);
                }
            }

            if (currentState != "")
            {

                if (stateInfo.IsName("Base Layer.run") && currentState != "run")
                {
                    for (int j = 0; j < animator.Length; j++)
                    {
                        animator[j].SetBool("runToIdle", true);
                    }
                }

                if (stateInfo.IsName("Base Layer.die") && currentState != "die")
                {
                    for (int j = 0; j < animator.Length; j++)
                    {
                        animator[j].SetBool("dieToIdle", true);
                    }
                }

                switch (currentState)
                {


                    case "run":
                        for (int j = 0; j < animator.Length; j++)
                        {
                            animator[j].SetBool("idleToRun", true);
                        }
                        break;

                    case "damage":
                        for (int j = 0; j < animator.Length; j++)
                        {
                            animator[j].SetBool("idleToDamage", true);
                        }
                        break;

                    case "attack01":
                        for (int j = 0; j < animator.Length; j++)
                        {
                            animator[j].SetBool("idleToAttack01", true);
                        }
                        break;

                    case "die":
                        for (int j = 0; j < animator.Length; j++)
                        {
                            animator[j].SetBool("idleToDie", true);
                        }
                        break;

                    default:
                        break;
                }
                currentState = "";
            }
        }
        GUILayout.EndVertical();
    }



}
