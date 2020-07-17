using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionVisualization : MonoBehaviour {
    [SerializeField] private Image connectionImage = null;

    void Start() {
        if (connectionImage == null) {
            connectionImage = GetComponent<Image>();
        }
    }
    public void SetPosition(Transform start, Transform end) {

    }
}