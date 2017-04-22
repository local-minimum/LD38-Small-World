using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGenerator : MonoBehaviour {

	[SerializeField]
	public PersonProfile currentPerson;

	public Conversation GenerateConversation(ConversationQuality quality) {
		var category = currentPerson.GetRandomCategory (quality);
		var text = "hello";
		return new Conversation (category, quality, text);
	}

	public List<Conversation> GenerateConversations(ConversationQuality quality, int numberOfConversions) {
		var conversations = new List<Conversation> ();
		for (int i = 0; i < numberOfConversions; ++i) {
			conversations.Add(GenerateConversation(quality));
		}
		return conversations;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
