using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionVisualization : MonoBehaviour {
    private Image connectionImage = null;

    void Start() {
        connectionImage = GetComponent<Image>();
    }
    public void SetPosition(Vector3 start, Vector3 end) {
        Vector3 pos = end + start;
        transform.localPosition = pos / 2;
        transform.rotation = Quaternion.Euler(0, 0, pos.y);
        transform.localScale = new Vector3(Vector3.Distance(start, end) / 90, 0.1f, 0);
    }
}