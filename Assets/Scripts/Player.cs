using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private float walkSpeed;
    private float runSpeed;
    private int amountThrown;
    private int amountOfBands;
    //private List<Wristband> bands = new List<Wristband>();

    public Player(int amountOfWristbands /*maximum should be two*/)
    {
        amountOfBands = amountOfWristbands;
        walkSpeed = 2;
        runSpeed = 4;
        amountThrown = 0;
    }

    public float GetWalkSpeed() { return walkSpeed; }
    public float GetRunSpeed() { return runSpeed; } 
    public int GetAmountOfBands() { return amountOfBands; }

    public void ReduceAmountThrown() { amountThrown -= 1; } //Called when band is retrieved

    public void IncreaseAmountThrown() { amountThrown += 1; }
}
