using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;
using UnityEngine.SceneManagement;

public class Room : Singleton<Room> {

    [SerializeField]
    ConversationPiece silentResponse;

    [SerializeField]
    Sprite selfIcon;

    [SerializeField]
    int difficultyLvl = 1;

    PersonProfile m_personProfile;

    public PersonProfile Frenemy {

        get {
            if (m_personProfile == null)
            {
                m_personProfile = GetComponent<PersonProfile>();
            }
            if (m_personProfile == null)
            {
                m_personProfile = gameObject.AddComponent<PersonProfile>();
            }
            return m_personProfile;
        }
    }

    public int Difficulty
    {
        get
        {
            return difficultyLvl;
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
        StartCoroutine(BlaLoop());
        mouthAnim.SetTrigger(talkTrigger);
        HealthUI.instance.ShowHealthBar();
        otherHappy = true;
        frenemyCat = ConversationCategory.Greeting;
        DialogueDisplayer.instance.ShowDialogue(ConversationGenerator.instance.GenerateConversation(ConversationCategory.Greeting), Frenemy.icon, false, ConversationCallbackMe);

    }

    ConversationCategory frenemyCat = ConversationCategory.Greeting;
    ConversationCategory playerCat = ConversationCategory.Silent;

    public void Response(ConversationPiece response)
    {
        SpeakerSystem.instance.FadeToWorld();
        
        mouthAnim.SetTrigger(noTalkTrigger);
        MiniGameLoader.instance.UnloadCurrent();
        MiniGameControllerUI.instance.HideAll();
        HealthUI.instance.ShowHealthBar();
        playerCat = response.Category;
        if (Frenemy.likes.Contains(response.Category))
        {
            otherHappy = true;
            otherPiecesThisTurn = 1;
        } else if (Frenemy.dislikes.Contains(response.Category))
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
        SpeakerSystem.instance.FadeToWorld();
        mouthAnim.SetTrigger(noTalkTrigger);
        MiniGameLoader.instance.UnloadCurrent();
        MiniGameControllerUI.instance.HideAll();
        HealthUI.instance.ShowHealthBar();
        otherHappy = false;
        playerCat = ConversationCategory.Silent;
        difficultyLvl++;
        otherPiecesThisTurn = Random.Range(2, 5);
        DialogueDisplayer.instance.ShowDialogue(ConversationGenerator.instance.GenerateConversation(ConversationCategory.Silent), selfIcon, true, ConversationCallbackMe);
    }

    void ConversationCallbackMe()
    {
        keep_blabla = false;
        otherPiecesThisTurn--;
        ConversationPiece piece = GetPiece();
        mouthAnim.SetTrigger(talkTrigger);
        StartCoroutine(BlaLoop());
        if (HealthUI.IsDead)
        {
            DialogueDisplayer.instance.ShowDialogue(piece, Frenemy.icon, false, DeathCallback);
            StartCoroutine(FadeDeath());
        }
        else if (otherPiecesThisTurn > 0)
        {
            DialogueDisplayer.instance.ShowDialogue(piece, Frenemy.icon, false, ConversationCallbackMe);
            
        }
        else {
            
            DialogueDisplayer.instance.ShowDialogue(piece, Frenemy.icon, false, ConversationCallackOther);
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
                piece = ConversationGenerator.instance.GenerateConversation(frenemyCat, ConversationQuality.Good);
                playerCat = frenemyCat;
            }
            else
            {
                Debug.Log("Player " + playerCat + ", Other Unhappy");
                piece = ConversationGenerator.instance.GenerateConversation(playerCat, ConversationQuality.Bad);
                frenemyCat = ConversationCategory.Silent;
            }
            HealthUI.instance.Increase();
            otherHappy = true;
        }
        else if (playerCat == frenemyCat)
        {
            Debug.Log("Player and Frenemy " + frenemyCat);
            piece = ConversationGenerator.instance.GenerateConversation(frenemyCat, ConversationQuality.Good);
        }
        else
        {
            Debug.Log("Frenemy select new topic");
            piece = ConversationGenerator.instance.GenerateConversation(ConversationQuality.Good);
            frenemyCat = piece.Category;
        }
        
        return piece;
    }
    void ConversationCallackOther()
    {
        keep_blabla = false;
        ProfileViewer.instance.HideProfile();


        convoPieces--;
        if (convoPieces >= 0)
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

    bool keep_blabla;

    IEnumerator<WaitForSeconds> BlaLoop()
    {
        keep_blabla = true;
        while (keep_blabla)
        {
            SpeakerSystem.instance.Bla();
            yield return new WaitForSeconds(0.5f);
        }
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
        ProfileViewer.instance.ShowProfile(Frenemy);
        yield return new WaitForSeconds(delayBeforeGreet);
        Greet();
        yield return new WaitForSeconds(delayBeforeHide);
        ProfileViewer.instance.HideProfile();
    }
}
