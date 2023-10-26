using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public MeshRenderer[] meshs;
    public Transform bulletPos;
    public GameObject bullet;

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

    public void Awake()
    {
        InvokeRepeating("ShotStart",1f,1f);
        
    }
    public void ShotStart()
    {
        StartCoroutine("Shot");
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
        if (instantBullet != null)
            Destroy(instantBullet);
    }
    /*
    IEnumerator OnDamage()
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);
    }
    */
}
