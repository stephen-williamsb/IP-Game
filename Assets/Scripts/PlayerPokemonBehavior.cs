using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonBehavior : MonoBehaviour
{
public string displayName = "";
public int cost = 100;//(for shop) (later)
public int level = 0;
public int healthHealStat = 5;
public int statusHealStat = 1;
public int maxLifeforce = 20; //Clicks before death
public int currentLifeforce;
public GameObject evolution = null;//(blank if none) (later)
public GameObject displaySprite = null;// what is displayed when the pokemon is equipped
public GameObject[] clickAreas = null;//(a array to enable and disable areas when clicked) (Later)

    void Start()
{
    
}

// Update is called once per frame
void Update()
{
    
}
}
