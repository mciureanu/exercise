using System;
using System.Collections.Generic;
using System.Linq;

public class ProcessManager
{
    private readonly IProcessRepository _repository;
    private readonly IProcessScheduler _processScheduler;

    public ProcessManager(IProcessRepository repository, IProcessScheduler processScheduler)
    {
        _repository = repository;
        _processScheduler = processScheduler;
    }

    /// <summary>
    /// Factory method to create the default process manager
    /// </summary>
    /// <param name="maxItems"></param>
    /// <returns></returns>
    public static ProcessManager CreateDefault(int maxItems)
    {
        var repository = new InMemoryNotOptimizedProcessRepository();
        var processScheduler = new ProcessSchedulerDefault(maxItems, repository); 
        return new ProcessManager(repository, processScheduler);
    }

    /// <summary>
    /// Factory method to created fifo based process
    /// </summary>
    /// <param name="maxItems"></param>
    /// <returns></returns>
    public static ProcessManager CreateFifoProcess(int maxItems)
    {
        var repository = new InMemoryNotOptimizedProcessRepository();
        var processScheduler = new ProcessSchedulerFifo(maxItems, repository);
        return new ProcessManager(repository, processScheduler);
    }

    /// <summary>
    /// Factory method to create
    /// </summary>
    /// <param name="maxItems"></param>
    /// <returns></returns>
    public static ProcessManager CreatePriorityBased(int maxItems)
    {
        var repository = new InMemoryNotOptimizedProcessRepository();
        var processScheduler = new ProcessSchedulerPriorityBased(maxItems, repository);
        return new ProcessManager(repository, processScheduler);
    }



    public bool AddProcess(Process process)
    {
        return _processScheduler.AddProcess(process);
    }

    public void Kill(int pid)
    {
        var process = _repository.GetByPid(pid);
        if (process == null)
            throw new InvalidOperationException($"Cannot find process with pid {pid}");
        process.Kill();
        _repository.Remove(pid);
    }

    public void KillGroup (Priority priority)
    {
        var group = _repository.GetAll(priority).ToList();
        foreach (var process in group)
        {
            process.Kill();
            _repository.Remove(process.PID);
        }
    }

    public void KillAll()
    {
        foreach (var process in _repository.GetAllSortedByDate())
        {
            process.Kill();
            _repository.Remove(process.PID);
        }
    }

    public List<Process> GetRunningProcesses ()
    {
        return _repository.GetAllSortedByDate().ToList();
    }

    public List<Process> GetRunningProcessesOrderedByPriority()
    {
        return _repository.GetAllSortedByPriority().ToList();
    }

    public List<Process> GetRunningProcessesOrderedByPid()
    {
        return _repository.GetAllSortedByPid().ToList();
    }

}

