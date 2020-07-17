using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReader : MonoBehaviour {
    [Header("ANN Settings")]
    [SerializeField] int epochs = 100;
    [SerializeField] double alpha = 0.05;
    [SerializeField] int numberOfHiddenLayers = 0;
    [SerializeField] int numberOfHiddenNeurons = 4;
    [Header("Inputs & Desired Outputs")]
    [SerializeField] private List<double> inputs = new List<double>();
    [SerializeField] private List<double> desiredOutputs = new List<double>();
    [Header("Activation Functions")]
    [SerializeField] private ActivationFunctions inputLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    [SerializeField] private FeedForwardArtificialNeuralNetwork FFANN = null;

    void Start() {
        FFANN = new FeedForwardArtificialNeuralNetwork(epochs, alpha, numberOfHiddenLayers, numberOfHiddenNeurons, inputs, desiredOutputs, inputLayerActivationFunction, hiddenLayerActivationFunction, outputLayerActivationFunction); ;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            FFANN.Train();
        }
    }
}