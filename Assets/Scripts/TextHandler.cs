using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///<summary>
///This class handles all text changes for the game canvas object.
/// </summary>
public class TextHandler : MonoBehaviour
{
    public Text[] statusEffectsText; // values change in order of Poison, Paralyzed, Burn, Sleep, Frozen
    public Text clientHealthText; //The text object assosiated with client pokemons health
    public Text clientPokeName; //The text object assosiated with client pokemons name
    public Text playerPokeName; //The text object assosiated with player pokemons name
    public Text lifePointsText; //The text object assosiated with lifepoints of the players pokemon
    public Text timerText; //The text object assosiated with how long the current client pokemon has been out
    public Text playerCashText; //The text object assosiated with how much cash the current player has
    public Text rareCandyText; //The text object assosiated with how much a rare candy costs
    private GameManagerBehavior gameManager; //The game manager
    private ClientPokemonBehavior clientPokemon; // the client pokemon
    private PlayerPokemonBehavior playerPokemon; //the player pokemon
    

    void Start()
    {
        //Get Game manger
        gameManager = FindFirstObjectByType<GameManagerBehavior>();
    }

    ///<summary>
    ///Gets the player and clients current pokemon from the game manager.
    /// </summary>
    public void GetPokemon()
    {
        clientPokemon = gameManager.currentClientPokemon.GetComponent<ClientPokemonBehavior>();
        playerPokemon = gameManager.currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>();
    }
    /**
     * 
     */
    ///<summary>
    ///Updates all text in the game canvas excluding the timer text. This includes the text for the client pokemon, the player pokemon and the rare candy.
    /// </summary>
    public void UpdateAllText()
    {
        GetPokemon();
        UpdateEnemyValues();
        UpdatePlayerValues();
        rareCandyText.text = "$" + gameManager.rareCandyPrice;
    }

    ///<summary>
    ///Updates the text for the client pokemon, the player pokemon, rare candy price and player cash.
    /// </summary>
    public void UpdateEnemyValues()
    {
        clientPokeName.text = clientPokemon.displayName;
        for (int i = 0; i < statusEffectsText.Length; i++)
        {
            if (clientPokemon.currentEffects[i] <= 0)
            {
                statusEffectsText[i].text = "none";
            }
            else
            {
                statusEffectsText[i].text = "" + clientPokemon.currentEffects[i];
            }
        }
        clientHealthText.text = clientPokemon.currentHealth + " / " + clientPokemon.maxHealth;
    }
    ///<summary>
    ///Updates the text for the client pokemon, the player pokemon and the rare candy.
    /// </summary>
    public void UpdatePlayerValues()
    {
        playerPokeName.text = playerPokemon.displayName + " " + gameManager.currentPokemonLevel;
        lifePointsText.text = ""+playerPokemon.currentLifeforce;
        playerCashText.text = "" + gameManager.playerCash;

    }
    /// <summary>
    /// Updates the timer for how long the current client pokemon has been out.
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTimeTakenText(float time)
    {
        timerText.text = "" + Mathf.FloorToInt(time);
    }
}
