using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ConversationPiece {

    [SerializeField]
	private readonly ConversationCategory category;

    [SerializeField]
    private readonly ConversationQuality quality;

    [SerializeField]
    private readonly string text;

	public ConversationPiece(ConversationCategory category, ConversationQuality quality, string text) {
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
