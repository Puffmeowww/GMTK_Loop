using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "Story/Story")]
public class Story : ScriptableObject
{
    public int id;

    [Tooltip("Only 3 strings")]
    public List<string> ChoiceTextList;

    public List<int> SuccessOrder;
    public List<int> FailedOrder;

    public List<String> SuccessStoryText;
    public List<String> FailStoryText;

    public string SuccessText;
}
