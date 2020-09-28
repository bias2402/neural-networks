using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ANN", order = 1)]
public class SOANN : ScriptableObject {
    [SerializeReference] public FeedForwardArtificialNeuralNetwork ann;
}
