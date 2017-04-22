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

    public void ShowDialogue(ConversationPiece piece, Sprite icon, Action callback)
    {
        iconImage.sprite = icon;
        dialogueText.text = piece.Text;
        contentType.sprite = GetSpriteFromCategory(piece.Category);
        _callback = callback;
        dialogueParent.SetActive(true);
    }

    [SerializeField]
    Sprite[] categories;

    Sprite GetSpriteFromCategory(ConversationCategory category)
    {
        return categories[(int)category];
    }

    public void Continue()
    {
        dialogueParent.SetActive(false);
        if (_callback != null)
        {
            _callback();
        }
    }
    
}
