using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateCharacter", menuName = "Character")]
public class CharacterScriptableObject : ScriptableObject
{

    [Header("ClassType")]
    public GameObject character;
    public ClassType ClassType;

    [Space(10)]

    [Header("Spawn")]
    [Range(0,20)] public float spawnTimer;

    [Space(10)]

    [Header("Movement")]
    public float MovementSpeed;
    public float StopDistance;

    [Space(10)]

    [Header("Attack")]
    public float Damage;
    public float AttackDistance;
    public float AttackTimer;

    [Space(10)]

    [Header("Health")]
    public float MaxHealth;
    public float CurrentHealth;

    [Space(10)]

    [Header("Gold")]
    public float Price;
    public float KillReward;

    [Space(10)]
    public LayerMask LayerMask;


   

}
