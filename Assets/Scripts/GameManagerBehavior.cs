using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public int playerCash = 0;
    [SerializeField]
    float timeTaken = -5; //Time taken since creation of this
    public ClientMood mood = ClientMood.Happy;
    public GameObject currentClientPokemon;
    public GameObject currentPlayerPokemon;
    public int currentPokemonLevel;
    public int rareCandyPrice;
    public GameObject[] playerParty;
    public Queue<GameObject> clientQueue;
    public GameObject[] possibleClientPokemon;
    private TextHandler textHandler;
    private int currentClientIndex = -1;

    public enum ClientMood{Happy, Neutral, Angry};
    // Start is called before the first frame update
    void Start()
    {
        clientQueue = new Queue<GameObject>();
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fielded = true;
        textHandler = gameObject.GetComponent<TextHandler>();
        for (int i = 0; i < 3; i++)
        {
            clientQueue.Enqueue(createPokemon());
        }
        
        playerParty[0] = currentPlayerPokemon;
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        rareCandyPrice = 50 + 20 * currentPokemonLevel;
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
        textHandler.updateTimeTakenText(timeTaken);
        textHandler.updateAllText();
    }
    public void nextPokemon()
    {
        playerCash += currentClientPokemon.GetComponent<ClientPokemonBehavior>().moneyGivenOnSuccess;
        clientQueue.Enqueue(createPokemon());
        Destroy(currentClientPokemon);
        currentClientPokemon = clientQueue.Dequeue();
        currentClientPokemon.SetActive(true);
        timeTaken = 0;
        textHandler.getPokemon();
        textHandler.updateAllText();
    }
    public void healClient()
    {
        PlayerPokemonBehavior playerPokemon = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>();
        currentClientPokemon.GetComponent<ClientPokemonBehavior>().healThis(playerPokemon.healthHealStat, playerPokemon.statusHealStat);
        textHandler.updateAllText();
    }
    private GameObject createPokemon()
    {
        int randomRoll = Random.Range(0, possibleClientPokemon.Length);
        while (randomRoll == currentClientIndex)
        {
            randomRoll = Random.Range(0, possibleClientPokemon.Length);
        }
        currentClientIndex = randomRoll;
        GameObject newPokemon = Instantiate(possibleClientPokemon[currentClientIndex], currentClientPokemon.transform.position, currentClientPokemon.transform.rotation);
        newPokemon.SetActive(false);
        return newPokemon;
    }
    public void levelUpCurrentPokemon()
    {
        if (rareCandyPrice > playerCash)
        {
            return;
        }
        playerCash -= rareCandyPrice;
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().handleLevelUp();
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        rareCandyPrice = 50 + 20 * currentPokemonLevel;
        textHandler.updateAllText();
    }
    public void switchPokemonTo(int slotNum)
    {
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fielded = false;
        currentPlayerPokemon = playerParty[slotNum];
        currentPokemonLevel = currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().level;
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fieldThis();
        rareCandyPrice = 50 + 20 * currentPokemonLevel;
        textHandler.updateAllText();
    }

}
