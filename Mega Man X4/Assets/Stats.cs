using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Custom/Stats")]
public class Stats : ScriptableObject
{
    [Range(0, 20)] public int maxHealth;
    public bool facingRight;
    public bool beingDamaged;
    public bool isJumping;
    public bool isMoving;
    public bool isToLand;
    public bool isGrounded;
    public bool isShooting;
}