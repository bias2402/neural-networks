using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentExampleControls : MonoBehaviour {
    [SerializeField] private bool isAIAgent = false;
    [SerializeField] private BasicANNInitializer ANNInitializer = null;
    [SerializeField] private Transform camTrans = null;
    [SerializeField] private SOANNData trainingData;
    [SerializeField] private SOANNData liveData;
    private bool isRecievingInput = false;
    private double forward = 0;
    private double turn = 0;

    void Start() {
        if (camTrans == null) camTrans = Camera.main.transform;
    }

    void Update() {
        if (!isAIAgent) {
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
                if (ANNInitializer.GetIsVisualizing()) {
                    camTrans.position = new Vector3(0, 0, -425);
                    camTrans.rotation = Quaternion.Euler(0, 0, 0);
                }
                ANNInitializer.CreateANN();
                ANNInitializer.Run(true);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                trainingData.CleanData();
            }

            if (isRecievingInput) RecordData();
        } else {
            if (!ANNInitializer.isWorking) {
                liveData.ClearData();
                RecordData();

                ANNInitializer.PassData(liveData.CreateInputs(), liveData.CreateDesiredOutputs(), true);
                ANNInitializer.Run(false);

                switch (ANNInitializer.GetFiringOutputNeuron()) {
                    case 0:
                        forward = 10 * Time.deltaTime;
                        turn = 0;
                        break;
                    case 1:
                        forward = 0;
                        turn = -150 * Time.deltaTime;
                        break;
                    case 2:
                        forward = 0;
                        turn = 150 * Time.deltaTime;
                        break;
                }
                Walk();
                Turn();
            }
        }
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * 20);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, (transform.forward + transform.right) * 20);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, (transform.forward - transform.right) * 20);
    }

    RaycastHit Raycast(Vector3 direction) {
        Ray ray = new Ray(transform.position + Vector3.down * 0.5f, direction);
        Physics.Raycast(ray, out RaycastHit hit, 20);
        return hit;
    }

    void RecordData() {
        RaycastHit hit0 = Raycast(transform.forward);
        RaycastHit hit45 = Raycast(transform.forward + transform.right);
        RaycastHit hit215 = Raycast(transform.forward - transform.right);

        if (!isAIAgent) {
            trainingData.AddData(hit0.collider != null ? 1 : 0,
                        hit45.collider != null ? 1 : 0,
                        hit215.collider != null ? 1 : 0,
                        hit0.collider != null ? Vector3.Distance(transform.position, hit0.collider.transform.position) : -1,
                        hit45.collider != null ? Vector3.Distance(transform.position, hit45.collider.transform.position) : -1,
                        hit215.collider != null ? Vector3.Distance(transform.position, hit215.collider.transform.position) : -1,
                        forward > 0 ? 1 : 0,
                        turn < 0 ? 1 : 0,
                        turn > 0 ? 1 : 0);
        } else {
            liveData.AddData(hit0.collider != null ? 1 : 0,
                        hit45.collider != null ? 1 : 0,
                        hit215.collider != null ? 1 : 0,
                        hit0.collider != null ? Vector3.Distance(transform.position, hit0.collider.transform.position) : -1,
                        hit45.collider != null ? Vector3.Distance(transform.position, hit45.collider.transform.position) : -1,
                        hit215.collider != null ? Vector3.Distance(transform.position, hit215.collider.transform.position) : -1,
                        forward > 0 ? 1 : 0,
                        turn < 0 ? 1 : 0,
                        turn > 0 ? 1 : 0);
        }
    }

    void Walk() => transform.Translate(Vector3.forward * (float)forward);

    void Turn() => transform.Rotate(Vector3.up, (float)turn);
}