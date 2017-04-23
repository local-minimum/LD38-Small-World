using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour {

    public int m_Difficulty = 3;
    public Rigidbody2D m_SpawnablePrefab;
    public float m_Timeout = 5.0f;

    private ConversationGenerator m_ConvGenerator;
    private float m_TimeLeft = 0.0f;
    private bool m_Playing = false;

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
                
        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, badConvCount);

        m_TimeLeft = m_Timeout;
        Debug.Log("m_TimeLeft=" + m_TimeLeft);

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
    Transform SpawnParent;
    
    private void SpawnObject(ConversationPiece conversationPiece)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D)Instantiate(m_SpawnablePrefab, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.GetComponent<MiniGameConversationObject>().m_ConversationPiece = conversationPiece;
        prefabClone.transform.SetParent(SpawnParent);
    }

    public void Update()
    {
        if (m_Playing)
        {
            m_TimeLeft -= Time.deltaTime;
            if (m_TimeLeft < 0)
            {
                Debug.Log("Timeout");
                Room.instance.ResponseSilent();
                m_Playing = false;
            }
        }
    }
}
