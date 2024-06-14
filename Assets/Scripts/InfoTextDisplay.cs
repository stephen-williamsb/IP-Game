using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The display information for each player pokemon when selected during the menu.
/// </summary>
public class InfoTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI title; //Pokemon name
    public TextMeshProUGUI paragraphTextLeft; //Level, current/max life force, self heal stat, health heal stat
    public TextMeshProUGUI paragraphTextRight; //Status heals

    /// <summary>
    /// Refreshes the info window and displays the information of the provided pokemon.
    /// </summary>
    /// <param name="pokemon">The pokemon whos info you wish to display</param>
    public void Refresh(PlayerPokemonBehavior pokemon)
    {
        title.text = pokemon.displayName;
        paragraphTextLeft.text = "Level: "+ pokemon.level+ "\r\nLF: "+ pokemon.currentLifeforce+ "/"+ pokemon.maxLifeforce+ "\r\nLF Heal: "+ pokemon.selfHealStat+ "/sec\r\nHeal Heal: "+ pokemon.healthHealStat+ "/click";
        paragraphTextRight.text = "Status Heal\r\nPoison:  " + pokemon.statusHealStat[0] + "\r\nParalyzed: " + pokemon.statusHealStat[1] + "\r\nBurn: "+ pokemon.statusHealStat[2] + "\r\nSleep:"+ pokemon.statusHealStat[3] + "\r\nFrozen: "+ pokemon.statusHealStat[4];
    }
}
