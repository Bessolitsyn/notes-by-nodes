
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class User : Node
	{	
		
		public IEnumerable<Node> IsOwnerOf { get; private set; }
	
		
		public abstract void AddIntoOwner(Node item);
		
		public abstract void RemoveFromOwner(Node item);
	
	}
}
