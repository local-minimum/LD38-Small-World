using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneSequencer : MonoBehaviour {

    [SerializeField]
    GameObject[] prefabMouths;

    [SerializeField]
    string startScene;

    [SerializeField]
    float preTime = 1f;

    [SerializeField]
    float spawnSpeed = 0.016f;

    [SerializeField]
    float duration = 15;

    [SerializeField]
    AnimationCurve sizeOverTimeMin;

    [SerializeField]
    float maxSpawn = 100;

    [SerializeField]
    float maxRotation = 15;

    [SerializeField]
    AnimationCurve sizeOverTimeMax;

    [SerializeField]
    GameObject gameOverText;

    void Start () {
        StartCoroutine(AnimEnd());
	}


    IEnumerator<WaitForSeconds> AnimEnd()
    {
        Transform spawn = GetSpawn();
        spawn.position = Vector3.zero;
        yield return new WaitForSeconds(preTime);
        float startTime = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - startTime < duration && maxSpawn > 0)
        {
            float progress = (Time.timeSinceLevelLoad - startTime) / duration;
            MiniGameAudioEffector.instance.EmitRandomSound();
            spawn = GetSpawn();
            SetSize(spawn, progress);
            SetRotation(spawn);
            SetPosition(spawn, progress);
            maxSpawn--;
            yield return new WaitForSeconds(spawnSpeed);
        }

        gameOverText.SetActive(true);

        while (Time.timeSinceLevelLoad - startTime < duration)
        {
            MiniGameAudioEffector.instance.EmitRandomSound();
            yield return new WaitForSeconds(spawnSpeed);
        }
        SceneManager.LoadScene(startScene);
    }

    Transform GetSpawn()
    {
        return Instantiate(prefabMouths[Random.Range(0, prefabMouths.Length)], transform).transform;
    }

    void SetSize(Transform t, float progress)
    {
        float scale = Random.Range(sizeOverTimeMin.Evaluate(progress), sizeOverTimeMax.Evaluate(progress));
        t.localScale = new Vector3((Random.value < 0.5f ? 1 : -1) * scale, scale, 1);
    }

    void SetRotation(Transform t)
    {
        t.Rotate(transform.forward, Random.Range(-maxRotation, maxRotation));
    }

    void SetPosition(Transform t, float progress)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
        pos.z = progress;
        t.position = pos;
    }
}
