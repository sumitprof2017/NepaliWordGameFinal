using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("AdManager")]
    public AdManager adManagerObj;
    [Header("LineRederer")]
    public LineManager lineManagerObj;
    [Header("InstantiatingObject")]
    public ItemScript ItemScriptObj;
    [Header("LevelManager")]
    public LevelManager levelManagerObj;
    [Header("ButtonScript")]
    public ButtonScript buttonScriptObj;
    public GameObject PointStorer, levelManagerStore, LevelComplete, hintHolder, showAddButton;
    public string WordToSplit;
    public int numerOfObjectToSpawn, Length_of_Word;
    GameObject ThreePointParentStore, FourPointParentStore, FivePointParentStore, SixPointParentStore, SevenPointParentStore, EightPointParentStore, NinePointParentStore, TenPointParentStore,RollStorer;
    public List<GameObject> List_Of_Level=new List<GameObject>();
    public GameObject LevelNameStore;
    public int levelCount = 0, coinValue =0;              
    int LevelNameValue = 1;                          
    public AudioSource audioPlayer;
    public AudioClip levComplete_AudioClip;
    public AudioClip continue_Clip;
    public AudioClip background_Clip;
    public TMP_InputField CoinText;
    public List<WordFeature> listOfWordFeature = new List<WordFeature>();
    public string[] array_Of_split_character,originalArrayOfSplitCharacter;
    void Awake()
    {
        /*PlayerPrefs.SetString("levelCount", "");     //sets the game data to empty
        PlayerPrefs.SetString("coinValue", "");*/

        audioPlayer.PlayOneShot(background_Clip);
        LevelComplete.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(onClickContinue);
        ThreePointParentStore = GameObject.Find("ThreePoint");
        FourPointParentStore = GameObject.Find("FourPoint");
        FivePointParentStore = GameObject.Find("FivePoint");
        SixPointParentStore = GameObject.Find("SixPoint");
        SevenPointParentStore = GameObject.Find("SevenPoint");
        EightPointParentStore = GameObject.Find("EightPoint");
        NinePointParentStore = GameObject.Find("NinePoint");
        TenPointParentStore = GameObject.Find("TenPoint");
        RollStorer = GameObject.Find("Roll");

        hintHolder = GameObject.Find("Hint");
        LevelNameStore = GameObject.Find("LevelName");
        CoinText=GameObject.Find("CoinInputField").GetComponent<TMP_InputField>();
        CoinText.text = coinValue.ToString();
        RollStorer.SetActive(false);
        hintHolder.SetActive(false);
        ThreePointParentStore.SetActive(false);
        FourPointParentStore.SetActive(false);
        FivePointParentStore.SetActive(false);
        SixPointParentStore.SetActive(false);
        SevenPointParentStore.SetActive(false);
        EightPointParentStore.SetActive(false);
        NinePointParentStore.SetActive(false);
        TenPointParentStore.SetActive(false);
        LevelNameStore.SetActive(false);
        levelManagerStore.SetActive(false);
        levelCount = 0;

        string levelCounter = PlayerPrefs.GetString("levelCount");
        print("levle counter" + levelCounter);
        string CoinVal = PlayerPrefs.GetString("coinValue");
        print("coin value pref counter" + CoinVal);
        if (!levelCounter.Equals(""))
        {
            levelCount = int.Parse(levelCounter);
        }
        /*  coinValue = 50;
          CoinText.text = coinValue.ToString();*/

        if (!CoinVal.Equals(""))
        {
            coinValue = int.Parse(CoinVal);
            CoinText.text = coinValue.ToString();

        }
        print("current coin value" + coinValue);
        /*        coinValue = 100;
        */
        StartNewLevel();
    }

    [SerializeField]
    public HandAnimation handanimation;
    public void StartNewLevel()
    {
        adManagerObj.LoadInterstitialAd();
        adManagerObj.LoadRewardedAd();
        print("level count value  is " + levelCount);
        
        WordToSplit = listOfWordFeature[levelCount].words;
        TMP_Text levelName = LevelNameStore.GetComponent<TMP_Text>();
        levelCount++;
        levelName.text = "Level " +levelCount;
        levelCount--;
        
        PlayerPrefs.SetString("levelCount", levelCount.ToString());

        print(WordToSplit);
        int number_to_split = listOfWordFeature[levelCount].noToSplitMainWord;
        array_Of_split_character=levelManagerObj.SplitDevanagariString(WordToSplit);
        originalArrayOfSplitCharacter = levelManagerObj.SplitDevanagariString(WordToSplit);
        Length_of_Word=array_Of_split_character.Length;
        print("the length of word is " + Length_of_Word);
        numerOfObjectToSpawn = Length_of_Word;
        RollStorer.SetActive(true);
        hintHolder.SetActive(true);
        if (Length_of_Word == 3)
        {
            PointStorer = ThreePointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
        }
        else if (Length_of_Word == 4)
        {
            PointStorer = FourPointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);          
        }
        else if (Length_of_Word == 5)
        {
            PointStorer = FivePointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
        }
        else if (Length_of_Word == 6)
        {
            PointStorer = SixPointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
        }
        else if(Length_of_Word == 7)
        {
            PointStorer = SevenPointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
            
        }
        else if (Length_of_Word == 8)
        {
            PointStorer = EightPointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
            
        }
        else if (Length_of_Word == 9)
        {
            PointStorer = NinePointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
           
        }
        else if (Length_of_Word == 10)
        {
            PointStorer = TenPointParentStore;
            PointStorer.SetActive(true);
            levelManagerStore.SetActive(true);
            
        }
     
        StartCoroutine(EnableHand());
    }

    IEnumerator EnableHand()
    {
        yield return new WaitForSeconds(1.2f);
        if (levelCount == 0)
        {
            handanimation.gameObject.SetActive(true);
        }
    }

    private int gameOverCounter = 0;
    public void GameOver()
    {
        print("word before change" + WordToSplit);
        WordToSplit = "";
        levelManagerObj.concatenatedString = "";
        print("word after change" + WordToSplit);
        coinValue = coinValue + 25;
        CoinText.text = coinValue.ToString();
        PlayerPrefs.SetString("coinValue", coinValue.ToString());

        RollStorer.SetActive(false);
        PointStorer.SetActive(false);
        LevelNameStore.SetActive(false);
        hintHolder.SetActive(false);
        showAddButton.SetActive(false);
        LevelComplete.SetActive(true);
        audioPlayer.PlayOneShot(levComplete_AudioClip);
        /*adManagerObj.LoadInterstitialAd();
        adManagerObj.LoadBannerAd();*/
        gameOverCounter++;
        if (gameOverCounter >= 3)
        {
            StartCoroutine(AdOnLevelComplete());
            gameOverCounter = 0; // reset after showing ad
        }
    }


    public IEnumerator AdOnLevelComplete()
    {
        yield return new WaitForSeconds(0.5f);
        adManagerObj.ShowInterstitialAd();
        /*adManagerObj.LoadBannerAd();*/

    }

    public void onClickContinue()
    {
        audioPlayer.PlayOneShot(continue_Clip);
        adManagerObj.DestroyBannerAd();
        LevelComplete.SetActive(false); 
        PointStorer.SetActive(false);
        levelManagerStore.SetActive(false);
        /*LevelNameValue = LevelNameValue + 1;*/
        levelCount = levelCount + 1;                   //increasing the value of listOfwords
        print("Updated Value of j is " + levelCount);
        showAddButton.SetActive(true) ;
        StartNewLevel();
    }

    private void OnDestroy()
    {
       /* PlayerPrefs.SetString("levelCount", levelCount.ToString());
        PlayerPrefs.SetString("coinValue", coinValue.ToString());*/
    }
}

[Serializable]
public class WordFeature
{
    public string words;
    public int noToSplitMainWord;
    public List<SubString> subWords;
}

[Serializable]
public class SubString
{
    public string word;
    public int noToSplit;
}

