using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Pool;
using UnityEngine.UI;
//테스트
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook ,joystickLook;
    private Vector3 rotationTarget;
    private TouchControl touchControl;
    public bool isPC;
    bool isTargeting = false;
    bool isShot = false;
    bool isGroggy = false;
    public bool isRecharging = false;
    public bool isBorder;
    float x;
    float y;
    PlayerEvent pEven;

    public GameObject[] items;
    public bool[] hasItems;
    int ItemIndex;
    private int preItems = 0;
    GameObject nearObject;
    public int health;
    public int mana;

    public int maxHealth;
    public int maxMana;

    GameObject btnImg;

    Animator anim;

    [SerializeField]
    private Slider hpbar;
    [SerializeField]
    private Slider mpbar;

    void Start()
    {
        pEven = GetComponent<PlayerEvent>();
        btnImg = GameObject.FindGameObjectWithTag("ImgBtn");
        hpbar.value = (float)health / (float)maxHealth; //체력바 초기화
        mpbar.value = (float)mana / (float)maxMana;
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

    public void OnUseItem(InputAction.CallbackContext context)
    {
        UseItem();
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        isTargeting = false;
        touchControl = new TouchControl();
    }

    void Update()
    {
        anim.SetBool("idleToStun", isGroggy);
        if (!isGroggy)
        {
            if (isPC)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mouseLook);

                if (Physics.Raycast(ray, out hit))
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
        if(mana < 10 && !isRecharging)
            StartCoroutine("RechargeMana");

        StopToWall();
    }

    public void movePlayer()
    {
        
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        if(!isBorder)
            transform.Translate(movement * speed * Time.deltaTime, Space.World);

        anim.SetBool("idleToRun", movement != Vector3.zero);
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
        if (!isShot && mana > 0)
        {
            if (pEven == null)
                return;

            x = joystickLook.x;
            y = joystickLook.y;
            if (x != 0 || y != 0)
            {

                isTargeting = true;

            }
            else if (x == 0 && y == 0 && isTargeting == true && mana >= 1)
            {
                mana--;
                pEven.Use();
                isShot = true;
                Invoke("AttackOut", 0.82f);
                mpbar.value = (float)mana / (float)maxMana;
                isTargeting = false;
            }

        }
        anim.SetBool("idleToAttack01", isShot);
    }


    void AttackOut()
    {
        Debug.Log("shot!");
        isShot = false;
    }

    private void UseItem()
    {
        if (items[ItemIndex] != null)
        {
            switch (items[ItemIndex].GetComponent<Item>().type)
            {
                case Item.Type.HealPotion:
                    //Debug.Log("Use 1");
                    //Debug.Log(items[ItemIndex].GetComponent<Item>().effec);
                    if (health < maxHealth)
                    {
                        health += items[ItemIndex].GetComponent<Item>().effec;
                        if (health > maxHealth)
                            health = maxHealth;
                        hpbar.value = (float)health / (float)maxHealth; //체력바 새로고침


                        hasItems[ItemIndex] = false;
                        ItemIndex = 2;
                        btnImg.GetComponent<ImageChange>().ChangeImage(ItemIndex);
                    }
                    break;
                case Item.Type.ManaPotion:
                    //Debug.Log("Use 2");
                    if (mana < maxMana)
                    {
                        mana += items[ItemIndex].GetComponent<Item>().effec;
                        if (mana > maxMana)
                            mana = maxMana;
                        mpbar.value = (float)mana / (float)maxMana;


                        hasItems[ItemIndex] = false;
                        ItemIndex = 2;
                        btnImg.GetComponent<ImageChange>().ChangeImage(ItemIndex);
                    }
                    //마나 상승 mana += manapotionvalue; 'ㅅ'
                    break;

            }
            //Debug.Log("Use 3");
            
        }
    }

    private void Die()
    {
        anim.SetBool("idleToDie", health == 0);


        //여기서 사망 후 결과 처리 하면 될듯??
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Magic")
        {
            Magic magic = other.GetComponent<Magic>();
            health -= magic.damage;
            //Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            hpbar.value = (float)health / (float)maxHealth; //체력바 새로고침
            move.x = 0; move.y = 0;
            if (health == 0)
            {
                Die();  //플레이어 체력 0일때 진입
            }
            else //아니면 일반 피격 효과  기절탄일때와 아닐때 스턴o, x도 나중에 구분해줘야할듯
            {
                isGroggy = true;
                transform.GetChild(3).gameObject.SetActive(true);
                Invoke("GroggyOut", 1.0f);
            }

            //StartCoroutine(OnDamage());
        }
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            ItemIndex = item.value;
            hasItems[preItems] = false;
            hasItems[ItemIndex] = true;
            preItems = ItemIndex;
            btnImg.GetComponent<ImageChange>().ChangeImage(ItemIndex);
            Destroy(other.gameObject);

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
            nearObject = other.gameObject;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
            nearObject = null;
    }


    void GroggyOut()
    {
        transform.GetChild(3).gameObject.SetActive(false);
        isGroggy = false;
    }
    
    IEnumerator RechargeMana()
    {
        isRecharging = true;
        while(mana < 10)
        {
            yield return new WaitForSecondsRealtime(2.0f);
            mana++;
            mpbar.value = (float)mana / (float)maxMana;
            if (mana >= 10)
            {
                isRecharging = false;
                yield break;
            }
        }
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 1, LayerMask.GetMask("Wall"));
    }
}

