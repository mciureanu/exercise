using System;

public class Process
{
    public int PID { get; private set; }

    public Priority Priority { get; private set; }

    public Process(int pID, Priority priority)
    {
        PID = pID;
        Priority = priority;
    }

    public void Kill()
    {
        Console.WriteLine($"Process {PID} killed");
    }
}
