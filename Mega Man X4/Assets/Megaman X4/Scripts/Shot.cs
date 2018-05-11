using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] float speed = 10f;
    [SerializeField] float damage = 1f;
    [SerializeField] float lifetime = 1f;
    

    private bool facingRight;

    void OnEnable()
    { 
        Vector2 speedVector;
        Invoke("SelfDisable", lifetime);
        if (!playerStats.facingRight)
        {
            speedVector = new Vector2(-speed, 0f);
        }
        else
        {
            speedVector = new Vector2(speed, 0f);
        }
        GetComponent<Rigidbody2D>().velocity = speedVector;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MakeDamage();
        SelfDisable();
    }

    void SelfDisable()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    virtual public void MakeDamage() { }
}