using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGAsteroidPlayer : MiniGamePlayerBase
{    
    public Rigidbody2D m_Asteroid;
    public Rigidbody2D m_Player;
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
        Play(3);
    }

    override public void Play(int difficulty)
    {
        int goodConvCount = 3;
        int badConvCount = 7;
                
        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, badConvCount);

        m_TimeLeft = m_Timeout;
        Debug.Log("m_TimeLeft=" + m_TimeLeft);

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


    private void SpawnPlayer()
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D clone = (Rigidbody2D)Instantiate(m_Player, transform.position, transform.rotation);
        clone.transform.position = randomScreenPosition;
        clone.transform.SetParent(Asteroids);
    }

    private void SpawnObject(Rigidbody2D rigidbody)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D)Instantiate(rigidbody, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.transform.SetParent(Asteroids);
    }

    private void SpawnConversationoid(ConversationPiece conversationPiece)
    {
        Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        Rigidbody2D prefabClone = (Rigidbody2D)Instantiate(m_Asteroid, transform.position, transform.rotation);
        prefabClone.transform.position = randomScreenPosition;
        prefabClone.transform.SetParent(Asteroids);
    }

    public void Update()
    {
        if (m_Playing)
        {
            m_TimeLeft -= Time.deltaTime;
            if (m_TimeLeft < 0)
            {
                Room.instance.ResponseSilent();
                m_Playing = false;
            }
        }
    }
}
