using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ANNVisualizationHandler : MonoBehaviour {
    [SerializeField] private GameObject INeuron = null;
    [SerializeField] private GameObject HNeuron = null;
    [SerializeField] private GameObject ONeuron = null;
    [SerializeField] private GameObject neuronCounter = null;
    [SerializeField] private Transform neuronPool = null;
    [SerializeField] private Transform connectionPool = null;
    [SerializeField] private Transform counterPool = null;
    private int xOffset = 180;
    private int yOffset = 80;

    public void CreateVisualization(int nInputNeurons, int nHiddenNeurons, int nHiddenLayers, int nOutputNeurons) {
        int nI = nInputNeurons > 5 ? 4 : nInputNeurons;
        int nH = nHiddenNeurons > 5 ? 4 : nHiddenNeurons;
        int nO = nOutputNeurons > 5 ? 4 : nOutputNeurons;

        int maxNNeuorns = Mathf.Max(nI, nH, nO);
        int xStart = (2 + nHiddenLayers) % 2 == 0 ? (2 + nHiddenLayers) / 2 * xOffset - xOffset / 2 : (2 + nHiddenLayers) / 2 * xOffset;
        int yStart =  maxNNeuorns % 2 == 0 ? maxNNeuorns / 2 * yOffset - yOffset / 2 : maxNNeuorns / 2 * yOffset;

        //Input layer placement
        if (nInputNeurons > 5) {
            for (int i = 0; i < nI; i++) {
                GameObject neuron = Instantiate(INeuron, neuronPool);
                neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * i) - (maxNNeuorns - nI) * yOffset / 2, 0);
                if (neuron.transform.localPosition.y < 0) {
                    neuron.transform.localPosition += new Vector3(0, -40, 0);
                } else {
                    neuron.transform.localPosition += new Vector3(0, 40, 0);
                }
            }
            GameObject counter = Instantiate(neuronCounter, counterPool);
            counter.transform.localPosition = new Vector3(-xStart, 0, 0);
            counter.GetComponent<CounterVisualization>().SetCounterValue(nInputNeurons - 4);
        } else {
            for (int i = 0; i < nI; i++) {
                GameObject neuron = Instantiate(INeuron, neuronPool);
                neuron.transform.localPosition = new Vector3(-xStart, yStart - (yOffset * i) - (maxNNeuorns - nI) * yOffset / 2, 0);
            }
        }
        //Hidden layer placement
        for (int i = 0; i < nHiddenLayers; i++) {
            if (nHiddenNeurons > 5) {
                for (int j = 0; j < nH; j++) {
                    GameObject neuron = Instantiate(HNeuron, neuronPool);
                    neuron.transform.localPosition = new Vector3(-xStart + (xOffset * (i + 1)), yStart - (yOffset * j) - (maxNNeuorns - nH) * yOffset / 2, 0);
                    if (neuron.transform.localPosition.y < 0) {
                        neuron.transform.localPosition += new Vector3(0, -40, 0);
                    } else {
                        neuron.transform.localPosition += new Vector3(0, 40, 0);
                    }
                }
                GameObject counter = Instantiate(neuronCounter, counterPool);
                counter.transform.localPosition = new Vector3(-xStart + (xOffset * (i + 1)), 0, 0);
                counter.GetComponent<CounterVisualization>().SetCounterValue(nHiddenNeurons - 4);
            } else {
                for (int j = 0; j < nH; j++) {
                    GameObject neuron = Instantiate(HNeuron, neuronPool);
                    neuron.transform.localPosition = new Vector3(-xStart + (xOffset * (i + 1)), yStart - (yOffset * j) - (maxNNeuorns - nH) * yOffset / 2, 0);
                }
            }
        }
        //Output layer placement
        if (nOutputNeurons > 5) {
            for (int i = 0; i < nO; i++) {
                GameObject neuron = Instantiate(ONeuron, neuronPool);
                neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * i) - (maxNNeuorns - nO) * yOffset / 2, 0);
                if (neuron.transform.localPosition.y < 0) {
                    neuron.transform.localPosition += new Vector3(0, -40, 0);
                } else {
                    neuron.transform.localPosition += new Vector3(0, 40, 0);
                }
            }
            GameObject counter = Instantiate(neuronCounter, counterPool);
            counter.transform.localPosition = new Vector3(xStart, 0, 0);
            counter.GetComponent<CounterVisualization>().SetCounterValue(nOutputNeurons - 4);
        } else {
            for (int i = 0; i < nO; i++) {
                GameObject neuron = Instantiate(ONeuron, neuronPool);
                neuron.transform.localPosition = new Vector3(xStart, yStart - (yOffset * i) - (maxNNeuorns - nO) * yOffset / 2, 0);
            }
        }
    }
}