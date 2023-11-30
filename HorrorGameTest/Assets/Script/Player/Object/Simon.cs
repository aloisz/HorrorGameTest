using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Simon : InteractiveObj
{
    public override void Interact(PlayerController player)
    {
        BatterieChecker(player);
    }


    private void BatterieChecker(PlayerController player)
    {
        switch (player.numberOfBatterie)
        {
            case <1:
                Debug.Log("Il vous faut au moins une batterie en main !");
                break;
            
            case >=1:
                Debug.Log("Bien bien");
                break;
            
                
        }
        player.RemoveBatterie(player.numberOfBatterie);
    }
    
}
