using System.Collections.Generic;
using UnityEngine;

public class PlayerSpacecraft : MonoBehaviour
{
    public float m_AccelerationForce = 10.0f;
    public float m_RotationForce = 10.0F;

    public float soundThreshold = 0.2f;
    private Rigidbody2D m_RigidBody;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (collided)
        {
            return;
        }

        float rotation = -Input.GetAxis("Horizontal");
        m_RigidBody.AddTorque(rotation * m_RotationForce);
        float acceleration = Input.GetAxis("Vertical");
        if (Mathf.Abs(acceleration) > soundThreshold)
        {
            MiniGameAudioEffector.instance.EmitRandomSound();
        }
        m_RigidBody.AddForce(transform.right * acceleration * m_AccelerationForce);
    }

    bool collided = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.name == "Conversationoid(Clone)")
        {
            collided = true;
            MiniGamePlayerBase.instance.Playing = false;
            StartCoroutine(delayDestroy(collision.gameObject.GetComponent<MiniGameConversationObject>().m_ConversationPiece));
        }
    }

    IEnumerator<WaitForSeconds> delayDestroy(ConversationPiece piece)
    {
        MiniGameAudioEffector.instance.EmitRandomSelectSound();
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Conversationoid!!!!");
        Room.instance.Response(piece);
        

    }
}

