using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Gate : MonoBehaviour {
    public List<Gate> inputs;
    public List<GateConnection> InputConnections;
    public bool isSelected = false;
    public bool logicLevel = false;
    public Material materialHigh;
    public Material materialLow;
    private Renderer _logicIndicatorRenderer;
    private Vector3 _mouseOffset;
    public abstract bool GetOutput();

    void Start() {
        InputConnections = new List<GateConnection>();
        GameObject logicIndicator = transform.Find("LogicIndicator").gameObject;
        _logicIndicatorRenderer = logicIndicator.GetComponent<Renderer>();
    }

    private void Update() {
        SetOutline();
        logicLevel = GetOutput();
        UpdateLogicIndicator();
    }

    private void SetOutline() {
        Outline outline = transform.gameObject.GetComponent<Outline>();

        if (outline == null) {
            outline = transform.gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.blue;
            outline.OutlineWidth = 14.0f;
        }

        outline.enabled = isSelected;
    }

    public void UpdateLogicIndicator() {
        if (GetOutput()) {
            _logicIndicatorRenderer.material = materialHigh;
        } else {
            _logicIndicatorRenderer.material = materialLow;
        }
    }

    private void OnMouseDown() {
        // Calculate the offset between the gate's position and the mouse position in world space
        _mouseOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag() {
        if (!GameStateManager.Instance.IsConnectionManagerActive) {
            Vector3 newPos = GetMouseWorldPos() + _mouseOffset;
            gameObject.transform.position = newPos;
        }
    }

// Method to get mouse position in world coordinates
    private Vector3 GetMouseWorldPos() {
        Vector3 mousePoint = Input.mousePosition;

        // Assuming your gate is in the X-Z plane and your camera is orthographic
        mousePoint.z = Camera.main.transform.position.y;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}