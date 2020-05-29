/// <summary>
/// The process scheduler decides how to schedule a new process. To satisfy Single Responsibility Principle, 
/// it should contain only the logic of scheduling - in our case if another process should be removed or not, 
/// so that the new process can be scheduled, and in this case - which one. This is an implementation 
/// of the strategy pattern. Separating only the logic might give nice benefits - for example ability to switch
/// the strategy at runtime, from FIFO based to priority based. 
/// 
/// Other examples of schedulers might decide to use a waiting queue, 
/// and schedule at a later time the process, or might decide to boost the capacity for 5 minutes, and remove 
/// processes only after this time has passed, and so on.
/// </summary>
public interface IProcessScheduler
{
    public bool AddProcess(Process process);
}
