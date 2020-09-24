using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationFunctions { ReLU, Sigmoid, TanH }
public class FeedForwardArtificialNeuralNetwork {
    [SerializeField] private int epochs = 1;
    [SerializeField] private double alpha = 0.05;
    [SerializeField] private int numberOfHiddenLayers = 0;
    [SerializeField] private int numberOfHiddenNeurons = 4;
    [SerializeField] private List<List<double>> inputs = new List<List<double>>();
    [SerializeField] private List<double> outputs = new List<double>();
    [SerializeField] private List<List<double>> desiredOutputs = new List<List<double>>();
    [SerializeField] private List<Layer> layers = new List<Layer>();
    [SerializeField] private ActivationFunctions hiddenLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private ActivationFunctions outputLayerActivationFunction = ActivationFunctions.Sigmoid;
    [SerializeField] private bool isDelaying = false;
    [SerializeField] private int layerIndex = 0;
    [SerializeField] private int epochCounter = 0;
    [SerializeField] private int trainingIndex = 0;

    //Get & Set methods
    #region
    public List<Layer> GetLayers() { return layers; }

    public int GetCurrentEpoch() { return epochCounter; }
    #endregion

    public FeedForwardArtificialNeuralNetwork(int epochs, double alpha, int numberOfHiddenLayers, int numberOfHiddenNeuronsPerHiddenLayer, List<List<double>> inputs,
                List<List<double>> desiredOutputs, ActivationFunctions hidden, ActivationFunctions output, bool isDelaying) {
        this.epochs = epochs;
        this.alpha = alpha;
        this.numberOfHiddenLayers = numberOfHiddenLayers;
        this.numberOfHiddenNeurons = numberOfHiddenNeuronsPerHiddenLayer;
        this.inputs = inputs;
        this.desiredOutputs = desiredOutputs;
        hiddenLayerActivationFunction = hidden;
        outputLayerActivationFunction = output;
        this.isDelaying = isDelaying;
        Initialize();
    }

    void Initialize() {
        //Input layer
        layers.Add(new Layer(inputs.Count));                                                                //Create the input layer

        //Hidden layers
        List<double> prevOutputs = new List<double>();
        for (int i = 0; i < numberOfHiddenLayers; i++) {
            prevOutputs.Clear();
            for (int j = 0; j < layers[layers.Count - 1].GetNeurons().Count; j++) {  
                prevOutputs.Add(layers[layers.Count - 1].GetNeurons()[j].GetOutput());                              //Get a reference to the previous layer's neurons' outputs
            }
            layers.Add(new Layer(numberOfHiddenNeurons, new List<double>(prevOutputs)));                        //Create a new hidden layers
        }
        prevOutputs.Clear();

        //Output layer
        for (int i = 0; i < layers[layers.Count - 1].GetNeurons().Count; i++) {
            prevOutputs.Add(layers[layers.Count - 1].GetNeurons()[i].GetOutput());                              //Get a reference to the previous layer's neurons' outputs
        }
        layers.Add(new Layer(desiredOutputs.Count, prevOutputs));                                           //Create the output layer

        //Set activation functions (AF) for layers
        for (int i = 1; i < layers.Count - 1; i++) {
            layers[i].SetActivationFunctionForLayersNeurons(hiddenLayerActivationFunction);                     //Set AF for hidden layers
        }
        layers[layers.Count - 1].SetActivationFunctionForLayersNeurons(outputLayerActivationFunction);      //Set AF for output layer

        for (int i = 0; i < layers.Count; i++) {                                                            //Go through all layers
            for (int j = 0; j < layers[i].GetNeurons().Count; j++) {                                            //Go through all neurons
                string name = "";                                                                       
                if (layers[i] == layers[0]) {
                    name += "Input " + i + ", " + j;
                } else if (layers[i] == layers[layers.Count - 1]) {
                    name += "Output " + i + ", " + j;
                } else {
                    name += "Hidden " + i + ", " + j;
                }

                layers[i].GetNeurons()[j].SetName(name);                                                        //Build a name (string) and set the name. Name is used for debugging
            }
        }
    }

    public void PassInputs(List<List<double>> inputs) {
        this.inputs = inputs;
    }

    public bool Train() {
        if (isDelaying) {                                                                                   //If execution is delayed
            if (epochCounter < epochs) {                                                                        //As long as the epoch limit hasn't been reached
                for (int j = 0; j < layers[0].GetNeurons().Count; j++) {
                    layers[0].PassDataToNeuron(j, inputs[j][trainingIndex]);                                            //Pass information to the input neurons
                }
                for (int j = 0; j < layers[layers.Count - 1].GetNeurons().Count; j++) {
                    layers[layers.Count - 1].PassDataToNeuron(j, desiredOutputs[j][trainingIndex]);                     //Pass desiredOutputs to the output neurons
                }
                CalculateOutput();
                Backpropagation();
                layerIndex++;
                if (layerIndex >= layers.Count) layerIndex = 0;
                trainingIndex++;
                if (trainingIndex >= inputs[0].Count) {
                    trainingIndex = 0;
                    epochCounter++;
                }
                return false;                                                                                       
            } else {                                                                                            //If the epochs limit is reached, reset and print outputs
                epochCounter = 0;
                for (int i = 0; i < outputs.Count; i++) {
                    Debug.Log(outputs[i]);
                }
                return true;
            }
        } else {                                                                                            //If the execution isn't delayed
            for (int i = 0; i < epochs; i++) {                                                                  //Run through all the epochs in one frame
                for (int j = 0; j < inputs[0].Count; j++) {                                                         //Run through all the training data
                    for (int k = 0; k < layers[0].GetNeurons().Count; k++) {
                        layers[0].PassDataToNeuron(k, inputs[k][j]);                                                        //Pass information to the input neurons
                    }
                    for (int k = 0; k < layers[layers.Count - 1].GetNeurons().Count; k++) {
                        layers[layers.Count - 1].PassDataToNeuron(k, desiredOutputs[k][j]);                                 //Pass desiredOutputs to the output neurons
                    }
                    trainingIndex = j;                                                                                  //Set trainingIndex to i, so it can be used for backpropagation
                    CalculateOutput();
                    Backpropagation();
                }
            }
            for (int i = 0; i < outputs.Count; i++) {
                Debug.Log(outputs[i]);
            }
            return true;
        }
    }

    public int Run() {
        if (layerIndex == 0) outputs.Clear();                                                               //Clear the outputs if layer index equals 0, to assure the list is cleared

        for (int i = 0; i < layers[0].GetNeurons().Count; i++) {
            layers[0].GetNeurons()[i].SetInputValueForInputNeuron(inputs[i][0]);
        }

        CalculateOutput();

        for (int i = 0; i < outputs.Count; i++) {
            Debug.Log(outputs[i]);
        } 

        if (outputs.Count > 0) {                                                                            //If outputs count is greater than 0, it means the outputs were properly calculated
            double max = -1;
            int neuronIndex = 0;
            for (int i = 0; i < outputs.Count; i++) {
                if (outputs[i] > max) {                                                                             //Find the best output, which is the neuron that should decide the action
                    max = outputs[i];
                    neuronIndex = i;
                }
            }
            return neuronIndex;
        } else {
            return -1;
        }
    }

    public void Reset() {
        for (int i = 0; i < layers.Count; i++) {
            for (int j = 0; j < layers[i].GetNeurons().Count; j++) {
                layers[i].GetNeurons()[j].ResetVisualNeuronColor();                                                 //Reset the colors of the visual neurons
            }
        }
    }

    public void SetNextLayerAsWorking() {
        foreach (Neuron n in layers[layerIndex].GetNeurons()) {
            n.NeuronIsWorking();                                                                                //Call for an update of the visual neurons in the next layer
        }
    }

    void CalculateOutput() {
        if (isDelaying) {                                                                                   //If the execution is delayed
            if (layerIndex == 0) outputs.Clear();                                                               //If the layer equals 0, reset outputs just in case
            foreach (Neuron n in layers[layerIndex].GetNeurons()) {                                             //For each neuron in the layer
                n.CalculateOutput();                                                                                //Calculate the output
                if (layerIndex == layers.Count - 1) {                                                               //Is this the output layer
                    outputs.Add(n.GetOutput());                                                                         //For output neurons, add the calculated value to outputs
                }
            }
        } else {                                                                                            //If the execution isn't delayed
            outputs.Clear();                                                                                    //Clear outputs
            for (int i = 0; i < layers.Count; i++) {                                                            //Run through all layers
                for (int j = 0; j < layers[i].GetNeurons().Count; j++) {                                            //Run through all neurons of the layer
                    layers[i].GetNeurons()[j].CalculateOutput();                                                        //Calculate the neurons output
                    if (i == layers.Count - 1) {
                        outputs.Add(layers[i].GetNeurons()[j].GetOutput());                                                 //For output neurons, add the calculated value to outputs
                    }
                }
            }
        }
    }

    void Backpropagation() {
        int outputLayer = layers.Count - 1;
        int hiddenLayers = layers.Count > 2 ? layers.Count - 2 : 0;
        //Output layer
        double error = 0;
        for (int i = 0; i < layers[outputLayer].GetNeurons().Count; i++) {   //Iterate the neurons in the output layer
            //Calculate the error for the neuron by subtracting the actual output from the desired output  of this output neuron
            error = desiredOutputs[i][trainingIndex] - layers[outputLayer].GetNeurons()[i].GetOutput();
            //Calculate the errorGradient for the neuron (used for the errorGradientSum in the hidden layer to follow)
            layers[outputLayer].GetNeurons()[i].SetErrorGradient(layers[outputLayer].GetNeurons()[i].GetOutput() * (1 - layers[outputLayer].GetNeurons()[i].GetOutput()) * error, true);
            //Update the neuron's weights
            for (int j = 0; j < layers[outputLayer].GetNeurons()[i].GetWeights().Count; j++) {
                layers[outputLayer].GetNeurons()[i].GetWeights()[j] += alpha * layers[outputLayer].GetNeurons()[i].GetInputs()[j] * error;
            }
            //Update the neuron's bias
            layers[outputLayer].GetNeurons()[i].SetBias(alpha * -1 * layers[outputLayer].GetNeurons()[i].GetErrorGradient());
        }
        //Hidden layer
        for (int i = hiddenLayers; i > 1; i--) {    //Iterate the hidden layers
            for (int j = 0; j < layers[i].GetNeurons().Count; j++) {     //Iterate the neurons
                //Calculate the errorGradient for the neuron (used for the errorGradientSum in the hidden layer to follow)
                layers[i].GetNeurons()[j].SetErrorGradient(layers[i].GetNeurons()[j].GetOutput() * (1 - layers[i].GetNeurons()[j].GetOutput()), true);
                //Calculate the errorGradientSum for the previous layer
                double errorGradientSum = 0;
                for (int k = 0; k < layers[i + 1].GetNeurons().Count; k++) {
                    errorGradientSum += layers[i + 1].GetNeurons()[k].GetErrorGradient() * layers[i + 1].GetNeurons()[k].GetWeights()[j];
                }
                //Multiply the neuron's errorGradient with the errorGradientSum to update the errorGradient of the neuron (must be updated according to all outgoing connection weigths)
                layers[i].GetNeurons()[j].SetErrorGradient(errorGradientSum, false);
                //Update the neuron's weights
                for (int k = 0; k < layers[i].GetNeurons()[j].GetWeights().Count; k++) {
                    layers[i].GetNeurons()[j].GetWeights()[k] += alpha * layers[i].GetNeurons()[j].GetInputs()[k] * layers[i].GetNeurons()[j].GetErrorGradient();
                }
                //Update the neuron's bias
                layers[i].GetNeurons()[j].SetBias(alpha * -1 * layers[i].GetNeurons()[j].GetErrorGradient());
            }
        }
    }
}