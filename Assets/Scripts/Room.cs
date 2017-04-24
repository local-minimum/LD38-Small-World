using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;
using UnityEngine.SceneManagement;

public class Room : Singleton<Room> {

    [SerializeField]
    ConversationGenerator _Conversation;

    [SerializeField]
    ConversationPiece silentResponse;

    [SerializeField]
    Sprite selfIcon;

    [SerializeField]
    int difficultyLvl = 1;

    public int Difficulty
    {
        get
        {
            return difficultyLvl;
        }
    }

    public ConversationGenerator Conversation
    {
        get {
            return _Conversation;
        }
    }

    [SerializeField]
    int convoPieces = 4;

    [SerializeField]
    int otherPiecesThisTurn = 1;

    [SerializeField]
    Animator mouthAnim;

    [SerializeField]
    string talkTrigger = "Talk";

    [SerializeField]
    string noTalkTrigger = "NoTalk";
    bool otherHappy;

    public void Greet() {
        mouthAnim.SetTrigger(talkTrigger);
        HealthUI.instance.ShowHealthBar();
        otherHappy = true;
        frenemyCat = ConversationCategory.Greeting;
        DialogueDisplayer.instance.ShowDialogue(_Conversation.GenerateConversation(ConversationCategory.Greeting), _Conversation.currentPerson.icon, false, ConversationCallbackMe);

    }

    ConversationCategory frenemyCat = ConversationCategory.Greeting;
    ConversationCategory playerCat = ConversationCategory.Silent;

    public void Response(ConversationPiece response)
    {
        mouthAnim.SetTrigger(noTalkTrigger);
        MiniGameLoader.instance.UnloadCurrent();
        MiniGameControllerUI.instance.HideAll();
        HealthUI.instance.ShowHealthBar();
        playerCat = response.Category;
        if (_Conversation.currentPerson.likes.Contains(response.Category))
        {
            otherHappy = true;
            otherPiecesThisTurn = 1;
        } else if (_Conversation.currentPerson.dislikes.Contains(response.Category))
        {
            if (otherHappy)
            {
                difficultyLvl++;
            }
            otherHappy = false;
            otherPiecesThisTurn = 2;
        }
        DialogueDisplayer.instance.ShowDialogue(response, selfIcon, true, ConversationCallbackMe);
    }

    public void ResponseSilent()
    {
        mouthAnim.SetTrigger(noTalkTrigger);
        MiniGameLoader.instance.UnloadCurrent();
        MiniGameControllerUI.instance.HideAll();
        HealthUI.instance.ShowHealthBar();
        otherHappy = false;
        playerCat = ConversationCategory.Silent;
        difficultyLvl++;
        otherPiecesThisTurn = Random.Range(2, 5);
        DialogueDisplayer.instance.ShowDialogue(_Conversation.GenerateConversation(ConversationCategory.Silent), selfIcon, true, ConversationCallbackMe);
    }

    void ConversationCallbackMe()
    {
        otherPiecesThisTurn--;
        ConversationPiece piece = GetPiece();
        mouthAnim.SetTrigger(talkTrigger);
        if (HealthUI.IsDead)
        {
            DialogueDisplayer.instance.ShowDialogue(piece, _Conversation.currentPerson.icon, false, DeathCallback);
            StartCoroutine(FadeDeath());
        }
        else if (otherPiecesThisTurn > 0)
        {
            DialogueDisplayer.instance.ShowDialogue(piece, _Conversation.currentPerson.icon, false, ConversationCallbackMe);
            
        }
        else {
            
            DialogueDisplayer.instance.ShowDialogue(piece, _Conversation.currentPerson.icon, false, ConversationCallackOther);
        }
    }

    IEnumerator<WaitForSeconds> FadeDeath()
    {
        yield return new WaitForSeconds(1f);
        DialogueDisplayer.instance.Continue();
        MiniGameLoader.instance.LoadDeath();
    }

    void DeathCallback()
    {

    }

    ConversationPiece GetPiece()
    {
        ConversationPiece piece;
        if (!otherHappy)
        {
            if (playerCat == ConversationCategory.Silent)
            {
                Debug.Log("Player Silent, Other Unhappy");
                piece = _Conversation.GenerateConversation(frenemyCat, ConversationQuality.Good);
                playerCat = frenemyCat;
            }
            else
            {
                Debug.Log("Player " + playerCat + ", Other Unhappy");
                piece = _Conversation.GenerateConversation(playerCat, ConversationQuality.Bad);
                frenemyCat = ConversationCategory.Silent;
            }
            HealthUI.instance.Increase();
            otherHappy = true;
        }
        else if (playerCat == frenemyCat)
        {
            Debug.Log("Player and Frenemy " + frenemyCat);
            piece = _Conversation.GenerateConversation(frenemyCat, ConversationQuality.Good);
        }
        else
        {
            Debug.Log("Frenemy select new topic");
            piece = _Conversation.GenerateConversation(ConversationQuality.Good);
            frenemyCat = piece.Category;
        }
        
        return piece;
    }
    void ConversationCallackOther()
    {
        ProfileViewer.instance.HideProfile();

        convoPieces--;
        if (convoPieces > 0)
        {
            HealthUI.instance.HideHealthBar();
            MiniGameLoader.instance.LoadRandom();
        } else
        {
            RoomOutro.instance.ShowOutro(LoadNextRoomScene);
        }
    }

    [SerializeField]
    string nextScene;

    void LoadNextRoomScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    void Start()
    {
        StartCoroutine(DelayConvo());
    }

    [SerializeField]
    float delayBeforeProfile = 0.5f;

    [SerializeField]
    float delayBeforeGreet = 2f;

    [SerializeField]
    float delayBeforeHide = 10f;

    IEnumerator<WaitForSeconds> DelayConvo()
    {
        yield return new WaitForSeconds(delayBeforeProfile);
        ProfileViewer.instance.ShowProfile(_Conversation.currentPerson);
        yield return new WaitForSeconds(delayBeforeGreet);
        Greet();
        yield return new WaitForSeconds(delayBeforeHide);
        ProfileViewer.instance.HideProfile();
    }
}
