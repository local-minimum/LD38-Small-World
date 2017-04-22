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
    GameObject dialogueParent;

    Action _callback;

    bool displaying = false;

    public void ShowDialogue(ConversationPiece piece, Sprite icon, Action callback)
    {
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
    Sprite[] categories;

    Sprite GetSpriteFromCategory(ConversationCategory category)
    {
        return categories[(int)category];
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
