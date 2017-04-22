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

    public string FullName
    {
        get { return "Hello Kitty"; }
    }

    public string CommonHistory {

        get
        {
            return "Sat behind you in history class in 4th and 5th grade.";
        }
    }

    public Sprite icon;

}
