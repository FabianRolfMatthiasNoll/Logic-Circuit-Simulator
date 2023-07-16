
public class AndGate : Gate
{
    public override bool GetOutput()
    {
        if (inputs.Count == 0) return false;
        
        foreach (var input in inputs)
        {
            if (!input.GetOutput()) return false;
        }

        return true;
    }
}
