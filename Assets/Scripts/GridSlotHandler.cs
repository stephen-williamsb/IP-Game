using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// No clue what this does. 
/// </summary>
public class GridSlotHandler : MonoBehaviour
{
    public int slotIndex; // 格子的索引
    public GameManagerBehavior gameManager;
    public GameObject slotImage;
    public Image[] otherSlotImages; // 其他需要激活的Image列表

    void Start()
    {
        // 获取GameManagerBehavior对象
        gameManager = FindObjectOfType<GameManagerBehavior>();
        Debug.Log("GameManager found: " + (gameManager != null));
        
        // // 获取格子内的Image组件
        // slotImage = gameObject.GetComponentInChildrenAAAAA<Image>();
        // Debug.Log("Slot Image found: " + (slotImage != null));
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

            
            // find fielded pokemon and set to not fielded
            for (int i = 0; i < gameManager.playerParty.Length; i++)
            {
                GameObject pokemon = gameManager.playerParty[i];
                PlayerPokemonBehavior behavior = pokemon.GetComponent<PlayerPokemonBehavior>();
                if (behavior.fielded)
                {
                    Debug.Log("Found fielded Pokemon: " + pokemon.name);
                    Transform[] children = pokemon.GetComponentsInChildren<Transform>();
                    foreach(Transform render in children)
                    {
                        GameObject current = render.gameObject;
                        if(current != pokemon)
                        {
                            current.SetActive(false);
                        }
                    }
                    behavior.fielded = false;

                    // 激活其他需要激活的Image
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

            // 将点击的宝可梦设置为活跃状态
            gameManager.SwitchPokemonTo(slotIndex);
            Transform[] renders = clickedPokemon.GetComponentsInChildren<Transform>();
            
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
