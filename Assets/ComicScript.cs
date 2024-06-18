using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComicScript : MonoBehaviour
{
    public Sprite[] comicPanels = new Sprite[7];
    public Image displaySprite = null;
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        displaySprite = GetComponent<Image>();
        displaySprite.sprite = comicPanels[0];
        currentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentIndex == comicPanels.Length)
            {
                SceneManager.LoadScene("Game");
            }
            displaySprite.sprite = comicPanels[currentIndex];
            currentIndex++;
        }
    }
}
