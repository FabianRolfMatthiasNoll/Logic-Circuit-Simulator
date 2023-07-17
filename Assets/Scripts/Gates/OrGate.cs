using System;

namespace Gates
{
    public class OrGate : Gate
    {
        public override bool GetOutput()
        {
            foreach (var input in inputs)
            {
                if (input.logicLevel == true) return true;
            }

            return false;
        }
    }
}