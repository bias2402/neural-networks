using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronVisualization : MonoBehaviour {
    [SerializeField] private Image neuronImage = null;

    void Start() {
        if (neuronImage == null) {
            neuronImage = GetComponent<Image>();
        }
    }
    public void SetPosition() {

    }
}