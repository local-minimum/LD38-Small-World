using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;
using UnityEngine.UI;
using System;

public class DialogueDisplayer : Singleton<DialogueDisplayer> {

    [SerializeField]
    Image iconImage;

    [SerializeField]
    Text dialogueText;

    [SerializeField]
    Image contentType;

    [SerializeField]
    Text nameField;

    [SerializeField]
    GameObject dialogueParent;

    Action _callback;

    bool displaying = false;

    public void ShowDialogue(ConversationPiece piece, Sprite icon, bool isPlayer, Action callback)
    {
        nameField.text = isPlayer ? "YOU:" : Room.instance.Conversation.currentPerson.FullName + ":";

        iconImage.sprite = icon;
        dialogueText.text = piece.Text;

        if (piece.Category == ConversationCategory.Silent || piece.Category == ConversationCategory.Greeting)
        {
            contentType.enabled = false;
        }
        else {
            contentType.sprite = GetSpriteFromCategory(piece.Category);
            contentType.enabled = true;
        }
        _callback = callback;
        dialogueParent.SetActive(true);
        displaying = true;
    }

    [SerializeField]
    Sprite familySprite;

    [SerializeField]
    Sprite foodSprite;

    [SerializeField]
    Sprite memorySprite;

    [SerializeField]
    Sprite sportsSprite;

    [SerializeField]
    Sprite politicsSprite;

    [SerializeField]
    Sprite weatherSprite;

    [SerializeField]
    Sprite workSprite;


    public Sprite GetSpriteFromCategory(ConversationCategory category)
    {
        switch (category)
        {
            case ConversationCategory.Family:
                return familySprite;
            case ConversationCategory.Food:
                return foodSprite;
            case ConversationCategory.Nostalgy:
                return memorySprite;
            case ConversationCategory.Politics:
                return politicsSprite;
            case ConversationCategory.Sport:
                return sportsSprite;
            case ConversationCategory.Weather:
                return weatherSprite;
            case ConversationCategory.Work:
                return workSprite;
            default:
                return null;
        }
    }

    public void Continue()
    {
        if (displaying)
        {
            displaying = false;
            dialogueParent.SetActive(false);
            if (_callback != null)
            {
                _callback();
            }
        }
    }
    
    void Update()
    {
        if (displaying)
        {
            if (Input.GetButtonDown("Submit"))
            {
                Continue();
            }
        }
    }

}
