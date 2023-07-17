using System;
using Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public LineRenderer linePrefab;  // Assign this in the inspector

    private Gate _selectedGate;
    public Button activationButton;
    public Button modeSwitchButton;
    public Color activeColor;
    public Color inactiveColor;
    public bool connectMode = true;

    private void Start()
    {
        activationButton.onClick.AddListener(SwitchActivation);
        modeSwitchButton.onClick.AddListener(SwitchMode);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.IsConnectionManagerActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.TryGetComponent(out Gate gate))
            {
                if (_selectedGate == null)
                {
                    // Select this gate
                    _selectedGate = gate;
                    gate.isSelected = true;
                }
                else if (_selectedGate == gate)
                {
                    // Deselect the gate
                    _selectedGate = null;
                    gate.isSelected = false;
                }
                else
                {
                    if (connectMode)
                    {
                        ConnectGates(_selectedGate, gate);    
                    }
                    else
                    {
                        DisconnectGates(_selectedGate, gate);
                    }
                    

                    // Deselect the previously selected gate
                    _selectedGate.isSelected = false;
                    gate.isSelected = false;
                    _selectedGate = null;
                }
            }
        }
    }

    private void ConnectGates(Gate gate1, Gate gate2)
    {
        if (FindConnection(gate1, gate2) != null || FindConnection(gate2, gate1) != null) return;
        
        gate2.inputs.Add(gate1);

        var line = Instantiate(linePrefab);
        line.SetPosition(0, gate1.transform.position);
        line.SetPosition(1, gate2.transform.position);
        var connection = new GateConnection(line, gate1, gate2);
        gate2.InputConnections.Add(connection);
    }
    
    private void DisconnectGates(Gate gate1, Gate gate2)
    {
        // Find the connection between gate1 and gate2 and destroy it
        GateConnection connection = FindConnection(gate1, gate2);
        if (connection != null)
        {
            // Remove the connection from gate2's input connections
            gate2.InputConnections.Remove(connection);

            Destroy(connection.line.gameObject);
        }
    }

    
    private GateConnection FindConnection(Gate gate1, Gate gate2)
    {
        // Check if any of gate2's input connections are coming from gate1
        foreach (GateConnection connection in gate2.InputConnections)
        {
            if (connection.sourceGate == gate1)
            {
                return connection;
            }
        }

        // If no connection was found, return null
        return null;
    }

    private void SwitchActivation()
    {
        GameStateManager.Instance.IsConnectionManagerActive = !GameStateManager.Instance.IsConnectionManagerActive;
        
        var buttonColors = activationButton.colors;
        var newColor = GameStateManager.Instance.IsConnectionManagerActive ? activeColor : inactiveColor;
        buttonColors.normalColor = newColor;
        buttonColors.highlightedColor = newColor;
        buttonColors.pressedColor = Color.Lerp(newColor, Color.black, 0.2f);
        buttonColors.selectedColor = newColor;
        activationButton.colors = buttonColors;
    }

    private void SwitchMode()
    {
        connectMode = !connectMode;
        var buttonText = modeSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = connectMode ? "Connecting" : "Disconnecting";
    }
}

