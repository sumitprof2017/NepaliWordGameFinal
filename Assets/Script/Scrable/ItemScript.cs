using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class ItemScript : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    public string letter_Storer;
    public LevelManager levelObj;
    public GameManager GameManagerObj;
    public GameObject lineManagerGameObject;
    public LineManager lineManagerObj;
 
    public void OnPointerDown(PointerEventData eventData)
    {
        lineManagerObj.lineCreator.enabled = false;
        lineManagerGameObject.transform.position = transform.position;
        lineManagerObj.lineCreator.positionCount = 3;
        lineManagerObj.lineCreator.SetPosition(0, lineManagerGameObject.transform.position);
        lineManagerObj.lineCreator.SetPosition(lineManagerObj.lineCreator.positionCount - 2, transform.position);

        lineManagerGameObject.SetActive(true);
        print("line manager gameobject is active");

        
        if (!lineManagerObj.linesList.Contains(gameObject))
        {
            lineManagerObj.linesList.Add(gameObject);
        }
        letter_Storer = gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;
       levelObj.StartCoroutine(levelObj.Add_letter_ToList(letter_Storer));

        StartCoroutine(EnableLineRenderer());
    }
    private IEnumerator EnableLineRenderer()
    {
        // Wait for a short duration 
        yield return new WaitForSeconds(0.1f);

        // Enable the LineRenderer after the delay
        lineManagerObj.lineCreator.enabled = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //onPointer enter adding the items to line renderer
        if (lineManagerGameObject.activeSelf)
        {
            print("item name is: " + gameObject.name);
                                     
            if (!lineManagerObj.linesList.Contains(gameObject))                 
            {
                lineManagerObj.linesList.Add(gameObject);                       //Adding ImageItems to linesList on PointerEnter
                print("Image is added to linesList");
                lineManagerObj.lineCreator.positionCount++;
                lineManagerObj.lineCreator.SetPosition(lineManagerObj.lineCreator.positionCount - 2, gameObject.transform.position);
                letter_Storer = gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;      // adding Image_letter to List_for_letter onPointerEnter
                print("letter_storer ma yo xa"+letter_Storer);
                levelObj.StartCoroutine(levelObj.Add_letter_ToList(letter_Storer));

            }

        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        
        lineManagerGameObject.SetActive(false);
        lineManagerObj.lineCreator.positionCount = 0;
        lineManagerObj.linesList.Clear();
        StopCoroutine(EnableLineRenderer());
        for (int j = 0; j < levelObj.listOfSubstring.Count; j++)
        {
            
            print("the connected word is " + levelObj.showConnectedWord.text);
            string currentword = levelObj.listOfSubstring[j].word.Replace(" ","");
            print("current words are " + currentword);
 
            if (string.Equals(levelObj.showConnectedWord.text, currentword, StringComparison.OrdinalIgnoreCase))
            {
                
                levelObj.LevelEndCheck(levelObj.showConnectedWord.text) ;
                print("connected words and subwords are same");
                int numberToSplit = levelObj.listOfSubstring[j].noToSplit;
                for (int k = 0; k < numberToSplit; k++)
                {
                    TMP_Text instantiatedImage1Text = levelObj.ParentHolder.transform.GetChild(j).transform.GetChild(numberToSplit - 2).transform.GetChild(k).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
                    instantiatedImage1Text.text = levelObj.List_for_letter[k].ToUpper();
                    
                }
            }
            else
            {
                Debug.Log("not same words");
            }
        }
        StartCoroutine(levelObj.DelayTimeBeforeLevelComplete());
        
    }

   
}
