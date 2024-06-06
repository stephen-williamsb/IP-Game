using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonBehavior : MonoBehaviour
{
    public string displayName = "";
    public int cost = 100;//(for shop) (later)
    public int level = 0;
    public int currentLifeforce;
    public int healthHealStat = 5;
    public int statusHealStat = 1;
    public bool fielded = false;
    public float selfHealStat = 1f; //Second it takes to heal 
    public int maxLifeforce = 20; //Clicks before death
    public GameObject evolution = null;//(blank if none) (later)
    public GameObject displaySprite = null;// what is displayed when the pokemon is equipped
    public GameObject[] clickAreas = null;//(a array to enable and disable areas when clicked) (Later)
    private float timer;

    private void Start()
    {
        currentLifeforce = maxLifeforce;
        randomizeClickAreas();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!fielded && currentLifeforce < maxLifeforce )
        {
            timer += Time.deltaTime;
            if( timer >= selfHealStat) {
                timer = 0;
                currentLifeforce++;
            }
        }
    }
    public void handleHealing()
    {
        FindFirstObjectByType<GameManagerBehavior>().healClient();
        randomizeClickAreas();
    }
    private void randomizeClickAreas()
    {
        foreach (var thing in clickAreas)
        {
            thing.SetActive(false);
        }
        clickAreas[Random.Range(0, clickAreas.Length)].SetActive(true);
    }

}
