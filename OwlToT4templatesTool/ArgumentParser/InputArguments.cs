using CommandLine;

namespace OwlToT4templatesTool.ArgumentParser
{
    /// <summary>
    /// Input arguments application
    /// </summary>
    public class InputArguments
    {

        [Option(shortName: 'r', longName: "readOntology", Required = false, HelpText = "Read RDF and create T4 templates")]
        public bool ReadOntology { get; set; }

        [Option(shortName: 'a', longName: "add record template as dataset for class", Required = false, HelpText = "It require r key. If true create T4 datasets templates for onto classes")]
        public bool AddRecords { get; set; }

        [Option(shortName: 'c', longName: "readOntologyClass", Required = false, HelpText = "ReadRDF and create T4 templates for specified class ")]
        public string? ReadClassOntology { get; set; }

        [Option(shortName: 'd', longName: "deleteTemplateFiles", Required = false, HelpText = "Delete existed T4 template files")]
        public bool DeleteTemplatesfiles { get; set; }
  
    }
}
