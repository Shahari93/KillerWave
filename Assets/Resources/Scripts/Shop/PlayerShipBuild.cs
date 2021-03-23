using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class PlayerShipBuild : MonoBehaviour, IUnityAdsListener
{
    [SerializeField]
    GameObject[] visualWeapons;
    GameObject textBoxPanel;
    GameObject bankObj;
    GameObject buyButton;
    GameObject tmpSelection;
    int bankBalace = 600;
    bool isPurchasMade = false;

    bool purchaseMade = false;
    [SerializeField]
    SOActorModel defaultPlayerShip;
    GameObject playerShip;
    GameObject target;

    [Header("Ads")]
    public string gameID = "123456";
    public string placementId_rewardedvideo = "rewardedVideo";
    public bool adStarted;
    private bool testMode = true;

    void Start()
    {
        Advertisement.AddListener(this);
        isPurchasMade = false;
        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bankBalace.ToString();
        textBoxPanel = GameObject.Find("textBoxPanel");
        buyButton = GameObject.Find("BUY?").gameObject;
        buyButton.SetActive(false);
        TurnOffPlayerShipVisuals();
        TurnOffSelectionHighlights();
        CheckPlatform();
        PreparePlayerShipForUpgrade();
    }

    void PreparePlayerShipForUpgrade()
    {
        playerShip = GameObject.Instantiate(Resources.Load("Prefabs/Player/player_ship")) as GameObject;
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<Player>().ActorStats(defaultPlayerShip);
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
    public void WatchAds()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) // checking if the user have good internet
        {
            ShowRewardedAds();
        }
    }
    void ShowRewardedAds()
    {
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
            TurnOffSelectionHighlights();
        }
    }

    void TurnOffPlayerShipVisuals()
    {
        for (int i = 0; i < visualWeapons.Length; i++)
        {
            visualWeapons[i].gameObject.SetActive(false);
        }
    }
    void TurnOffSelectionHighlights()
    {
        GameObject[] selections = GameObject.FindGameObjectsWithTag("Selection");
        for (int i = 0; i < selections.Length; i++)
        {
            if (selections[i].GetComponentInParent<ShopPiece>())
            {
                if (selections[i].GetComponentInParent<ShopPiece>().SOShopSelection.iconName == "sold Out")
                {
                    selections[i].SetActive(false);
                }
            }
        }
    }

    void UpdateDescriptionBox()
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponent<ShopPiece>().SOShopSelection.iconName;
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponent<ShopPiece>().SOShopSelection.iconDescription;
    }

    void LackOfCredits()
    {
        if (bankBalace < System.Int32.Parse(tmpSelection.GetComponentInChildren<Text>().text))
        {
            Debug.Log("CAN'T BUY");
        }
    }
    void Affordable()
    {
        if (bankBalace >= System.Int32.Parse(tmpSelection.GetComponentInChildren<Text>().text))
        {
            Debug.Log("CAN BUY");
            buyButton.SetActive(true);
        }
    }
    void SoldOut()
    {
        Debug.Log("SOLD OUT");
    }

    //public void WatchAdvert()
    //{
    //    if (Application.internetReachability != NetworkReachability.NotReachable)
    //    {
    //        ShowRewardedAds();
    //    }
    //}
    public void BuyItem()
    {
        Debug.Log("PURCHASED");
        purchaseMade = true;
        buyButton.SetActive(false);
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = "";
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = "";

        for (int i = 0; i < visualWeapons.Length; i++)
        {
            if (visualWeapons[i].name == tmpSelection.GetComponent<ShopPiece>().SOShopSelection.iconName)
            {
                visualWeapons[i].SetActive(true);
            }
        }

        UpgradeToShip(tmpSelection.GetComponent<ShopPiece>().SOShopSelection.iconName);

        bankBalace = bankBalace - System.Int16.Parse(tmpSelection.GetComponent<ShopPiece>().SOShopSelection.itemCost);
        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bankBalace.ToString();
        tmpSelection.transform.Find("itemText").GetComponentInChildren<Text>().text = "SOLD";
    }

    void UpgradeToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefabs/Player/" + upgrade)) as GameObject;
        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
        if (shipItem.name == "c. Bomb(Clone)")
        {
            shipItem.transform.localPosition = new Vector3(0, -.6f, 0.615f);
        }
    }

    public void StartGame()
    {
        if (purchaseMade)
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

    public void AttemptSelection(GameObject buttonName)
    {
        if (buttonName)
        {
            TurnOffSelectionHighlights();
            tmpSelection = buttonName;
            tmpSelection.transform.GetChild(1).gameObject.SetActive(true);
        }

        UpdateDescriptionBox();

        //not sold
        if (buttonName.GetComponentInChildren<Text>().text != "SOLD")
        {
            //can afford
            Affordable();

            //can not afford
            LackOfCredits();
        }

        else if (target.name == "WATCH AD")
        {
            Debug.Log("Pressed");
            WatchAds();
        }

        else if (buttonName.GetComponentInChildren<Text>().text == "SOLD")
        {
            SoldOut();
        }
    }
}