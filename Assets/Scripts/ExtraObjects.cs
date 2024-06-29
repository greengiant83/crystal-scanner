using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] extraObjects;

    private void OnEnable()
    {
        if (extraObjects != null)
        {
            foreach (var extraObject in extraObjects)
            {
                extraObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (extraObjects != null)
        {
            foreach (var extraObject in extraObjects)
            {
                extraObject.SetActive(false);
            }
        }
    }
}
