using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Gate : MonoBehaviour
{
    public List<Gate> inputs;

    public bool isSelected = false;
    
    private RaycastHit _raycastHit;
    public abstract bool GetOutput();

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _raycastHit;
        
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out _raycastHit)) 
        {
            Transform _selection = _raycastHit.transform;

            if (_selection.CompareTag("Selectable"))
            {
                Outline outline = _selection.gameObject.GetComponent<Outline>();

                if (outline == null)
                {
                    outline = _selection.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.blue;
                    outline.OutlineWidth = 14.0f;
                }

                // Toggle outline enabled/disabled on each click
                isSelected = !isSelected;
                outline.enabled = isSelected;
            }
        }
    }
}
