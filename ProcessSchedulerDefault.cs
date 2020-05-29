/// <summary>
/// Default implementation of the scheduler, that 
/// </summary>
public class ProcessSchedulerDefault : IProcessScheduler
{
    private int _maxSize;
    private IProcessRepository _repository;

    public ProcessSchedulerDefault(int maxSize, IProcessRepository repository)
    {
        _maxSize = maxSize;
        _repository = repository;
    }

    public bool AddProcess(Process process)
    {
        if (_repository.GetSize() >= _maxSize)
            return false;
        _repository.Save(process);
        return true;
    }
}
