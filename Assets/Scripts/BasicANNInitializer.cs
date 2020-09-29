﻿using System;
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
    private int outputNeuronFiring = 0;
    private bool isWorking = false;
    private float workingCounter = 0;

    [Header("Inputs & Desired Outputs")]
    [SerializeReference] private List<List<double>> inputs = new List<List<double>>();
    [SerializeReference] private List<List<double>> desiredOutputs = new List<List<double>>();

    [Header("Activation Functions")]
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    [Header("Multi-frame Training (hover 'epochSteps')")]
    [Tooltip("This will split the epochs into multiple steps (frames) during training: " +
        "1000 epochs, 100 per step = 10 steps/frames of 100 epochs. If the last step exceeds the number " +
        "of epochs, it will automatically stop: 1000 epochs, 800 per step = 1 frame " +
        "of 800 epochs, 1 frame of 200 epochs")]
    [SerializeField] private int epochSteps = 1;

    [Header("Visualization")]
    [SerializeField] private bool isVisualizingANN = false;
    [SerializeField] private ANNVisualizationHandler visualizationHandler = null;
    [Tooltip("Activating this delay will make the ANN ignore the Multi-frame settings, since this " +
        "will visualize each epoch!")]
    [SerializeField] private bool isDelayingExecution = false;
    [SerializeField] private float delay = 1;
    private float delayCounter = 0;

    private bool isCalculating = false;
    private bool isTraining = false;
    private DateTime start;

    void OnValidate() {
        epochs = epochs <= 0 ? 1 : epochs;
        alpha = alpha <= 0 ? 0.05 : alpha;
        numberOfHiddenLayers = numberOfHiddenLayers < 0 ? 0 : numberOfHiddenLayers;
        numberOfHiddenNeurons = numberOfHiddenNeurons < 2 ? 2 : numberOfHiddenNeurons;
        isDelayingExecution = isVisualizingANN ? isDelayingExecution : false;
        delay = isDelayingExecution && delay <= 0 ? 0.5f : delay;
        epochSteps = epochSteps < epochs && epochSteps > 0 ? epochSteps : 0;
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
    }

    public void PassData(List<List<double>> inputs, List<List<double>> desiredOutputs, bool passOnToCurrentANN = false) {
        this.inputs = inputs;
        this.desiredOutputs = desiredOutputs;

        if (passOnToCurrentANN) {
            FFANN.PassInputs(inputs);
        }
    }

    public void Run(bool isTraining) {
        start = DateTime.Now;
        this.isTraining = isTraining;
        isCalculating = !isTraining;
    }

    void Update() {
        if (isCalculating || isTraining) {
            isWorking = true;
            if (isDelayingExecution) {
                if (delayCounter == 0) {
                    FFANN.SetNextLayerAsWorking();
                    delayCounter += Time.deltaTime;
                } else if (delayCounter < delay) {
                    delayCounter += Time.deltaTime;
                } else {
                    delayCounter = 0;
                    if (isCalculating) {
                        outputNeuronFiring = FFANN.Run();
                        if (outputNeuronFiring != -1) isCalculating = false;
                    } else if (isTraining) {
                        bool isDone = FFANN.Train();
                        if (isDone) isTraining = false;
                    }
                }
            } else {
                if (isCalculating) {
                    outputNeuronFiring = FFANN.Run();
                    Debug.Log("Neuron Firing: " + outputNeuronFiring);
                    if (outputNeuronFiring != -1) isCalculating = false;
                } else if (isTraining) {
                    bool isDone = FFANN.Train(epochSteps);
                    if (isDone) {
                        isTraining = false;
                        Debug.Log("Total runs (epocs x inputs): " + (epochs * inputs[0].Count));
                        Debug.Log("Time spent training: " + (DateTime.Now.TimeOfDay - start.TimeOfDay) + "min");
                    }
                }
            }
        } else {
            if (isWorking) {
                workingCounter += Time.deltaTime;
                if (workingCounter > 0.01f) {
                    workingCounter = 0;
                    isWorking = false;
                }
            }
        }
    }

    public int GetFiringOutputNeuron() { return outputNeuronFiring; }

    public bool GetIsVisualizing() { return isVisualizingANN; }

    public bool IsReadyForRun() { return !isWorking; }
}