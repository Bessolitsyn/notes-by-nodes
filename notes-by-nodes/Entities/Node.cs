
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public abstract partial class Node
	{	
		
		public DateTime CreationDate { get; set; }
		
		public string Description { get; set; }
		
		public virtual string Name { get; set; }
		
		public string Text { get; set; }
		
		public string Type { get; set; }
		
		public string Uid { get; set; }
		
		public abstract IEnumerable<Node> HasChildNodes { get; }
		
		protected List<Node> hasChildNodes = [];
		
		public abstract User HasOwner { get; }
		
		protected User hasOwner;
		
		public abstract Node HasParentNode { get; }
		
		protected Node hasParentNode;
	
		
		public abstract void AddIntoChildNodes(Node item);
		
		public abstract void RemoveFromChildNodes(Node item);
		
		public abstract void SetOwner(User item);
		
		public abstract void SetParentNode(Node item);
	
	}
}
