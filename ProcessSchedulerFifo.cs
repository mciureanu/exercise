using System.Linq;

/// <summary>
/// Implementation of the FIFO based scheduler. It uses the process repository to retrieve and remove the
/// oldest process in case the max capacity has been reached.
/// </summary>
public class ProcessSchedulerFifo : IProcessScheduler
{
    private int _maxSize;
    private IProcessRepository _repository;

    public ProcessSchedulerFifo(int maxSize, IProcessRepository repository)
    {
        _maxSize = maxSize;
        _repository = repository;
    }

    public bool AddProcess(Process process)
    {
        if (_repository.GetSize() >= _maxSize)
        {
            var oldest = _repository.GetAllSortedByDate().FirstOrDefault();
            _repository.Remove(oldest.PID);
        }
        _repository.Save(process);
        return true;
    }
}
