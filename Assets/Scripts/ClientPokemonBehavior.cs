using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// Determines the behavior of the client pokemon.
/// </summary>
public class ClientPokemonBehavior : MonoBehaviour
{
    public string displayName = ""; //Pokemons display name.
    public int currentHealth; //pokemons current health
    public int moneyGivenOnSuccess = 0; //Money = 100 * (2 * (maxHealth - Current health)/maxHealth) + (50 * numStatusEffects) * client mood
    public int maxStartHealth = 25; //pokemons max start health
    public int maxHealth = 50; //pokemons max 
    public int numStatusEffects = 0;
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    public int[] currentEffects = new int[5]; // Poison, Paralyzed, Burn, Sleep, Frozen
    [SerializeField]
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    int[] effectChance = null; //Status effect chance(Later)
    [SerializeField]
    int effectDamage = 20; //If affected by a status effect, how many points it takes to heal the status effect.
    private float timer =0;
    


    // Start is called before the first frame update
    void Start()
    {
        //Assign status effects
        currentEffects = new int[5];
        for (int i = 0; i < currentEffects.Length; i++)
        {
            currentEffects[i] = 0;
        }
        for (int i = 0; i < currentEffects.Length; i++)
        {
            int roll = Random.Range(0, 101);
            if (effectChance[i] > roll)
            {
                currentEffects[i] = effectDamage;
                numStatusEffects++;
            }
            
        }
        currentHealth = Random.Range(0, maxStartHealth+1);
        moneyGivenOnSuccess = maxHealth*2 - currentHealth + (25*numStatusEffects);
        print("Health of " + name + " set as " + currentHealth);

    }
    private void Update()
    {
        timer += Time.deltaTime;
        int secondsTillStatusDamage = 5;
        if (timer >= secondsTillStatusDamage && (currentEffects[0]>0 || currentEffects[2]>0))
        {
            InflictStatusDamage();
            timer = 0;
        }


    }
    /// <summary>
    /// If pokemon is burned or poisoned then increase the status effect.
    /// </summary>
    private void InflictStatusDamage()
    {
     if(currentEffects[0]>0 )
        {
            currentEffects[0]++;
        }
     if(currentEffects[2] > 0)
        {
            currentEffects[2]++;
        }
    }

    /// <summary>
    /// Heals this pokemons health and status by the given amounts. If both health and status are fully healed then prompts the gamemanager to switch the next pokemon.
    /// </summary>
    /// <param name="playerHealing"> The amount of health to heal</param>
    /// <param name="playerStatusHealing">An array of status effects to heal in order of Poison, Paralyzed, Burn, Sleep, Frozen</param>
    public void HealThis(int playerHealing, int[] playerStatusHealing)
    {
        currentHealth += playerHealing;
        if(currentHealth > maxHealth) { 
            currentHealth = maxHealth;
        }

        if(numStatusEffects != 0)
        {
            for (int i = 0; i < currentEffects.Length; i++)
            {
                if(currentEffects[i] == 0)
                {
                    continue;
                }
                currentEffects[i] -= playerStatusHealing[i];
                if (currentEffects[i] <= 0)
                {
                    numStatusEffects--;
                    currentEffects[i] = 0;
                }
            }
        }
        if(numStatusEffects == 0 && currentHealth == maxHealth)
        {
            FindFirstObjectByType<GameManagerBehavior>().NextPokemon();
            print("Pokemon is fully healed!");
        }
    }

}
