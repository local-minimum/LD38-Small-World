using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LocalMinimum.Collections;

public class PersonProfile : MonoBehaviour {

    public ConversationQuality genderEncoding;

    [SerializeField]
    int nLikes = 2;

    List<ConversationCategory> m_likes;
	public List<ConversationCategory> likes
    {
        get
        {
            if (m_likes == null)
            {
                SelectLikesDislikes();
            }
            return m_likes;
        }
    }

    [SerializeField]
    int nDislikes = 3;

    List<ConversationCategory> m_dislikes;
    public List<ConversationCategory> dislikes
    {
        get
        {
            if (m_likes == null)
            {
                SelectLikesDislikes();
            }
            return m_dislikes;
        }
    }

    void SelectLikesDislikes()
    {
        List<ConversationCategory> availible = new List<ConversationCategory>() {

            ConversationCategory.Family,
            ConversationCategory.Food,
            ConversationCategory.Nostalgy,
            ConversationCategory.Politics,
            ConversationCategory.Sport,
            ConversationCategory.Weather,
            ConversationCategory.Work
        };

        availible = availible.Shuffle(new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup * 100f)));

        m_likes = availible.Where((e, i) => i < nLikes).ToList();
        m_dislikes = availible.Where((e, i) => i >= nLikes).Where((e, i) => i < nDislikes).ToList();
    }

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
    string _firstName;

    public string FullName
    {
        get {
            if (string.IsNullOrEmpty(_name)) {
                LoadNameHistory();
            }
            return _name;
        }
    }

    public string FirstName
    {
        get
        {
            if (string.IsNullOrEmpty(_firstName))
            {
                LoadNameHistory();
            }

            return _firstName;
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
