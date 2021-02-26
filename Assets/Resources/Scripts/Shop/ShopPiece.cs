using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPiece : MonoBehaviour
{
    [SerializeField] SOShopSelection shopSelection;
    public SOShopSelection SOShopSelection
    {
        get
        {
            return shopSelection;
        }
        set
        {
            shopSelection = value;
        }
    }

    private void Awake()
    {
        if(GetComponentInChildren<SpriteRenderer>() !=null) // checks if there is sprite renderer component 
        {
            GetComponentInChildren<SpriteRenderer>().sprite = shopSelection.icon;
        }
        if(transform.Find("itemText")) // showing the cost of the item in the selection grid
        {
            GetComponentInChildren<TextMesh>().text = shopSelection.itemCost;
        }
    }
}
