
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities.Ontology
{
	public abstract partial class NoteOnto : NodeOnto
	{	
		
		public IEnumerable<ContentOnto> HasContent { get; private set; }
		
		public IEnumerable<NoteOnto> HasReference { get; private set; }
		
		public IEnumerable<NoteOnto> IsRefernced { get; private set; }
	
		
		public abstract void AddIntoContent(ContentOnto item);
		
		public abstract void RemoveFromContent(ContentOnto item);
		
		public abstract void AddIntoReference(NoteOnto item);
		
		public abstract void RemoveFromReference(NoteOnto item);
		
		public abstract void AddIntoRefernced(NoteOnto item);
		
		public abstract void RemoveFromRefernced(NoteOnto item);
	
	}
}
