
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool.Ontology
{
	public abstract partial class Note : Node
	{	
		
		public abstract IEnumerable<Content> hasContent { get; set; }
		
		public abstract IEnumerable<Note> hasReference { get; set; }
		
		public abstract IEnumerable<Note> isRefernced { get; set; }
	
	
	}
}
