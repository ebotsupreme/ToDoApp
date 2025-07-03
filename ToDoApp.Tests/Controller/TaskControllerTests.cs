
using Moq;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.Service;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.Tests.Controller;
public class TaskControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService = new();
    private readonly TaskController _taskController;

    private const string ExpectedDescription = "Take out trash.";
    private const string UpdatedDescription = "Drink more water.";

    public TaskControllerTests()
    {
        _taskController = new TaskController(_mockTaskService.Object);
    }

    [Fact]
    public void ViewAllTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), UpdatedDescription);
        List<ToDoItem> tasks = [task1, task2];
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(tasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        // Here you need to verify what ViewAllTasks is expected to do,
        // e.g. call Menu.PrintPrompt or _mockTaskService.GetAllExistingTasks()

    }


}
