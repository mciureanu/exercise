using System.Linq;

public class ProcessSchedulerPriorityBased : IProcessScheduler
{
    private int _maxSize;
    private IProcessRepository _repository;

    public ProcessSchedulerPriorityBased(int maxSize, IProcessRepository repository)
    {
        _maxSize = maxSize;
        _repository = repository;
    }

    public bool AddProcess(Process process)
    {
        if (_repository.GetSize() >= _maxSize)
        {
            var lowestPriority = _repository.GetAllSortedByPriority().FirstOrDefault();
            if (process.Priority > lowestPriority.Priority)
                _repository.Remove(lowestPriority.PID);
            else return false;
        }
        
        _repository.Save(process);
        return true;
    }
}
