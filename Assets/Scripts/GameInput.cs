using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private RunnerInputActions runnerInputActions;
    private void Awake() {
        runnerInputActions = new RunnerInputActions();
        runnerInputActions.Runner.Enable();
    }
    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = runnerInputActions.Runner.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
