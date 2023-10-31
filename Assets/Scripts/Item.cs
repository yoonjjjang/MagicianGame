using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { HealPotion, ManaPotion};
    public Type type;
    public int value;
    public int effec;

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}