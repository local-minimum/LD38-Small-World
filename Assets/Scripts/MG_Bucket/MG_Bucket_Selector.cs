using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Bucket_Selector : MonoBehaviour {


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
        MG_Bucket_Item item = other.GetComponent<MG_Bucket_Item>();
        if (item == null || other == selected || !item.speeding || (MG_Bucket_Spawner.instance as MG_Bucket_Spawner).HasSelection)
        {
            return;
        }

        if (selected == null)
        {
            item.speeding = false;
            selected = other;
            m_Piece = item.piece;
            StartCoroutine(AnimateSelection(other.transform));
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

        (MiniGamePlayerBase.instance as MG_Bucket_Spawner).EndGame();
    }


}
