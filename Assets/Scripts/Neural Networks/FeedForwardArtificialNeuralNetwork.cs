﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationFunctions { ReLU, Sigmoid, TanH }
public class FeedForwardArtificialNeuralNetwork {
    private int epochs = 1;
    private double alpha = 0.05;
    private int numberOfHiddenLayers = 0;
    private int numberOfHiddenNeurons = 4;
    private List<double> inputs = new List<double>();
    private List<double> outputs = new List<double>();
    private List<double> desiredOutputs = new List<double>();
    public List<Layer> layers { get; internal set; } = new List<Layer>();
    private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    public FeedForwardArtificialNeuralNetwork(int epochs, double alpha, int numberOfHiddenLayers, int numberOfHiddenNeuronsPerHiddenLayer, List<double> inputs, 
                List<double> desiredOutputs, ActivationFunctions hidden, ActivationFunctions output) {
        this.epochs = epochs;
        this.alpha = alpha;
        this.numberOfHiddenLayers = numberOfHiddenLayers;
        this.numberOfHiddenNeurons = numberOfHiddenNeuronsPerHiddenLayer;
        this.inputs = inputs;
        this.desiredOutputs = desiredOutputs;
        hiddenLayerActivationFunction = hidden;
        outputLayerActivationFunction = output;
        Initialize();
    }

    void Initialize() {
        //Input layer
        layers.Add(new Layer(inputs.Count, inputs, true));                                //Create the input layer

        //Hidden layers
        List<double> prevOutputs = new List<double>();
        for (int i = 0; i < numberOfHiddenLayers; i++) {
            prevOutputs.Clear();
            for (int j = 0; j < layers[layers.Count - 1].neurons.Count; j++) {  
                prevOutputs.Add(layers[layers.Count - 1].neurons[j].output);        //Get a reference to the previous layer's neurons' outputs
            }
            layers.Add(new Layer(numberOfHiddenNeurons, new List<double>(prevOutputs), false));              //Create a new hidden layers
        }
        prevOutputs.Clear();

        //Output layer
        for (int i = 0; i < layers[layers.Count - 1].neurons.Count; i++) {
            prevOutputs.Add(layers[layers.Count - 1].neurons[i].output);            //Get a reference to the previous layer's neurons' outputs
        }
        layers.Add(new Layer(desiredOutputs.Count, prevOutputs, false));                   //Create the output layer

        //Set activation functions (AF) for layers
        for (int i = 1; i < layers.Count - 1; i++) {
            layers[i].SetActivationFunctionForLayersNeurons(hiddenLayerActivationFunction);             //Set AF for hidden layers
        }
        layers[layers.Count - 1].SetActivationFunctionForLayersNeurons(outputLayerActivationFunction);  //Set AF for output layer

        for (int i = 0; i < layers.Count; i++) {
            for (int j = 0; j < layers[i].neurons.Count; j++) {
                string name = "";
                if (layers[i] == layers[0]) {
                    name += "Input " + i + ", " + j;
                } else if (layers[i] == layers[layers.Count - 1]) {
                    name += "Output " + i + ", " + j;
                } else {
                    name += "Hidden " + i + ", " + j;
                }

                layers[i].neurons[j].SetName(name);
            }
        }
    }

    public void Train() {
        for (int i = 0; i < epochs; i++) {
            Run();
        }
        Debug.ClearDeveloperConsole();
        for (int i = 0; i < outputs.Count; i++) {
            Debug.Log("Output " + i + ": " + outputs[i]);
        }
    }

    void Run() {
        CalculateOutput();
        Backpropagation();
    }

    void CalculateOutput() {
        outputs.Clear();
        for (int i = 0; i < layers.Count; i++) {
            for (int j = 0; j < layers[i].neurons.Count; j++) {
                layers[i].neurons[j].CalculateOutput();
                if (i == layers.Count - 1) {
                    outputs.Add(layers[i].neurons[j].output);
                }
            }
        }
    }

    void Backpropagation() {
        int outputLayer = layers.Count - 1;
        int hiddenLayers = layers.Count > 2 ? layers.Count - 2 : 0;
        //Output layer
        double error = 0;
        for (int i = 0; i < layers[outputLayer].neurons.Count; i++) {   //Iterate the neurons in the output layer
            //Calculate the error for the neuron
            error = desiredOutputs[i] - layers[outputLayer].neurons[i].output;
            //Calculate the errorGradient for the neuron
            layers[outputLayer].neurons[i].errorGradient = layers[outputLayer].neurons[i].output * (1 - layers[outputLayer].neurons[i].output) * error;
            //Update the neuron's weights
            for (int j = 0; j < layers[outputLayer].neurons[i].weights.Count; j++) {
                layers[outputLayer].neurons[i].weights[j] += alpha * layers[outputLayer].neurons[i].inputs[j] * error;
            }
            //Update the neuron's bias
            layers[outputLayer].neurons[i].bias += alpha * -1 * layers[outputLayer].neurons[i].errorGradient;
        }
        //Hidden layer
        for (int i = hiddenLayers; i > 1; i--) {    //Iterate the hidden layers
            for (int j = 0; j < layers[i].neurons.Count; j++) {     //Iterate the neurons
                //Calculate the neuron's errorGradient
                layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                //Calculate the errorGradientSum for the previous layer
                double errorGradientSum = 0;
                for (int k = 0; k < layers[i + 1].neurons.Count; k++) {
                    errorGradientSum += layers[i + 1].neurons[k].errorGradient * layers[i + 1].neurons[k].weights[j];
                }
                //Multiply the neuron's errorGradient with the errorGradientSum
                layers[i].neurons[j].errorGradient *= errorGradientSum;
                //Update the neuron's weights
                for (int k = 0; k < layers[i].neurons[j].weights.Count; k++) {
                    layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                }
                //Update the neuron's bias
                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
            }
        }
    }
}