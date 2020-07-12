using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationFunctions { ReLU, Sigmoid, TanH }
public class FeedForwardArtificialNeuralNetwork : MonoBehaviour {
    private ActivationFunctionHandler activationFunctionHandler = null;

    [SerializeField] private List<double> inputs = new List<double>();
    [SerializeField] private List<double> outputs = new List<double>();
    private List<Layer> layers = new List<Layer>();
    [SerializeField] private int numberOfHiddenLayers = 0;
    [SerializeField] private ActivationFunctions inputLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;

    void Start() {
        InitializeANN();
        CalculateOutput();
    }
    void InitializeANN() {
        activationFunctionHandler = new ActivationFunctionHandler();
        layers.Add(new Layer(inputs.Count, inputs));

        List<double> prevOutputs = new List<double>();
        for (int i = 0; i < numberOfHiddenLayers; i++) {
            prevOutputs.Clear();
            for (int j = 0; j < layers[layers.Count - 1].neurons.Count; j++) {
                prevOutputs.Add(layers[layers.Count - 1].neurons[j].output);
            }
            layers.Add(new Layer(inputs.Count, prevOutputs));
        }
        prevOutputs.Clear();

        for (int i = 0; i < layers[layers.Count - 1].neurons.Count; i++) {
            prevOutputs.Add(layers[layers.Count - 1].neurons[i].output);
        }
        layers.Add(new Layer(inputs.Count, prevOutputs));

        layers[0].SetActivationFunctionForLayersNeurons(inputLayerActivationFunction);
        for (int i = 1; i < layers.Count - 1; i++) {
            layers[i].SetActivationFunctionForLayersNeurons(hiddenLayerActivationFunction);
        }
        layers[layers.Count - 1].SetActivationFunctionForLayersNeurons(outputLayerActivationFunction);

        for (int i = 0; i < layers.Count; i++) {
            for (int j = 0; j < layers[i].neurons.Count; j++) {
                layers[i].neurons[j].SetActivationFunctionHandler(activationFunctionHandler);
            }
        }
    }
    void CalculateOutput() {
        for (int i = 0; i < layers.Count; i++) {
            for (int j = 0; j < layers[i].neurons.Count; j++) {
                layers[i].neurons[j].CalculateOutput();
                if (i == layers.Count - 1) {
                    outputs.Add(layers[i].neurons[j].output);
                }
            }
        }
    }
}

public class Layer {
    public List<Neuron> neurons { get; internal set; } = new List<Neuron>();

    public Layer(int numberOfNeuronsForLayer, List<double> inputs) {
        for (int i = 0; i < numberOfNeuronsForLayer; i++) {
            neurons.Add(new Neuron(inputs));
        }
    }
    public void SetActivationFunctionForLayersNeurons(ActivationFunctions activationFunction) {
        for (int i = 0; i < neurons.Count; i++) {
            neurons[i].SetActivationFunction(activationFunction);
        }
    }
}

public class Neuron {
    private ActivationFunctionHandler activationFunctionHandler = null;
    private List<double> weights = new List<double>();
    private List<double> inputs = new List<double>();
    private double bias = 0;
    public double output { get; internal set; } = 0;
    private ActivationFunctions activationFunction = ActivationFunctions.Sigmoid;

    public Neuron(List<double> inputs) {
        if (inputs.Count <= 0) {
            Debug.LogError("A neuron must have a positive number of inputs!");
            return;
        }
        bias = Random.Range(-1f, 1f);
        this.inputs = inputs;
        for (int i = 0; i < inputs.Count; i++) {
            weights.Add(Random.Range(-1f, 1f));
        }
    }
    public void SetActivationFunction(ActivationFunctions activationFunction) {
        this.activationFunction = activationFunction;
    }
    public void SetActivationFunctionHandler(ActivationFunctionHandler activationFunctionHandler) {
        this.activationFunctionHandler = activationFunctionHandler;
    }
    public void UpdateWeights(double gradient) {
        for (int i = 0; i < weights.Count; i++) {
            weights[i] += gradient;
        }
    }
    public void UpdateBias(double gradient) {
        bias += gradient;
    }
    public void CalculateOutput() {
        double value = 0;
        for (int i = 0; i < inputs.Count; i++) {
            value += inputs[i] * weights[i];
        }
        value -= bias;
        output = activationFunctionHandler.TriggerActivationFunction(activationFunction, value);
    }
}

public class ActivationFunctionHandler {
    public double TriggerActivationFunction(ActivationFunctions activationFunction, double value) {
        switch (activationFunction) {
            case ActivationFunctions.ReLU:
                return ReLU(value);
            case ActivationFunctions.Sigmoid:
                return Sigmoid(value);
            case ActivationFunctions.TanH:
                return TanH(value);
        }
        Debug.LogError("The activation function wasn't set properly!");
        return value;
    }
    double ReLU(double value) {
        if (value > 0) return value;
        else return 0;
    }
    double Sigmoid(double value) {
        double k = (double)System.Math.Exp(value);
        return k / (1.0f + k);
    }
    double TanH(double value) {
        double k = (double)System.Math.Exp(-2 * value);
        return 2 / (1.0f + k) - 1;
    }
}