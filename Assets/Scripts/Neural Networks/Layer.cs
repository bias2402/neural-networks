using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Layer {
    public List<Neuron> neurons { get; internal set; } = new List<Neuron>();

    public Layer(int numberOfNeuronsForLayer, List<double> inputs, bool isInputLayer) {
        if (isInputLayer) {
            for (int i = 0; i < numberOfNeuronsForLayer; i++) {
                neurons.Add(new Neuron(inputs[i]));
            }
        } else {
            for (int i = 0; i < numberOfNeuronsForLayer; i++) {
                neurons.Add(new Neuron(inputs));
            }
        }
    }

    public void SetActivationFunctionForLayersNeurons(ActivationFunctions activationFunction) {
        for (int i = 0; i < neurons.Count; i++) {
            neurons[i].SetActivationFunction(activationFunction);
        }
    }
}