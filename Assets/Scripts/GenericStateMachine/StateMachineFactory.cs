using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class StateMachineFactory<T, U> where U : Enum {
    private readonly Dictionary<U, IState<T>> states = new Dictionary<U, IState<T>>();

    [Inject] DiContainer _container;

    public IState<T> GetState(U state) {
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
}
