using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Andtech.TodoMD
{

	public class TaskList
	{
		public List<Task> Tasks { get; private set; }
		public string Path { get; private set; }

		public static TaskList Read(string path)
		{
			var reader = new StreamReader(path);
			var list = Parse(reader);
			list.Path = path;
			return list;
		}

		public static TaskList Parse(params string[] lines) => Parse(string.Join(Environment.NewLine, lines));

		public static TaskList Parse(string lines)
		{
			byte[] byteArray = Encoding.ASCII.GetBytes(lines);
			var reader = new StreamReader(new MemoryStream(byteArray));
			return Parse(reader);
		}

		public static TaskList Parse(StreamReader reader)
		{
			var list = new TaskList()
			{
				Tasks = new List<Task>(),
			};

			var parents = new Stack<string>();
			parents.Push(string.Empty);

			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				if (string.IsNullOrEmpty(line))
				{
					continue;
				}

				var leadingWhitespaceMatch = Regex.Match(line, @"^\s+");
				var leadingWhitespace = leadingWhitespaceMatch.Success ? leadingWhitespaceMatch.Value : string.Empty;

				if (leadingWhitespace.Length > parents.Peek().Length)
				{
					parents.Push(leadingWhitespace);
				}
				else
				{
					while (leadingWhitespace.Length < parents.Peek().Length)
					{
						parents.Pop();
					}

					if (leadingWhitespace.Length > parents.Peek().Length)
					{
						parents.Push(leadingWhitespace);
					}
				}

				var task = Task.Parse(line.Trim());
				task.Level = parents.Count - 1;

				list.Tasks.Add(task);
			}

			return list;
		}
	}
}
