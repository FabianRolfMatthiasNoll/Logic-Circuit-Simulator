using UnityEngine.Serialization;

namespace Classes {
    using UnityEngine;

    [System.Serializable]
    public class InputConnection {
        public Renderer RendererObject { get; private set; }
        public NewGate ConnectedGate { get; set; }

        public bool logicLevel;

        public InputConnection(Renderer rendererObject, NewGate connectedGate) {
            this.RendererObject = rendererObject;
            this.ConnectedGate = connectedGate;
            this.logicLevel = false;
        }
    }
}