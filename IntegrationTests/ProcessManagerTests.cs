using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace IptiQTest.IntegrationTests
{
    public class ProcessManagerTests
    {
        /// <summary>
        /// Add a process (1/3) 
        /// </summary>
        [Fact]
        public void TestDefaultProcessManager()
        {
            var processManager = ProcessManager.CreateDefault(2); //max number of processes - 2
            processManager.AddProcess(new Process(1, Priority.Low));
            processManager.AddProcess(new Process(2, Priority.Low));
            
            ///Tests that adding a process returns false, and after this operation, we would still have 2 processes in the list
            Assert.False(processManager.AddProcess(new Process(3, Priority.Low)));

            var processes = processManager.GetRunningProcesses();
            Assert.Equal(2, processes.Count);

            processManager.Kill(1);
            processes = processManager.GetRunningProcesses();
            Assert.Single(processes);
            Assert.Equal(2, processes[0].PID);

        }



        /// <summary>
        /// Add a process - FIFO approach (2/3) 
        /// </summary>
        [Fact]
        public void TestFifoProcessManager()
        {
            var processManager = ProcessManager.CreateFifoProcess(2); //max number of processes - 2
            processManager.AddProcess(new Process(1, Priority.Low));
            processManager.AddProcess(new Process(2, Priority.Low));
            processManager.AddProcess(new Process(3, Priority.Low));

            //check if we have two processes
            var processes = processManager.GetRunningProcesses();
            Assert.Equal(2, processes.Count);

            //check that the oldest one (1) was removed
            Assert.Equal(2, processes[0].PID);
            Assert.Equal(3, processes[1].PID);
        }

        /// <summary>
        /// Add a process - priority based (3/3) 
        /// </summary>
        [Fact]
        public void TestPriorityProcessManager()
        {
            var processManager = ProcessManager.CreatePriorityBased(3); //max number of processes - 2
            processManager.AddProcess(new Process(1, Priority.Medium));
            processManager.AddProcess(new Process(2, Priority.Low));
            processManager.AddProcess(new Process(3, Priority.High));


            //add a low priority process. Since it's not higher than any existing processes - nothing will happen (it's the same as process 2, but not higher)
            var result = processManager.AddProcess(new Process(4, Priority.Low));
            Assert.False(result); //we have an indication that the process was not added

            //add a medium priority process, the low process should be removed (2)
            result = processManager.AddProcess(new Process(4, Priority.Medium));
            Assert.True(result);  //the process was added

            //check the expected running processes -1,3,4, since 2 was lowest amd it was removed
            var processes = processManager.GetRunningProcesses();
            //check that the oldest one (1) was removed
            Assert.Equal(1, processes[0].PID);
            Assert.Equal(3, processes[1].PID);
            Assert.Equal(4, processes[2].PID);

        }

        /// <summary>
        /// Test listing running processes
        /// </summary>
        [Fact]
        public void TestProcessManagerListProcesses()
        {
            var processManager = ProcessManager.CreateDefault(5); //max number of processes - 2
            processManager.AddProcess(new Process(4, Priority.High));
            processManager.AddProcess(new Process(1, Priority.Medium));
            processManager.AddProcess(new Process(2, Priority.Low));
            processManager.AddProcess(new Process(5, Priority.Medium));
            processManager.AddProcess(new Process(3, Priority.High));

            var processes = processManager.GetRunningProcesses(); //in order that they were added
            Assert.Equal(4, processes[0].PID);
            Assert.Equal(1, processes[1].PID);
            Assert.Equal(2, processes[2].PID);
            Assert.Equal(5, processes[3].PID);
            Assert.Equal(3, processes[4].PID);


            processes = processManager.GetRunningProcessesOrderedByPid(); //in order that they were added
            Assert.Equal(1, processes[0].PID);
            Assert.Equal(2, processes[1].PID);
            Assert.Equal(3, processes[2].PID);
            Assert.Equal(4, processes[3].PID);
            Assert.Equal(5, processes[4].PID);

            processes = processManager.GetRunningProcessesOrderedByPriority(); //in order that they were added
            Assert.Equal(2, processes[0].PID);
            Assert.Equal(1, processes[1].PID);
            Assert.Equal(5, processes[2].PID);
            Assert.Equal(4, processes[3].PID);
            Assert.Equal(3, processes[4].PID);
        }

        /// <summary>
        /// Test listing running processes
        /// </summary>
        [Fact]
        public void TestProcessManagerKillProcesses()
        {
            var processManager = ProcessManager.CreateDefault(6); //max number of processes - 2
            processManager.AddProcess(new Process(4, Priority.High));
            processManager.AddProcess(new Process(1, Priority.Medium));
            processManager.AddProcess(new Process(2, Priority.Low));
            processManager.AddProcess(new Process(5, Priority.Medium));
            processManager.AddProcess(new Process(3, Priority.High));
            processManager.AddProcess(new Process(6, Priority.High));

            processManager.Kill(3);
            var processes = processManager.GetRunningProcesses();
            Assert.Equal(5, processes.Count);
            


            processManager.KillGroup(Priority.High);
            processes = processManager.GetRunningProcesses();
            Assert.Equal(3, processes.Count);
            Assert.Equal(1, processes[0].PID);
            Assert.Equal(2, processes[1].PID);
            Assert.Equal(5, processes[2].PID);


            processManager.KillAll();
            processes = processManager.GetRunningProcesses();
            Assert.Empty(processes);
        }
    }
}
