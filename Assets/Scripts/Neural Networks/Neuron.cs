using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Neuron {
    [Header("Neuron Settings")]
    [SerializeField] private bool isInputNeuron = false;
    [SerializeField] private double inputValue = 0;
    public double bias = 0;
    [SerializeReference] public List<double> weights = new List<double>();
    [SerializeReference] public List<double> inputs = new List<double>();
    public double output = 0;
    public double desiredOutput = 0;
    public double errorGradient = 0;
    [SerializeField] private string name = "";

    [Header("Visualization")]
    private bool isVisualizing = false;
    private NeuronVisualization neuronVisualization = null;

    private ActivationFunctions activationFunction = ActivationFunctions.Sigmoid;

    public Neuron(List<double> inputs) {
        if (inputs.Count <= 0) {
            Debug.LogError("A neuron must have a positive number of inputs!");
            return;
        }
        bias = UnityEngine.Random.Range(-1f, 1f);
        this.inputs = inputs;
        for (int i = 0; i < inputs.Count; i++) {
            weights.Add(UnityEngine.Random.Range(-1f, 1f));
        }
    }

    public Neuron() => isInputNeuron = true;

    public Neuron(double inputValue) {
        this.inputValue = inputValue;
    }

    public void SetInputValueForInputNeuron(double inputValue) => this.inputValue = inputValue;

    public void SetDesiredOutputValueForOutputNeuron(double desiredOutputValue) => desiredOutput = desiredOutputValue;

    public void CalculateOutput() {
        if (isInputNeuron) {
            output = inputValue;
            inputValue = 0;

            if (isVisualizing) {
                neuronVisualization.UpdateNeuronImage((float)output);
                neuronVisualization.UpdateConnection((float)output);
            }
            return;
        }

        double value = 0;
        if (inputs.Count != weights.Count) Debug.LogError("Inputs and weights must be the same amount! Inputs: " + 
            inputs.Count + ", Weights: " + weights.Count + ", Neuron: " + name);
        for (int i = 0; i < inputs.Count; i++) {
            value += inputs[i] * weights[i];
        }
        value -= bias;
        output = ActivationFunctionHandler.TriggerActivationFunction(activationFunction, value);

        if (isVisualizing) {
            neuronVisualization.UpdateNeuronImage((float)output);
            neuronVisualization.UpdateConnection((float)output);
        }
    }

    public void SetupNeuronVisualization(NeuronVisualization neuronVisualization) {
        this.neuronVisualization = neuronVisualization;
        if (neuronVisualization != null) isVisualizing = true;
    }

    public void NeuronIsWorking() { 
        if (neuronVisualization != null) neuronVisualization.NeuronWorking();
    }

    public void ResetVisualNeuronColor() {
        if (neuronVisualization != null) neuronVisualization.ResetNeuronColor();
    }

    public void SetActivationFunction(ActivationFunctions activationFunction) => this.activationFunction = activationFunction;

    public void SetName(string name) => this.name = name;
}