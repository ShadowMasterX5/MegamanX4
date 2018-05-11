using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Custom/Player Stats")]
public class PlayerStats : Stats
{
    public bool isControllable;
    public bool isDashing;
    public bool isOnLadder;
    public bool onTopOfLadder;
    public bool hasLadderAbove;
    public bool ladderFromTop;
}