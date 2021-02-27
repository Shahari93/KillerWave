using System;
using UnityEngine;

public class PlayerShipBuild : MonoBehaviour
{
    [Header("Purchse")]
    [SerializeField] GameObject[] weaponVisuals;
    [SerializeField] SOActorModel defaultPlayerShip;
    GameObject playerShip;
    GameObject buyButton;
    GameObject bankObj;
    int bankBalace = 600;
    bool isPurchasMade = false;

    [Header("Shop Buttons")]
    [SerializeField] GameObject[] shopbuttons;
    GameObject textBoxPanel;
    GameObject target;
    GameObject tempSelection; // store the raycast selection so that we can check to see what we have made contact with
    Camera mainCamera = null;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        textBoxPanel = GameObject.Find("textBoxPanel");
        TurnOffSelectionHighlight();

        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bankBalace.ToString();
        buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;

        //reset the visuals of the player's ship.
        //TurnOffPlayerShipVisuals();

        // creates a player's ship so that when it has all the upgrades applied, it can be sent into the game to be played.
        //PreparePlayerShipForUpgrade();
    }

    private void TurnOffSelectionHighlight() // setting the selection quad off at the start of the game
    {
        for (int i = 0; i < shopbuttons.Length; i++)
        {
            shopbuttons[i].SetActive(false);
        }
    }

    private void Update()
    {
        AttemptSelection();
    }

    //In this method we reset the target game object to remove any previous data.
    GameObject ReturnClickedObject(out RaycastHit raycastHit) // hit contain information of what collider the ray has made contact with.
    {
        GameObject target = null;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 100, out raycastHit))
        {
            target = raycastHit.collider.gameObject; //we take the reference of the game object it has hit and store it in the target game object
        }
        return target;
    }

    //check when a condition is made when the player has made contact by tapping the screen or clicking a mouse button in our shop scene.
    private void AttemptSelection() // responsible for receiving the player's input for a button selection
    {
        if (Input.GetMouseButtonDown(0)) // checking if the player left clicked on the mouse
        {
            RaycastHit raycastHit;
            target = ReturnClickedObject(out raycastHit); // returning the target game object from the ReturnClickedObject method
            if (target != null) // checking if there is some gameobject in the target variable
            {
                if (target.transform.Find("itemText")) // checking if there is the item text game object
                {
                    TurnOffSelectionHighlight(); // turn off every selection quad
                    Select();
                    UpdateDescriptionBox();

                    ////UPGRADE NOT ALREADY SOLD
                    if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
                    {
                        // have coins
                        Affordable();
                        //lack of coins
                        LackOfCredits();
                    }
                    else if (target.transform.Find("itemText").GetComponent<TextMesh>().text == "SOLD")
                    {
                        //SoldOut();
                    }
                }
            }
        }
    }

    //checks whether the bank integer is equal or greater than the value of the button that we have selected(target).
    void Affordable()
    {
        if (bankBalace >= System.Int32.Parse(target.transform.GetComponent<ShopPiece>().SOShopSelection.itemCost)) // bankBalance is int, and itemCost is string
        {
            Debug.Log("Can Buy");
            buyButton.SetActive(true);
        }
    }
    void LackOfCredits()
    {
        if(bankBalace<System.Int32.Parse(target.transform.GetComponent<ShopPiece>().SOShopSelection.itemCost))
        {
            Debug.Log("Can't Buy");
        }
    }

    private void Select()
    {
        tempSelection = target.transform.Find("SelectionQuad").gameObject;
        tempSelection.SetActive(true);
    }

    private void UpdateDescriptionBox() // grab the selected button's asset file variable content, and apply it to the TextMesh text component of textboxPanel.
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tempSelection.GetComponentInParent<ShopPiece>().SOShopSelection.iconName;
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tempSelection.GetComponentInParent<ShopPiece>().SOShopSelection.iconDescription;
    }
}
