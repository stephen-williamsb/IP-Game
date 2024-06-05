using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClientPokemonBehavior : MonoBehaviour
{
    public string displayName = "";
    public int currentHealth = 0; //pokemons current health
    public int maxHealth = 100; //pokemons max health
    public int[] currentEffects = null; //Current Status effects(current/max) (array) (later)
    public int moneyGivenOnSuccess = 0; //Money = 100 * (2 * (maxHealth - Current health)/maxHealth) + (50 * numStatusEffects) * client mood
    [SerializeField]
    int[] effectChance = null; //Status effect chance(Later)
    [SerializeField]
    int timeTaken = 0; //Time taken since creation of this
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
