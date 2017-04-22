using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour {

    public int m_Difficulty = 3;
    public Rigidbody2D asteroid;
    void Start()
    {
        //Room.instance.Conversation.GenerateConversations();

        Camera cam = Camera.main;
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);

        for (int y = 0; y < 3 * m_Difficulty; y++)
        {
            Vector3 randomScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
            Rigidbody2D asteroidClone = (Rigidbody2D)Instantiate(asteroid, transform.position, transform.rotation);
            asteroidClone.transform.position = randomScreenPosition;
        }
    }
}
