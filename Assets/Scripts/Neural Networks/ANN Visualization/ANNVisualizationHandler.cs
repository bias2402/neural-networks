using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ANNVisualizationHandler : MonoBehaviour {
    [SerializeField] private Transform ANNCamera = null;
    [SerializeField] private GameObject INeuron = null;
    [SerializeField] private GameObject HNeuron = null;
    [SerializeField] private GameObject ONeuron = null;
    [SerializeField] private GameObject connection = null;
    [SerializeField] private GameObject neuronCounter = null;
    [SerializeField] private GameObject layerCounter = null;
    [SerializeField] private GameObject hiddenNeuronCrossCounter = null;
    [SerializeField] private Transform neuronPool = null;
    [SerializeField] private Transform counterPool = null;
    private int xOffset = 180;
    private int yOffset = 80;

    public void CreateVisualization(int nInputNeurons, int nHiddenNeurons, int nHiddenLayers, int nOutputNeurons, List<Layer> layers) {
        if (ANNCamera != null) ANNCamera.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -425);

        List<VisualNeuron> visualNeurons = new List<VisualNeuron>();
        int numberOfInputNeurons = nInputNeurons > 5 ? 4 : nInputNeurons;
        int numberOfHiddenNeurons = nHiddenNeurons > 5 ? 4 : nHiddenNeurons;
        int numberOfOutputNeurons = nOutputNeurons > 5 ? 4 : nOutputNeurons;
        int numberOfHiddenLayers = nHiddenLayers > 3 ? 2 : nHiddenLayers;
        Debug.Log(numberOfHiddenLayers);

        int maxNumberOfNeurons = Mathf.Max(numberOfInputNeurons, numberOfHiddenNeurons, numberOfOutputNeurons);
        int xStart = (2 + numberOfHiddenLayers) % 2 == 0 ? (2 + numberOfHiddenLayers) / 2 * xOffset - xOffset / 2 : (2 + numberOfHiddenLayers) / 2 * xOffset;
        int yStart =  maxNumberOfNeurons % 2 == 0 ? maxNumberOfNeurons / 2 * yOffset - yOffset / 2 : maxNumberOfNeurons / 2 * yOffset;

        for (int i = 0; i < layers.Count ; i++) {
            if (nHiddenLayers > 3 && i == 2) {
                GameObject lCounter = Instantiate(layerCounter, counterPool);
                lCounter.GetComponent<CounterVisualization>().SetCounterValue(nHiddenLayers - 2);
                lCounter.transform.localPosition = new Vector3(0, -yOffset - (maxNumberOfNeurons - numberOfHiddenNeurons) * yOffset / 2, 0);
                lCounter = Instantiate(layerCounter, counterPool);
                lCounter.GetComponent<CounterVisualization>().SetCounterValue(nHiddenLayers - 2);
                lCounter.transform.localPosition = new Vector3(0, yOffset - (maxNumberOfNeurons - numberOfHiddenNeurons) * yOffset / 2, 0);
                GameObject hNCounter = Instantiate(hiddenNeuronCrossCounter, counterPool);
                hNCounter.transform.localPosition = Vector3.zero;
                hNCounter.GetComponent<CounterVisualization>().SetCounterValue((nHiddenLayers - 2) * nHiddenNeurons);
                i = layers.Count - 3;
                continue;
            }

            if (i == 0) {                                       //Input layer
                if (nInputNeurons > 5) {
                    for (int j = 0; j < numberOfInputNeurons; j++) {
                        GameObject neuron = Instantiate(INeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfInputNeurons) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                        if (j == 1) j = numberOfInputNeurons - 3;
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(-xStart, 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nInputNeurons - 4);
                } else {
                    for (int j = 0; j < numberOfInputNeurons; j++) {
                        GameObject neuron = Instantiate(INeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfInputNeurons) * yOffset / 2, 0);
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                    }
                }
            } else if (i == layers.Count - 1) {                 //Output layer
                if (layers[i].GetNeurons().Count > 5) {
                    for (int j = 0; j < numberOfOutputNeurons; j++) {
                        GameObject neuron = Instantiate(ONeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfOutputNeurons) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                        if (j == 1) j = numberOfOutputNeurons - 3;
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(xStart, 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nOutputNeurons - 4);
                } else {
                    for (int j = 0; j < numberOfOutputNeurons; j++) {
                        GameObject neuron = Instantiate(ONeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfOutputNeurons) * yOffset / 2, 0);
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                    }
                }
            } else {                                            //Hidden layers
                if (nHiddenNeurons > 5) {
                    for (int j = 0; j < numberOfHiddenNeurons; j++) {
                        GameObject neuron = Instantiate(HNeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart + (xOffset * (i < 4 ? i : i - (nHiddenLayers - numberOfHiddenLayers))), yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfHiddenNeurons) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                        if (j == 1) j = numberOfHiddenNeurons - 3;
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(-xStart + (xOffset * (i < 4 ? i : i - (nHiddenLayers - numberOfHiddenLayers))), 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nHiddenNeurons - 4);
                } else {
                    for (int j = 0; j < numberOfHiddenNeurons; j++) {
                        GameObject neuron = Instantiate(HNeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart + (xOffset * i), yStart - (yOffset * j) - (maxNumberOfNeurons - numberOfHiddenNeurons) * yOffset / 2, 0);
                        VisualNeuron vs = new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.position, j);
                        visualNeurons.Add(vs);
                        vs.neuronVisualization.PrepareVisualNeuron(layers[i].GetNeurons()[j]);
                    }
                }
            }
        }

        for (int i = 0; i < layers.Count - 1; i++) {
            List<VisualNeuron> currentLayerNeurons = visualNeurons.Where(n => n.layer == i).ToList();
            List<VisualNeuron> nextLayerNeurons = visualNeurons.Where(n => n.layer == i + 1).ToList();
            foreach (VisualNeuron current in currentLayerNeurons) {
                List<Vector3> positions = new List<Vector3>();
                foreach (VisualNeuron next in nextLayerNeurons) {
                    current.neuronVisualization.CreateNewConnection(connection, next.neuronIndex, current.position, next.position);
                }
            }
        }
    }
}

public struct VisualNeuron {
    public NeuronVisualization neuronVisualization { get; internal set; }

    public int layer { get; internal set; }

    public Vector3 position { get; internal set; }

    public int neuronIndex { get; internal set; }

    public VisualNeuron(NeuronVisualization neuronVisualization, int layer, Vector3 position, int neuronIndex) {
        this.neuronVisualization = neuronVisualization;
        this.layer = layer;
        this.position = position;
        this.neuronIndex = neuronIndex;
    }
}