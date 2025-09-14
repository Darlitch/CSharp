using Model.Enums;

namespace Model;

public class Fork
{
    public string? Owner { get; private set; } = null;
    public ForkState State { get; private set; } = ForkState.Available;

    public void TakeFork(string owner)
    {
        Owner = owner;
        State = ForkState.InUse;
    }

    public void ReleaseFork()
    {
        Owner = null;
        State = ForkState.Available;
    }
}