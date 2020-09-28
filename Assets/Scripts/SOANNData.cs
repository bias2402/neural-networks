using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ANNData", order = 1)]
public class SOANNData : ScriptableObject {
    public List<double> hit0 = new List<double>();
    public List<double> hit45 = new List<double>();
    public List<double> hit215 = new List<double>();
    public List<double> dist0 = new List<double>();
    public List<double> dist45 = new List<double>();
    public List<double> dist215 = new List<double>();
    public List<double> wDown = new List<double>();
    public List<double> aDown = new List<double>();
    public List<double> dDown = new List<double>();

    public int GetListsCount() { return hit0.Count; }

    public void AddData(double hit0, double hit45, double hit215, double dist0, double dist45, double dist215, double wDown, double aDown, double dDown) {
        this.hit0.Add(hit0);
        this.hit45.Add(hit45);
        this.hit215.Add(hit215);
        this.dist0.Add(dist0);
        this.dist45.Add(dist45);
        this.dist215.Add(dist215);
        this.wDown.Add(wDown);
        this.aDown.Add(aDown);
        this.dDown.Add(dDown);
    }

    public void CleanData() {
        for (int i = 0; i < hit0.Count; i++) {
            for (int j = i + 1; j < hit0.Count; j++) {
                if (hit0[i] == hit0[j] &&
                    hit45[i] == hit45[j] &&
                    hit215[i] == hit215[j] &&
                    dist0[i] == dist0[j] &&
                    dist45[i] == dist45[j] &&
                    dist215[i] == dist215[j]) {
                    Debug.Log(hit0[i] + " " + hit0[j]);
                    Debug.Log(hit45[i] + " " + hit45[j]);
                    Debug.Log(hit215[i] + " " + hit215[j]);
                    Debug.Log(dist0[i] + " " + dist0[j]);
                    Debug.Log(dist45[i] + " " + dist45[j]);
                    Debug.Log(dist215[i] + " " + dist215[j]);

                    hit0.RemoveAt(j);
                    hit45.RemoveAt(j);
                    hit215.RemoveAt(j);
                    dist0.RemoveAt(j);
                    dist45.RemoveAt(j);
                    dist215.RemoveAt(j);
                    wDown.RemoveAt(j);
                    aDown.RemoveAt(j);
                    dDown.RemoveAt(j);
                }
            }
        }
    }

    public void ClearData() {
        hit0.Clear();
        hit45.Clear();
        hit215.Clear();
        dist0.Clear();
        dist45.Clear();
        dist215.Clear();
        wDown.Clear();
        aDown.Clear();
        dDown.Clear();
    }

    public List<List<double>> CreateInputs() {
        List<List<double>> inputs = new List<List<double>>();
        inputs.Add(new List<double>(hit0));
        inputs.Add(new List<double>(hit45));
        inputs.Add(new List<double>(hit215));
        inputs.Add(new List<double>(dist0));
        inputs.Add(new List<double>(dist45));
        inputs.Add(new List<double>(dist215));
        return inputs;
    }

    public List<List<double>> CreateDesiredOutputs() {
        List<List<double>> desiredOutputs = new List<List<double>>();
        desiredOutputs.Add(new List<double>(wDown));
        desiredOutputs.Add(new List<double>(aDown));
        desiredOutputs.Add(new List<double>(dDown));
        return desiredOutputs;
    }
}