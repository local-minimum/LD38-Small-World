﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum.Collections;

public class MG_BurpPlayer : MiniGamePlayerBase {

    [SerializeField]
    Transform spawnParent;

    public float m_Timeout = 5.0f;

    private ConversationGenerator m_ConvGenerator;
    private float m_TimeLeft = 0.0f;


    private void Start()
    {
        if (Room.IsInstanciated)
        {
            m_ConvGenerator = Room.instance.Conversation;
        }

        if (m_ConvGenerator == null)
        {
            m_ConvGenerator = GetComponent<ConversationGenerator>();
        }

        StartCoroutine(DelayPlay());
    }

    IEnumerator<WaitForSeconds> DelayPlay()
    {
        yield return new WaitForSeconds(1f);
        Play(1);
    }

    public override void Play(int difficulty)
    {
        int goodConvCount = 2;
        int badConvCount = 4;

        var goodConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Good, goodConvCount);
        var badConversations = m_ConvGenerator.GenerateConversations(ConversationQuality.Bad, badConvCount);

        m_TimeLeft = m_Timeout;
        Debug.Log("m_TimeLeft=" + m_TimeLeft);
        m_Playing = true;
        StartCoroutine(SpawnAll(goodConversations, badConversations));

    }

    [SerializeField]
    float spawnFreq = 0.2f;

    IEnumerator<WaitForSeconds> SpawnAll(List<ConversationPiece> Goodies, List<ConversationPiece> Baddies)
    {
        Goodies.AddRange(Baddies);
        var All = Goodies.Shuffle(new System.Random(Mathf.RoundToInt(Time.time)));
        foreach (var piece in All)
        {
            Spawn(piece);
            yield return new WaitForSeconds(spawnFreq);
        }
    }


    MG_BurpItem Spawn(ConversationPiece piece)
    {
        MG_BurpItem item = GetFromCategory(piece.Category);
        if (item == null)
        {
            return null;
        }

        MG_BurpItem clone = Instantiate(item, spawnParent, false);
        clone.transform.localPosition = Vector3.zero;
        clone.piece = piece;
        return clone;
    }

    [SerializeField]
    MG_BurpItem family;

    [SerializeField]
    MG_BurpItem food;

    [SerializeField]
    MG_BurpItem memory;

    [SerializeField]
    MG_BurpItem sports;

    [SerializeField]
    MG_BurpItem politics;

    [SerializeField]
    MG_BurpItem weather;

    [SerializeField]
    MG_BurpItem work;


    public MG_BurpItem GetFromCategory(ConversationCategory category)
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

    bool responed = false;

    public void EndGame()
    {
        if (responed)
        {
            return;
        }
        m_Playing = false;
        responed = true;
        ConversationPiece piece = MG_BurpSelector.instance.piece;
        if (piece == null)
        {
            Room.instance.ResponseSilent();
        } else
        {
            Room.instance.Response(piece);
        }
    }

    private bool m_Playing = false;

    public void Update()
    {
        if (m_Playing) { 
            m_TimeLeft -= Time.deltaTime;
            if (m_TimeLeft < 0)
            {
                Debug.Log("Timeout");
                EndGame();
            }
        }
    }
}