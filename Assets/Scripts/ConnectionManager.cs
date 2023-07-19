using System;
using System.Collections.Generic;
using Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour {
    public LineRenderer linePrefab;

    private List<GateConnection> _gateConnections;

    private NewGate _selectedGate;
    private int connectorIndex;
    public Button activationButton;
    public Button modeSwitchButton;
    public Color activeColor;
    public Color inactiveColor;
    public bool connectMode = true;

    private void Start() {
        _gateConnections = new List<GateConnection>();
        activationButton.onClick.AddListener(SwitchActivation);
        modeSwitchButton.onClick.AddListener(SwitchMode);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.IsConnectionManagerActive) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                //TODO: Check which Connector is clicked, Sort first output then input turn to indexes.
                var hitName = hit.transform.name;

                connectorIndex = hitName switch {
                    "Input01" => 1,
                    "Input02" => 2,
                    "Input03" => 3,
                    "Output" => 0,
                    _ => -1
                };
                if(connectorIndex != -1) {
                    var gateHit = hit.transform.parent;
                    if(gateHit.TryGetComponent(out NewGate gate)) {
                        if (_selectedGate == null) {
                            // Select this gate
                            _selectedGate = gate;
                            gate.SetOutline(true, connectorIndex);
                        } else if (_selectedGate == gate) {
                            // Deselect the gate
                            _selectedGate = null;
                            gate.SetOutline(false, connectorIndex);
                        } else {
                            if (connectMode) {
                                ConnectGates(_selectedGate, gate, connectorIndex);
                            } else {
                                //DisconnectGates(_selectedGate, gate);
                            }
                            // Deselect the previously selected gate
                            _selectedGate.SetOutline(false, connectorIndex);
                            gate.SetOutline(false, connectorIndex);
                            _selectedGate = null;
                        }
                    }
                    connectorIndex = -1;
                }
            }
        }
    }

    private void ConnectGates(NewGate sourceGate, NewGate targetGate, int inputConnector) {
        if (!targetGate.IsInputFree(inputConnector) || IsConnectionExisting(sourceGate, targetGate)) return;

        targetGate.UpdateInput(inputConnector, sourceGate, sourceGate.logicLevel);
        var line = Instantiate(linePrefab);
        line.SetPosition(0, sourceGate.GetConnectionCoordinates(0)); // 0 because its a output
        line.SetPosition(1, sourceGate.GetConnectionCoordinates(inputConnector));
        
        _gateConnections.Add(new GateConnection(line, sourceGate, targetGate));
    }

    private bool IsConnectionExisting(NewGate sourceGate, NewGate targetGate) {
        if (_gateConnections.Count == 0) return false;
        foreach (var connection in _gateConnections) {
            if (connection.AreGatesConnected(sourceGate, targetGate)) return true;
        }
        return false;
    }


    private void SwitchActivation() {
        GameStateManager.Instance.IsConnectionManagerActive = !GameStateManager.Instance.IsConnectionManagerActive;

        var buttonColors = activationButton.colors;
        var newColor = GameStateManager.Instance.IsConnectionManagerActive ? activeColor : inactiveColor;
        buttonColors.normalColor = newColor;
        buttonColors.highlightedColor = newColor;
        buttonColors.pressedColor = Color.Lerp(newColor, Color.black, 0.2f);
        buttonColors.selectedColor = newColor;
        activationButton.colors = buttonColors;

        _selectedGate = null;
    }

    private void SwitchMode() {
        connectMode = !connectMode;
        var buttonText = modeSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = connectMode ? "Connecting" : "Disconnecting";
    }
}