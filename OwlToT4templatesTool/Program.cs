// See https://aka.ms/new-console-template for more information
using OwlToT4templatesTool;
using OwlToT4templatesTool.ArgumentParser;



CMDParse.ParseArguments(args);

Console.WriteLine("this is OWLtoT4templates");
string templatesDirectory = $"c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\Ontology\\";
Console.WriteLine("this is templatesDirectory " + Directory.GetParent(templatesDirectory));
Console.WriteLine("Warning! Don't save own files to it.");


if (CMDParse.InputArguments.ReadOntology)
{ 
	try
	{
        string ontoDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\notes-by-nodes.rdf";        
        string nameSpace = "notes_by_nodes.Entities.Ontology";
        OntologyToT4toolExecuter.ReadOntology(ontoDirectory, templatesDirectory, nameSpace);
        Console.WriteLine("Done reading!");
    }
	catch (Exception ex)
	{
        Console.WriteLine(ex);
        
    }
	
}

if (CMDParse.InputArguments.ReadClassOntology!=null)
{
    try
    {
        string ontoClass = CMDParse.InputArguments.ReadClassOntology;
        string ontoDirectory = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\notes-by-nodes.rdf";
        string nameSpace = "notes_by_nodes.Entities.Ontology";
        OntologyToT4toolExecuter.ReadOntologyForOneClass(ontoClass, ontoDirectory, templatesDirectory, nameSpace);
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

        OntologyToT4toolExecuter.DeleteFiles(templatesDirectory);
        Console.WriteLine("Done deleting!");
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex);
    }

}

public static class OntologyToT4toolExecuter
{

    public static void DeleteFiles(string templatesDirectory)
    {
        var files = Directory.GetFiles(templatesDirectory);
        foreach (var item in files)
        {
            File.Delete(item);
        }

    }
    public static void ReadOntology(string ontoDirectory, string templatesDirectory, string nameSpace)
    {

        OntologyToT4tool.CreateAllT4Templates(ontoDirectory, templatesDirectory, nameSpace, true);
    }

    public static void ReadOntologyForOneClass(string ontoClass, string ontoDirectory, string templatesDirectory, string nameSpace)
    {

        OntologyToT4tool.CreateClassT4Templates(ontoClass, ontoDirectory, templatesDirectory, nameSpace, true);
    }
}




