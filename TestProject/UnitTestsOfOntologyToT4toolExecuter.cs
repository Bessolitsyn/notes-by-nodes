using OwlToT4templatesTool;
namespace TestProject
{
    public class UnitTestsOfOntologyToT4toolExecuter
    {
        [Fact]
        public void ReadOntologyForOneClass()
        {
            string ontoDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Ontology\\notes-by-nodes.rdf";
            OntologyToT4toolExecuter.TemplatesDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.TemplatesDirectoryForRecords = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.NameSpace = "OWLtoT4templates.TOntology";
            OntologyToT4toolExecuter.NameSpaceForRecords = "OWLtoT4templates.TRecords";
            OntologyToT4toolExecuter.DeleteFiles(OntologyToT4toolExecuter.TemplatesDirectory);

            OntologyToT4toolExecuter.ReadOntologyForOneClass("http://notes-by-nodes/ontologies/2025/Node", ontoDirectory1, false);

            Assert.True(Directory.GetFiles(OntologyToT4toolExecuter.TemplatesDirectory).Length == 3);
            
        }

        [Fact]
        public void ReadOntologyTestRecordWithParentRecord()
        {
            string ontoDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Ontology\\notes-by-nodes.rdf";
            OntologyToT4toolExecuter.TemplatesDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.TemplatesDirectoryForRecords = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.NameSpace = "OWLtoT4templates.TOntology";
            OntologyToT4toolExecuter.NameSpaceForRecords = "OWLtoT4templates.TRecords";
            OntologyToT4toolExecuter.DeleteFiles(OntologyToT4toolExecuter.TemplatesDirectory);

            OntologyToT4toolExecuter.ReadOntologyForOneClass("http://notes-by-nodes/ontologies/2025/Node", ontoDirectory1, true);
            OntologyToT4toolExecuter.ReadOntologyForOneClass("http://notes-by-nodes/ontologies/2025/Note", ontoDirectory1, true);

            Assert.True(Directory.GetFiles(OntologyToT4toolExecuter.TemplatesDirectory).Length == 10);

        }

        [Fact]
        public void ReadOntology()
        {
            string ontoDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Ontology\\notes-by-nodes.rdf";
            OntologyToT4toolExecuter.TemplatesDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.TemplatesDirectoryForRecords = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\Ontology";
            OntologyToT4toolExecuter.NameSpace = "OWLtoT4templates.TOntology";
            OntologyToT4toolExecuter.NameSpaceForRecords = "OWLtoT4templates.TRecords";
            //string ontoDirectory1 = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Ontology\\notes-by-nodes.rdf";
            OntologyToT4toolExecuter.ReadOntology(ontoDirectory1, true);
            Assert.True(true);

        }
    }
}