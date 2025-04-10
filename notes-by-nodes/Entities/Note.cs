
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public abstract partial class Note : Node
	{	
		
		public abstract IEnumerable<Content> HasContent { get; }
		
		protected List<Content> hasContent = [];
		
		public abstract IEnumerable<Note> HasReference { get; }
		
		protected List<Note> hasReference = [];
		
		public abstract IEnumerable<Note> IsRefernced { get; }
		
		protected List<Note> isRefernced = [];
	
		
		public abstract void AddIntoContent(Content item);
		
		public abstract void RemoveFromContent(Content item);
		
		public abstract void AddIntoReference(Note item);
		
		public abstract void RemoveFromReference(Note item);
		
		public abstract void AddIntoRefernced(Note item);
		
		public abstract void RemoveFromRefernced(Note item);
	
	}
}
