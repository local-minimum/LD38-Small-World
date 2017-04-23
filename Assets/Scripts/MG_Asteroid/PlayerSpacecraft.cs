using UnityEngine;

public class PlayerSpacecraft : MonoBehaviour
{
    public float m_AccelerationForce = 10.0f;
    public float m_RotationForce = 10.0F;

    private Rigidbody2D m_RigidBody;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float rotation = -Input.GetAxis("Horizontal");
        m_RigidBody.AddTorque(rotation * m_RotationForce);
        float acceleration = Input.GetAxis("Vertical");
        m_RigidBody.AddForce(transform.right * acceleration * m_AccelerationForce);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Conversationoid")
        {
            //MGAsteroidPlayer.instance.PlayerGotConversationPiece(collision.gameObject.GetComponent<ConversationPiece>()); 
            Room.instance.Response(collision.gameObject.GetComponent<ConversationPiece>());
            Destroy(collision.gameObject);
        }
    }
}

