using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class vFxContentController : MonoBehaviour, IContentController
{
    [SerializeField] private VisualEffect effect;
    [SerializeField] private float deactivateDelay;

    public event System.Action OnContentHidden;

    private void OnEnable()
    {
        effect.Play();

        
    }

    public void HideContent()
    {
        effect.Stop();
        Invoke("onDelayElapsed", deactivateDelay);
    }

    private void onDelayElapsed()
    {
        OnContentHidden?.Invoke();
    }
}
