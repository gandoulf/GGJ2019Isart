using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASoundEmitter : MonoBehaviour
{

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float soundWeightDecrementation;
    [SerializeField]
    private float timeBeforeDecrementation;

    //to put back in private
    private float soundWeight = 0;
    public float SoundWeight { get { return soundWeight; } }
    private Coroutine currentCoroutine;

    protected virtual void Start()
    {
        Invoke("DecrementSoundWeight", timeBeforeDecrementation);
        currentCoroutine = null;
    }

    public void ResetSoundWeight()
    {
        soundWeight = 0;
    }

    public virtual void ComputeSoundWeight(float soundValue)
    {
        soundWeight = soundValue;
        if (soundWeight > 1.0f)
            soundWeight = 1.0f;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(DecrementSoundWeight());
    }

    protected virtual void OnSoundOver()
    {

    }

    private IEnumerator DecrementSoundWeight()
    {
        while (soundWeight != 0.0f)
        {
            yield return new WaitForSeconds(timeBeforeDecrementation);
            soundWeight -= soundWeightDecrementation;
            if (soundWeight < 0.0f)
                soundWeight = 0.0f;
        }
        currentCoroutine = null;
        OnSoundOver();
        yield return null;
    }
}
