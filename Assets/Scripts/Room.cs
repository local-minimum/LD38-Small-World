using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class Room : Singleton<Room> {

    ConversationGenerator _Conversation;

    public ConversationGenerator Conversation
    {
        get {
            return _Conversation;
        }
    }

    public void Response(ConversationPiece response)
    {

    }

    public void ResponseSilent()
    {

    }

}
