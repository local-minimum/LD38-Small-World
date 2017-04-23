using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour {

    public Vector2 m_Speed = new Vector2(20, 20);
    
    private Vector2 m_Direction;
    private Vector2 m_Movement;
    private Rigidbody2D m_RigidbodyComponent;

    private void Start()
    {
        m_RigidbodyComponent = GetComponent<Rigidbody2D>();
        m_Direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        m_Movement = new Vector2(
          m_Speed.x * m_Direction.x,
          m_Speed.y * m_Direction.y);

        m_RigidbodyComponent.velocity = m_Movement;
    }
}
