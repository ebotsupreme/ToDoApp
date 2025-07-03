
using Moq;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.Tests.Controller;
public class TaskControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService = new();
    private readonly Mock<IMenu> _mockMenu = new();
    private readonly TaskController _taskController;

    public TaskControllerTests()
    {
        _taskController = new TaskController(_mockTaskService.Object, _mockMenu.Object);
    }

    [Fact]
    public void ViewAllTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription) { IsDone = false };
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription) { IsDone = false };
        List<ToDoItem> tasks = [task1, task2];
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(tasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        _mockMenu.Verify(menu => menu.PrintPrompt(PromtsAndMessages.YourTasks), Times.Once());
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.Is<List<ToDoItem>>(list => list.Count == 2)), Times.Once());
        _mockMenu.Verify(menu => menu.PrintInfoForTasks(1, "[ ]", TestConstants.ExpectedDescription), Times.Once());
        _mockMenu.Verify(menu => menu.PrintInfoForTasks(2, "[ ]", TestConstants.UpdatedDescription), Times.Once());
    }

    [Fact]
    public void ViewAllTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        List<ToDoItem> tasks = [];
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoTasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        _mockMenu.Verify(menu => menu.PrintPrompt(ErrorMessages.NoTasks), Times.Once());
        _mockTaskService.Verify(service => service.StoreCurrentTasksList(It.IsAny<List<ToDoItem>>()), Times.Never());
    }
}
