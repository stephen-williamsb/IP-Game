using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public int playerCash = 0;
    [SerializeField]
    float timeTaken = 0; //Time taken since creation of this
    public ClientMood mood = ClientMood.Happy;
    public GameObject currentClientPokemon;
    public GameObject currentPlayerPokemon;
    public GameObject[] playerParty;
    public GameObject[] clientQueue;
    public GameObject[] possibleClientPokemon;

    public enum ClientMood{Happy, Neutral, Angry};
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerPokemon.GetComponent<PlayerPokemonBehavior>().fielded = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
    }
    public void nextPokemon()
    {

    }


}
