using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

    public sealed class StateMachineBehaviourService : IInitializable, ILateDisposable {
        private StateMachineBehaviourExecuter executer = null;

        public Action StateMachineUpdate;
        public Action StateMachineLateUpdate;
        public Action StateMachineFixedUpdate;

        public StateMachineBehaviourService() {
            GameObject newExecuter = new GameObject("StateMachineBehaviourExecuter");
            executer = newExecuter.AddComponent<StateMachineBehaviourExecuter>();
        }


        public void Initialize() {
            executer.service = this;
            GameObject.DontDestroyOnLoad(executer);
        }
        public void LateDispose() {
            executer.service = null;
            executer = null;
        }
    } 
