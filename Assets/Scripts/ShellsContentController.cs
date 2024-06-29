using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellsContentController : MonoBehaviour, IContentController
{
    [SerializeField] private float revealAnimationSpeed = 1;
    [SerializeField] private AnimationCurve revealCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float hideAnimationSpeed = 1;
    [SerializeField] private AnimationCurve hideCurve;

    public event Action OnContentHidden;

    private Material material;
    private int revealPropertyId;

    private void Awake()
    {
        material = GetComponentInChildren<MeshRenderer>().sharedMaterial;
        revealPropertyId = Shader.PropertyToID("_Reveal");
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(revealAnimation());
    }

    public void HideContent()
    {
        StopAllCoroutines();
        StartCoroutine(hideAnimation());
    }


    private IEnumerator revealAnimation()
    {
        float v = 0;
        while (v < 1)
        {
            v += Time.deltaTime * revealAnimationSpeed;
            v = Mathf.Clamp01(v);

            transform.localScale = Vector3.one * scaleCurve.Evaluate(v);
            //material.SetFloat(revealPropertyId, revealCurve.Evaluate(v));
            material.SetFloat("_Reveal", revealCurve.Evaluate(v));

            yield return null;
        }

        transform.localScale = Vector3.one;
        material.SetFloat(revealPropertyId, 1);
    }

    private IEnumerator hideAnimation()
    {
        float v = 1;
        while (v > 0)
        {
            v -= Time.deltaTime * hideAnimationSpeed;
            v = Mathf.Clamp01(v);

            transform.localScale = Vector3.one * scaleCurve.Evaluate(v);
            material.SetFloat(revealPropertyId, hideCurve.Evaluate(v));

            yield return null;
        }

        material.SetFloat(revealPropertyId, 0);
        OnContentHidden?.Invoke();
    }
}
