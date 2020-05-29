# How to run the project #
In order to run the project, install *.NET core SDK* - https://dotnet.microsoft.com/download. Then run this in command line, in the folder of the project:

**dotnet test**

which will run the tests in IntegrationTests/ProcessManagerTests.cs. My intention was that the tests hopefully are self explanatory, and serves as "evidence that the code does what it's supposed to do".


# Description #
The ProcessManager uses a repository (IProcessRepository interface) and a scheduler (IProcessScheduler interface) in order to store and schedule tasks. ProcessManager uses factory methods to create different types of instances, based on the type of IProcessScheduler. IProcessScheduler gives the possibility to choose the implementation of the algorithm at runtime (strategy pattern). Having each class handle different topics - scheduler it's about scheduling, repository about maintaining state, and ProcessManager as the main interface for process management, ensures the Single Responsibility Principle is satisfied.

The ability to create new strategies for scheduling (IProcessScheduler) or other repositories implementations (IProcessRepository) ensures Open Closed Principle - the process manager is open for extensions,  but closed for modifications. Possible extensions might include repository implemented in a database to support perhaps a distributed process manager, and different other types of schedulers - waiting queue based schedulers for examples (using a queue where processes will wait until another process is killed).


## Assumptions ##
- Killing a process means removing it from the repository (we are not interested by processes that are killed)

## Possible improvements ##
- Multi-threading support. ProcessManager does not support operations in a multi-threaded. 
- Better performance in the in-memory repository (using indexes/sorted lists), right now I didn't spend too much time on that.
