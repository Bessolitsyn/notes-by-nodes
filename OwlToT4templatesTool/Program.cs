// See https://aka.ms/new-console-template for more information
using OwlToT4templatesTool;
using OwlToT4templatesTool.ArgumentParser;

CMDParse.ParseArguments(args);

Console.WriteLine("this is OWLtoT4templates");

if (CMDParse.InputArguments.ReadOntology)
{ 
	try
	{
        string ontoDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\notes-by-nodes.rdf";
        string templatesDirectory = $"c:\\Users\\tocha\\source\\notes-by-nodes\\OwlToT4templatesTool\\Ontology\\";
        string nameSpace = "OwlToT4templatesTool.Ontology";
        OntologyToT4tool.ReadOntology(ontoDirectory, templatesDirectory, nameSpace);
        Console.WriteLine("Done reading!");
    }
	catch (Exception ex)
	{
        Console.WriteLine(ex);
        
    }
	
}

if (CMDParse.InputArguments.DeleteTemplatesfiles)
{
    try
    {

        string templatesDirectory = $"c:\\Users\\tocha\\source\\notes-by-nodes\\OwlToT4templatesTool\\Ontology\\";
        OntologyToT4tool.DeleteFiles(templatesDirectory);
        Console.WriteLine("Done deleting!");
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex);
    }

}




