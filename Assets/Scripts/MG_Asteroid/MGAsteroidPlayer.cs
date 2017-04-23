using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGAsteroidPlayer : MiniGamePlayerBase
{    
    public Rigidbody2D m_Asteroid;
    public Rigidbody2D m_Player;

    override public void Play(int difficulty)
    {
        int goodConvCount = 3;
        int badConvCount = 7;
                
        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, badConvCount);

        m_TimeLeft = m_Timeout;
        Debug.Log("m_TimeLeft=" + m_TimeLeft);

        SpawnPlayer();

        foreach (var conversationPiece in goodConversations)
        {
            SpawnObject(conversationPiece);
        }

        foreach (var conversationPiece in badConversations)
        {
            SpawnObject(conversationPiece);
        }
        m_Playing = true;
    }

    [SerializeField]
    Transform Asteroids;


    private void SpawnPlayer()
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D clone = (Rigidbody2D)Instantiate(m_Player, transform.position, transform.rotation);
        clone.transform.position = randomScreenPosition;
        clone.transform.SetParent(Asteroids);
    }

    private void SpawnObject(ConversationPiece conversationPiece)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D)Instantiate(m_Asteroid, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.GetComponent<MiniGameConversationObject>().m_ConversationPiece = conversationPiece;
        prefabClone.transform.SetParent(Asteroids);
    }   

    public override void EndGame()
    {
        m_Playing = false;
        Room.instance.ResponseSilent();
    }
}
