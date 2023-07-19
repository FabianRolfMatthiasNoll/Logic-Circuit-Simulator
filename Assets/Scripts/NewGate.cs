using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class NewGate : MonoBehaviour {
    public List<InputConnection> inputConnections = new List<InputConnection>();
    public List<NewGate> outputConnections;
    private List<Outline> _connectorOutlines;

    public bool logicLevel = false;

    private Renderer _outputIndicator;
    public Material materialHigh;
    public Material materialLow;
    public Material materialOff;

    private Outline _outline;

    private void Start() {
        _connectorOutlines = new List<Outline>();
        _connectorOutlines.Add(transform.Find("Output").gameObject.GetComponent<Outline>());
        _connectorOutlines.Add(transform.Find("Input01").gameObject.GetComponent<Outline>());
        _connectorOutlines.Add(transform.Find("Input02").gameObject.GetComponent<Outline>());
        _connectorOutlines.Add(transform.Find("Input03").gameObject.GetComponent<Outline>());

        foreach (var connectorOutline in _connectorOutlines) {
            connectorOutline.enabled = false;
        }

        inputConnections.Add(
            new InputConnection(transform.Find("Input01_Indicator").gameObject.GetComponent<Renderer>(), null));
        inputConnections.Add(
            new InputConnection(transform.Find("Input02_Indicator").gameObject.GetComponent<Renderer>(), null));
        inputConnections.Add(
            new InputConnection(transform.Find("Input03_Indicator").gameObject.GetComponent<Renderer>(), null));

        _outputIndicator = transform.Find("Output_Indicator").gameObject.GetComponent<Renderer>();
    }

    private void Update() {
        SetLogicIndicator();
    }

    public bool IsInputFree(int index) {
        return inputConnections[index].ConnectedGate == null;
    }

    public void UpdateInput(int index, NewGate gate, bool LogicLevel) {
        inputConnections[index].ConnectedGate = gate;
        inputConnections[index].logicLevel = LogicLevel;
    }

    public void RemoveInput(int index) {
        //TODO: Implement
    }

    private void SetLogicIndicator() {
        foreach (var input in inputConnections) {
            if (input.ConnectedGate != null) {
                input.RendererObject.material = input.logicLevel ? materialHigh : materialLow;
            } else {
                input.RendererObject.material = materialOff;
            }
        }

        _outputIndicator.material = logicLevel ? materialHigh : materialLow;
    }

    public Vector3 GetConnectionCoordinates(int index) {
        return GetConnector(index).position;
    }

    public void SetOutline(bool state, int connectorIndex) {
        _connectorOutlines[connectorIndex].enabled = state;
    }

    private Transform GetConnector(int connectorIndex) {
        return connectorIndex switch {
            0 => transform.Find("Output_Indicator").transform,
            1 => transform.Find("Input01_Indicator").transform,
            2 => transform.Find("Input02_Indicator").transform,
            3 => transform.Find("Input03_Indicator").transform,
        };
    }
}