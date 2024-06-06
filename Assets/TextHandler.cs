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
    GameManagerBehavior gameManager;
    ClientPokemonBehavior clientPokemon;
    PlayerPokemonBehavior playerPokemon;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManagerBehavior>();
        clientPokemon = gameManager.currentClientPokemon.GetComponent<ClientPokemonBehavior>();
        playerPokemon = gameManager.currentClientPokemon.GetComponent<PlayerPokemonBehavior>();
        Invoke("updateAllText", 1);
    }

    public void updateAllText()
    {
        updateEnemyValues();
    }
    private void updateEnemyValues()
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
}
