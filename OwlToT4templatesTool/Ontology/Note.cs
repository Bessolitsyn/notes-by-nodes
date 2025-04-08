
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class Note : Node
	{	
		
		public IEnumerable<Content> HasContent { get; private set; }
		
		public IEnumerable<Note> HasReference { get; private set; }
		
		public IEnumerable<Note> IsRefernced { get; private set; }
	
		
		public abstract void AddIntoContent(Content item);
		
		public abstract void RemoveFromContent(Content item);
		
		public abstract void AddIntoReference(Note item);
		
		public abstract void RemoveFromReference(Note item);
		
		public abstract void AddIntoRefernced(Note item);
		
		public abstract void RemoveFromRefernced(Note item);
	
	}
}
