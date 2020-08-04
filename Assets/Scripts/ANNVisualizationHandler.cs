using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANNVisualizationHandler : MonoBehaviour {
    [SerializeField] private GameObject INeuron = null;
    [SerializeField] private GameObject HNeuron = null;
    [SerializeField] private GameObject ONeuron = null;
    [SerializeField] private GameObject neuronCounter = null;
    [SerializeField] private GameObject connection = null;
    [SerializeField] private Transform neuronPool = null;
    [SerializeField] private Transform connectionPool = null;
    [SerializeField] private Transform counterPool = null;
    private int xOffset = 180;
    private int yOffset = 80;
    private List<VisualNeuron> visualNeurons = new List<VisualNeuron>();

    public void CreateVisualization(int nInputNeurons, int nHiddenNeurons, int nHiddenLayers, int nOutputNeurons, List<Layer> layers) {
        int nI = nInputNeurons > 5 ? 4 : nInputNeurons;
        int nH = nHiddenNeurons > 5 ? 4 : nHiddenNeurons;
        int nO = nOutputNeurons > 5 ? 4 : nOutputNeurons;

        int maxNNeuorns = Mathf.Max(nI, nH, nO);
        int xStart = (2 + nHiddenLayers) % 2 == 0 ? (2 + nHiddenLayers) / 2 * xOffset - xOffset / 2 : (2 + nHiddenLayers) / 2 * xOffset;
        int yStart =  maxNNeuorns % 2 == 0 ? maxNNeuorns / 2 * yOffset - yOffset / 2 : maxNNeuorns / 2 * yOffset;

        for (int i = 0; i < layers.Count ; i++) {
            if (i == 0) {                                       //Input layer
                if (nInputNeurons > 5) {
                    for (int j = 0; j < nI; j++) {
                        GameObject neuron = Instantiate(INeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * j) - (maxNNeuorns - nI) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(-xStart, 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nInputNeurons - 4);
                } else {
                    for (int j = 0; j < nI; j++) {
                        GameObject neuron = Instantiate(INeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * j) - (maxNNeuorns - nI) * yOffset / 2, 0);
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                }
            } else if (i == layers.Count - 1) {                 //Output layer
                if (layers[i].neurons.Count > 5) {
                    for (int j = 0; j < nO; j++) {
                        GameObject neuron = Instantiate(ONeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * j) - (maxNNeuorns - nO) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(xStart, 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nOutputNeurons - 4);
                } else {
                    for (int j = 0; j < nO; j++) {
                        GameObject neuron = Instantiate(ONeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * j) - (maxNNeuorns - nO) * yOffset / 2, 0);
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                }
                List<VisualNeuron> currentNeurons = visualNeurons.Where(n => n.layer == i).ToList();
                List<VisualNeuron> prevNeurons = visualNeurons.Where(n => n.layer == i - 1).ToList();
                foreach(VisualNeuron vnc in currentNeurons) {
                    foreach (VisualNeuron vnp in prevNeurons) {
                        ConnectionVisualization cv = Instantiate(connection, connectionPool).GetComponent<ConnectionVisualization>();
                        cv.SetPosition(vnp.position, vnc.position);
                    }
                }
            } else {                                            //Hidden layers
                if (nHiddenNeurons > 5) {
                    for (int j = 0; j < nH; j++) {
                        GameObject neuron = Instantiate(HNeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart + (xOffset * (i)), yStart - (yOffset * j) - (maxNNeuorns - nH) * yOffset / 2, 0);
                        if (neuron.transform.localPosition.y < 0) {
                            neuron.transform.localPosition += new Vector3(0, -40, 0);
                        } else {
                            neuron.transform.localPosition += new Vector3(0, 40, 0);
                        }
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                    GameObject counter = Instantiate(neuronCounter, counterPool);
                    counter.transform.localPosition = new Vector3(-xStart + (xOffset * (i)), 0, 0);
                    counter.GetComponent<CounterVisualization>().SetCounterValue(nHiddenNeurons - 4);
                } else {
                    for (int j = 0; j < nH; j++) {
                        GameObject neuron = Instantiate(HNeuron, neuronPool);
                        neuron.transform.localPosition = new Vector3(-xStart + (xOffset * (i)), yStart - (yOffset * j) - (maxNNeuorns - nH) * yOffset / 2, 0);
                        visualNeurons.Add(new VisualNeuron(neuron.GetComponent<NeuronVisualization>(), i, neuron.transform.localPosition));
                    }
                }
            }
        }
    }
}
public struct VisualNeuron {
    public NeuronVisualization neuronVisualization { get; internal set; }
    public int layer { get; internal set; }
    public List<ConnectionVisualization> connections { get; internal set; }
    public Vector3 position { get; internal set; }

    public VisualNeuron(NeuronVisualization neuronVisualization, int layer, Vector3 position) {
        this.neuronVisualization = neuronVisualization;
        this.layer = layer;
        connections = new List<ConnectionVisualization>();
        this.position = position;
    }
    public void AddConnection(ConnectionVisualization connection) {
        connections.Add(connection);
    }
}