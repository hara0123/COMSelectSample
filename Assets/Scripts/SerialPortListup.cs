using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;

public class SerialPortListup : MonoBehaviour
{
    public int portNum { get; private set; }
    public string[] portName { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        portName = SerialPort.GetPortNames();
        portNum = portName.Length;
    }
}
