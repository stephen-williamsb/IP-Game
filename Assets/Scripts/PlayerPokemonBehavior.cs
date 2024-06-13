using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonBehavior : MonoBehaviour
{
    public string displayName = "";
    public int cost = 100;//(for shop) (later)
    public int level = 0;
    public float currentLifeforce;
    public int healthHealStat = 5;
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    public int[] statusHealStat = new int[5];
    public bool fielded = false;
    public float selfHealStat = 1; //healing per second to this when not fielded
    public float maxLifeforce = 20; //Clicks before death
    public GameObject evolution = null;//(blank if none) (later)
    public GameObject displaySprite = null;// what is displayed when the pokemon is equipped
    public GameObject[] clickAreas = null;//(a array to enable and disable areas when clicked) (Later)
    private float timer;
    private int currentClickIndex = -1;

    private void Start()
    {
        currentLifeforce = maxLifeforce;
        if (fielded)
        {
            RandomizeClickAreas();
        }
        
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
    public void HandleHealing()
    {
        if (currentLifeforce <= 0)
        {
            return;
        }
        FindFirstObjectByType<GameManagerBehavior>().HealClient();
        RandomizeClickAreas();
        currentLifeforce--;
    }
    private void RandomizeClickAreas()
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
    public void HandleLevelUp()
    {
        currentLifeforce = currentLifeforce * 1.1f;
        maxLifeforce = maxLifeforce * 1.1f;
        selfHealStat = selfHealStat * 1.1f;
        maxLifeforce = Mathf.Round(maxLifeforce * 10.0f) * 0.1f;
        selfHealStat = Mathf.Round(selfHealStat * 10.0f) * 0.1f;
        print("Current life force " + currentLifeforce);
        currentLifeforce = Mathf.Round(currentLifeforce * 10.0f) * 0.1f;
        print("new life force: " + currentLifeforce);
        level++;
    }
    public void FieldThis()
    {
        fielded = true;
        Transform[] children = this.GetComponentsInChildren<Transform>();
        foreach (Transform render in children)
        {
            GameObject current = render.gameObject;
            if (current != gameObject)
            {
                current.SetActive(false);
                print(render.gameObject + " was disabled");
            }
        }
        displaySprite.SetActive(true);
        RandomizeClickAreas();

    }


}
