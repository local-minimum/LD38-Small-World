using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapHandler : MonoBehaviour {
    
    Renderer[] m_Renderers;

    bool m_IsWrappingX = false;
    bool m_IsWrappingY = false;
    
    float m_ScreenWidth;
    float m_ScreenHeight;

    void Start()
    {
        m_Renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        foreach (var renderer in m_Renderers)
        {
            if (renderer.isVisible)
            {
                m_IsWrappingX = false;
                m_IsWrappingY = false;
                return;
            }
        }

        if (m_IsWrappingX && m_IsWrappingY)
        {
            return;
        }

        Camera cam = Camera.main;
        Vector3 newPosition = transform.position;

        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
        
        if (!m_IsWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            m_IsWrappingX = true;
        }
        
        if (!m_IsWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            m_IsWrappingY = true;
        }
        
        transform.position = newPosition;
    }      
}
