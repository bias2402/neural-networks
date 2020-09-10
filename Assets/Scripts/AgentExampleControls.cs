using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentExampleControls : MonoBehaviour {
    [SerializeField] private bool isAIAgent = false;
    [SerializeField] private BasicANNInitializer ANNInitializer = null;
    private bool isRecievingInput = false;
    private Dictionary<string, List<double>> inputs = new Dictionary<string, List<double>>();
    private double forward = 0;
    private double turn = 0;
    private double collision = 0;

    void Start() {
        inputs.Add("walk", new List<double>());
        inputs.Add("turn", new List<double>());
        inputs.Add("hit", new List<double>());
        inputs.Add("col", new List<double>());
    }

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
                for (int i = 0; i < inputs["walk"].Count; i++) {
                    for (int j = i; j < inputs["walk"].Count; j++) {
                        if (inputs["walk"][i] == inputs["walk"][j] &&
                            inputs["turn"][i] == inputs["turn"][j] &&
                            inputs["hit"][i] == inputs["hit"][j] &&
                            inputs["col"][i] == inputs["col"][j]) {
                            inputs["walk"].RemoveAt(j);
                            inputs["turn"].RemoveAt(j);
                            inputs["hit"].RemoveAt(j);
                            inputs["col"].RemoveAt(j);
                        }
                    }
                }

                ANNInitializer.PassInputs(new List<List<double>> { inputs["walk"], inputs["turn"], inputs["hit"] });
                ANNInitializer.PassDesiredOutputs(inputs["col"]);
                ANNInitializer.CreateANN();
            }

            if (isRecievingInput) {
                RaycastHit hit = Raycast(Vector3.forward);
                inputs["walk"].Add(forward);
                inputs["turn"].Add(turn);
                inputs["hit"].Add(hit.collider != null ? 1 : 0);
                inputs["col"].Add(collision);
            }
        }
    }

    void Walk() => transform.Translate(Vector3.forward * (float)forward);

    void Turn() => transform.Rotate(Vector3.up, (float)turn);

    void OnCollisionEnter(Collision collision) => this.collision = 1;

    void OnCollisionExit(Collision collision) => this.collision = 0;
}
