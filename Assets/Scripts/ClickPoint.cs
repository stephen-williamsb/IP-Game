using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPoint : MonoBehaviour
{
    public Boolean didPlayerClickThis()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetComponent<Collider2D>().OverlapPoint(mousePosition);
    }
    void OnMouseDown()
    {
        if (didPlayerClickThis())
        {
            //Instantiate(effect, this.pos, this.rotat);
            GetComponentInParent<PlayerPokemonBehavior>().handleHealing();
        }
    }
}
