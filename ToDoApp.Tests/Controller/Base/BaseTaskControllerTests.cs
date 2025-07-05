
using Moq;
using System.Linq.Expressions;
using ToDoApp.Controller;
using ToDoApp.Model;
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

    protected static List<ToDoItem> CreateTestTaskList(string description1, string description2,
        bool isDone1 = false, bool isDone2 = false)
    {
        var task1 = new ToDoItem(Guid.NewGuid(), description1) { IsDone = isDone1 };
        var task2 = new ToDoItem(Guid.NewGuid(), description2) { IsDone = isDone2 };
        return [task1, task2];
    }

    protected static ToDoItem CreateTestTask(string description, bool isDone = false)
    {
        return new ToDoItem(Guid.NewGuid(), description) { IsDone = isDone };
    }

    protected void VerifyMenuPrompt(string prompt)
    {
        _mockMenu.Verify(menu => menu.PrintPrompt(prompt), Times.Once());
    }
    protected void VerifyMenuPrintInfoForTasks(int index, string status, string description)
    {
        _mockMenu.Verify(menu => menu.PrintInfoForTasks(index, status, description), Times.Once());
    }

    protected void VerifyStoreCurrentTasksListCount(int expectedCount)
    {
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.Is<List<ToDoItem>>
            (list => list.Count == expectedCount)), Times.Once());
    }

    protected void VerifyStoreCurrentTasksListNeverCalled()
    {
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.IsAny<List<ToDoItem>>()), Times.Never());
    }

    protected void VerifyServiceMethodNeverCalled(Expression<Action<ITaskService>> expression)
    {
        _mockTaskService.Verify(expression, Times.Never());
    }
    protected void VerifyServiceMethodIsCalled(Expression<Action<ITaskService>> expression)
    {
        _mockTaskService.Verify(expression, Times.Once());
    }
}
