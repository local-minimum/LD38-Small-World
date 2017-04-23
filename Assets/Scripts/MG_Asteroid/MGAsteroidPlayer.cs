using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGAsteroidPlayer : MiniGamePlayerBase
{    
    public Rigidbody2D m_Asteroid;
    public Rigidbody2D m_Player;
    public Rigidbody2D m_Conversationoid;

    private void Start()
    {
        if (Room.IsInstanciated)
        {
            m_ConvGenerator = Room.instance.Conversation;
        }
        
        if (m_ConvGenerator == null)
        {
            m_ConvGenerator = GetComponent<ConversationGenerator>();
        }
    }

    override public void Play(int difficulty)
    {
        int goodConvCount = 2;
        int badConvCount = 5;
                
        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, ConversationQuality.Good, badConvCount);


        m_TimeLeft = m_Timeout;

        SpawnObject(m_Player);

        for (int i = 0; i < 5; i++)
        {
            SpawnObject(m_Asteroid);
        }

        foreach (var conversationPiece in goodConversations)
        {
            SpawnConversationoid(conversationPiece);
        }

        foreach (var conversationPiece in badConversations)
        {
            SpawnConversationoid(conversationPiece);
        }
        m_Playing = true;
    }

    [SerializeField]
    Transform Asteroids;
    
    private void SpawnObject(Rigidbody2D rigidbody)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D) Instantiate(rigidbody, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.transform.SetParent(Asteroids);
    }

    private void SpawnConversationoid(ConversationPiece conversationPiece)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D) Instantiate(m_Conversationoid, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.transform.SetParent(Asteroids);
        prefabClone.GetComponent<MiniGameConversationObject>().m_ConversationPiece = conversationPiece;

        Sprite sprite = DialogueDisplayer.instance.GetSpriteFromCategory(conversationPiece.Category);
        prefabClone.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public override void EndGame()
    {
        m_Playing = false;
        Room.instance.ResponseSilent();
    }
}
