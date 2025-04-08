
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities.Ontology
{
	public abstract partial class NodeOnto
	{	
		
		public DateTime CreationDate { get; set; }
		
		public string Description { get; set; }
		
		public string Name { get; set; }
		
		public string Text { get; set; }
		
		public string Type { get; set; }
		
		public string Uid { get; set; }
		
		public IEnumerable<NodeOnto> HasChildNodes { get; private set; }
		
		public UserOnto HasOwner { get; private set; }
		
		public NodeOnto HasParentNode { get; private set; }
	
		
		public abstract void AddIntoChildNodes(NodeOnto item);
		
		public abstract void RemoveFromChildNodes(NodeOnto item);
		
		public abstract void SetOwner(UserOnto item);
		
		public abstract void SetParentNode(NodeOnto item);
	
	}
}
