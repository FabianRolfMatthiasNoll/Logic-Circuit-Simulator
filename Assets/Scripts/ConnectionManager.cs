using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TODO: GateConnection must save connectors instead of gates or better the combination of Gates and Connectors
public class ConnectionManager : MonoBehaviour {
    // Public fields
    public LineRenderer linePrefab;
    public Button activationButton;
    public Button modeSwitchButton;
    public Color activeColor;
    public Color inactiveColor;

    // Private fields
    private List<GateConnection> _gateConnections = new List<GateConnection>();
    private NewGate _sourceGate;
    private NewGate _targetGate;
    private bool connectMode = true;

    private void Start() {
        activationButton.onClick.AddListener(SwitchActivation);
        modeSwitchButton.onClick.AddListener(SwitchMode);
    }

    private void Update() {
        HandleMouseInput();
    }

    private void HandleMouseInput() {
        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.IsConnectionManagerActive) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                HandleHitObject(hit);
            }
        }
    }

    private void HandleHitObject(RaycastHit hit) {
        var connectorIndex = GetConnectorIndex(hit.transform.name);
        if(connectorIndex != -1 && hit.transform.parent.TryGetComponent(out NewGate gate)) {
            if (connectorIndex == 0) {
                HandleSourceGateSelection(gate, connectorIndex);
            } else {
                HandleTargetGateSelection(gate, connectorIndex);
            }
        }
    }

    private void HandleSourceGateSelection(NewGate gate, int connectorIndex) {
        if (_sourceGate != null) {
            _sourceGate.ResetOutline();
            _sourceGate = null;
        } else {
            _sourceGate = gate;
            _sourceGate.SetOutline(true, connectorIndex);
        }
    }

    private void HandleTargetGateSelection(NewGate gate, int connectorIndex) {
        if (_sourceGate == null) {
            ResetAll();
            return;
        }

        if (_targetGate != null) {
            _targetGate.ResetOutline();
            _targetGate = null;
        } else {
            _targetGate = gate;
            _targetGate.SetOutline(true, connectorIndex);
            if (connectMode) {
                ConnectGates(connectorIndex);
                ResetAll();
            } else {
                // Disconnect logic here
                ResetAll();
            }
        }
    }


    private int GetConnectorIndex(string hitName) {
        return hitName switch {
            "Input01" => 1,
            "Input02" => 2,
            "Input03" => 3,
            "Output" => 0,
            _ => -1
        };
    }

    private void ConnectGates(int connectorIndex) {
        if (_targetGate.IsInputFree(connectorIndex) && !ConnectionExists(_sourceGate, _targetGate)) {
            _targetGate.UpdateInput(connectorIndex, _sourceGate, _sourceGate.logicLevel);
            var line = Instantiate(linePrefab);
            line.SetPosition(0, _sourceGate.GetConnectionCoordinates(0));
            line.SetPosition(1, _targetGate.GetConnectionCoordinates(connectorIndex));
            
            _gateConnections.Add(new GateConnection(line, _sourceGate, _targetGate));
        }
    }

    private bool ConnectionExists(NewGate sourceGate, NewGate targetGate) {
        return _gateConnections.Exists(connection => connection.AreGatesConnected(sourceGate, targetGate));
    }

    private void ResetAll() {
        if (_sourceGate != null) {
            _sourceGate.ResetOutline();
            _sourceGate = null;
        }
        if (_targetGate != null) {
            _targetGate.ResetOutline();
            _targetGate = null;
        }
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

        ResetAll();
    }

    private void SwitchMode() {
        connectMode = !connectMode;
        var buttonText = modeSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = connectMode ? "Connecting" : "Disconnecting";
        ResetAll();
    }
}
