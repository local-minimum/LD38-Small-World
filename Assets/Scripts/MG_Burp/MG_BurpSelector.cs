﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class MG_BurpSelector : Singleton<MG_BurpSelector> {

    
    Collider2D selected = null;
    public bool HasSelected
    {
        get
        {
            return selected != null;
        }
    }

    ConversationPiece m_Piece = null;

    public ConversationPiece SelectedPiece
    {
        get
        {
            return m_Piece;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MG_BurpItem item = other.GetComponent<MG_BurpItem>();
        if (item == null || other == selected)
        {
            return;
        }

        if (selected == null)
        {
            selected = other;
            m_Piece = item.piece;
            MiniGameAudioEffector.instance.EmitRandomSelectSound();
            StartCoroutine(AnimateSelection(other.transform));
        } else
        {
            Destroy(other.gameObject);
        }
    }

    [SerializeField]
    float duration = 0.5f;

    [SerializeField]
    float sizeMultiplier = 5;

    IEnumerator<WaitForSeconds> AnimateSelection(Transform otherT)
    {
        float startTime = Time.timeSinceLevelLoad;
        float progress = 0;
        float startSize = otherT.localScale.x;
        float targetSize = sizeMultiplier * startSize;

        while (progress < 1)
        {
            progress = (Time.timeSinceLevelLoad - startTime) / duration;
            otherT.localScale = Vector2.one * Mathf.Lerp(startSize, targetSize, progress);
            yield return new WaitForSeconds(0.016f);
        }

        (MiniGamePlayerBase.instance as MG_BurpPlayer).EndGame();
    }


}
