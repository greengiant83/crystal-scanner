using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField] private SerialPortMonitor SerialPort;
    [SerializeField] private Transform CrystalsContainer;
    [SerializeField] private int RevealContentRotationTriggerValue = 1500;
    [SerializeField] private float CrystalActivationDelay;

    private Dictionary<string, Crystal> crystals = new Dictionary<string, Crystal>();
    private Crystal currentCrystal;
    private int? startingRotation;

    void Start()
    {
        //Cursor.visible = false;

        SerialPort.OnObjectEnter += SerialPort_OnObjectEnter;
        SerialPort.OnObjectLeave += SerialPort_OnObjectLeave;
        SerialPort.OnRotation += SerialPort_OnRotation;

        foreach(Transform child in CrystalsContainer)
        {
            crystals.Add(child.gameObject.name.ToLower(), child.GetComponent<Crystal>());
            child.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        SerialPort.OnObjectEnter -= SerialPort_OnObjectEnter;
        SerialPort.OnObjectLeave -= SerialPort_OnObjectLeave;
        SerialPort.OnRotation += SerialPort_OnRotation;
    }

    private void SerialPort_OnObjectEnter(string obj)
    {
        obj = obj.ToLower();
        if (!crystals.ContainsKey(obj)) return;

        hideCurrentCrystal(); //NOTE: I dont really expect this to be needed

        currentCrystal = crystals[obj];
        Invoke("showCrystal", CrystalActivationDelay);
    }

    private void SerialPort_OnObjectLeave(string obj)
    {
        hideCurrentCrystal();
    }

    private void SerialPort_OnRotation(int rotation)
    {
        if(!startingRotation.HasValue) startingRotation = rotation;

        var angle = rotation * 0.1f;
        CrystalsContainer.localRotation = Quaternion.AngleAxis(angle, Vector3.up);

        if (currentCrystal != null && Mathf.Abs(rotation - startingRotation.Value) > RevealContentRotationTriggerValue) currentCrystal.ShowContent();
    }

    private void showCrystal()
    {
        if (currentCrystal == null) return;

        currentCrystal.gameObject.SetActive(true);
        startingRotation = null;
    }

    private void hideCurrentCrystal()
    {
        currentCrystal?.HideCrystal();
        currentCrystal = null;
    }
}
