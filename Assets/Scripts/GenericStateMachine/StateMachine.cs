using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using Zenject;

public sealed class StateMachine<T, U> where U : Enum {

    private IState<T> currentState;
    private T stateObject = default;
    public IState<T> getCurrentState => currentState;

    private readonly Dictionary<U, IState<T>> states = new Dictionary<U, IState<T>>();
    [Inject] DiContainer _container;


    private StateMachine(DiContainer container, T stateObject) {
        if (!typeof(U).IsEnum) {
            throw new ArgumentException($"{nameof(U)} must be an enumerated type");
        }
        this.stateObject = stateObject;
    }
    
    public void ChangeState(U stateName) {
        IState<T> newState = GetState(stateName);
        if (newState == null) {
            Debug.LogError($"Failed to change state to {stateName}.");
            return;
        }
        currentState?.OnStateExit(stateObject);
        currentState = newState;
        currentState?.OnStateEnter(stateObject);
    }
    /// <summary>
    /// Instantiates the state and injects it with the container.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private IState<T> GetState(U state) {
        if (!states.TryGetValue(state, out IState<T> newState)) {
            Type type = Type.GetType($"{typeof(T).Namespace}.{state}");
            if (type == null) {
                Debug.LogError($"Failed to get state {state}. Class not found.");
                return null;
            }
            newState = (IState<T>)Activator.CreateInstance(type);
            _container.Inject(newState);
            states.Add(state, newState);
        }
        return states[state];
    }
    /// <summary>
    /// Use this to instantiate the state machine.
    /// </summary>
    /// <param name="_container"></param>
    /// <param name="currObject"></param>
    /// <returns></returns>
    public static StateMachine<T, U> InstantiateStateMachine(T currObject) {
        return DiContainerAPI.Container.Instantiate<StateMachine<T, U>>(new object[] { currObject });
    }
    
}
