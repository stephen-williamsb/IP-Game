using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerBehavior;

/// <summary>
/// Determines the behavior of the players pokemon.
/// </summary>
public class PlayerPokemonBehavior : MonoBehaviour
{
    public string displayName = ""; //Pokemons display name.
    public int level = 0; //the pokemons current level.
    public float currentLifeforce; //Current amount of 
    public int healthHealStat = 5; //How much health this gives per click.
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    public int[] statusHealStat = new int[5]; //How much status this heals per click.
    public bool fielded = false; //true if this is the players current pokemon.
    public float selfHealStat = 1; //healing per second to this when not fielded.
    public float maxLifeforce = 20; //Max amount of times this can be clicked before dying.
    public GameObject evolution = null;//(blank if none) (later)
    public GameObject displaySprite = null;// what is displayed when the pokemon is equipped
    public GameObject[] clickAreas = null;//An array to enable and disable areas when clicked
    private float timer; //internal timer for self heal.
    private int currentClickIndex = -1;
    private PokeMood mood = PokeMood.Happy;

    public enum PokeMood { Happy, Neutral, Sad };

    private void Start()
    {
        mood = PokeMood.Happy;
        currentLifeforce = maxLifeforce;
        if (fielded)
        {
            RandomizeClickAreas();
        }
        
    }
    private void CalcHappiness()
    {
        if (currentLifeforce>maxLifeforce*.4)
        {
            mood = PokeMood.Happy;
        }
        if (currentLifeforce <= maxLifeforce*.4)
        {
            mood = PokeMood.Neutral;
        }
        if (currentLifeforce <= maxLifeforce * .1)
        {
            mood = PokeMood.Sad;
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
    /// <summary>
    /// Heals the current client pokemon by this pokemons health heal stat and status heal stats.
    /// </summary>
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
    /// <summary>
    /// Randomizes the click areas based on what is in this pokemons clickAreas array.
    /// </summary>
    private void RandomizeClickAreas()
    {
        foreach (var clickArea in clickAreas)
        {
            clickArea.SetActive(false);
        }
        int randomRoll = Random.Range(0, clickAreas.Length);
        while (randomRoll == currentClickIndex)
        {
            randomRoll = Random.Range(0, clickAreas.Length); 
        }
        currentClickIndex = randomRoll;
        clickAreas[currentClickIndex].SetActive(true);
    }

    /// <summary>
    /// Levels up this pokemon upgraded its max lifeforce and selfheal stat.
    /// </summary>
    public void HandleLevelUp()
    {
        currentLifeforce = currentLifeforce * 1.1f;
        maxLifeforce = maxLifeforce * 1.1f;
        selfHealStat = selfHealStat * 1.1f;
        maxLifeforce = Mathf.Floor(maxLifeforce);
        selfHealStat = Mathf.Floor(selfHealStat);
        currentLifeforce = Mathf.Floor(currentLifeforce);
        level++;
        if(level == 15) {
            if (evolution != null)
            {
                FindFirstObjectByType<GameManagerBehavior>().EvolvePokemon(this);
            }
            
        }
    }
    
    /// <summary>
    /// Fields this pokemon and randomizes the click areas.
    /// </summary>
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
