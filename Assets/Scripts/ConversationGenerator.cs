using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rant;
using Rant.Resources;
using System.IO;

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
		var rant = new RantEngine();
		var asset = Resources.Load("Rantionary-3.0.17") as TextAsset;
		var stream = new MemoryStream(asset.bytes);
		var package = RantPackage.Load(stream);
		rant.LoadPackage (package);
		var pgm = RantProgram.CompileString(@"<name-male> likes to <verb-transitive> <noun.pl> with <pro.dposs-male> pet <noun-animal> on <noun.pl  -dayofweek>.");
		// Run the program
		var output = rant.Do(pgm);
		Debug.Log (output);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
