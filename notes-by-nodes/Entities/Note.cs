
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public abstract partial class Note : Node, INote
	{	
		
		public abstract IEnumerable<Content> HasContent { get; }
		
		protected List<Content> hasContent = [];
		
		public abstract IEnumerable<Note> HasReference { get; }
		
		protected List<Note> hasReference = [];
		
		public abstract IEnumerable<Note> IsRefernced { get; }
		
		protected List<Note> isRefernced = [];
		}

	public interface INote
	{
		
		void AddIntoContent(Content item);
		
		void RemoveFromContent(Content item);
		
		void AddIntoReference(Note item);
		
		void RemoveFromReference(Note item);
		
		void AddIntoRefernced(Note item);
		
		void RemoveFromRefernced(Note item);
	
	}
}
