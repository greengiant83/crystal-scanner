using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private float showAnimationSpeed = 1;
    [SerializeField] private AnimationCurve showAnimationCurve;
    [SerializeField] private float hideAnimationSpeed = 1;
    [SerializeField] private float explodeAnimationSpeed = 1;

    private SkinnedMeshRenderer crystalMesh;
    private float showBlendValue = 1;
    private IContentController contentController;

    private void Awake()
    {
        crystalMesh = GetComponentInChildren<SkinnedMeshRenderer>(true);
        contentController = content.GetComponent<IContentController>();
        if(contentController != null) contentController.OnContentHidden += ContentController_OnContentHidden;
        content.SetActive(false);
    }

    private void OnDestroy()
    {
        if (contentController != null) contentController.OnContentHidden -= ContentController_OnContentHidden;
    }

    private void OnEnable()
    {
        ShowCrystal();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void ContentController_OnContentHidden()
    {
        content.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowCrystal()
    {
        if (!gameObject.activeSelf) return;

        crystalMesh.gameObject.SetActive(true);
        content.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(showCrystalAnimation());
    }

    public void HideCrystal()
    {
        StopAllCoroutines();
        if (!gameObject.activeSelf) return;

        if (crystalMesh.gameObject.activeSelf)
        {
            StartCoroutine(hideCrystalAnimation());
        }
        else
        {
            if (contentController != null)
            {
                contentController.HideContent();
            }
            else
            {
                content.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ShowContent()
    {
        StopAllCoroutines();
        StartCoroutine(explodeCrystalAnimation());
    }

    private IEnumerator showCrystalAnimation()
    {
        crystalMesh.SetBlendShapeWeight(1, 0); //Reset Explosion Blend

        while (showBlendValue > 0)
        {
            showBlendValue -= Time.deltaTime * showAnimationSpeed;
            showBlendValue = Mathf.Clamp01(showBlendValue);
            
            crystalMesh.SetBlendShapeWeight(0, showAnimationCurve.Evaluate(showBlendValue) * 100);

            yield return null;
        }

        crystalMesh.SetBlendShapeWeight(0, 0);
    }

    private IEnumerator hideCrystalAnimation()
    {
        crystalMesh.SetBlendShapeWeight(1, 0); //Reset Explosion Blend

        while (showBlendValue < 1)
        {
            showBlendValue += Time.deltaTime * hideAnimationSpeed;
            showBlendValue = Mathf.Clamp01(showBlendValue);

            crystalMesh.SetBlendShapeWeight(0, showBlendValue * 100);

            yield return null;
        }

        crystalMesh.SetBlendShapeWeight(0, 100);
        gameObject.SetActive(false);
    }

    private IEnumerator explodeCrystalAnimation()
    {
        crystalMesh.SetBlendShapeWeight(0, 0); //Ensure reveal blend is in proper state
        content.gameObject.SetActive(true);

        while (showBlendValue < 1)
        {
            showBlendValue += Time.deltaTime * explodeAnimationSpeed;
            showBlendValue = Mathf.Clamp01(showBlendValue);

            crystalMesh.SetBlendShapeWeight(1, showBlendValue * 100);

            yield return null;
        }

        crystalMesh.gameObject.SetActive(false);
    }
}
