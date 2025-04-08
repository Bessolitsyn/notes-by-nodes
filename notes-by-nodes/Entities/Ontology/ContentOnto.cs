
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities.Ontology
{
	public abstract partial class ContentOnto
	{	
		
		public DateTime CreationDate { get; set; }
		
		public string Description { get; set; }
		
		public string Name { get; set; }
		
		public string Type { get; set; }
		
		public string Uid { get; set; }
	
	
	}
}
