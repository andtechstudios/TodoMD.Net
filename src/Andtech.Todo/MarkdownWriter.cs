using System.Linq;
using System.Text;

namespace Andtech.Todo
{

	public class MarkdownWriter
	{

		public string ToString(Task task)
		{
			var builder = new StringBuilder();
			var indentation = string.Join(string.Empty, Enumerable.Repeat('\t', task.Level));

			builder.Append(indentation);
			builder.Append("*");
			builder.Append(task.IsCompleted ? " [x]" : " [ ]");
			builder.Append($" {task.Title}");
			if (!string.IsNullOrEmpty(task.Description))
			{
				builder.Append($": {task.Description}");
			}

			return builder.ToString();
		}
	}
}
