using UnityEngine;
using UnityEngine.UI;

public class GridSlotHandler : MonoBehaviour
{
    public int slotIndex; // 格子的索引
    public GameManagerBehavior gameManager;
    private Image slotImage;

    void Start()
    {
        // 获取GameManagerBehavior对象
        gameManager = FindObjectOfType<GameManagerBehavior>();
        Debug.Log("GameManager found: " + (gameManager != null));
        
        // 获取格子内的Image组件
        slotImage = gameObject.GetComponentInChildren<Image>();
        Debug.Log("Slot Image found: " + (slotImage != null));
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot clicked: " + slotIndex);

        // 检查是否点击了有效的格子
        if (slotIndex >= 0 && slotIndex < gameManager.playerParty.Length)
        {
            GameObject clickedPokemon = gameManager.playerParty[slotIndex];
            Debug.Log("Clicked Pokemon: " + clickedPokemon.name);

            // 如果点击的宝可梦已经是fielded状态，则不做任何操作
            if (clickedPokemon.GetComponent<PlayerPokemonBehavior>().fielded)
            {
                Debug.Log("Pokemon already fielded: " + clickedPokemon.name);
                return;
            }

            // 寻找当前fielded状态的宝可梦并将其设置为不活跃状态
            for (int i = 0; i < gameManager.playerParty.Length; i++)
            {
                GameObject pokemon = gameManager.playerParty[i];
                PlayerPokemonBehavior behavior = pokemon.GetComponent<PlayerPokemonBehavior>();
                if (behavior.fielded)
                {
                    Debug.Log("Found fielded Pokemon: " + pokemon.name);
                    pokemon.SetActive(false);
                    behavior.fielded = false;

                    // 将之前场上宝可梦对应格子的子对象图像设为激活状态
                    Image previousSlotImage = gameManager.playerParty[i].transform.GetComponentInChildren<Image>();
                    if (previousSlotImage != null)
                    {
                        Debug.Log("Activating image for slot: " + i);
                        previousSlotImage.gameObject.SetActive(true);
                    }

                    break;
                }
            }

            // 将点击的宝可梦设置为活跃状态
            clickedPokemon.SetActive(true);
            clickedPokemon.GetComponent<PlayerPokemonBehavior>().fielded = true;
            Debug.Log("Activating Pokemon: " + clickedPokemon.name);

            // 将点击格子的子对象图像设为未激活状态
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
}
