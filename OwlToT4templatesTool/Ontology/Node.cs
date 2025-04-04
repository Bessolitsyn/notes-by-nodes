
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class Node
	{	
		
		public abstract string Description { get; set; }
		
		public abstract string Name { get; set; }
		
		public abstract IEnumerable<string> Text { get; set; }
		
		public abstract string Type { get; set; }
		
		public abstract string Uid { get; set; }
		
		public abstract IEnumerable<Node> hasChildNodes { get; set; }
		
		public abstract User hasOwner { get; set; }
		
		public abstract Node hasParentNode { get; set; }
	
	
	}
}
