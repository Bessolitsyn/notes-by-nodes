
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class Node
	{	
		
		public DateTime CreationDate { get; set; }
		
		public string Description { get; set; }
		
		public string Name { get; set; }
		
		public string Text { get; set; }
		
		public string Type { get; set; }
		
		public string Uid { get; set; }
		
		public IEnumerable<Node> HasChildNodes { get; private set; }
		
		public User HasOwner { get; private set; }
		
		public Node HasParentNode { get; private set; }
	
		
		public abstract void AddIntoChildNodes(Node item);
		
		public abstract void RemoveFromChildNodes(Node item);
		
		public abstract void SetOwner(User item);
		
		public abstract void SetParentNode(Node item);
	
	}
}
