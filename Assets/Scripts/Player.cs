using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int amountThrown;
    private List<Wristband> bands = new List<Wristband>();

    public Player(int amountOfWristbands /*maximum should be two*/)
    {
        if (amountOfWristbands == 1) bands.Add(new Wristband());
        else if (amountOfWristbands == 2)
        {
            bands.Add(new Wristband());
            bands.Add(new Wristband());
        }
        amountThrown = 0;
    }

    public List<Wristband> getBands() { return bands; }

    public void ReduceAmountThrown() { amountThrown -= 1; } //Called when band is retrieved

    //Player is going to have two bands so seperate methods are going to be made for them
    public void ThrowBandOne()
    {
        //if first band hasn't been thrown
        if (!bands[0].GetThrown())
        {
            bands[0].UpdateThrown(true);
            amountThrown += 1;
        } 
    }

    public void ThrowBandTwo()
    {
        if (bands.Count != 2) return;
        if (!bands[1].GetThrown())
        {
            bands[1].UpdateThrown(true);
            amountThrown += 1;
        }
    }
}
