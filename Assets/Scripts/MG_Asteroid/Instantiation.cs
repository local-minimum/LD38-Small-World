using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour {

    public int m_Difficulty = 3;
    public Rigidbody2D m_SpawnablePrefab;

    private ConversationGenerator m_ConvGenerator;

    private void Start()
    {
        m_ConvGenerator = Room.instance.Conversation;
        if (m_ConvGenerator == null)
        {
            m_ConvGenerator = GetComponent<ConversationGenerator>();
        }

        StartCoroutine(DelayPlay());
    }

    IEnumerator<WaitForSeconds> DelayPlay()
    {
        yield return new WaitForSeconds(0.5f);
        Play();
    }

    public void Play()
    {        
        int goodConvCount = 3;
        int badConvCount = 7;
        
        
        var goodConversations = Room.instance.Conversation.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = Room.instance.Conversation.GenerateConversations(ConversationQuality.Bad, badConvCount);
        
        foreach (var conversationPiece in goodConversations)
        {
            SpawnObject(conversationPiece);
        }

        foreach (var conversationPiece in badConversations)
        {
            SpawnObject(conversationPiece);
        }
    }

    private void SpawnObject(ConversationPiece conversationPiece)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D)Instantiate(m_SpawnablePrefab, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.GetComponent<MiniGameConversationObject>().m_ConversationPiece = conversationPiece;
    }
}
