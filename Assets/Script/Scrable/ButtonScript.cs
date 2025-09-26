using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.AI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler
{
    [Header("AdManager")]
    public AdManager adManagerObj;
    [Header("GameManager")]
    public GameManager gameManagerObj;
    [Header("levelManager")]
    public LevelManager levelManagerObj;
    int newValue;
    [Header("Center")]
    public GameObject GameObjectForCenterPoint;
    [Header("Roll")]
    public AudioSource audiosource_R;
    [Header("Roll")]
    public AudioClip clip_Roll;
    GameObject coinNotify;
    GameObject showAddButton;
    public List<Transform> OnButtonClick_new_List = new List<Transform>();
    private void Start()
    {
        coinNotify = GameObject.Find("CoinNotification");
        showAddButton = GameObject.Find("ShowRewardedAddBtn");
        coinNotify.SetActive(false);
        gameObject.GetComponent<Button>().onClick.AddListener(OnRollButtonClick);
        gameManagerObj.hintHolder.GetComponent<Button>().onClick.AddListener(OnHintButtonClick);
        showAddButton.GetComponent<Button>().onClick.AddListener(OnAddButtonClick);
    }
    public void OnAddButtonClick()
    {
        Debug.Log("Reward Button is Clicked");
        adManagerObj.ShowRewardedAd();
    }
    public void OnRollButtonClick()
    {

        GetComponent<Button>().interactable = false;

        for (int i = 0; i < levelManagerObj.ChildPointsList.Count; i++)
        {
            OnButtonClick_new_List.Add(levelManagerObj.ChildPointsList[i]);
        }
        moveItemsToCenter();
    }
    Coroutine moveItemToNewPlace;
    public void moveItemsToCenter()
    {
        print("count of instantiate object" + levelManagerObj.instantiatedObjects.Count);
        for (int i = 0; i < levelManagerObj.instantiatedObjects.Count; i++)
        {
            print("move item to center " + i);
            LeanTween.move(levelManagerObj.instantiatedObjects[i], GameObjectForCenterPoint.transform.position, 0.2f).setOnComplete(() =>
            {
                print(levelManagerObj.instantiatedObjects.Count - 1);
                print("i value" + i);
                if (i == levelManagerObj.instantiatedObjects.Count)
                {
                    if (moveItemToNewPlace == null)
                        moveItemToNewPlace = StartCoroutine(MoveItemToNewPlace());

                }
            });
        }

    }

    public IEnumerator MoveItemToNewPlace()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < levelManagerObj.instantiatedObjects.Count; i++)
        {
            newValue = Random.Range(0, OnButtonClick_new_List.Count);
            LeanTween.move(levelManagerObj.instantiatedObjects[i], OnButtonClick_new_List[newValue].transform.position, 0.1f);
            OnButtonClick_new_List.Remove(OnButtonClick_new_List[newValue]);
        }
        yield return new WaitForSeconds(0.2f);
        moveItemToNewPlace = null;
        GetComponent<Button>().interactable = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audiosource_R.PlayOneShot(clip_Roll);
    }

    public void OnHintButtonClick()
    {
        if (gameManagerObj.coinValue >= 10)
        {
            
            
            for (int j = 0; j < levelManagerObj.listOfSubstring.Count; j++)
            {
                
                int num_To_Split = levelManagerObj.listOfSubstring[j].noToSplit;
                string subWord = levelManagerObj.listOfSubstring[j].word;
                string[] arrayOfSubWord = levelManagerObj.SplitDevanagariString(subWord);
                for (int k = 0; k < num_To_Split; k++)
                {
                    TMP_Text nullText = levelManagerObj.ParentHolder.transform.GetChild(j).transform.GetChild(num_To_Split - 2).transform.GetChild(k).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();


                    if (string.IsNullOrEmpty(nullText.text))
                    {
                        print("text is null");
                        nullText.text = arrayOfSubWord[k].ToUpper();
                        k = num_To_Split;
                        j = levelManagerObj.listOfSubstring.Count;
                        gameManagerObj.coinValue = gameManagerObj.coinValue - 10;
                        gameManagerObj.CoinText.text = gameManagerObj.coinValue.ToString();
                    }
                    else
                    {
                        print("text is not null");

                    }

                }
            }
        }

        else
        {
            print("not enough coin");
            StartCoroutine(CoinMsgNotify());
        }

    }


    public IEnumerator CoinMsgNotify()
    {
        coinNotify.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        coinNotify.SetActive(false);
    }
}
