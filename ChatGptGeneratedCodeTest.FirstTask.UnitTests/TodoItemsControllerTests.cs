using ChatGptGeneratedCodeTest.FirstTask.Controllers;
using ChatGptGeneratedCodeTest.FirstTask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatGptGeneratedCodeTest.FirstTask.UnitTests;

[TestClass]
public class TodoItemsControllerTests
{
    private DbContextOptions<TodoContext> _options;

    [TestInitialize]
    public void TestInitialize()
    {
        // Use Guid.NewGuid() to ensure a unique in-memory database per test run.
        var dbName = Guid.NewGuid().ToString();

        var builder = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: dbName);
        _options = builder.Options;
    }

    [TestMethod]
    public async Task CreateAndGetTodoItem()
    {
        // Arrange
        TodoItem todoItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Description for test todo"
        };

        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            var actionResult = await controller.PostTodoItem(todoItem);
            var createdResult = actionResult.Result as CreatedAtActionResult;
            var itemId = (createdResult.Value as TodoItem).Id;
            var getResult = await controller.GetTodoItem(itemId);

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(getResult);
            Assert.IsInstanceOfType(createdResult, typeof(CreatedAtActionResult));
            Assert.IsInstanceOfType(getResult, typeof(ActionResult<TodoItem>));
            Assert.AreEqual(todoItem.Title, (getResult.Value as TodoItem).Title);
            Assert.AreEqual(todoItem.Description, (getResult.Value as TodoItem).Description);
        }
    }

    [TestMethod]
    public async Task DeleteTodoItem()
    {
        // Arrange
        TodoItem todoItem = new TodoItem
        {
            Title = "Test Todo to Delete",
            Description = "Description for the test todo that will be deleted"
        };

        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            var actionResult = await controller.PostTodoItem(todoItem);
            var createdResult = actionResult.Result as CreatedAtActionResult;
            var itemId = (createdResult.Value as TodoItem).Id;

            var deleteResult = await controller.DeleteTodoItem(itemId);
            var getResult = await controller.GetTodoItem(itemId);

            // Assert
            Assert.IsNotNull(deleteResult);
            Assert.IsInstanceOfType(deleteResult, typeof(NoContentResult));
            Assert.AreEqual(StatusCodes.Status404NotFound, (getResult.Result as NotFoundResult).StatusCode);
        }
    }

    [TestMethod]
    public async Task GetAllTodoItems()
    {
        // Arrange
        TodoItem todoItem1 = new TodoItem
        {
            Title = "Test Todo 1",
            Description = "Description for test todo 1"
        };

        TodoItem todoItem2 = new TodoItem
        {
            Title = "Test Todo 2",
            Description = "Description for test todo 2"
        };

        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            await controller.PostTodoItem(todoItem1);
            await controller.PostTodoItem(todoItem2);

            var getResult = await controller.GetTodoItems();

            // Assert
            Assert.IsNotNull(getResult);
            Assert.IsInstanceOfType(getResult, typeof(ActionResult<IEnumerable<TodoItem>>));
            Assert.AreEqual(2, getResult.Value.Count());
        }
    }

    [TestMethod]
    public async Task UpdateTodoItem()
    {
        // Arrange
        TodoItem todoItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Description for test todo"
        };

        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            var actionResult = await controller.PostTodoItem(todoItem);
            var createdResult = actionResult.Result as CreatedAtActionResult;

            var newItem = createdResult.Value as TodoItem;
            newItem.Title = "Updated Test Todo";
            newItem.Description = "Description for updated test todo";
            await controller.PutTodoItem(newItem.Id, newItem);

            var updatedItemResult = await controller.GetTodoItem(newItem.Id);

            // Assert
            Assert.IsNotNull(updatedItemResult);
            Assert.IsInstanceOfType(updatedItemResult, typeof(ActionResult<TodoItem>));
            Assert.AreEqual(newItem.Title, updatedItemResult.Value.Title);
            Assert.AreEqual(newItem.Description, updatedItemResult.Value.Description);
        }
    }

    [TestMethod]
    public async Task UpdateTodoItemWithMismatchedIdReturnsBadRequest()
    {
        // Arrange
        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            TodoItem newItem = new TodoItem()
            {
                Id = 12345,
                Title = "Mismatched Id",
                Description = "Should not be allowed to update"
            };

            var result = await controller.PutTodoItem(1, newItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }

    [TestMethod]
    public async Task GetUnknownTodoItemIdReturnsNotFound()
    {
        // Arrange
        using (var context = new TodoContext(_options))
        {
            var controller = new TodoItemsController(context);

            // Act
            long unknownId = 12345;
            var result = await controller.GetTodoItem(unknownId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}