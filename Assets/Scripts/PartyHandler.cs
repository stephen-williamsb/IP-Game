using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class PartyHandler : MonoBehaviour
{
    public Sprite[] displaySprites = new Sprite[6];
    public Image[] partyImageIcons = new Image[6];
    public GameManagerBehavior gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerBehavior>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshImages();
    }
    public void RefreshImages()
    {
        for (int i = 0; i < displaySprites.Length; i++)
        {
            if (displaySprites[i] != null)
            {
                PlayerPokemonBehavior current = gameManager.playerParty[i].GetComponent<PlayerPokemonBehavior>();
                //displaySprites[i] = current.displaySprite;
                continue;
            }
            displaySprites[i] = null;
        }
            for (int i = 0; i < displaySprites.Length; i++)
        {

            partyImageIcons[i].sprite = displaySprites[i];
        }
    }
}
