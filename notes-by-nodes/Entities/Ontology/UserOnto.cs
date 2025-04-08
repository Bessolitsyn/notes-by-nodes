
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities.Ontology
{
	public abstract partial class UserOnto : NodeOnto
	{	
		
		public IEnumerable<NodeOnto> IsOwnerOf { get; private set; }
	
		
		public abstract void AddIntoOwner(NodeOnto item);
		
		public abstract void RemoveFromOwner(NodeOnto item);
	
	}
}
