using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class Room : Singleton<Room> {

    [SerializeField]
    ConversationGenerator _Conversation;

    public ConversationGenerator Conversation
    {
        get {
            return _Conversation;
        }
    }

    public void Response(ConversationPiece response)
    {
        //TODO: Do UI stuff
        //Ask if next scene
    }

    public void ResponseSilent()
    {
        //TODO: Do UI stuff
        //Ask if next scene
    }
}
