namespace Andtech.Todo.Test
{

	public class TaskTests
	{

		[Test]
		public void ParseBasic()
		{
			var task = Task.Parse("Call Saul");
			Assert.That(task.Title, Is.EqualTo("Call Saul"));
		}

		[Test]
		public void ParseBullet()
		{
			var task = Task.Parse("* Call Saul");
			Assert.Pass();
		}

		[Test]
		public void ParseDash()
		{
			var task = Task.Parse("- Call Saul");
			Assert.Pass();
		}

		[Test]
		public void ParseNumbering()
		{
			var task = Task.Parse("22. Call Saul");
			Assert.Pass();
		}

		[Test]
		public void ParseTaskWithoutStatus()
		{
			var task = Task.Parse("Call Saul");
			Assert.That(task.Title, Is.EqualTo("Call Saul"));
			Assert.That(task.IsCompleted, Is.EqualTo(false));
		}

		[Test]
		public void ParseWithMarkdownLink()
		{
			var task = Task.Parse("Call [Saul](http://bettercallsaul.amc.com/)");
			Assert.That(task.Title, Is.EqualTo("Call [Saul](http://bettercallsaul.amc.com/)"));
			Assert.That(task.Description, Is.EqualTo(string.Empty));
		}

		[Test]
		public void ParseWithMarkdown()
		{
			var task = Task.Parse("Call Saul: it *is* **important** that `we` speak to him urgently");
			Assert.That(task.Title, Is.EqualTo("Call Saul"));
			Assert.That(task.Description, Is.EqualTo("it *is* **important** that `we` speak to him urgently"));
		}

		[Test]
		public void ParseTags()
		{
			var task = Task.Parse("Call Saul #work #legal");
			CollectionAssert.AreEqual(new string[] { "work", "legal" }, task.Tags);
		}

		[Test]
		public void ParseDate()
		{
			var task = Task.Parse("Call Saul due:2009-04-26");
			Assert.That(task.DueDate.HasValue, Is.True);
			Assert.That(DateOnly.FromDateTime(task.DueDate.Value), Is.EqualTo(DateOnly.Parse("2009-04-26")));
		}

		[Test]
		public void ParseAssignees()
		{
			var task = Task.Parse("Call Saul @me @jpinkman @swhite");
			CollectionAssert.AreEqual(new string[] { "me", "jpinkman", "swhite" }, task.Assignees);
		}

		[Test]
		public void ParseMultiple()
		{
			var task = Task.Parse("Call Saul #work #legal due:2009-04-26 @me @jpinkman @swhite");
			CollectionAssert.AreEqual(new string[] { "work", "legal" }, task.Tags);
			Assert.That(task.DueDate.HasValue, Is.True);
			Assert.That(DateOnly.FromDateTime(task.DueDate.Value), Is.EqualTo(DateOnly.Parse("2009-04-26")));
			CollectionAssert.AreEqual(new string[] { "me", "jpinkman", "swhite" }, task.Assignees);
		}

		[Test]
		public void ParseComplete()
		{
			var task = Task.Parse("* [x] Call Saul: better call saul #work due:2009-04-26 @jpinkman");
			Assert.That(task.IsCompleted, Is.EqualTo(true));
			Assert.That(task.Title, Is.EqualTo("Call Saul"));
			Assert.That(task.Description, Is.EqualTo("better call saul"));
			CollectionAssert.AreEqual(new string[] { "work" }, task.Tags);
			Assert.That(task.DueDate.HasValue, Is.True);
			Assert.That(DateOnly.FromDateTime(task.DueDate.Value), Is.EqualTo(DateOnly.Parse("2009-04-26")));
			CollectionAssert.AreEqual(new string[] { "jpinkman" }, task.Assignees);
		}
	}
}