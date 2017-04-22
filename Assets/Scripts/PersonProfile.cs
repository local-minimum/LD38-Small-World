using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonProfile : MonoBehaviour {

	[SerializeField]
	public List<ConversationCategory> likes;

	[SerializeField]
	public List<ConversationCategory> dislikes;

	public ConversationCategory GetRandomCategory(ConversationQuality quality) {
		switch (quality) {
		case ConversationQuality.Good:
			return likes [Random.Range (0, likes.Count)];
		case ConversationQuality.Bad:
			return dislikes [Random.Range (0, dislikes.Count)];
		}
		throw new UnityException ("Incorrect quality.");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
