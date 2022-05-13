using Microsoft.VisualStudio.TestTools.UnitTesting;
using Todo.Data;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Bogus;
using Todo.Api.Requests;
using System;

namespace ToDoTests
{
    [TestClass]
    public class ToDoRepositoryTests
    {
        [TestMethod]
        public void TestItemComplete()
        {

            var context = InitializeTestContext();
            var id = context.TodoItems.First(itm => !itm.Completed.HasValue).Id;

            var repo = new TodoRepository(context);
            var result = repo.Complete(id).Result;
            Assert.IsNotNull(context.TodoItems.Find(id).Completed);

        }

        private TodoContext InitializeTestContext()
        {
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
            return context;
        }

        [TestMethod]
        public void TestListWithoutCompletedItems()
        {

            var context = InitializeTestContext();

            var repo = new TodoRepository(context);
            var result = repo.List(false).Result;
            Assert.IsTrue(result.Any(itm => itm.Completed.HasValue) == false);

        }

        [TestMethod]
        public void TestListWithCompletedItems()
        {

            var context = InitializeTestContext();

            var repo = new TodoRepository(context);
            var result = repo.List(true).Result;
            Assert.IsTrue(result.Any(itm => itm.Completed.HasValue));

        }
    }
}