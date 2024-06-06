using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text[] statusEffectsText; // Poison, Paralyzed, Burn, Sleep, Frozen
    public Text clientHealthText;
    public Text clientPokeName;
    public Text playerPokeName;
    public Text lifePointsText;
    public Text timerText;
    public Text playerCashText;
    private GameManagerBehavior gameManager;
    private ClientPokemonBehavior clientPokemon;
    private PlayerPokemonBehavior playerPokemon;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManagerBehavior>();
        getPokemon();
        Invoke("updateAllText", 0.1f);
    }
    public void getPokemon()
    {
        clientPokemon = gameManager.currentClientPokemon.GetComponent<ClientPokemonBehavior>();
        playerPokemon = gameManager.currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>();
    }

    public void updateAllText()
    {
        updateEnemyValues();
        updatePlayerValues();
    }
    public void updateEnemyValues()
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
    public void updatePlayerValues()
    {
        playerPokeName.text = playerPokemon.displayName;
        lifePointsText.text = ""+playerPokemon.currentLifeforce;
        playerCashText.text = "" + gameManager.playerCash;

    }
    public void updateTimeTakenText(float time)
    {
        timerText.text = "" + Mathf.FloorToInt(time);
    }
}
