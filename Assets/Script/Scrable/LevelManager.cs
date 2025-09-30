using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Item")]
    public ItemScript ItemScriptObj;
    [Header("LineRederer")]
    public LineManager lineManagerObj;
    [Header("GameManager")]
    public GameManager GameManagerObj; 
    
    [Header("ParentHolder")]
    public GameObject ParentHolder;
    
    public GameObject animationText, ImageObject,ImageObject1, instantiatedItem,instantiatedItemOnLevel,levelToSubSplit;
    public List<GameObject> instantiatedSubWords = new List<GameObject>();
    
    public List<GameObject> instantiatedObjects = new List<GameObject>();
    public List<GameObject> instantiatedItemOnLevelList= new List<GameObject>();
    public List<Transform> ChildPointsList = new List<Transform>();
    public List<string> List_for_letter;
    /*List<char> List_Of_Character=new List<char>();*/
    public List<SubString> listOfSubstring=new List<SubString>();
    public string concatenatedString;
    public TMP_Text image_Letter, LevelLetterStorer,showConnectedWord;
    public List<string> totalConnectedWordList;
    public AudioClip play_Clip_On_CorrectWord;
    public AudioSource audioPlayer_LEC;
    void OnEnable()
    {
        
        totalConnectedWordList = new List<string>();
        StartCoroutine(StartSequence());
    }
    public void LevelEndCheck(string connectedWords)
    {
        if(!totalConnectedWordList.Contains(connectedWords))
        {
            audioPlayer_LEC.PlayOneShot(play_Clip_On_CorrectWord);
            totalConnectedWordList.Add(connectedWords);
            print("total connectedWords count is " + totalConnectedWordList.Count);
        }
        
    }
    private void OnDisable()
    {
        StopCoroutine(StartSequence());
    }
    IEnumerator StartSequence()
    {
        yield return null;
        GameManagerObj.LevelNameStore.SetActive(true);
        List_for_letter = new List<string>();
        for (int i = 0; i < GameManagerObj.numerOfObjectToSpawn; i++)
        {
            //Instantiating image and adding to list
            instantiatedItem = Instantiate(ImageObject, GameManagerObj.PointStorer.transform.GetChild(i).transform.position, Quaternion.identity, GameManagerObj.PointStorer.transform.GetChild(i));
            instantiatedObjects.Add(instantiatedItem);
            
        }
        listOfSubstring = GameManagerObj.listOfWordFeature[GameManagerObj.levelCount].subWords;
        for (int j = 0; j < listOfSubstring.Count; j++)
        {
            string wordToInstantiate = listOfSubstring[j].word;
            int numberToSplit = listOfSubstring[j].noToSplit;
            print("no to split is" + numberToSplit);
            string[] array = SplitDevanagariString(wordToInstantiate);
            print("the array contains " + array.Length);
            for (int k = 0; k < array.Length; k++)
            {
                print("k value is" + k);
                print("j value is" + j);
                int newChildIndex = numberToSplit - 2;
                print("child count to show" + newChildIndex);
                print("gameobj name" + ParentHolder.transform.GetChild(j).gameObject.name);
                print("gameobj name" + ParentHolder.transform.GetChild(j).transform.GetChild(newChildIndex).gameObject.name);
                print("gameobj name" + ParentHolder.transform.GetChild(j).transform.GetChild(newChildIndex).transform.GetChild(k).gameObject.name);
                GameObject instantiatedItemOnLevel = Instantiate(ImageObject1, ParentHolder.transform.GetChild(j).transform.GetChild(numberToSplit - 2).transform.GetChild(k).transform.position,Quaternion.identity, ParentHolder.transform.GetChild(j).transform.GetChild(numberToSplit - 2).transform.GetChild(k));
                instantiatedItemOnLevelList.Add(instantiatedItemOnLevel);
            }
        }
        
        System.Random rng = new System.Random();
        int n = GameManagerObj.array_Of_split_character.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = GameManagerObj.array_Of_split_character[k];
            GameManagerObj.array_Of_split_character[k] = GameManagerObj.array_Of_split_character[n];
            GameManagerObj.array_Of_split_character[n] = value;
        }

        for (int i = 0; i < GameManagerObj.array_Of_split_character.Length; i++)
        {
            ChildPointsList.Add(GameManagerObj.PointStorer.transform.GetChild(i));
            print("child list contain" + ChildPointsList[i]);
        }

        for (int i = 0; i < GameManagerObj.array_Of_split_character.Length; i++)               //Taking letter from ArrayOfWordSplit[] and displaying in image 
        {
            image_Letter = ChildPointsList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
            image_Letter.text = GameManagerObj.array_Of_split_character[i];
            print("Letter in ImageText is " + image_Letter.text);
        }

    }

    public string[] SplitDevanagariString(string input)
    {
        // Initialize a list to store the split characters
        System.Collections.Generic.List<string> splitCharacters = new System.Collections.Generic.List<string>();
        string appendWord = "";
        /*// Iterate through the string using StringInfo to handle surrogate pairs
        System.Globalization.StringInfo si = new System.Globalization.StringInfo(input);
        for (int i = 0; i < si.LengthInTextElements; i++)
        {
            splitCharacters.Add(si.SubstringByTextElements(i, 1));
        }

        // Limit the result to no.toSplit characters
        if (splitCharacters.Count > numberToSplit)
        {
            print("character count after split is: " + splitCharacters.Count);
            print("split character count is greater than no. to split");
            splitCharacters.RemoveRange(numberToSplit, splitCharacters.Count - numberToSplit);
        }*/

        for (int i = 0; i <input.Length ; i++)
        {
            print("count " + i);
            char character = input[i];
            if (character == ' ')
            {
                print("space found");
                splitCharacters.Add(appendWord);
                print(splitCharacters.Count);
                appendWord = "";
                continue;
            }
            appendWord += character;
            print("append word is " + appendWord);
        }
        return splitCharacters.ToArray();
    }
    
    public IEnumerator Add_letter_ToList(string letterReceived)                //Adding Letter to List_for_letter after OnPointerEnter is called. 
    {
        List_for_letter.Add(letterReceived);
        Debug.Log("the letter added in List_for_letter is " + letterReceived);
        print("Count of List_for_letter is " + List_for_letter.Count);
        showConnectedWord=GameObject.Find("ShowConnectedWord").GetComponent<TMP_Text>();
        showConnectedWord.text =showConnectedWord.text + letterReceived;
        print("the connected words are "+showConnectedWord.text);
   
        yield return new WaitForSeconds(0.2f);
    }
    public void UndoInstantiatedObjects()
    {
        for(int i=0;i<instantiatedObjects.Count;i++)
        {
            Destroy(instantiatedObjects[i]);
            
        }
        for (int i = 0; i < instantiatedItemOnLevelList.Count; i++)
        {
            Destroy(instantiatedItemOnLevelList[i]);
        }

        // Clear the list to remove references to the destroyed objects
        instantiatedObjects.Clear();
        instantiatedItemOnLevelList.Clear();
    }

    public IEnumerator DelayTimeBeforeLevelComplete()
    {
        
        if (totalConnectedWordList.Count == listOfSubstring.Count || AreAllImageLetterTextsFilled())
        {
            showConnectedWord.text = "";
            string[] arrayForAnimationText = { "Amazing!", "Great job!", "Superb!", "Well done!", "Perfect!", "Marvelous!" };
            int value = Random.Range(0, 6);
            TMP_Text textanim=animationText.GetComponent<TMP_Text>();
            textanim.text = arrayForAnimationText[value];

            LeanTween.scale(animationText, new Vector3(1.5f, 1.5f, 1.5f), 1f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);
           
            yield return new WaitForSeconds(2f);
            LeanTween.scale(animationText, new Vector3(0f, 0f, 0f), 0f);
            List_for_letter.Clear();
            ChildPointsList.Clear();
            lineManagerObj.lineCreator.positionCount = 0;
            lineManagerObj.lineCreator.enabled = false;
            lineManagerObj.linesList.Clear();
            print("line list is cleared");
           ItemScriptObj.lineManagerGameObject.SetActive(false);
            UndoInstantiatedObjects();
            GameManagerObj.GameOver();
        }
        showConnectedWord.text = "";
        List_for_letter.Clear();
    }

    public bool AreAllImageLetterTextsFilled()
    {
        GameObject ParentHolder = this.ParentHolder;
        if (ParentHolder == null)
        {
            Debug.LogError(" ParentHolder is not assigned!");
            return false;
        }

        // Get all nested children under ParentHolder
        Transform[] allChildren = ParentHolder.GetComponentsInChildren<Transform>(true);

        bool foundAny = false;

        foreach (Transform child in allChildren)
        {
            // Look specifically for "ImageLetter"
            if (child.name == "Image_Letter")
            {
                foundAny = true;

                // Try getting any TMP text component
                TMP_Text tmp = child.GetComponent<TMP_Text>();

                if (tmp == null)
                {

                    return false;
                }

                // Check if text is empty or whitespace
                if (string.IsNullOrWhiteSpace(tmp.text))
                {

                    return false;
                }
            }
        }

        if (!foundAny)
        {

            return false;
        }


        return true;
    }

    public IEnumerator EndGameFromHint()
    {
       
          
            string[] arrayForAnimationText = { "Amazing!", "Great job!", "Superb!", "Well done!", "Perfect!", "Marvelous!" };
            int value = Random.Range(0, 6);
            TMP_Text textanim = animationText.GetComponent<TMP_Text>();
            textanim.text = arrayForAnimationText[value];

            LeanTween.scale(animationText, new Vector3(1.5f, 1.5f, 1.5f), 1f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

            yield return new WaitForSeconds(2f);
            LeanTween.scale(animationText, new Vector3(0f, 0f, 0f), 0f);
            List_for_letter.Clear();
            ChildPointsList.Clear();
            lineManagerObj.lineCreator.positionCount = 0;
            lineManagerObj.lineCreator.enabled = false;
            lineManagerObj.linesList.Clear();
            print("line list is cleared");
            ItemScriptObj.lineManagerGameObject.SetActive(false);
            UndoInstantiatedObjects();
            GameManagerObj.GameOver();
        
        List_for_letter.Clear();
    }

}
