using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurretManager : MonoBehaviour
{
    [SerializeField] private GameManager moneyManager;
    [SerializeField] private GameObject floor1;
    [SerializeField] private GameObject floor2;
  
    bool enemyFloor1_full;
    bool enemyFloor2_full;
    bool changeTurretFloor1;
    bool changeTurretFloor2;

    public void Start()
    {
        floor1.SetActive(true);
        floor2.SetActive(false);
    }


    public void Update()
    {
        if (!enemyFloor1_full) TurretOptions(floor1, ref enemyFloor1_full,  0);
     
        if (enemyFloor1_full && !enemyFloor2_full) TurretOptions(floor2,ref enemyFloor2_full, 200);
       
        if (enemyFloor1_full && !changeTurretFloor1) ChangeTurret(floor1, ref enemyFloor1_full, ref changeTurretFloor1);
      
        if (enemyFloor2_full && !changeTurretFloor2) ChangeTurret(floor2, ref enemyFloor2_full, ref changeTurretFloor2);
      
    }

    void ChangeTurret(GameObject floor, ref bool full, ref bool changeFloor)
    {
        if (moneyManager.AImoney > 500)
        {
            full = true;
            changeFloor = true;
            moneyManager.AImoney -= 250;

            floor.transform.GetChild(1).gameObject.SetActive(true);
            floor.transform.GetChild(0).gameObject.SetActive(false);
        }
        else return;
    }

    void TurretOptions(GameObject floor ,ref bool full,  float value)
    {
        if (moneyManager.AImoney > 300 + value)
        {
            full = true;
            moneyManager.AImoney -= 250;
            floor.transform.GetChild(1).gameObject.SetActive(true);
            floor2.SetActive(true);
        }
        else if (moneyManager.AImoney > 150 + value)
        {
            full = true;
            moneyManager.AImoney -= 100;
            floor.transform.GetChild(0).gameObject.SetActive(true);
            floor2.SetActive(true);
        }
        else return;
    }
}
