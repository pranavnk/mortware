using Microsoft.VisualStudio.TestTools.UnitTesting;
using Todo.Data;
using Todo.Api;
using Moq;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Todo.Api.Requests;
using System;
using System.Linq;
using System.Linq.Expressions;
using Todo.Api.Controllers;

namespace ToDo.API.Tests
{
    [TestClass]
    public class ToDoControllerTests
    {
        [TestMethod]
        public void TestComplete()
        {
            var mockRepo = InitializeTestRepository();
            
            var controller = new TodoController(mockRepo);
        }

        private ITodoRepository InitializeTestRepository()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var dbOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("Todo")
                .Options;
            var context = new TodoContext(dbOptions);
            var itemFaker = new Faker<TodoItem>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Completed, f => f.Random.Bool() ? f.Date.Past() : null)
            .RuleFor(t => t.Created, (f, t) => f.Date.Past(refDate: t.Completed))
            .RuleFor(t => t.Text, f => f.Lorem.Sentence());

            var items = itemFaker.Generate(5);

            context.TodoItems.AddRange(items);
            context.SaveChanges();
            mockRepo.Setup(p => p.Complete(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => id);

            return mockRepo.Object;
        }
    }
}