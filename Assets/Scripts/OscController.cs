using OscJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscController : MonoBehaviour
{
    [SerializeField] private SerialPortMonitor SerialMonitor;

    private OscClient oscClient;

    // Start is called before the first frame update
    void Start()
    {
        oscClient = new OscClient("127.0.0.1", 8000);
        SerialMonitor.OnRotation += SerialMonitor_OnRotation;
        SerialMonitor.OnObjectEnter += SerialMonitor_OnObjectEnter;
        SerialMonitor.OnObjectLeave += SerialMonitor_OnObjectLeave;
    }


    private void OnDestroy()
    {
        SerialMonitor.OnRotation -= SerialMonitor_OnRotation;
        SerialMonitor.OnObjectEnter -= SerialMonitor_OnObjectEnter;
        SerialMonitor.OnObjectLeave -= SerialMonitor_OnObjectLeave;

        oscClient?.Dispose();
    }

    private void SerialMonitor_OnRotation(int rotation)
    {
        oscClient.Send("/rotation", rotation);
    }

    private void SerialMonitor_OnObjectEnter(string obj)
    {
        int id = int.Parse(obj[1].ToString());
        oscClient.Send("/objectEnter", id);
        //oscClient.Send("/objectEnter", obj);
    }

    private void SerialMonitor_OnObjectLeave(string obj)
    {
        oscClient.Send("/objectLeave", obj);
    }
}
