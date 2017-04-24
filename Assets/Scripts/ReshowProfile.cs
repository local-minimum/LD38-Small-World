using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReshowProfile : MonoBehaviour {

    [SerializeField]
    float reshowDuration = 10;

    bool manualShowing = false;

    public void Activate()
    {

        if (!manualShowing && !ProfileViewer.instance.ShowingProfile)
        {
            ProfileViewer.instance.ShowProfile(Room.instance.Conversation.currentPerson);
            StartCoroutine(DelayClose());
        }
    }

    IEnumerator<WaitForSeconds> DelayClose()
    {
        manualShowing = true;
        yield return new WaitForSeconds(reshowDuration);
        if (!ProfileViewer.instance.ShowingProfile)
        {
            ProfileViewer.instance.HideProfile();
        }
        manualShowing = false;
    }
}
