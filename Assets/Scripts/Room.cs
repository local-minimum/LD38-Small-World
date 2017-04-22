using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class Room : Singleton<Room> {

    [SerializeField]
    ConversationGenerator _Conversation;

    [SerializeField]
    ConversationPiece silentResponse;

    [SerializeField]
    Sprite selfIcon;

    [SerializeField]
    int difficultyLvl = 1;

    public ConversationGenerator Conversation
    {
        get {
            return _Conversation;
        }
    }

    [SerializeField]
    int convoPieces = 4;

    bool otherHappy;

    public void Greet() {
        otherHappy = true;
        DialogueDisplayer.instance.ShowDialogue(_Conversation.GenerateConversation(ConversationCategory.Greeting), _Conversation.currentPerson.icon, ConversationCallackOther);

    }

    public void Response(ConversationPiece response)
    {
        if (_Conversation.currentPerson.likes.Contains(response.Category))
        {
            otherHappy = true;
        } else if (_Conversation.currentPerson.dislikes.Contains(response.Category))
        {
            if (otherHappy)
            {
                difficultyLvl++;
            }
            otherHappy = false;
        }

        DialogueDisplayer.instance.ShowDialogue(response, selfIcon, ConversationCallbackMe);

    }

    public void ResponseSilent()
    {
        otherHappy = false;
        difficultyLvl++;
        DialogueDisplayer.instance.ShowDialogue(_Conversation.GenerateConversation(ConversationCategory.Silent), selfIcon, ConversationCallbackMe);
    }

    void ConversationCallbackMe()
    {
        DialogueDisplayer.instance.ShowDialogue(_Conversation.GenerateConversation(otherHappy ? ConversationQuality.Good : ConversationQuality.Bad), _Conversation.currentPerson.icon, ConversationCallackOther);
    }

    void ConversationCallackOther()
    {
        convoPieces--;
        if (convoPieces > 0)
        {
            MiniGameLoader.instance.LoadRandom();
        } else
        {
            Debug.Log("Here's loading transition to next room");
        }
    }

    void Start()
    {
        StartCoroutine(DelayConvo());
    }

    IEnumerator<WaitForSeconds> DelayConvo()
    {
        yield return new WaitForSeconds(2);
        Greet();
    }
}
