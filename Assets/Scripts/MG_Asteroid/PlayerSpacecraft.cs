﻿using UnityEngine;

public class PlayerSpacecraft : MonoBehaviour
{
    public float m_Speed = 5.0f;
    public float m_RotationSpeed = 100.0F;

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * m_Speed;
        float rotation = Input.GetAxis("Horizontal") * m_RotationSpeed;

        if (translation > 0)
        {
            translation *= Time.deltaTime;
            transform.Translate(translation, 0, 0);
        }     

        rotation *= Time.deltaTime;        
        transform.Rotate(0, 0, rotation);

    }
}

