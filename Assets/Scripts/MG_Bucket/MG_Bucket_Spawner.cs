using System.Collections.Generic;
using UnityEngine;
using LocalMinimum.Collections;

public class MG_Bucket_Spawner : MiniGamePlayerBase {

    [SerializeField]
    Transform[] spawnParents;

    [SerializeField]
    MG_Bucket_Selector[] selectors;

    public bool HasSelection
    {
        get
        {
            for (int i = 0; i < selectors.Length; i++)
            {
                if (selectors[i].HasSelected)
                {
                    return true;
                }
            }
            return false;   

        }
    }

    public override void EndGame()
    {
        if (!m_Playing)
        {
            return;
        }
        m_Playing = false;

        ConversationPiece piece = null;
        foreach (var selector in selectors)
        {
            if (selector.HasSelected)
            {
                piece = selector.SelectedPiece;
                break;
            }
        }

        if (piece == null)
        {
            Room.instance.ResponseSilent();
        }
        else
        {
            Debug.Log("Bucket selected " + piece.Category);
            Room.instance.Response(piece);
        }
    }

    public override void Play(int difficulty)
    {
        Debug.Log("Play " + m_ConvGenerator);
        int goodConvCount = 2;
        int badConvCount = 8;

        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, ConversationQuality.Good, badConvCount);

        MG_Bucket_Hand[] hands = FindObjectsOfType<MG_Bucket_Hand>().Shuffle(new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup * 100)));
        for (int i=0; i<hands.Length; i++)
        {
            if (i == 0)
            {
                hands[i].HandOn();
            } else
            {
                hands[i].HandOff();
            }
        }
        m_TimeLeft = m_Timeout;
        Debug.Log("m_TimeLeft=" + m_TimeLeft);
        m_Playing = true;
        StartCoroutine(SpawnAll(goodConversations, badConversations));
    }

    [SerializeField]
    float dualDropProb = 0.25f;

    [SerializeField]
    float spawnFreq = 0.4f;

    IEnumerator<WaitForSeconds> SpawnAll(List<ConversationPiece> Goodies, List<ConversationPiece> Baddies)
    {
        Goodies.AddRange(Baddies);
        var All = Goodies.Shuffle(new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup * 100f)));
        foreach (var piece in All)
        {
            int parent = -1;
            if (Random.value < dualDropProb)
            {
                Spawn(piece, -1, out parent);
                yield return new WaitForSeconds(spawnFreq * 0.5f);
            }
            Spawn(piece, parent, out parent);
            yield return new WaitForSeconds(spawnFreq);
        }
    }


    MG_Bucket_Item Spawn(ConversationPiece piece, int notParent, out int parent)
    {
        MG_Bucket_Item item = GetFromCategory(piece.Category);
        if (item == null)
        {
            parent = -1;
            return null;
        }
        if (notParent < 0)
        {
            parent = Random.Range(0, spawnParents.Length);
        } else
        {
            int tmp = Random.Range(0, spawnParents.Length - 1);
            if (tmp == notParent)
            {
                tmp++;
            }
            parent = tmp;
        }
        MG_Bucket_Item clone = Instantiate(item, spawnParents[parent], false);
        clone.transform.localPosition = Vector3.zero;
        clone.piece = piece;
        return clone;
    }

    [SerializeField]
    MG_Bucket_Item family;

    [SerializeField]
    MG_Bucket_Item food;

    [SerializeField]
    MG_Bucket_Item memory;

    [SerializeField]
    MG_Bucket_Item sports;

    [SerializeField]
    MG_Bucket_Item politics;

    [SerializeField]
    MG_Bucket_Item weather;

    [SerializeField]
    MG_Bucket_Item work;


    public MG_Bucket_Item GetFromCategory(ConversationCategory category)
    {
        switch (category)
        {
            case ConversationCategory.Family:
                return family;
            case ConversationCategory.Food:
                return food;
            case ConversationCategory.Nostalgy:
                return memory;
            case ConversationCategory.Politics:
                return politics;
            case ConversationCategory.Sport:
                return sports;
            case ConversationCategory.Weather:
                return weather;
            case ConversationCategory.Work:
                return work;
            default:
                return null;
        }
    }
}
