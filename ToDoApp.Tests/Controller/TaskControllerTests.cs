
using Moq;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Tests.Controller.Base;

namespace ToDoApp.Tests.Controller;
public class TaskControllerTests : BaseTaskControllerTests
{
    [Fact]
    public void ViewAllTasks_ShouldSucceed_WhenTasksExist()
    {
        // Arrange
        var incompleteTasks = CreateTestTask(TestConstants.ExpectedDescription, false);
        var completeTasks = CreateTestTask(TestConstants.UpdatedDescription, true);
        var allTasks = incompleteTasks.Concat(completeTasks).ToList();
        _mockTaskService.Setup(service => service.GetAllExistingTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(allTasks));

        // Act
        _taskController.ViewAllTasks();

        // Assert
        VerifyMenuPrompt(PromtsAndMessages.YourTasks);
        VerifyStoreCurrentTasksList(2);
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
        var incompleteTask1 = CreateTestTask(TestConstants.ExpectedDescription);
        var incompleteTask2 = CreateTestTask(TestConstants.UpdatedDescription);
        var incompleteTasks = incompleteTask1.Concat(incompleteTask2).ToList();
        _mockTaskService.Setup(service => service.GetAllIncompleteTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(incompleteTasks));

        // Act
        _taskController.ViewIncompleteTasks();

        // Assert
        VerifyMenuPrompt(PromtsAndMessages.YourIncompleteTasks);
        VerifyStoreCurrentTasksList(2);
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
        var completeTask1 = CreateTestTask(TestConstants.ExpectedDescription, true);
        var completeTask2 = CreateTestTask(TestConstants.UpdatedDescription, true);
        var completeTasks = completeTask1.Concat(completeTask2).ToList();
        _mockTaskService.Setup(service => service.GetAllCompletedTasks())
            .Returns(Result<IReadOnlyList<ToDoItem>>.Ok(completeTasks));

        // Act
        _taskController.ViewCompletedTasks();

        // Assert
        VerifyMenuPrompt(PromtsAndMessages.YourCompletedTasks);
        VerifyStoreCurrentTasksList(2);
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
}
