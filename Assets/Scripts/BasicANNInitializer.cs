using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BasicANNInitializer : MonoBehaviour {
    [Header("ANN Data")]
    [SerializeField] private SOANNData ANNData = null;

    [Header("ANN Settings")]
    [SerializeField] private FeedForwardArtificialNeuralNetwork FFANN = null;
    [SerializeField] private int epochs = 100;
    [SerializeField] private double alpha = 0.05;
    [SerializeField] private int numberOfHiddenLayers = 0;
    [SerializeField] private int numberOfHiddenNeurons = 4;
    [SerializeField] private bool isDelayingExecution = false;
    [SerializeField] private float delay = 1;
    [SerializeField] private int epochSteps = 1;
    private float delayCounter = 0;
    private int outputNeuronFiring = 0;

    [Header("Inputs & Desired Outputs")]
    [SerializeReference] private List<List<double>> inputs = new List<List<double>>();
    [SerializeReference] private List<List<double>> desiredOutputs = new List<List<double>>();

    [Header("Activation Functions")]
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    [Header("Visualization")]
    [SerializeField] private bool isVisualizingANN = false;
    [SerializeField] private ANNVisualizationHandler visualizationHandler = null;

    private bool isCalculating = false;
    private bool isTraining = false;

    void OnValidate() {
        epochs = epochs <= 0 ? 1 : epochs;
        alpha = alpha <= 0 ? 0.05 : alpha;
        numberOfHiddenLayers = numberOfHiddenLayers < 0 ? 0 : numberOfHiddenLayers;
        numberOfHiddenNeurons = numberOfHiddenNeurons < 2 ? 2 : numberOfHiddenNeurons;
        isDelayingExecution = isVisualizingANN ? isDelayingExecution : false;
        delay = isDelayingExecution && delay <= 0 ? 0.5f : delay;
        epochSteps = isDelayingExecution ? epochSteps : epochs;
    }

    public void CreateANN() {
        if (ANNData != null) {
            inputs = new List<List<double>>(ANNData.CreateInputs());
            desiredOutputs = new List<List<double>>(ANNData.CreateDesiredOutputs());
        }
        FFANN = new FeedForwardArtificialNeuralNetwork(epochs, alpha, numberOfHiddenLayers, numberOfHiddenNeurons, inputs, desiredOutputs,
            hiddenLayerActivationFunction, outputLayerActivationFunction, isDelayingExecution);

        if (isVisualizingANN) {
            try {
                visualizationHandler.CreateVisualization(inputs.Count, numberOfHiddenNeurons, numberOfHiddenLayers, desiredOutputs.Count, FFANN.GetLayers());
            } catch (NullReferenceException e) {
                Debug.LogError(e);
            }
        }
        FFANN.Train();
    }

    public void PassData(List<List<double>> inputs, List<List<double>> desiredOutputs, bool passOnToCurrentANN = false) {
        this.inputs = inputs;
        this.desiredOutputs = desiredOutputs;

        if (passOnToCurrentANN) {
            FFANN.PassInputs(inputs);
        }
    }

    public void Run(bool isTraining) {
        this.isTraining = isTraining;
        isCalculating = !isTraining;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            CreateANN();
        }

        if (Input.GetKeyDown(KeyCode.T) && !isCalculating && !isTraining) {
            isCalculating = false;
            isTraining = true;
            FFANN.Reset();
        }

        if (Input.GetKeyDown(KeyCode.C) && !isCalculating && !isTraining) {
            isCalculating = true;
            isTraining = false;
            FFANN.Reset();
        }

        if (isCalculating || isTraining) {
            if (isDelayingExecution) {
                for (int i = 0; i < epochSteps; i++) {
                    if (delayCounter == 0) {
                        FFANN.SetNextLayerAsWorking();
                        delayCounter += Time.deltaTime;
                    } else if (delayCounter < delay) {
                        delayCounter += Time.deltaTime;
                    } else {
                        delayCounter = 0;
                        if (isCalculating) {
                            outputNeuronFiring = FFANN.Run();
                            if (outputNeuronFiring != -1) {
                                isCalculating = false;
                            }
                        } else if (isTraining) {
                            bool isDone = FFANN.Train();
                            if (isDone) {
                                isTraining = false;
                            }
                        }
                    }
                }
            } else {
                delayCounter = 0;
                if (isCalculating) {
                    outputNeuronFiring = FFANN.Run();
                } else if (isTraining) {
                    FFANN.Train();
                }
            }
        }
    }

    public int GetFiringOutputNeuron() { return outputNeuronFiring; }

    public bool GetIsVisualizing() { return isVisualizingANN; }
}