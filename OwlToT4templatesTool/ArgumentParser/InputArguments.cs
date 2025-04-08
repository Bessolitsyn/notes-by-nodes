using CommandLine;

namespace OwlToT4templatesTool.ArgumentParser
{
    /// <summary>
    /// Input arguments application
    /// </summary>
    public class InputArguments
    {

        [Option(shortName: 'r', longName: "readOntology", Required = false, HelpText = "Read and create T4 templates")]
        public bool ReadOntology { get; set; }

        [Option(shortName: 'c', longName: "readOntologyClass", Required = false, HelpText = "Read and create T4 templates for specified class ")]
        public string ReadClassOntology { get; set; }

        [Option(shortName: 'd', longName: "deleteTemplateFiles", Required = false, HelpText = "Delete existed T4 template files")]
        public bool DeleteTemplatesfiles { get; set; }
  
    }
}
