using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float damage = 10;
    public LayerMask whatToHit;
    public Transform BusterProjectileMin;
    public Transform BusterProjectileMed;
    public Transform BusterProjectileMax;
    public int chargeLimitMed = 30;
    public int chargeLimitMax = 60;
    
    int charge;

    Transform firePoint;
    Transform direction;
    Quaternion projectileRotation;  
    

    string shootKey = "z";

	// Use this for initialization
	void Start () {
		firePoint = transform.Find("FirePoint");
        direction = transform.Find("Direction");
	    projectileRotation = new Quaternion();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(shootKey)) 
        {
            charge++;
        }
        if(Input.GetKeyUp(shootKey))
        {
            Shoot();
            if (charge > chargeLimitMax)
            {
                Effect("max");
            }
            else if (charge > chargeLimitMed)
            {
                Effect("med");
            }   
            else
            {
                Effect("min");
            }
            
            charge = 0;
        }

	}


    void Shoot () {
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 fireDirection = new Vector2(direction.position.x, direction.position.y);
        //Physics2D.Raycast(Origem, Direção, Distancia, Oque não acertar)
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, fireDirection, 100, whatToHit);
        Debug.DrawLine(firePointPosition, fireDirection);
        
        //Debug.Log("firePosition = " + firePointPosition + "fireDirection = " + fireDirection + "minus = " + (fireDirection - firePointPosition));
        if(hit.collider != null) //acertou
        {
            
        }


	}

    void Effect(string projectileType)
    {
        if (firePoint.position.x > direction.position.x) // character is turning left
        {
            projectileRotation.Set(0, 0, 1, 0);
        }
        else                                              // character is turning left
        {
            projectileRotation.Set(0, 0, 0, 1);
        }

        if (projectileType.Equals("min"))
        {
            Instantiate(BusterProjectileMin, firePoint.position, projectileRotation);
        }
        else if (projectileType.Equals("med"))
        {
            Instantiate(BusterProjectileMed, firePoint.position, projectileRotation);
        }
        else
        {
            Instantiate(BusterProjectileMax, firePoint.position, projectileRotation);
        }
        
        
    }
}
