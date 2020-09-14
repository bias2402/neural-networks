using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentExampleControls : MonoBehaviour {
    [SerializeField] private bool isAIAgent = false;
    [SerializeField] private BasicANNInitializer ANNInitializer = null;
    private bool isRecievingInput = false;
    private double forward = 0;
    private double turn = 0;
    private double collision = 0;
    private DataContainer data = new DataContainer();

    RaycastHit Raycast(Vector3 direction) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction);
        Physics.Raycast(ray, out hit, 5);
        return hit;
    }

    void Update() {
        if (isAIAgent) {

        } else {
            isRecievingInput = false;
            forward = 0;
            turn = 0;

            if (Input.GetKey(KeyCode.W)) {
                forward = 10 * Time.deltaTime;
                Walk();
                isRecievingInput = true;
            }

            if (Input.GetKey(KeyCode.A)) {
                turn = -150 * Time.deltaTime;
                Turn();
                isRecievingInput = true;
            }

            if (Input.GetKey(KeyCode.D)) {
                turn = 150 * Time.deltaTime;
                Turn();
                isRecievingInput = true;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                ANNInitializer.PassData(data);
                ANNInitializer.CreateANN();
            }

            if (isRecievingInput) {
                RaycastHit hit = Raycast(Vector3.forward);

            }
        }
    }

    void Walk() => transform.Translate(Vector3.forward * (float)forward);

    void Turn() => transform.Rotate(Vector3.up, (float)turn);

    void OnCollisionEnter(Collision collision) => this.collision = 1;

    void OnCollisionExit(Collision collision) => this.collision = 0;
}

public struct DataContainer {
    public List<bool> hit0;
    public List<bool> hit45;
    public List<bool> hit215;
    public List<float> dist0;
    public List<float> dist45;
    public List<float> dist215;

    public void AddData(bool hit0, bool hit45, bool hit215, float dist0, float dist45, float dist215) {
        this.hit0.Add(hit0);
        this.hit45.Add(hit45);
        this.hit215.Add(hit215);
        this.dist0.Add(dist0);
        this.dist45.Add(dist45);
        this.dist215.Add(dist215);
    }

    public void CleanData() {
        for (int i = 0; i < hit0.Count; i++) {
            for (int j = i; j < dist0.Count; j++) {
                if (hit0[i] == hit0[j] &&
                    hit45[i] == hit45[j] &&
                    hit215[i] == hit215[j] &&
                    dist0[i] == dist0[j] &&
                    dist45[i] == dist45[j] &&
                    dist215[i] == dist215[j]) {
                    hit0.RemoveAt(j);
                    hit45.RemoveAt(j);
                    hit215.RemoveAt(j);
                    dist0.RemoveAt(j);
                    dist45.RemoveAt(j);
                    dist215.RemoveAt(j);
                }
            }
        }
    }
}