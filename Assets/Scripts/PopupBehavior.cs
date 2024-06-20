using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBehavior : MonoBehaviour
{
    private GameManagerBehavior gameManager;

    // Start is called before the first frame update
    void Start()
    {
	gameManager = GameObject.FindObjectOfType<GameManagerBehavior>();
	if (gameManager == null)
	{
	    Debug.LogError("Game manager not found in the scene.");
	}

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Show popup
    public void ShowPopup(Sprite popupSprite)
    {
	gameObject.SetActive(true);
    }

    // Hide popup
    public void HidePopup()
    {
        print("hide popup called");
        gameObject.SetActive(false);
        
    }

    public void OnClick()
    {
	gameManager.OnPopupClicked(this);
    }
}
