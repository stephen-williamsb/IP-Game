using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerBehavior;

/// <summary>
/// Determines the behavior of the player's pokemon.
/// </summary>
public class PlayerPokemonBehavior : MonoBehaviour
{
    public string displayName = ""; // Pokemon's display name.
    public int level = 0; // The pokemon's current level.
    public float currentLifeforce; // Current amount of life force.
    public int healthHealStat = 5; // How much health this gives per click.
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    public int[] statusHealStat = new int[5]; // How much status this heals per click.
    public bool fielded = false; // True if this is the player's current pokemon.
    public float selfHealStat = 1; // Healing per second to this when not fielded.
    public float maxLifeforce = 20; // Max amount of times this can be clicked before dying.
    public GameObject evolution = null; // (blank if none) (later)
    public GameObject displaySprite = null; // What is displayed when the pokemon is equipped
    public GameObject[] clickAreas = null; // An array to enable and disable areas when clicked
    public Sprite[] moodSprites = new Sprite[3]; // Array to hold mood sprites: Happy, Neutral, Sad
    private SpriteRenderer displaySpriteRenderer; // SpriteRenderer of the DisplaySprite child
    private float timer; // Internal timer for self heal.
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

        // Initialize the SpriteRenderer
        if (displaySprite != null)
        {
            displaySpriteRenderer = displaySprite.GetComponent<SpriteRenderer>();
        }
    }

    private void CalcHappiness()
    {
        if (currentLifeforce > maxLifeforce * .75)
        {
            mood = PokeMood.Happy;
        }
        else if (currentLifeforce <= maxLifeforce * .15)
        {
            mood = PokeMood.Sad;
        }
        else
        {
            mood = PokeMood.Neutral;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!fielded && currentLifeforce < maxLifeforce)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                currentLifeforce += selfHealStat;
            }
        }

        // Update mood and sprite based on current life force
        CalcHappiness();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (displaySpriteRenderer != null)
        {
            switch (mood)
            {
                case PokeMood.Happy:
                    displaySpriteRenderer.sprite = moodSprites[0];
                    break;
                case PokeMood.Neutral:
                    displaySpriteRenderer.sprite = moodSprites[1];
                    break;
                case PokeMood.Sad:
                    displaySpriteRenderer.sprite = moodSprites[2];
                    break;
            }
        }
    }

    /// <summary>
    /// Heals the current client pokemon by this pokemon's health heal stat and status heal stats.
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
    /// Randomizes the click areas based on what is in this pokemon's clickAreas array.
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
    /// Levels up this pokemon, upgrading its max life force and self-heal stat.
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
        if (level == 15)
        {
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
