using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;
    public GameObject obj;
    int ItemIndex;
    public GameObject[] items;
    public int health;
    public int mana;
    public int maxHealth;
    public int maxMana;

    GameObject btnImg;
    private void Start()
    {
        btnImg = GameObject.FindGameObjectWithTag("ImgBtn");
    }
    public void Use()
    {
        StartCoroutine("Shot");
    }

    public void UseItem()
    {
        obj = GameObject.Find("wizard_01");
        ItemIndex = obj.GetComponent<PlayerController>().ItemIndex;
        items = obj.GetComponent<PlayerController>().items;
        health = obj.GetComponent<PlayerController>().health;
        mana = obj.GetComponent<PlayerController>().mana;
        maxHealth = obj.GetComponent<PlayerController>().maxHealth;
        maxMana = obj.GetComponent<PlayerController>().maxMana;
        mana = obj.GetComponent<PlayerController>().mana;

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
                        obj.GetComponent<PlayerController>().hpbar.value = (float)health / (float)maxHealth; //체력바 새로고침


                        obj.GetComponent<PlayerController>().hasItems[ItemIndex] = false;
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
                        obj.GetComponent<PlayerController>().mpbar.value = (float)mana / (float)maxMana;


                        obj.GetComponent<PlayerController>().hasItems[ItemIndex] = false;
                        ItemIndex = 2;
                        btnImg.GetComponent<ImageChange>().ChangeImage(ItemIndex);
                    }
                    //마나 상승 mana += manapotionvalue; 'ㅅ'
                    break;

            }
            //Debug.Log("Use 3");

        }
    }
    IEnumerator Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        //Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();

        float speed = 10.0f; // 움직이는 속도
        Vector3 direction = bulletPos.forward;

        // 총알을 천천히 이동시키는 루프
        float elapsedTime = 0f;
        float maxDuration = 2.0f; // 이동이 완료되는데 걸리는 시간

        while (elapsedTime < maxDuration && instantBullet != null)
        {
            instantBullet.transform.position += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝나면 총알을 파괴
        if( instantBullet != null)
            Destroy(instantBullet);

        yield return new WaitForSeconds(0.5f);
    }
}
