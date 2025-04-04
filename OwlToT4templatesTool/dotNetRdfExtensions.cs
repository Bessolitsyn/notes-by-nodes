using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;

namespace OwlToT4templatesTool
{
    public static class OntologyPropertyExtension
    {
        public static bool IsOwlDatatypeProperty(this OntologyProperty property)
        {
            try
            {

                foreach (INode node in property.Types)
                {
                    if (node.NodeType == NodeType.Uri)
                    {
                        var uriNode = (UriNode)node;
                        if (uriNode.Uri.ToString() == OntologyHelper.OwlDatatypeProperty)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new OntologyPropertyExtensionException("OntologyPropertyExtension", ex);
            }
            return false;
        }
        public static bool IsOwlObjectProperty(this OntologyProperty property)
        {
            try
            {
                foreach (INode node in property.Types)
                {
                    if (node.NodeType == NodeType.Uri)
                    {
                        var uriNode = (UriNode)node;
                        if (uriNode.Uri.ToString() == OntologyHelper.OwlObjectProperty)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new OntologyPropertyExtensionException("OntologyPropertyExtension", ex);
            }
            return false;


        }
        public static bool IsOwlFunctionalProperty(this OntologyProperty property)
        {
            try
            {
                foreach (INode node in property.Types)
                {
                    if (node.NodeType == NodeType.Uri)
                    {
                        var uriNode = (UriNode)node;
                        if (uriNode.Uri.ToString() == "http://www.w3.org/2002/07/owl#FunctionalProperty")
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new OntologyPropertyExtensionException("OntologyPropertyExtension", ex);
            }
            return false;

        }


    }

    public class OntologyPropertyExtensionException(string message, Exception innerException) : Exception(message, innerException)
    {
    }
}
