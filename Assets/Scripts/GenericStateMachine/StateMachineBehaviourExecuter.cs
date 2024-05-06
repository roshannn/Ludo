using System;
using UnityEngine;

public sealed class StateMachineBehaviourExecuter : MonoBehaviour {

    public StateMachineBehaviourService service;

    private void Update() {
        service.StateMachineUpdate?.Invoke();
    }

    private void FixedUpdate() {
        service.StateMachineFixedUpdate?.Invoke();
    }

    private void LateUpdate() {
        service.StateMachineLateUpdate?.Invoke();

    }

}
