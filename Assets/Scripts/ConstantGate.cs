public class ConstantGate : Gate
{
    public bool ConstantOutput { get; set; }

    public ConstantGate(bool output)
    {
        ConstantOutput = output;
    }

    public override bool GetOutput()
    {
        return ConstantOutput;
    }
}
