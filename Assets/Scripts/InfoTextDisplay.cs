using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI paragraphTextLeft;
    public TextMeshProUGUI paragraphTextRight;

    public void Refresh(PlayerPokemonBehavior pokemon)
    {
        title.text = pokemon.displayName;
        paragraphTextLeft.text = "Level: "+ pokemon.level+ "\r\nLF: "+ pokemon.currentLifeforce+ "/"+ pokemon.maxLifeforce+ "\r\nLF Heal: "+ pokemon.selfHealStat+ "/sec\r\nHeal Heal: "+ pokemon.healthHealStat+ "/click";
        paragraphTextRight.text = "Status Heal\r\nPoison:  " + pokemon.statusHealStat[0] + "\r\nParalyzed: " + pokemon.statusHealStat[1] + "\r\nBurn: "+ pokemon.statusHealStat[2] + "\r\nSleep:"+ pokemon.statusHealStat[3] + "\r\nFrozen: "+ pokemon.statusHealStat[4];
    }
}
