using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

//TODO: change the RV to work only one time in a game
public class PlayerShipBuildOld : MonoBehaviour, IUnityAdsListener
{
    [Header("Purchse")]
    [SerializeField] GameObject[] weaponVisuals = null;
    [SerializeField] SOActorModel defaultPlayerShip = null;
    GameObject playerShip;
    GameObject buyButton;
    GameObject bankObj;
    [SerializeField] int bankBalace = 600;
    bool isPurchasMade = false;

    [Header("Shop Buttons")]
    [SerializeField] GameObject[] shopbuttons = null;
    GameObject textBoxPanel;
    GameObject target;
    GameObject tempSelection; // store the raycast selection so that we can check to see what we have made contact with
    Camera mainCamera = null;

    [Header("Ads")]
    public string gameID = "123456";
    public string placementId_rewardedvideo = "rewardedVideo";
    public bool adStarted;
    private bool testMode = true;



    private void Start()
    {
        Advertisement.AddListener(this);
        isPurchasMade = false;
        mainCamera = FindObjectOfType<Camera>();
        textBoxPanel = GameObject.Find("textBoxPanel");
        TurnOffSelectionHighlight();

        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bankBalace.ToString();
        buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;

        //reset the visuals of the player's ship.
        TurnOffPlayerShipVisuals();

        // creates a player's ship so that when it has all the upgrades applied, it can be sent into the game to be played.
        PreparePlayerShipForUpgrade();

        //Check on which platform the user is playing the game
        CheckPlatform();
    }

    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameID = "4029714";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            gameID = "4029715";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            gameID = "4029715";
        }
        Advertisement.Initialize(gameID, testMode);
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
                        SoldOut();
                    }
                }
                else if (target.name == "WATCH AD")
                {
                    Debug.Log("Pressed");
                    WatchAds();
                }
                else if (target.name == "BUY ?")
                {
                    BuyItem();
                }
                else if (target.name == "START")
                {
                    StartGame();
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
        if (bankBalace < System.Int32.Parse(target.transform.GetComponent<ShopPiece>().SOShopSelection.itemCost))
        {
            Debug.Log("Can't Buy");
        }
    }

    void SoldOut()
    {
        Debug.Log("SOLD OUT");
    }

    private void WatchAds()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) // checking if the user have good internet
        {
            Debug.Log("Has internet");
            ShowRewardedAds();
        }
    }

    private void BuyItem()
    {
        Debug.Log("Purchased");
        isPurchasMade = true;
        buyButton.SetActive(false);
        tempSelection.SetActive(false);
        for (int i = 0; i < weaponVisuals.Length; i++)
        {
            if (weaponVisuals[i].name == tempSelection.transform.parent.gameObject.GetComponent<ShopPiece>().SOShopSelection.iconName)
            {
                weaponVisuals[i].SetActive(true);
            }
        }
        UpgradeToShip(tempSelection.transform.parent.gameObject.GetComponent<ShopPiece>().SOShopSelection.iconName);
        bankBalace -= System.Int32.Parse(tempSelection.transform.parent.GetComponent<ShopPiece>().SOShopSelection.itemCost); //using System.Int32.Parse, so it reads the string value as an int value
        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bankBalace.ToString();
        tempSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";
    }

    //This method will load the game object of the item purchased to the player ship we play in our game
    private void UpgradeToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefabs/Player/" + upgrade)) as GameObject;
        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
        if(shipItem.name == "c. Bomb(Clone)")
        {
            shipItem.transform.localPosition = new Vector3(0, -.6f, 0.615f);
        }
    }

    private void StartGame()
    {
        if (isPurchasMade)
        {
            playerShip.name = "Upgraded Ship";
            if (playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health = 2;
            }
            DontDestroyOnLoad(playerShip);
        }
        SceneManager.LoadScene("level1");
    }


    // turn off every weapon visual in the game at the start of the scene
    private void TurnOffPlayerShipVisuals()
    {
        for (int i = 0; i < weaponVisuals.Length; i++)
        {
            weaponVisuals[i].gameObject.SetActive(false);
        }
    }

    private void PreparePlayerShipForUpgrade()
    {
        playerShip = GameObject.Instantiate(Resources.Load(("Prefabs/Player/player_ship"))) as GameObject;
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
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

    void ShowRewardedAds()
    {
        Debug.Log("Coroutine");
        StartCoroutine(WaitForAd());
    }
    IEnumerator WaitForAd()
    {
        string placementId = placementId_rewardedvideo;
        while (!Advertisement.IsReady(placementId))
        {
            yield return null;
        }
        Advertisement.Show(placementId);

    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult)
    {
        if (showResult == UnityEngine.Advertisements.ShowResult.Finished)
        {
            bankBalace += 300;
            bankObj.GetComponentInChildren<TextMesh>().text = bankBalace.ToString();
            TurnOffSelectionHighlight();
        }
    }
}