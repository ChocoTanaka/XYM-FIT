using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager_home : MonoBehaviour
{
    public static float MaxEnergy =12f;
    static float Energy = 12f;
    static int Energyspan;


    //Energy�̉�(gamemode�ł����{)
    private void Update()
    {
        if (Energy < MaxEnergy)
        {
            Energyspan++;
            //24���Ԃ�Max�܂ŉ񕜁i�Ȃ̂�2/5����=2*60*60*20F��0.2�񕜁j
            if (Energyspan == 86400)
            {
                Energyspan = 0;
                GSEnergy += 0.2f;
            }
        }
        
    }

    public static float GetMEnergy()
    {
        return MaxEnergy;
    }


    public static float GSEnergy
    {
        get { return Energy; }
        set { Energy = value; }
    }

    public static int GSEnergyspan
    {
        get { return Energyspan; }
        set { Energyspan = value; }
    }

}
