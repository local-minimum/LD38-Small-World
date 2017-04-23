using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rant;
using Rant.Resources;
using System.IO;
using System;

public class ConversationGenerator : MonoBehaviour {

	[SerializeField]
	public PersonProfile currentPerson;

	private RantEngine rant;

	private Dictionary<ConversationCategory, Dictionary<ConversationQuality, List<RantProgram>>> conversationMap = 
		new Dictionary<ConversationCategory, Dictionary<ConversationQuality, List<RantProgram>>> ();

	public ConversationPiece GenerateConversation(ConversationQuality quality) {
		var category = currentPerson.GetRandomCategory (quality);
		return GenerateConversation (category, quality);
	}

    public ConversationPiece GenerateConversation(ConversationQuality qualityCategory, ConversationQuality asQuality)
    {
        var category = currentPerson.GetRandomCategory(qualityCategory);
        return GenerateConversation(category, asQuality);
    }

    public ConversationPiece GenerateConversation(ConversationCategory category)
    {
		return GenerateConversation (category, ConversationQuality.Good);
    }

	public ConversationPiece GenerateConversation(ConversationCategory category, ConversationQuality quality) {
		var list = conversationMap [category] [quality];

		var text = rant.Do(list[UnityEngine.Random.Range(0, list.Count)]);

		return new ConversationPiece (category, quality, text);
	}

    public List<ConversationPiece> GenerateConversations(ConversationQuality quality, int numberOfConversions) {
		var conversations = new List<ConversationPiece> ();
		for (int i = 0; i < numberOfConversions; ++i) {
			conversations.Add(GenerateConversation(quality));
		}
		return conversations;
	}

    public List<ConversationPiece> GenerateConversations(ConversationQuality quality, ConversationQuality asQuality, int numberOfConversions)
    {
        var conversations = new List<ConversationPiece>();
        for (int i = 0; i < numberOfConversions; ++i)
        {
            conversations.Add(GenerateConversation(quality, asQuality));
        }
        return conversations;
    }

    public List<ConversationPiece> GenerateConversationsAsGood(ConversationQuality quality, int numberOfConversions)
    {
        var conversations = new List<ConversationPiece>();
        for (int i = 0; i < numberOfConversions; ++i)
        {
            conversations.Add(GenerateConversation(quality, ConversationQuality.Good));
        }
        return conversations;
    }

    // Use this for initialization
    void Start () {		
		rant = new RantEngine();
		var asset = Resources.Load("Rantionary-3.0.17") as TextAsset;
		var stream = new MemoryStream(asset.bytes);
		var package = RantPackage.Load(stream);
		rant.LoadPackage (package);

		foreach (ConversationCategory category in Enum.GetValues(typeof(ConversationCategory))) {
			foreach (ConversationQuality quality in Enum.GetValues(typeof(ConversationQuality))) {
				var resourceName = category.ToString () + "_" + quality.ToString ();
				var conversations = Resources.Load (resourceName) as TextAsset;
				List<RantProgram> lines = new List<RantProgram> ();
				using (StringReader sr = new StringReader(conversations.text)) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						lines.Add (RantProgram.CompileString(line));
					}
				}
				if (!conversationMap.ContainsKey (category)) {
					conversationMap.Add (category, new Dictionary<ConversationQuality, List<RantProgram>> ());
				}
				var qualityDict = conversationMap [category];
				qualityDict.Add (quality, lines);

				foreach (var line in lines) {
					Debug.Log (rant.Do (line));
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
