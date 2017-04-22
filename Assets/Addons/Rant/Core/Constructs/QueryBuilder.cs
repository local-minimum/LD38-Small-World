using Rant.Vocabulary.Querying;
using System.Collections.Generic;
using System.Linq;

using Rant.Core.Compiler.Syntax;

namespace Rant.Core.Constructs
{
	internal sealed class QueryBuilder
	{
		private readonly Dictionary<string, Query> _queries;
		private Query _activeQuery;

		public QueryBuilder()
		{
			_queries = new Dictionary<string, Query>();
		}

		public Query CurrentQuery { get { return _activeQuery; } }
		public Query GetQuery(string name)
		{
			Query q;
			if (!_queries.TryGetValue(name, out q))
			{
				q = _queries[name] = new Query();
			}
			return _activeQuery = q;
		}

		public void ResetQuery(string name)
		{
			_queries.Remove(name);
		}

		public IEnumerator<RST> RunQuery(Sandbox sb, string id)
		{
			Query q;
			if (!_queries.TryGetValue(id, out  q)) yield break;
			var action = q.Run(sb);
			while (action.MoveNext()) yield return action.Current;
		}
	}
}
