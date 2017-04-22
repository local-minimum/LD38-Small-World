using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour {

    public Vector2 m_Speed = new Vector2(1, 1);
    
    private Vector2 m_Direction;
    private Vector2 m_Movement;
    private Rigidbody2D m_RigidbodyComponent;

    private void Start()
    {
        m_RigidbodyComponent = GetComponent<Rigidbody2D>();
        m_Direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        /*
        var conversationPiece = GetComponent<MiniGameConversationObject>().m_ConversationPiece;

        if (conversationPiece != null)
        {
            var sprite = GetComponentInChildren<Sprite>();
            sprite = DialogueDisplayer.instance.GetSpriteFromCategory(conversationPiece.Category);
        }*/
    } 
    
    void Update()
    {
        m_Movement = new Vector2(
          m_Speed.x * m_Direction.x,
          m_Speed.y * m_Direction.y);
    }

    void FixedUpdate()
    {
        m_RigidbodyComponent.AddForce(m_Movement);
    }
}
