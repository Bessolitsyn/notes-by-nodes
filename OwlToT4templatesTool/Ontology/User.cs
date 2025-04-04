
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class User : Node
	{	
		
		public abstract IEnumerable<Node> isOwnerOf { get; set; }
	
	
	}
}
