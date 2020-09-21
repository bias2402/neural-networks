using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Neuron {
    [Header("Neuron Settings")]
    [SerializeField] private bool isInputNeuron = false;
    [SerializeField] private double inputValue = 0;
    [SerializeField] private double bias = 0;
    [SerializeReference] private List<double> weights = new List<double>();
    [SerializeReference] private List<double> inputs = new List<double>();
    [SerializeField] private double output = 0;
    [SerializeField] private double desiredOutput = 0;
    [SerializeField] private double errorGradient = 0;
    [SerializeField] private string name = "";

    [Header("Visualization")]
    [SerializeField] private bool isVisualizing = false;
    [SerializeField] private NeuronVisualization neuronVisualization = null;

    [SerializeField] private ActivationFunctions activationFunction = ActivationFunctions.Sigmoid;

    //Get & Set methods
    #region
    public void SetName(string name) => this.name = name;

    public void SetBias(double delta) => bias += delta;

    public List<double> GetWeights() { return weights; }

    public List<double> GetInputs() { return inputs; }

    public double GetOutput() { return output; }

    public double GetErrorGradient() { return errorGradient; }

    public void SetErrorGradient(double delta, bool set) => errorGradient = set ? delta : errorGradient * delta;

    public void SetInputValueForInputNeuron(double inputValue) => this.inputValue = inputValue;

    public void SetDesiredOutputValueForOutputNeuron(double desiredOutputValue) => desiredOutput = desiredOutputValue;

    public void SetActivationFunction(ActivationFunctions activationFunction) => this.activationFunction = activationFunction;
    #endregion

    //Hidden or Output neuron constructor
    public Neuron(List<double> inputs) {
        if (inputs.Count <= 0) {
            Debug.LogError("A neuron must have a positive number of inputs!");
            return;
        }
        bias = UnityEngine.Random.Range(-1f, 1f);                                                           //Randomize the start bias
        this.inputs = inputs;
        for (int i = 0; i < inputs.Count; i++) {
            weights.Add(UnityEngine.Random.Range(-1f, 1f));                                                     //Add a weight for each input and randomize the start value
        }
    }

    //Input neuron constructor for live-training
    public Neuron() => isInputNeuron = true;

    //Input neuron for pre-training
    public Neuron(double inputValue) {
        this.inputValue = inputValue;
    }

    public void CalculateOutput() {
        if (isInputNeuron) {                                                                                //If the neuron is an input neuron, set its output to input
            output = inputValue;

            if (isVisualizing) {
                neuronVisualization.UpdateNeuronImage((float)output);
                neuronVisualization.UpdateConnection((float)output);
            }
            return;
        }

        double value = 0;
        if (inputs.Count != weights.Count) Debug.LogError("Inputs and weights must be the same amount! Inputs: " + 
            inputs.Count + ", Weights: " + weights.Count + ", Neuron: " + name);                            //Ensure the lists of weights and inputs are equal in size
        for (int i = 0; i < inputs.Count; i++) {
            value += inputs[i] * weights[i];                                                                    //Calculate the output value by multiplying weights and inputs
        }
        value -= bias;                                                                                      //Subtract the bias to apply the bias of the neuron
        output = ActivationFunctionHandler.TriggerActivationFunction(activationFunction, value);            //Throw the through the chosen activation function to calculate the final output

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
        if (neuronVisualization != null) neuronVisualization.NeuronWorking();                               //If the neuron has a neuronVisualization, call its NeuronWorking method
    }

    public void ResetVisualNeuronColor() {
        if (neuronVisualization != null) neuronVisualization.ResetNeuronColor();                            //If the neuron has a neuronVisualization, call its ResetNeuronColor method
    }
}