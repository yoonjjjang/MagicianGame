using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;

    public void Use()
    {
        StartCoroutine("Shot");
    }

    IEnumerator Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        //Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();

        float speed = 10.0f; // �����̴� �ӵ�
        Vector3 direction = bulletPos.forward;

        // �Ѿ��� õõ�� �̵���Ű�� ����
        float elapsedTime = 0f;
        float maxDuration = 2.0f; // �̵��� �Ϸ�Ǵµ� �ɸ��� �ð�

        while (elapsedTime < maxDuration && instantBullet != null)
        {
            instantBullet.transform.position += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �̵��� ������ �Ѿ��� �ı�
        if( instantBullet != null)
            Destroy(instantBullet);

        yield return new WaitForSeconds(0.5f);
    }
}
