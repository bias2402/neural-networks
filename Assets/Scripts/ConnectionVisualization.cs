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
        transform.localPosition = (end + start) / 2;
        //transform.rotation = Quaternion.Euler(0, 0, (end - start).y);
        transform.localScale = new Vector3(0.1f, 0.1f, 0); //Vector3.Distance(start, end) / 90

    }
}