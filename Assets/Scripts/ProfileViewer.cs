using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LocalMinimum;

public class ProfileViewer : Singleton<ProfileViewer> {

    [SerializeField]
    Transform parent;

    [SerializeField]
    Transform likes;

    [SerializeField]
    Transform dislikes;

    [SerializeField]
    Text nameField;

    [SerializeField]
    Image icon;

    [SerializeField]
    Text commonHistory;

    [SerializeField]
    ProfileItem familyPrefab;

    [SerializeField]
    ProfileItem foodPrefab;

    [SerializeField]
    ProfileItem memoriesPrefab;

    [SerializeField]
    ProfileItem sportsPrefab;

    [SerializeField]
    ProfileItem politicsPrefab;

    [SerializeField]
    ProfileItem weatherPrefab;

    [SerializeField]
    ProfileItem workPrefab;

    public bool ShowingProfile
    {
        get
        {
            return parent.gameObject.activeSelf;
        }
    }

    public void ShowProfile(PersonProfile profile)
    {
        icon.sprite = profile.icon;
        nameField.text = profile.FullName;
        commonHistory.text = profile.CommonHistory;
        Clear(likes);
        Clear(dislikes);
        AddTo(profile.likes, likes);
        AddTo(profile.dislikes, dislikes);
        parent.gameObject.SetActive(true);
    }


    public void HideProfile()
    {
        parent.gameObject.SetActive(false);
    }

    void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    void AddTo(List<ConversationCategory> items, Transform parent)
    {
        foreach (ConversationCategory item in items)
        {
            Transform t = GetPrefab(item);
            if (t != null)
            {
                t.SetParent(parent);
            }
        }
    }

    Transform GetPrefab(ConversationCategory category)
    {
        switch (category)
        {
            case ConversationCategory.Family:
                return Instantiate(familyPrefab).transform;
            case ConversationCategory.Food:
                return Instantiate(foodPrefab).transform;
            case ConversationCategory.Nostalgy:
                return Instantiate(memoriesPrefab).transform;
            case ConversationCategory.Politics:
                return Instantiate(politicsPrefab).transform;
            case ConversationCategory.Sport:
                return Instantiate(sportsPrefab).transform;
            case ConversationCategory.Weather:
                return Instantiate(weatherPrefab).transform;
            case ConversationCategory.Work:
                return Instantiate(workPrefab).transform;
            default:
                return null;
        }
    }
}
