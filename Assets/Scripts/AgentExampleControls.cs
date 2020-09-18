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
                data.CleanData();

                List<List<double>> inputs = new List<List<double>>();
                List<List<double>> desiredOutputs = new List<List<double>>();
                inputs.Add(new List<double>(data.hit0));
                inputs.Add(new List<double>(data.hit45));
                inputs.Add(new List<double>(data.hit215));
                inputs.Add(new List<double>(data.dist0));
                inputs.Add(new List<double>(data.dist45));
                inputs.Add(new List<double>(data.dist215));
                desiredOutputs.Add(new List<double>(data.wDown));
                desiredOutputs.Add(new List<double>(data.aDown));
                desiredOutputs.Add(new List<double>(data.dDown));

                ANNInitializer.PassData(inputs, desiredOutputs);
                ANNInitializer.CreateANN();
            }

            if (isRecievingInput) {
                RaycastHit hit0 = Raycast(Vector3.forward);
                RaycastHit hit45 = Raycast(Vector3.forward + Vector3.right);
                RaycastHit hit215 = Raycast(Vector3.forward + Vector3.left);
                data.AddData(hit0.collider == null ? 1 : 0,
                             hit45.collider == null ? 1 : 0,
                             hit215.collider == null ? 1 : 0,
                             hit0.collider == null ? Vector3.Distance(transform.position, hit0.collider.transform.position) : -1,
                             hit45.collider == null ? Vector3.Distance(transform.position, hit45.collider.transform.position) : -1,
                             hit215.collider == null ? Vector3.Distance(transform.position, hit215.collider.transform.position) : -1,
                             forward > 0 ? 1 : 0,
                             turn < 0 ? 1 : 0,
                             turn > 0 ? 1 : 0);
            }
        }
    }

    void Walk() => transform.Translate(Vector3.forward * (float)forward);

    void Turn() => transform.Rotate(Vector3.up, (float)turn);

    void OnCollisionEnter(Collision collision) => this.collision = 1;

    void OnCollisionExit(Collision collision) => this.collision = 0;
}

public struct DataContainer {
    public List<double> hit0;
    public List<double> hit45;
    public List<double> hit215;
    public List<double> dist0;
    public List<double> dist45;
    public List<double> dist215;
    public List<double> wDown;
    public List<double> aDown;
    public List<double> dDown;

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