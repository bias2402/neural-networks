using System;
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
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    [SerializeField] private FeedForwardArtificialNeuralNetwork FFANN = null;

    [SerializeField] private bool doVisualizeANN = false;
    [SerializeField] private ANNVisualizationHandler visualizationHandler = null;

    void OnValidate() {
        epochs = epochs <= 0 ? 1 : epochs;
        alpha = alpha <= 0 ? 0.05 : alpha;
        numberOfHiddenLayers = numberOfHiddenLayers < 0 ? 0 : numberOfHiddenLayers;
        numberOfHiddenNeurons = numberOfHiddenNeurons < 2 ? 2 : numberOfHiddenNeurons;
        if (inputs.Count <= 0) {
            Debug.LogError("The network need inputs, preferbly at least 2!");
        } else if (inputs.Count < 2) {
            Debug.LogWarning("Having less than two inputs is ineffective for the network.");
        }
        if (desiredOutputs.Count <= 0) {
            Debug.LogError("Desired outputs are necessary for training the network!");
        }
    }
    void Start() {
        FFANN = new FeedForwardArtificialNeuralNetwork(epochs, alpha, numberOfHiddenLayers, numberOfHiddenNeurons, inputs, desiredOutputs, hiddenLayerActivationFunction, outputLayerActivationFunction); ;
        if (doVisualizeANN) {
            try {
                visualizationHandler.CreateVisualization(inputs.Count, numberOfHiddenNeurons, numberOfHiddenLayers, desiredOutputs.Count, FFANN.layers);
            } catch (NullReferenceException e) {
                Debug.LogError(e);
            }
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            FFANN.Train();
        }
    }
}