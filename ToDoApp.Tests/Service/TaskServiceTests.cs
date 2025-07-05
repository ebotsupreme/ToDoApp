
using ToDoApp.Shared.Interfaces;
using Moq;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Service;
using ToDoApp.Tests.Service.Base;

namespace ToDoApp.Tests.Service;

public class TaskServiceTests : BaseTaskServiceTests
{
    [Fact]
    public void AddNewTask_ShouldReturnSuccess_WhenDescriptionIsValid()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);
        _mockTaskRepository.Setup(repo => repo.CreateTask(TestConstants.ExpectedDescription))
            .Returns(Result<ToDoItem>.Ok(new ToDoItem(Guid.NewGuid(), TestConstants.ExpectedDescription)));

        // Act
        var service = CreateTaskService();
        var result = service.AddNewTask(TestConstants.ExpectedDescription);

        // Assert
        ShouldSucceedWithExpectedDescription(result, TestConstants.ExpectedDescription);
    }

    [Fact]
    public void AddNewTask_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        // no setup needed

        // Act
        var service = CreateTaskService();
        var result = service.AddNewTask("");

        // Assert
        ShouldFailWhenErrorMessagesStartsWith(result, ErrorMessages.EmptyDescription);
    }

    [Fact]
    public void AddNewTask_ShouldFail_WhenDescriptionIsDuplicate()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([new(Guid.NewGuid(), TestConstants.ExpectedDescription)]);

        // Act
        var service = CreateTaskService();
        var result = service.AddNewTask(TestConstants.ExpectedDescription);

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.DuplicateDescription);
    }

    [Fact]
    public void CompleteExistingTask_ShouldReturnSuccess_WhenTaskIsNotDone()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);

        _mockTaskRepository.Setup(repo => repo.MarkTaskAsDone(task))
            .Returns(() =>
            {
                task.IsDone = true;
                return Result<ToDoItem>.Ok(task);
            });
          
        // Act
        var service = CreateTaskService();
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
        var task = CreateTestTask(TestConstants.ExpectedDescription, true);

        // Act
        var service = CreateTaskService();
        var result = service.CompleteExistingTask(task);

        // Assert
        ShouldFailWhenErrorMessagesStartsWith(result, ErrorMessages.TaskAlreadyDone);
    }

    [Fact]
    public void DeleteExistingTask_ShouldReturnSuccess_WhenTaskIsDeleted()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);
        _mockTaskRepository.Setup(repo => repo.DeleteTask(task))
            .Returns(Result<Unit>.Ok(Unit.Value));

        // Act
        var service = CreateTaskService();
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
        var task = CreateTestTask(TestConstants.ExpectedDescription); 
        var errorMessage = string.Format(ErrorMessages.RepositoryDeleteErrorFormat, task.Id);
        _mockTaskRepository.Setup(repo => repo.DeleteTask(task))
            .Returns(Result<Unit>.Fail(errorMessage));

        // Act
        var service = CreateTaskService();
        var result = service.DeleteExistingTask(task);

        // Assert
        Assert.False(result.Success);
        Assert.StartsWith(ErrorMessages.RepositoryDeleteErrorFormat.Split('{')[0], result.ErrorMessage);
    }

    [Fact]
    public void UpdateExistingTask_ShouldSucceed_WhenTaskIsUpdated()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);
        _mockTaskRepository.Setup(repo => repo.UpdateTask(It.Is<ToDoItem>(t => t.Id == task.Id),
            TestConstants.UpdatedDescription))
            .Returns((ToDoItem t, string newDesc) =>
            {
                t.Description = newDesc;
                return Result<ToDoItem>.Ok(t);
            });

        // Act
        var service = CreateTaskService();
        var result = service.UpdateExistingTask(task, TestConstants.UpdatedDescription);

        // Assert
        ShouldSucceedWithExpectedDescription(result, TestConstants.UpdatedDescription);
    }

    [Fact]
    public void UpdateExistingTask_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        // No setup needed.

        // Act
        var service = CreateTaskService();
        var result = service.AddNewTask("");

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.EmptyDescription);
    }

    [Fact]
    public void UpdateExistingTask_ShouldFail_WhenDescriptionIsDuplicate()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);
        var existingTask = CreateTestTask(TestConstants.UpdatedDescription);
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([existingTask]);

        // Act
        var service = CreateTaskService();
        var result = service.UpdateExistingTask(task, TestConstants.UpdatedDescription);

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.DuplicateDescription);
    }

    [Fact]
    public void GetAllExistingTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var tasks = CreateTestTasksIReadOnlyList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription);
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns(tasks);

        // Act
        var service = CreateTaskService();
        var result = service.GetAllExistingTasks();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, t =>  t.Description == TestConstants.ExpectedDescription);
        Assert.Contains(tasks, t =>  t.Description == TestConstants.UpdatedDescription);
    }

    [Fact]
    public void GetAllExistingTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([]);

        // Act
        var service = CreateTaskService();
        var result = service.GetAllExistingTasks();

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.NoTasks);
    }

    [Fact]
    public void GetIncompleteTasks_ShouldSucceed_WhenTasksExist()
    {
        //// Arrange
        var tasks = CreateTestTasksIReadOnlyList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription, false, false);
        _mockTaskRepository.Setup(repo => repo.GetIncompleteTasks())
            .Returns(tasks);

        // Act
        var service = CreateTaskService();
        var result = service.GetAllIncompleteTasks();

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
        var service = CreateTaskService();
        var result = service.GetAllIncompleteTasks();

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.NoIncompleteTasks);
    }

    [Fact]
    public void GetAllCompletedTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var tasks = CreateTestTasksIReadOnlyList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription, true, true);
        _mockTaskRepository.Setup(repo => repo.GetCompletedTasks())
            .Returns(tasks);

        // Act
        var service = CreateTaskService();
        var result = service.GetAllCompletedTasks();

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
        var service = CreateTaskService();
        var result = service.GetAllCompletedTasks();

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.NoCompletedTasks);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void GetTaskByIndex_ShouldSucceed_WhenIndexIsValid_AndTaskExists(int index)
    {
        // Arrange
        var task1 = CreateTestTask(TestConstants.ExpectedDescription);
        var task2 = CreateTestTask(TestConstants.UpdatedDescription);
        List<ToDoItem> tasks = [task1, task2];
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns([task1, task2]);

        // Act
        var service = CreateTaskService();
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
        var tasks = CreateTestTasksIReadOnlyList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription);
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns(tasks);

        // Act
        var service = CreateTaskService();
        var result = service.GetTaskByIndex(index);

        // Assert
        ShouldFailWhenErrorMessagesStartsWith(result, ErrorMessages.TaskOutOfRangeFormat.Split('{')[0]);
    }

    [Fact]
    public void GetTaskByIndex_ShouldFail_WhenTaskIsNull()
    {
        // Arrange
        ToDoItem?[] tasks = [null];
        _mockTaskRepository.Setup(repo => repo.GetAllTasks())
            .Returns(tasks!);

        // Act
        var service = CreateTaskService();
        var result = service.GetTaskByIndex(0);

        // Assert
        ShouldFailWhenErrorMessagesStartsWith(result, ErrorMessages.TaskNotFound);
    }

    [Fact]
    public void StoreCurrentTasksList_ShouldSucceed_WhenCurrentTaskListExists()
    {
        // Arrange
        List<ToDoItem> tasks = CreateTestTasksList(TestConstants.ExpectedDescription, TestConstants.UpdatedDescription);

        // Act
        var service = CreateTaskService();
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
        var service = CreateTaskService();
        service.StoreCurrentTasksList(tasks);

        // Assert
        _mockTaskRepository.Verify(repo => repo.StoreCurrentTasks(It.IsAny<List<ToDoItem>>()), Times.Never());
    }

    [Fact]
    public void GetCurrentTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        List<ToDoItem> currentTasks = CreateTestTasksList(TestConstants.ExpectedDescription, 
            TestConstants.UpdatedDescription);
        _mockTaskRepository.Setup(repo => repo.GetCurrentTasks())
            .Returns(currentTasks);

        // Act
        var service = CreateTaskService();
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
        var service = CreateTaskService();
        var result = service.GetCurrentTasks();

        // Assert
        ShouldFailWhenErrorMessagesAreEqual(result, ErrorMessages.NoTasks);
    }
}
