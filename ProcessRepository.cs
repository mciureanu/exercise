using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the repository where the processes are stored. Depending on our use case. In order to satisfy
/// single responsibility principle - we don't want the process manager, nor the scheduler to handle process
/// information storage. This is handled by the process repository, giving us the freedom to optimize storage 
/// mechanisms, to allow distributted/database storage, to allow at a later time "process manager resume" functionality
/// in case it was abruptly shut down, and so on...
/// </summary>
public interface IProcessRepository 
{
    /// <summary>
    /// Saves a process
    /// </summary>
    /// <param name="process"></param>
    void Save(Process process);
    
    /// <summary>
    /// Gets the process by PID
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    Process GetByPid(int pid);

    /// <summary>
    /// Gets the  process list by date
    /// </summary>
    /// <returns></returns>
    IEnumerable<Process> GetAllSortedByDate();

    IEnumerable<Process> GetAll(Priority priority);

    IEnumerable<Process> GetAllSortedByPriority();

    IEnumerable<Process> GetAllSortedByPid();

    /// <summary>
    /// Get the number of processes in the repository
    /// </summary>
    /// <returns></returns>
    int GetSize();
    
    /// <summary>
    /// Removes process with given PID from the repository
    /// </summary>
    /// <param name="pid"></param>
    void Remove(int pid);
    
}

/// <summary>
/// An in-memory, not optimized repository (no indexes or structures to support fast retrieval, sorting, etc...)
/// </summary>
public class InMemoryNotOptimizedProcessRepository : IProcessRepository
{
    private List<Process> _processes = new List<Process>();

    public IEnumerable<Process> GetAll(Priority priority)
    {
        return _processes.Where(x=>x.Priority == priority);
    }

    public IEnumerable<Process> GetAllSortedByDate()
    {
        return new List<Process>(_processes);
    }

    public IEnumerable<Process> GetAllSortedByPriority()
    {
        return _processes.OrderBy(x=>x.Priority); 
    }


    public IEnumerable<Process> GetAllSortedByPid()
    {
        return _processes.OrderBy(x => x.PID); 
    }


    public Process GetByPid(int pid)
    {
        return _processes.FirstOrDefault(x=>x.PID == pid);
    }

    public int GetSize()
    {
        return _processes.Count;
    }

    public void Remove(int pid)
    {
        var process = GetByPid(pid);
        if (process == null) 
            throw new InvalidOperationException($"Can't find process with pid {0}");
        _processes.Remove(process);
    }

    public void Save(Process process)
    {
        //maybe check if we already have the same pid?
        if (GetByPid(process.PID) != null) 
            throw new InvalidOperationException("Process with same pid already exists!");
        _processes.Add(process);
    }


}
