using UnityEngine;

namespace Classes
{
    public class GateConnection
    {
        public LineRenderer line;
        public Gate sourceGate;
        public Gate targetGate;

        public GateConnection(LineRenderer line, Gate sourceGate, Gate targetGate)
        {
            this.line = line;
            this.sourceGate = sourceGate;
            this.targetGate = targetGate;
        }
    }
}