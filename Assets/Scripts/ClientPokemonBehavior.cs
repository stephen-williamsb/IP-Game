using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClientPokemonBehavior : MonoBehaviour
{
    public string displayName = "";
    public int currentHealth; //pokemons current health
    public int moneyGivenOnSuccess = 0; //Money = 100 * (2 * (maxHealth - Current health)/maxHealth) + (50 * numStatusEffects) * client mood
    public int maxStartHealth = 25; //pokemons max start health
    public int maxHealth = 50; //pokemons max 
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    public int[] currentEffects = new int[5]; // Poison, Paralyzed, Burn, Sleep, Frozen
    [SerializeField]
    [Tooltip("In order of: Poison, Paralyzed, Burn, Sleep, Frozen")]
    int[] effectChance = null; //Status effect chance(Later)
    private int effectDamage = 20;
    private int numStatusEffects = 0;
    


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

    }
    public void healThis(int playerHealing, int playerStatusHealing)
    {
        currentHealth += playerHealing;
        if(currentHealth > maxHealth) { 
            currentHealth = maxHealth;
        }

        if(numStatusEffects != 0)
        {
            int currentHealAmount = playerStatusHealing;
            for (int i = 0; i < currentEffects.Length; i++)
            {
                if (currentEffects[i] != 0)
                {
                    currentHealAmount -= currentEffects[i];
                    currentEffects[i] -= playerStatusHealing;
                    playerStatusHealing = currentHealAmount;
                    if (currentEffects[i] <= 0 )
                    {
                        currentEffects[i] = 0;
                        numStatusEffects--;
                    }
                }
                if(currentHealAmount <= 0)
                {
                    break;
                }
            }
        }
        if(numStatusEffects == 0 && currentHealth == maxHealth)
        {
            FindFirstObjectByType<GameManagerBehavior>().nextPokemon();
            print("Pokemon is fully healed!");
        }
    }

}
