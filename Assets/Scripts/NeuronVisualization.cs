using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronVisualization : MonoBehaviour {
    private Image neuronImage = null;

    void Start() {
        neuronImage = GetComponent<Image>();
    }
}