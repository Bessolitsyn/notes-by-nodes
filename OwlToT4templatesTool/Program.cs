// See https://aka.ms/new-console-template for more information
using OwlToT4templatesTool;
using OwlToT4templatesTool.ArgumentParser;



CMDParse.ParseArguments(args);

Console.WriteLine("this is OWLtoT4templates");
Console.WriteLine("this is templatesDirectory " + Directory.GetParent(OntologyToT4toolExecuter.TemplatesDirectory));
Console.WriteLine("Warning! Don't save own files to it.");


if (CMDParse.InputArguments.ReadOntology)
{ 
	try
	{    
        OntologyToT4toolExecuter.ReadOntology(OntologyToT4toolExecuter.OntoDirectory, CMDParse.InputArguments.AddRecords);
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
        
        OntologyToT4toolExecuter.ReadOntologyForOneClass(ontoClass, OntologyToT4toolExecuter.OntoDirectory, CMDParse.InputArguments.AddRecords);
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

        OntologyToT4toolExecuter.DeleteFiles(OntologyToT4toolExecuter.TemplatesDirectory);
        Console.WriteLine("Done deleting!");
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex);
    }

}

public static class OntologyToT4toolExecuter
{
    public static string OntoDirectory { get; set; } = "c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Ontology\\notes-by-nodes.rdf";
    public static string TemplatesDirectory { get; set; } = $"c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Entities\\";
    public static string NameSpace { get; set; } = "notes_by_nodes.Entities";
    public static string TemplatesDirectoryForRecords { get; set; } = $"c:\\Users\\tocha\\source\\notes-by-nodes\\notes-by-nodes\\Storage\\Dataset\\";
    public static string NameSpaceForRecords { get; set; } = "notes_by_nodes.Storage.Dataset";
    
    public static void DeleteFiles(string templatesDirectory)
    {
        
        var files = Directory.GetFiles(templatesDirectory);
        foreach (var item in files)
        {
            File.Delete(item);
        }
        RecodArguments.Clean();

    }
    public static void ReadOntology(string ontoDirectory, bool addRecord = false)
    {
       
        OntologyToT4tool.CreateAllT4Templates(ontoDirectory, addRecord, true);
    }

    public static void ReadOntologyForOneClass(string ontoClass, string ontoDirectory, bool addRecord = false)
    {

        OntologyToT4tool.CreateClassT4Templates(ontoClass, ontoDirectory, addRecord, true);
    }
}




