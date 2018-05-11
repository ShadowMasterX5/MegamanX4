using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour
{
    [SerializeField] GameObject shotPrefab;
    [SerializeField] Transform jumpShotStartPoint;
    [SerializeField] Transform runningShotStartPoint;
    [SerializeField] Transform standShotStartPoint;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] int shotAmount = 3;
    GameObject[] shotPool;

    void Awake()
    {
        shotPool = new GameObject[shotAmount];
        for (int i = 0; i < shotPool.Length; i++)
        {
            shotPool[i] = Instantiate(shotPrefab);
            shotPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (playerStats.isControllable)
        {
            ShootController();
        }
    }

    void ShootController()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShotPooling();
        }
    }

    void ShotPooling()
    {
        int shotIndex = 0;
        while (CurrentShotIsBusy(shotIndex))
        {
            shotIndex++;
            if (shotIndex >= shotAmount)
            {
                print("Sin Balas!");
                break;
            }
        }
        if (shotIndex < shotAmount)
        {
            playerStats.isShooting = true;
            SetupShot(shotPool[shotIndex]);
        }
    }

    bool CurrentShotIsBusy(int index)
    {
        return shotPool[index].activeInHierarchy;
    }

    void SetupShot(GameObject shot)
    {
        Transform shotStartPoint;
        shotStartPoint = GetShotStartPoint();
        shot.SetActive(true);
        shot.transform.position = shotStartPoint.position;
        shot.transform.rotation = shotStartPoint.rotation;
        shot.transform.localScale = shotStartPoint.parent.localScale; //parent porque megaman(padre del componente)
        //usa x-scale -1 para girar
    }

    Transform GetShotStartPoint()
    {
        if (!playerStats.isGrounded)
        {
            return jumpShotStartPoint;
        }
        else if (playerStats.isMoving)
        {
            return runningShotStartPoint;
        }
        else
        {
            return standShotStartPoint;
        }
    }

}