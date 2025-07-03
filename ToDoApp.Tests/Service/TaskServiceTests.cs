
using ToDoApp.Shared.Interfaces;
using Moq;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Service;

namespace ToDoApp.Tests.Service;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository = new();

    [Fact]
    public void AddNewTask_ShouldReturnSuccess_WhenDescriptionIsValid()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);

        _mockTaskRepository.Setup(repo => repo.CreateTask(TestConstants.ExpectedDescription))
            .Returns(Result<ToDoItem>.Ok(new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription)));

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask(TestConstants.ExpectedDescription);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.Equal(TestConstants.ExpectedDescription, result.Data!.Description);
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
            .Returns([new(Guid.NewGuid(), TestConstants.ExpectedDescription)]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask(TestConstants.ExpectedDescription);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.DuplicateDescription, result.ErrorMessage);
    }

    [Fact]
    public void CompleteExistingTask_ShouldReturnSuccess_WhenTaskIsNotDone()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);

        _mockTaskRepository.Setup(repo => repo.MarkTaskAsDone(task))
            .Returns(() =>
            {
                task.IsDone = true;
                return Result<ToDoItem>.Ok(task);
            });
          
        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.CompleteExistingTask(task);
        
        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.True(result.Data!.IsDone);
    }

    [Fact]
    public void CompleteExistingTask_ShouldFail_WhenTaskIsDone()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription) { IsDone = true };

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.CompleteExistingTask(task);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.StartsWith(ErrorMessages.TaskAlreadyDone, result.ErrorMessage);
    }

    [Fact]
    public void DeleteExistingTask_ShouldReturnSuccess_WhenTaskIsDeleted()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);

        _mockTaskRepository.Setup(repo => repo.DeleteTask(task))
            .Returns(Result<Unit>.Ok(Unit.Value));

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.DeleteExistingTask(task);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result);
        Assert.Equal(Unit.Value, result.Data);
    }

    [Fact]
    public void DeleteExistingTask_ShouldFail_WhenRepositoryThrows()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var errorMessage = string.Format(ErrorMessages.RepositoryDeleteErrorFormat, task.Id);

        _mockTaskRepository.Setup(repo => repo.DeleteTask(task))
            .Returns(Result<Unit>.Fail(errorMessage));

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.DeleteExistingTask(task);

        // Assert
        Assert.False(result.Success);
        Assert.StartsWith(ErrorMessages.RepositoryDeleteErrorFormat.Split('{')[0], result.ErrorMessage);
    }

    [Fact]
    public void UpdateExistingTask_ShouldSucceed_WhenTaskIsUpdated()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);

        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);

        _mockTaskRepository.Setup(repo => repo.UpdateTask(It.Is<ToDoItem>(t => t.Id == task.Id), TestConstants.UpdatedDescription))
            .Returns((ToDoItem t, string newDesc) =>
            {
                t.Description = newDesc;
                return Result<ToDoItem>.Ok(t);
            });

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.UpdateExistingTask(task, TestConstants.UpdatedDescription);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.Equal(TestConstants.UpdatedDescription, result.Data!.Description);
    }

    [Fact]
    public void UpdateExistingTask_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        // No setup needed.

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.AddNewTask("");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.EmptyDescription, result.ErrorMessage);
    }

    [Fact]
    public void UpdateExistingTask_ShouldFail_WhenDescriptionIsDuplicate()
    {
        // Arrange
        var task = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var existingTask = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);

        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([existingTask]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.UpdateExistingTask(task, TestConstants.UpdatedDescription);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.DuplicateDescription, result.ErrorMessage);
    }

    [Fact]
    public void GetAllExistingTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);

        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([task1, task2]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllExistingTasks();
        var tasks = result.Data!.ToList();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, t =>  t.Description == TestConstants.ExpectedDescription);
        Assert.Contains(tasks, t =>  t.Description == TestConstants.UpdatedDescription);
    }

    [Fact]
    public void GetAllExistingTasks_ShouldFail_WhenNoTasksExis()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllExistingTasks();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.NoTasks, result.ErrorMessage);
    }

    [Fact]
    public void GetIncompleteTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription) { IsDone = false };
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription) { IsDone = false };
        _mockTaskRepository.Setup(repo => repo.GetIncompleteTasks())
            .Returns([task1, task2]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllIncompleteTasks();
        var tasks = result.Data!.ToList();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, t => t.Description == TestConstants.ExpectedDescription);
        Assert.Contains(tasks, t => t.Description == TestConstants.UpdatedDescription);
        Assert.All(tasks, t => Assert.False(t.IsDone));
        Assert.All(tasks, t => Assert.False(t.IsDone));
    }

    [Fact]
    public void GetIncompleteTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetIncompleteTasks())
            .Returns([]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllIncompleteTasks();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.NoIncompleteTasks, result.ErrorMessage);
    }

    [Fact]
    public void GetAllCompletedTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription) { IsDone = true };
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription) { IsDone = true };
        _mockTaskRepository.Setup(repo => repo.GetCompletedTasks())
            .Returns([task1, task2]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllCompletedTasks();
        var tasks = result.Data!.ToList();

        // Assert
        Assert.True(result.Success);
        Assert.All(tasks, t => Assert.True(t.IsDone));
    }   

    [Fact]
    public void GetAllCompletedTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetCompletedTasks())
            .Returns([]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetAllCompletedTasks();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.NoCompletedTasks, result.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void GetTaskByIndex_ShouldSucceed_WhenIndexIsValid_AndTaskExists(int index)
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);
        List<ToDoItem> tasks = [task1, task2];
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([task1, task2]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetTaskByIndex(index);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(tasks[index], result.Data);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(-1)]
    public void GetTaskByIndex_ShouldFail_WhenIndexIsOutOfBounds(int index)
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([task1, task2]);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetTaskByIndex(index);
        
        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.StartsWith(ErrorMessages.TaskOutOfRangeFormat.Split('{')[0], result.ErrorMessage);
    }

    [Fact]
    public void GetTaskByIndex_ShouldFail_WhenTaskIsNull()
    {
        // Arrange
        ToDoItem?[] tasks = [null];
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns(tasks!);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetTaskByIndex(0);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.StartsWith(ErrorMessages.TaskNotFound, result.ErrorMessage);
    }

    [Fact]
    public void StoreCurrentTasksList_ShouldSucceed_WhenCurrentTaskListExists()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);
        List<ToDoItem> tasks = [task1, task2];
        
        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        service.StoreCurrentTasksList(tasks);

        // Assert
        _mockTaskRepository.Verify(repo => repo.StoreCurrentTasks(tasks), Times.Once());
    }

    public static TheoryData<List<ToDoItem>?> EmptyTaskLists =>
        [
            null,
            []
        ];

    [Theory]
    [MemberData(nameof(EmptyTaskLists))]
    public void StoreCurrentTasksList_ShouldFail_WhenNoTasksExists(List<ToDoItem>? tasks)
    {
        // Arrange
        // No setup needed

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        service.StoreCurrentTasksList(tasks);

        // Assert
        _mockTaskRepository.Verify(repo => repo.StoreCurrentTasks(It.IsAny<List<ToDoItem>>()), Times.Never());
    }

    [Fact]
    public void GetCurrentTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var task1 = new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription);
        var task2 = new ToDoItem(Guid.NewGuid(), TestConstants.UpdatedDescription);
        List<ToDoItem> currentTasks = [task1, task2];
        _mockTaskRepository.Setup(repo => repo.GetCurrentTasks())
            .Returns(currentTasks);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetCurrentTasks();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data!);
        Assert.Contains(result.Data, t => t.Description == TestConstants.ExpectedDescription);
        Assert.Contains(result.Data, t => t.Description == TestConstants.UpdatedDescription);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public void GetCurrentTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        var tasks = new List<ToDoItem>();
        _mockTaskRepository.Setup(repo => repo.GetCurrentTasks())
            .Returns(tasks);

        // Act
        var service = new TaskService(_mockTaskRepository.Object);
        var result = service.GetCurrentTasks();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ErrorMessages.NoTasks, result.ErrorMessage);
    }
}
