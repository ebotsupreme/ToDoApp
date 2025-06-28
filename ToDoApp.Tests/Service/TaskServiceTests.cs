
using ToDoApp.Shared.Interfaces;
using Xunit;
using Moq;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Service;

namespace ToDoApp.Tests.Service;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository = new();

    private const string ExpectedDescription = "Take out trash.";

    [Fact]
    public void AddNewTask_ShouldReturnSuccess_WhenDescriptionIsValid()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);

        _mockTaskRepository.Setup(repo => repo.CreateTask(ExpectedDescription))
            .Returns(Result<ToDoItem>.Ok(new ToDoItem(Guid.NewGuid(), ExpectedDescription)));

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask(ExpectedDescription);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.Equal(ExpectedDescription, result.Data!.Description);
    }

    [Fact]
    public void AddNewTask_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        // no setup needed

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask("");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.StartsWith(ErrorMessages.EmptyDescription, result.ErrorMessage);
    }

    [Fact]
    public void AddNewTask_ShouldFail_WhenDescriptionIsDuplicate()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([new(Guid.NewGuid(), ExpectedDescription)]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask(ExpectedDescription);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.DuplicateDescription, result.ErrorMessage);
    }
}
