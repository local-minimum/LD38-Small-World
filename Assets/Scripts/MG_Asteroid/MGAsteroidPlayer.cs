using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGAsteroidPlayer : MiniGamePlayerBase
{    
    public Rigidbody2D m_Asteroid;
    public Rigidbody2D m_Player;
    public Rigidbody2D m_Conversationoid;


    override public void Play(int difficulty)
    {
        int goodConvCount = 2;
        int badConvCount = 5;
                
        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, ConversationQuality.Good, badConvCount);


        m_TimeLeft = m_Timeout;

        SpawnObject(m_Player).position = Vector3.zero;

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
    
    private Transform SpawnObject(Rigidbody2D rigidbody)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D) Instantiate(rigidbody, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.transform.SetParent(Asteroids);
        return prefabClone.transform;
    }

    [SerializeField]
    float distanceToCenter = 0.3f;

    Vector3 GetConversationoidPos()
    {
        float d = 0.5f - distanceToCenter / 2f;
        Vector3 screenPos = new Vector3(Random.Range(d, 1) * Screen.width, Random.Range(d, 1) * Screen.height, Camera.main.farClipPlane / 2);
        
        if (Random.value < 0.5f) {
            screenPos.x = Screen.width - screenPos.x;
        }
        if (Random.value < 0.5f)
        {
            screenPos.y = Screen.height - screenPos.y;
        }
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void SpawnConversationoid(ConversationPiece conversationPiece)
    {
        Rigidbody2D prefabClone = (Rigidbody2D) Instantiate(m_Conversationoid, transform.position, transform.rotation);
        prefabClone.transform.position = GetConversationoidPos();
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
