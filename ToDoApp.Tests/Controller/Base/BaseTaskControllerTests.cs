
using Moq;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.Tests.Controller.Base;
public abstract class BaseTaskControllerTests
{
    protected readonly Mock<ITaskService> _mockTaskService = new();
    protected readonly Mock<IMenu> _mockMenu = new();
    protected readonly TaskController _taskController;

    public BaseTaskControllerTests()
    {
        _taskController = new TaskController(_mockTaskService.Object, _mockMenu.Object);
    }

    protected static List<ToDoItem> CreateTestTask(string description, bool isDone = false)
    {
        var task = new ToDoItem(Guid.NewGuid(), description) { IsDone = isDone };
        return [task];
    }

    protected void VerifyMenuPrompt(string prompt)
    {
        _mockMenu.Verify(menu => menu.PrintPrompt(prompt), Times.Once());
    }
    protected void VerifyMenuPrintInfoForTasks(int index, string status, string description)
    {
        _mockMenu.Verify(menu => menu.PrintInfoForTasks(index, status, description), Times.Once());
    }

    protected void VerifyStoreCurrentTasksList(int expectedCount)
    {
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.Is<List<ToDoItem>>
            (list => list.Count == expectedCount)), Times.Once());
    }

    protected void VerifyStoreCurrentTasksListNeverCalled()
    {
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.IsAny<List<ToDoItem>>()), Times.Never());

    }
}
