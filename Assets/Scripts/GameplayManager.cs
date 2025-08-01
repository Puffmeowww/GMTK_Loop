using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    private int CurrentStoryId;

    [HideInInspector]
    public List<int> CurrentOrder;

    [Header("Story")]
    [SerializeField]
    public List<Story> StoryList;

    [Header("UI Element")]
    [SerializeField]
    private GameObject InstructionText;
    [SerializeField]
    private GameObject ThreadLoop;
    [SerializeField]
    private GameObject ThreadLine_Looped;
    [SerializeField]
    private GameObject ThreadLine_NotLooped;
    [SerializeField]
    private List<GameObject> ThreadIconsInLoop;
    [SerializeField]
    private GameObject TextScroll;
    [SerializeField]
    private List<GameObject> ButtonList;
    [SerializeField]
    private GameObject PlayerChoiceText;
    [SerializeField]
    private GameObject KnittingAssets;
    [SerializeField]
    private GameObject FailAssets;
    [SerializeField]
    private GameObject SuccessAssets;
    [SerializeField]
    private GameObject NPC;
    [SerializeField]
    private GameObject NextLevelButton;

    [Header("Text Scroll")]
    [SerializeField]
    private GameObject TextPrefab;
    [SerializeField]
    private Transform Content;
    [SerializeField]
    private ScrollRect ScrollRect;
    [SerializeField]
    private GameObject TextClickPad;
    private GameObject currentTextBlock;

    private List<string> CurrentStoryTextList;
    private int CurrentLineIndex;

    private bool CurrentEnding;
    [Header("ObtainedText")]
    [SerializeField]
    private GameObject ObtainedText;
    [SerializeField]
    private List<string> ObtainedTextList;

    [Header("Achievement")]
    [SerializeField]
    private GameObject AchievementCanvas;
    [SerializeField]
    private GameObject AchievementText;


    /* Gameplay Related */
    public void ResetLevel()
    {
        
        NextLevelButton.SetActive(false);
        //Clear OrderList
        CurrentOrder.Clear();
        ClearContent();

        ObtainedText.SetActive(false);
        SetAllButtonActive(true);
        //show thread loop
        ThreadLoop.SetActive(true);
        ThreadLine_NotLooped.SetActive(true);
        ThreadLine_Looped.SetActive(false);
        //show instruction
        InstructionText.SetActive(true);
        
        //hide text scroll
        TextScroll.SetActive(false);
        //Disable text click
        TextClickPad.SetActive(false);
        //Reset line index
        CurrentLineIndex = 0;
        //make all thread icons to 50% transparency
        foreach(GameObject go in ThreadIconsInLoop)
        {
            ChangeTransparency(go, 0.3f);
        }
        //show buttons
        SetAllButtonActive(true);
        //Set button transparency
        foreach (GameObject b in ButtonList)
        {
            ChangeTransparency(b, 1f);
        }
        //set new button text
        SetAllButtonText(StoryList[CurrentStoryId].ChoiceTextList);

        KnittingAssets.SetActive(false);
        PlayerChoiceText.SetActive(false);
        SuccessAssets.SetActive(false);
        FailAssets.SetActive(false);

    }

    public void Start()
    {
        ResetLevel();
    }

    public void ClickSentense(int ButtonID)
    {
        //make current button transparent
        ChangeTransparency(ButtonList[ButtonID], 0.5f);
        ButtonList[ButtonID].GetComponent<Button>().enabled = false;


        //change thread icons to 100% transparancy
        ChangeTransparency(ThreadIconsInLoop[CurrentOrder.Count], 1f);
        //store order
        CurrentOrder.Add(ButtonID);

        //Check if all orders are made
        if(CurrentOrder.Count == 3)
        {
            if(CheckOrder() == 1 || CheckOrder() == 0)
            {
                ThreadLine_NotLooped.SetActive(false);
                ThreadLine_Looped.SetActive(true);
            }
            
            //hide all buttons
            SetAllButtonActive(false);

            StartCoroutine(WaitThenSwitchStory());
        }
    }

    private IEnumerator WaitThenSwitchStory()
    {
        yield return new WaitForSeconds(1f);

        ThreadLine_Looped.SetActive(false);
        switch (CheckOrder())
        {
            case 0:
                //success
                CurrentStoryTextList = StoryList[CurrentStoryId].SuccessStoryText;
                Debug.Log("Success");
                CurrentEnding = true;
                StartStory();
                break;
            case 1:
                //fail
                CurrentStoryTextList = StoryList[CurrentStoryId].FailStoryText;
                CurrentEnding = false;
                Debug.Log("Fail");
                StartStory();
                break;
            case 2:
                //restart
                Debug.Log("Not Right");
                ResetLevel();
                break;
            default:
                Debug.Log("NOOOOO");
                break;
        }
    }

    public int CheckOrder()
    {    
        if(CurrentOrder.SequenceEqual(StoryList[CurrentStoryId].SuccessOrder))
        {
            return 0;
        }
        else if (CurrentOrder.SequenceEqual(StoryList[CurrentStoryId].FailedOrder))
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    //after 3 orders all made
    public void StartStory()
    {
        //show knitting title
        KnittingAssets.SetActive(true);
        PlayerChoiceText.SetActive(true);
        //show text click
        TextClickPad.SetActive(true);
        //hide intruction
        InstructionText.SetActive(false);
        //hide threads
        ThreadLoop.SetActive(false);

        //show text scroll
        TextScroll.SetActive(true);
        //show player choice/order text
        PlayerChoiceText.GetComponent<TextMeshProUGUI>().text = StoryList[CurrentStoryId].ChoiceTextList[CurrentOrder[0]]
            +"\n"
            + StoryList[CurrentStoryId].ChoiceTextList[CurrentOrder[1]]
            + "\n"
            + StoryList[CurrentStoryId].ChoiceTextList[CurrentOrder[2]];

        //start scrolling text
        AddLine(CurrentStoryTextList[CurrentLineIndex]);

    }

    public void ClickText()
    {
        //if finish story && show all text
        if (CurrentLineIndex > CurrentStoryTextList.Count - 1 && currentTextBlock.GetComponent<TypewriterEffect>().IsAllText())
        {
            CurrentLineIndex = 0;
            TextClickPad.SetActive(false);
            ReachEnding();
            return;
        }

        //if all text, load next
        if(currentTextBlock.GetComponent<TypewriterEffect>().IsAllText())
        {
            AddLine(CurrentStoryTextList[CurrentLineIndex]);
            ScrollToBottom();
        }
        //if still loading, show full text
        else
        {
            //show all text
            currentTextBlock.GetComponent<TypewriterEffect>().StopAllCoroutines();
            currentTextBlock.GetComponent<TypewriterEffect>().ShowAllText();
        }

    }

    public void ReachEnding()
    {
        if (CurrentEnding)
        {
            ShowSuccessEnding();
        }
        else
        {
            ShowFailEnding();
        }

        KnittingAssets.SetActive(false);
        PlayerChoiceText.SetActive(false);
    }
    public void ShowFailEnding()
    {
        //show fail title
        FailAssets.SetActive(true);

        ObtainedText.SetActive(true);

        ObtainedText.GetComponent<TextMeshProUGUI>().text = StoryList[CurrentStoryId].FailSentence;
        ObtainedTextList.Add(StoryList[CurrentStoryId].FailSentence);

        NextLevelButton.SetActive(true);
    }
    public void ShowSuccessEnding()
    {
        //show success text
        SuccessAssets.SetActive(true);

        ObtainedText.SetActive(true);

        ObtainedText.GetComponent<TextMeshProUGUI>().text = StoryList[CurrentStoryId].SuccessSentence;
        ObtainedTextList.Add(StoryList[CurrentStoryId].SuccessSentence);

        NextLevelButton.SetActive(true);
    }


    /* Functional Related */
    public void ChangeTransparency(GameObject go,float a)
    {
        Image i = go.GetComponent<Image>();
        Color c = i.color;
        c.a = a;
        i.color = c;
    }

    public void SetAllButtonActive(bool b)
    {
        foreach(GameObject go in ButtonList)
        {
            go.SetActive(b);
            if(b)
            {
                go.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void SetAllButtonText(List<string> sl)
    {
        for(int i= 0;i<ButtonList.Count;i++)
        {
            //Debug.Log(ButtonList.Count + "&" + sl.Count);
            ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = sl[i];
        }
    }

    void AddLine(string text)
    {
        var go = Instantiate(TextPrefab, Content);
        currentTextBlock = go;

        go.GetComponent<TypewriterEffect>().StartTypeWriter(text);

        var textComp = go.GetComponent<TMP_Text>(); 
        textComp.text = text;

        CurrentLineIndex += 1;
        ScrollToBottom();
    }

    void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases(); 
        ScrollRect.verticalNormalizedPosition = 0f;
    }

    public void ClearContent()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoadNextLevel()
    {
        //cuurent id + 1
        CurrentStoryId += 1;
        ResetLevel();
    }

    public void OpenAchievement()
    {
        AchievementCanvas.SetActive(true);
        string fulltext = "";
        foreach(string s in ObtainedTextList)
        {
            fulltext += s;
            fulltext += "\n";
        }
        AchievementText.GetComponent<TextMeshProUGUI>().text = fulltext;
    }
    public void CloseAchievement()
    {
        AchievementCanvas.SetActive(false);
    }


}
