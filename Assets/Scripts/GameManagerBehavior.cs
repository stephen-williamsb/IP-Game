using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// This script is in charge of holding all of the important stats and game objects used for the game, as well as enforcing the games rules and currency states.
/// </summary>
public class GameManagerBehavior : MonoBehaviour
{
    public int playerCash = 0; //The current player balance.
    [SerializeField]
    float timeTaken = -5; //The amount of time taken since the creation of the current client pokemon.
    public ClientMood mood = ClientMood.Happy; //Current client mood, gets worse as time goes on.
    public GameObject currentClientPokemon; //The current client pokemon which needs to get healed by the player.
    public GameObject currentPlayerPokemon; //The current fielded player pokemon.
    public int currentPokemonLevel; //The current player pokemons level.
    public int rareCandyPrice; //The price of a rare candy which is determined by player level.
    public GameObject[] playerParty; //The players current party.
    public Queue<GameObject> clientQueue; //The other client pokemon which are in queue.
    public GameObject[] possibleClientPokemon; //The possible client pokemon that this object can choose to spawn. 
    [SerializeField]
    GameObject pokemonSpawnPoint = null;
    private TextHandler textHandler; //The script that handles the text changes,
    private int currentClientIndex = -1; //The index of the current client pokemon selected. based on the array of possibleClientPokemon.
    private int maxHappyTimer; //The maxiumum amount of time you can take and the client will still be happy
    private int maxNeutralTimer; //The maxiumum amount of time you can take and the client will still be neutral

    public enum ClientMood{Happy, Neutral, Angry};
   
    
    void Start()
    {
        //Initialize and fetch game objects.
        clientQueue = new Queue<GameObject>();
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fielded = true;
        textHandler = gameObject.GetComponent<TextHandler>();
        playerParty[0] = currentPlayerPokemon;
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        rareCandyPrice = 50 + 20 * currentPokemonLevel;

        //Create client pokemon and add them to queue.
        for (int i = 0; i < 3; i++)
        {
            clientQueue.Enqueue(CreatePokemon());
        }
        calculateMoodTimers();
        //Have the text handler fetch the created pokemon to display.
        textHandler.GetPokemon();
    }

    // Update is called once per frame
    void Update()
    {
        //Update timer and text
        timeTaken += Time.deltaTime;

        //Change client mood based on time
        if (mood == ClientMood.Happy && timeTaken>maxHappyTimer)
        {
            mood = ClientMood.Neutral;
        }
        if (mood == ClientMood.Neutral && timeTaken > maxNeutralTimer)
        {
            mood = ClientMood.Angry;
        }

        //Update text
        textHandler.UpdateTimeTakenText(timeTaken);
        textHandler.UpdateAllText();
    }
    /// <summary>
    /// Gets the next client pokemon in queue and destroys the current one after awarding the player cash.
    /// </summary>
    public void NextPokemon()
    {
        playerCash += AwardCash();
        clientQueue.Enqueue(CreatePokemon());
        Destroy(currentClientPokemon);
        currentClientPokemon = clientQueue.Dequeue();
        currentClientPokemon.SetActive(true);
        timeTaken = 0;
        mood = ClientMood.Happy;
        calculateMoodTimers();
        textHandler.GetPokemon();
        textHandler.UpdateAllText();
    }
    private void calculateMoodTimers()
    {
        ClientPokemonBehavior clientPoke = currentClientPokemon.GetComponent<ClientPokemonBehavior>();
        int points = clientPoke.maxHealth + clientPoke.numStatusEffects * 10;
        maxHappyTimer = (int)(points * 0.25);
        maxNeutralTimer = (int)(points * 0.75);
    }
    /// <summary>
    /// Awards the player cash based on the current pokemon and other factors.
    /// </summary>
    /// <returns>an int cash amount</returns>
    private int AwardCash()
    {
        int returnVal = currentClientPokemon.GetComponent<ClientPokemonBehavior>().moneyGivenOnSuccess;
        switch (mood)
        {
            case ClientMood.Happy:
                returnVal = (int)(returnVal * 1.5);
                break;
            case ClientMood.Neutral:
                break;
            case ClientMood.Angry:
                returnVal = (int)(returnVal * .5);
                break;

        }
        return returnVal;

    }
    /// <summary>
    /// Uses the current player pokemon to heal the current client pokemon.
    /// </summary>
    public void HealClient()
    {
        PlayerPokemonBehavior playerPokemon = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>();
        currentClientPokemon.GetComponent<ClientPokemonBehavior>().HealThis(playerPokemon.healthHealStat, playerPokemon.statusHealStat);
        textHandler.UpdateAllText();
    }
    /// <summary>
    /// Creates a new client pokemon, choosing from the list in GameManager. Will not choose the same pokemon twice.
    /// </summary>
    /// <returns>A client pokemon game object.</returns>
    private GameObject CreatePokemon()
    {
        int randomRoll = UnityEngine.Random.Range(0, possibleClientPokemon.Length);
        while (randomRoll == currentClientIndex)
        {
            randomRoll = UnityEngine.Random.Range(0, possibleClientPokemon.Length);
        }
        currentClientIndex = randomRoll;
        GameObject newPokemon = Instantiate(possibleClientPokemon[currentClientIndex], currentClientPokemon.transform.position, currentClientPokemon.transform.rotation);
        newPokemon.SetActive(false);
        return newPokemon;
    }

    /// <summary>
    /// Levels up the players current fielded pokemon and reduces their cash by that amount. 
    /// </summary>
    public void LevelUpCurrentPokemon()
    {
        if (rareCandyPrice > playerCash)
        {
            return;
        }
        playerCash -= rareCandyPrice;
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().HandleLevelUp();
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        rareCandyPrice = 50 + 20 * currentPokemonLevel;
    }
    public void EvolvePokemon(PlayerPokemonBehavior pokemon) {
        int index = 0;
        GameObject evolvedPokemon = Instantiate(pokemon.evolution, pokemonSpawnPoint.transform);
        for (int i = 0; i < playerParty.Length; i++)
        {
            if (playerParty[i] == pokemon.gameObject)
            {
                index = i; break;
            }
        }
        GameObject deleteThis = currentPlayerPokemon;

        playerParty[index] = evolvedPokemon;
        currentPlayerPokemon = evolvedPokemon;
        SwitchPokemonTo(index);
        Destroy(deleteThis);
        textHandler.GetPokemon();
        Debug.Log("Pokemon evolved: " + evolvedPokemon.name);
    }
    
    /// <summary>
    /// Switches the players current fielded pokemon and replaces it with the one at slotNum. Zero based indexing.
    /// </summary>
    /// <param name="slotNum"> The slot of the pokemon that is going to be fielded.</param>
    public void SwitchPokemonTo(int slotNum)
    {
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fielded = false;
        currentPlayerPokemon = playerParty[slotNum];
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().FieldThis();
        rareCandyPrice = 50 + 20 * currentPokemonLevel;
    }


}
