using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Layer {
    [SerializeField] private List<Neuron> neurons = new List<Neuron>();

    //Get & Set methods
    #region
    public List<Neuron> GetNeurons() { return neurons; }
    #endregion

    public Layer(int numberOfNeuronsForLayer, List<double> inputs) {                                        //Hidden and output layer
        for (int i = 0; i < numberOfNeuronsForLayer; i++) {
            neurons.Add(new Neuron(inputs));
        }
    }

    public Layer(int numberOfNeuronsForLayer) {                                                             //Input layer
        for (int i = 0; i < numberOfNeuronsForLayer; i++) {
            neurons.Add(new Neuron());
        }
    }

    public void PassDataToNeuron(int neuronIndex, double data) {
        neurons[neuronIndex].SetInputValueForInputNeuron(data);
    }

    public void SetActivationFunctionForLayersNeurons(ActivationFunctions activationFunction) {
        for (int i = 0; i < neurons.Count; i++) {
            neurons[i].SetActivationFunction(activationFunction);
        }
    }
}