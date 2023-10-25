using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public Transform target;
    public float speed;
    public float lifeDuration = 1f;
    private float lifeTimer;
    private Transform myTransform;
    public GameObject impactParticle;

    [HideInInspector]
    public Vector3 impactNormal;
    private bool hasCollided = false;

    void Awake()
    {
        myTransform = transform;
        lifeTimer = lifeDuration;
    }

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("enemy");
        target = go.transform;
        myTransform.LookAt(target);
    }

    void Update()
    {
        float amtToMove = speed * Time.deltaTime;
        myTransform.Translate(Vector3.forward * amtToMove);
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
        }
        Destroy(impactParticle, 1f);
    }
}

