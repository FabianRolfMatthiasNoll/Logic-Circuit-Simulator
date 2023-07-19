using UnityEngine;

namespace Classes {
    public class GateConnection {
        private LineRenderer line { get; set; }
        private NewGate sourceGate { get; set; }
        private NewGate targetGate { get; set; }

        public GateConnection(LineRenderer line, NewGate sourceGate, NewGate targetGate) {
            this.line = line;
            this.sourceGate = sourceGate;
            this.targetGate = targetGate;
        }

        public bool AreGatesConnected(NewGate source, NewGate target) {
            return (sourceGate == source && targetGate == target) ||
                   (sourceGate == target && targetGate == source);
        }
    }
}