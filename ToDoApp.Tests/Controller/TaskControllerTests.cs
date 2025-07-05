
using Moq;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Tests.Controller.Base;
using ToDoApp.View;

namespace ToDoApp.Tests.Controller;
public class TaskControllerTests : BaseTaskControllerTests
{
    [Fact]
    public void ViewAllTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var allTasks = CreateTestTaskList(TestConstants.ExpectedDescription, 
            TestConstants.UpdatedDescription, false, true);
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(allTasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        VerifyMenuPrompt(PromptsAndMessages.YourTasks);
        VerifyStoreCurrentTasksListCount(2);
        VerifyMenuPrintInfoForTasks(1, "[ ]", TestConstants.ExpectedDescription);
        VerifyMenuPrintInfoForTasks(2, "[X]", TestConstants.UpdatedDescription);
    }

    [Fact]
    public void ViewAllTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoTasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        VerifyMenuPrompt(ErrorMessages.NoTasks);
        VerifyStoreCurrentTasksListNeverCalled();
    }

    [Fact]
    public void ViewInCompleteTasks_ShouldSucceed_When_IncompleteTasksExists()
    {
        // Arrange
        var incompleteTasks = CreateTestTaskList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription);
        _mockTaskService.Setup(service => service.GetAllIncompleteTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(incompleteTasks));

        // Act
        _taskController.ViewIncompleteTasks();

        // Assert
        VerifyMenuPrompt(PromptsAndMessages.YourIncompleteTasks);
        VerifyStoreCurrentTasksListCount(2);
        VerifyMenuPrintInfoForTasks(1, "[ ]", TestConstants.ExpectedDescription);
        VerifyMenuPrintInfoForTasks(2, "[ ]", TestConstants.UpdatedDescription);
    }

    [Fact]
    public void ViewIncompleteTasks_ShouldFail_WhenNoIncompleteTasksExist()
    {
        // Arrange
        _mockTaskService.Setup(service => service.GetAllIncompleteTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoIncompleteTasks));

        // Act
        _taskController.ViewIncompleteTasks();

        // Assert
        VerifyMenuPrompt(ErrorMessages.NoIncompleteTasks);
        VerifyStoreCurrentTasksListNeverCalled();
    }

    [Fact]
    public void ViewCompletedTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var completeTasks = CreateTestTaskList(TestConstants.ExpectedDescription,
            TestConstants.UpdatedDescription, true, true);
        _mockTaskService.Setup(service => service.GetAllCompletedTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(completeTasks));

        // Act
        _taskController.ViewCompletedTasks();

        // Assert
        VerifyMenuPrompt(PromptsAndMessages.YourCompletedTasks);
        VerifyStoreCurrentTasksListCount(2);
        VerifyMenuPrintInfoForTasks(1, "[X]", TestConstants.ExpectedDescription);
        VerifyMenuPrintInfoForTasks(2, "[X]", TestConstants.UpdatedDescription);
    }

    [Fact]
    public void ViewCompletedTasks_ShouldFail_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskService.Setup(service => service.GetAllCompletedTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoTasks));

        // Act
        _taskController.ViewCompletedTasks();

        // Assert
        VerifyMenuPrompt(ErrorMessages.NoTasks);
        VerifyStoreCurrentTasksListNeverCalled();
    }

    [Fact]
    public void AddTask_ShouldSucceed_WhenInputIsValid()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);
        _mockMenu.Setup(menu => menu.UserInput()).Returns(TestConstants.ExpectedDescription);
        _mockTaskService.Setup(service => service.AddNewTask(TestConstants.ExpectedDescription))
            .Returns(Result<ToDoItem>.Ok(task));

        // Act
        _taskController.AddTask();

        // Assert
        VerifyMenuPrompt(PromptsAndMessages.TaskAdded + task.Description);
    }

    [Fact]
    public void AddTask_ShouldFail_WhenInputIsNotValid()
    {
        // Arrange
        _mockMenu.Setup(menu => menu.UserInput()).Returns("");

        // Act
        _taskController.AddTask();

        // Assert
        VerifyMenuPrompt(ErrorMessages.EmptyTask);
        VerifyServiceMethodNeverCalled(service => service.AddNewTask(It.IsAny<string>()));
    }

    [Fact]
    public void CompleteTask_ShouldSucceed_WhenTaskExists()
    {
        // Arrange
        var task = CreateTestTask(TestConstants.ExpectedDescription);
        _mockTaskService.Setup(service => service.GetCurrentTasks()).Returns(
            Result<IReadOnlyList<ToDoItem>>.Ok([task]));
        // index increment by + 1
        _mockMenu.Setup(menu => menu.UserInput()).Returns("1");
        _mockTaskService.Setup(service => service.CompleteExistingTask(task)).Returns(Result<ToDoItem>.Ok(task));

        // Act
        _taskController.CompleteTask();

        // Assert
        VerifyMenuPrompt(PromptsAndMessages.TaskMarkedDone);
        VerifyServiceMethodIsCalled(service => service.CompleteExistingTask(task));
    }
}
