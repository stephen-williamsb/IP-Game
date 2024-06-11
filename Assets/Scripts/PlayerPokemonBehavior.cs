using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonBehavior : MonoBehaviour
{
    public string displayName = "";
    public int cost = 100;//(for shop) (later)
    public int level = 0;
    public double currentLifeforce;
    public int healthHealStat = 5;
    public int[] statusHealStat = new int[5];
    public bool fielded = false;
    public double selfHealStat = 1; //healing per second to this when not fielded
    public double maxLifeforce = 20; //Clicks before death
    public GameObject evolution = null;//(blank if none) (later)
    public Sprite displaySprite = null;// what is displayed when the pokemon is equipped
    public GameObject[] clickAreas = null;//(a array to enable and disable areas when clicked) (Later)
    private float timer;
    private int currentClickIndex = -1;

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
            if( timer >= 1) {
                timer = 0;
                currentLifeforce+=selfHealStat;
            }
        }
    }
    public void handleHealing()
    {
        FindFirstObjectByType<GameManagerBehavior>().healClient();
        randomizeClickAreas();
        currentLifeforce--;
    }
    private void randomizeClickAreas()
    {
        foreach (var thing in clickAreas)
        {
            thing.SetActive(false);
        }
        int randomRoll = Random.Range(0, clickAreas.Length);
        while (randomRoll == currentClickIndex)
        {
            randomRoll = Random.Range(0, clickAreas.Length); 
        }
        currentClickIndex = randomRoll;
        clickAreas[currentClickIndex].SetActive(true);
    }
    public void handleLevelUp()
    {
        currentLifeforce = currentLifeforce * 1.1;
        maxLifeforce = maxLifeforce * 1.1;
        selfHealStat = selfHealStat * 1.1;
        level++;
    }


}
