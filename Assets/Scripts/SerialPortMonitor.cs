using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class SerialPortMonitor : MonoBehaviour
{
    private SerialPort serialPort;

    public event System.Action<string> OnObjectEnter;
    public event System.Action<string> OnObjectLeave;
    public event System.Action<int> OnRotation;

    private string port;

    void Start()
    {
        port = getSaveSerialPort();
        connectToSerial();
    }

    private void OnDestroy()
    {
        if (serialPort != null)
        {
            Debug.Log("Dispoing serial port.");
            serialPort?.Dispose();
        }
        Debug.Log("SerialPortMonitor destroyed");
    }

    private void connectToSerial()
    {
        try
        {
            if (serialPort != null) serialPort.Dispose();

            Debug.Log("Connecting to " + port + "...");
            serialPort = new SerialPort(port, 115200);
            serialPort.Open();
            Debug.Log("Connected");
        }
        catch(IOException ex)
        {
            Debug.Log("Error while connecting, " + ex.Message);
            serialPort.Dispose();
            serialPort = null;
        }
    }

    private string getSaveSerialPort()
    {
        if(File.Exists("config.txt"))
        {
            return File.ReadAllText("config.txt").Trim();
        }
        else
        {
            var port = SerialPort.GetPortNames().Last();
            File.WriteAllText("config.txt", port);
            return port;
        }
    }

    private void Update()
    {
        monitorSerialPort();
    }

    private void monitorSerialPort()
    {
        if (serialPort == null)
        {
            Debug.Log("No serial port");
            connectToSerial();
            return;
        }

        try
        {
            if (serialPort.IsOpen)
            {
                if (serialPort.BytesToRead > 0)
                {
                    var data = serialPort.ReadLine();
                    processSerialData(data);
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log("Serial disconnect? " + ex.Message);
            serialPort?.Dispose();
            connectToSerial();
        }
    }

    private void processSerialData(string data)
    {
        var tokens = data.Split(":");
        switch(tokens[0])
        {
            case "rot":
                processRotationMessage(tokens);
                break;
            case "object":
                processObjectMessage(tokens);
                break;
        }
    }

    private void processRotationMessage(string[] tokens)
    {
        if (int.TryParse(tokens[1], out int rot))
            OnRotation?.Invoke(rot);
    }

    private void processObjectMessage(string[] tokens)
    {
        if (tokens[1] == "enter")
            OnObjectEnter?.Invoke(tokens[2]);
        else if (tokens[1] == "leave")
            OnObjectLeave?.Invoke(tokens[2]);
    }
}
