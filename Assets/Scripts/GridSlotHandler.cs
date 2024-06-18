using System;
using UnityEngine;
using UnityEngine.UI;

public class GridSlotHandler : MonoBehaviour
{
    public int slotIndex; // index of the slot in the grid
    public GameManagerBehavior gameManager;
    public GameObject slotImage;
    public Image[] otherSlotImages; // other slot images in the grid
    public bool alreadyBought; // whether the Pokemon has been bought
    public Button buyButton; // the button for buying the Pokemon
    public int pokemonPrice; // the price of the Pokemon

    void Start()
    {
        // get the GameManagerBehavior script
        gameManager = FindObjectOfType<GameManagerBehavior>();
        Debug.Log("GameManager found: " + (gameManager != null));

        // set up the buy button
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        // update the buy button status
        UpdateBuyButton();
    }

    void Update()
    {
        // Update the buy button status every frame
        UpdateBuyButton();
    }

    void UpdateBuyButton()
    {
        if (alreadyBought)
        {
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<Text>().text = "Buy: " + pokemonPrice;

            // Check if the player has enough money
            if (gameManager.playerCash >= pokemonPrice)
            {
                buyButton.image.color = Color.green;
                buyButton.interactable = true;
            }
            else
            {
                buyButton.image.color = Color.red;
                buyButton.interactable = false;
            }
        }
    }

    public void OnSlotClicked()
    {
        if (!alreadyBought)
        {
            Debug.Log("Pokemon not bought yet: " + slotIndex);
            return;
        }

        Debug.Log("Slot clicked: " + slotIndex);

        // check if the slot index is valid
        if (slotIndex >= 0 && slotIndex < gameManager.playerParty.Length)
        {
            GameObject clickedPokemon = gameManager.playerParty[slotIndex];
            Debug.Log("Clicked Pokemon: " + clickedPokemon.name);

            // IF the clicked pokemon is already fielded, return
            if (clickedPokemon.GetComponent<PlayerPokemonBehavior>().fielded)
            {
                Debug.Log("Pokemon already fielded: " + clickedPokemon.name);
                return;
            }

            // find fielded pokemon and set to not fielded
            for (int i = 0; i < gameManager.playerParty.Length; i++)
            {
                GameObject pokemon = gameManager.playerParty[i];
                PlayerPokemonBehavior behavior = pokemon.GetComponent<PlayerPokemonBehavior>();
                if (behavior.fielded)
                {
                    Debug.Log("Found fielded Pokemon: " + pokemon.name);
                    Transform[] children = pokemon.GetComponentsInChildren<Transform>();
                    foreach (Transform render in children)
                    {
                        GameObject current = render.gameObject;
                        if (current != pokemon)
                        {
                            current.SetActive(false);
                        }
                    }
                    behavior.fielded = false;

                    // Activate all other slot images
                    foreach (Image img in otherSlotImages)
                    {
                        if (img != null && img.gameObject != slotImage.gameObject)
                        {
                            img.gameObject.SetActive(true);
                        }
                    }

                    break;
                }
            }

            // set the clicked pokemon to fielded
            gameManager.SwitchPokemonTo(slotIndex);
            Transform[] renders = clickedPokemon.GetComponentsInChildren<Transform>();

            clickedPokemon.GetComponent<PlayerPokemonBehavior>().fielded = true;
            Debug.Log("Activating Pokemon: " + clickedPokemon.name);

            // Set the clicked pokemon to active
            if (slotImage != null)
            {
                Debug.Log("Deactivating slot image for slot: " + slotIndex);
                slotImage.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Invalid slot index: " + slotIndex);
        }
    }

    public void OnBuyButtonClicked()
    {
        if (gameManager.playerCash >= pokemonPrice)
        {
            gameManager.playerCash -= pokemonPrice;
            alreadyBought = true;
            UpdateBuyButton();
            Debug.Log("Pokemon bought: " + slotIndex);
        }
        else
        {
            Debug.Log("Not enough money to buy Pokemon: " + slotIndex);
        }
    }
}
