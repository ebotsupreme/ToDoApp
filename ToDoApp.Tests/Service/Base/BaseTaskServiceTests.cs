using Moq;
using ToDoApp.Model;
using ToDoApp.Service;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.Tests.Service.Base;
public abstract class BaseTaskServiceTests
{
    protected readonly Mock<ITaskRepository> _mockTaskRepository = new();

    protected TaskService CreateTaskService()
    {
        return new TaskService(_mockTaskRepository.Object);
    }

    protected static void ShouldSucceedWithExpectedDescription(Result<ToDoItem> result, string expectedDescription)
    {
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.Equal(expectedDescription, result.Data!.Description);
    }

    protected static void ShouldFailWhenErrorMessagesStartsWith(Result<ToDoItem> result, string expectedErrorMessage)
    {
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.StartsWith(expectedErrorMessage, result.ErrorMessage);
    }

    protected static void ShouldFailWhenErrorMessagesAreEqual<T>(Result<T> result, string ExpectedErrorMessages)
    {
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ExpectedErrorMessages, result.ErrorMessage);
    }

    protected static ToDoItem CreateTestTask(string description, bool isDone = false)
    {
        var task = new ToDoItem(Guid.NewGuid(), description) { IsDone = isDone };
        return task;
    }

    protected static List<ToDoItem> CreateTestTasksList(string description1, string description2,
        bool isDone1 = false, bool isDone2 = false)
    {
        var task1 = CreateTestTask(description1, isDone1);
        var task2 = CreateTestTask(description2, isDone2);
        
        return [task1, task2];
    }

    protected static IReadOnlyList<ToDoItem> CreateTestTasksIReadOnlyList(string description1, string description2,
        bool isDone1 = false, bool isDone2 = false)
    {
        var task1 = CreateTestTask(description1, isDone1);
        var task2 = CreateTestTask(description2, isDone2);

        return new List<ToDoItem>() { task1, task2 }.AsReadOnly();
    }
}

