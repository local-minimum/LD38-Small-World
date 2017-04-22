using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGenerator : MonoBehaviour {

	[SerializeField]
	public PersonProfile currentPerson;

	public ConversationPiece GenerateConversation(ConversationQuality quality) {
		var category = currentPerson.GetRandomCategory (quality);
		var text = "hello";
		return new ConversationPiece (category, quality, text);
	}

	public List<ConversationPiece> GenerateConversations(ConversationQuality quality, int numberOfConversions) {
		var conversations = new List<ConversationPiece> ();
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
