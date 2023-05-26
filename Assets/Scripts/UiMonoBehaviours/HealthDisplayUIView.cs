using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthDisplayUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = _mainCamera;
    }

    public void SetHealth(int health)
    {
        _healthText.text = $"{health}";
    }

    void Update()
    {
        var directionToCamera = (Vector3)transform.position - _mainCamera.transform.position;
        var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
        transform.rotation = rotationToCamera;
    }
}
