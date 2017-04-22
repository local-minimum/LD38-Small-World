using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation {

	private readonly ConversationCategory category;
	private readonly ConversationQuality quality;
	private readonly string text;

	public Conversation(ConversationCategory category, ConversationQuality quality, string text) {
		this.category = category;
		this.quality = quality;
		this.text = text;
	}

	public ConversationCategory Category {
		get {
			return this.category;
		}
	}

	public ConversationQuality Quality {
		get {
			return this.quality;
		}
	}

	public string Text {
		get {
			return this.text;
		}
	}
}
