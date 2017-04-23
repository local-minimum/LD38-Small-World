using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersonProfile : MonoBehaviour {

    public ConversationQuality genderEncoding;

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

    string nameHistory;
    string _name;
    string _history;

    public string FullName
    {
        get {
            if (string.IsNullOrEmpty(_name)) {
                LoadNameHistory();
            }
            return _name;
        }
    }

    public string CommonHistory {

        get
        {
            if(string.IsNullOrEmpty(_history))
            {
                LoadNameHistory();        
            }
            return _history;
        }
    }

    void LoadNameHistory()
    {
        nameHistory = Room.instance.Conversation.GenerateConversation(ConversationCategory.History, genderEncoding).Text;
        string[] values = nameHistory.Split('|').Select(e => e.Trim()).Take(2).ToArray();
        _name = values[0];
        _history = values[1];
    }

    public Sprite icon;

}
