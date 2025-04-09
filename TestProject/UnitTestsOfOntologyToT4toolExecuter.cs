using OwlToT4templatesTool;
namespace TestProject
{
    public class UnitTestsOfOntologyToT4toolExecuter
    {
        [Fact]
        public void ReadOntologyForOneClass()
        {
            string ontoDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\notes-by-nodes.rdf";
            string templatesDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            string nameSpace1 = "OWLtoT4templates.Ontology";
            OntologyToT4toolExecuter.ReadOntologyForOneClass("http://notes-by-nodes/ontologies/2025/Node", ontoDirectory1, templatesDirectory1, nameSpace1);
            Assert.True(true);
            //OntologyToT4toolExecuter.DeleteFiles(templatesDirectory1);           
        }
    }
}