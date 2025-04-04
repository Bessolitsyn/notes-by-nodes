using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;

namespace OwlToT4templatesTool
{
    internal class OntologyToT4tool
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
            
            CreateT4Templates(ontoDirectory, templatesDirectory, nameSpace, true);
        }
        public static void CreateT4Templates(string ontoDirectory, string templatesDirectory, string nameSpace, bool replaceExistedFiles = false)
        {
            OntologyGraph onto = new();
            FileLoader.Load(onto, ontoDirectory);

            foreach (var ontoClass in onto.AllClasses)
            {
                var ontoClassStru = OntologyStructuresFactory.NewOntologyClassStru(ontoClass);
                CreateT4template(nameSpace, ontoClassStru, templatesDirectory, replaceExistedFiles);
            }
        }

        static ClassT4template CreateT4template(string nameSpace, OntologyClassStru ontologyClassStru, string sourceDirectory, bool replaceExistedFiles = false)
        {
            try
            {
                ClassT4template template = new();
                template.SetNamespace(nameSpace);
                template.SetClassName(ontologyClassStru.Name, ontologyClassStru.ParentClassName);
                ontologyClassStru.OwlDataProperties.ForEach(prop => template.AddProperty(prop.Name, prop.Type));
                ontologyClassStru.OwlObjectProperties.ForEach(prop => template.AddProperty(prop.Name, prop.Type));
                //template.AddMethod("AddReference()");
                template.SaveTemplateFiles(sourceDirectory, replaceExistedFiles);
                return template;
            }
            catch (Exception)
            {

                throw;
            }



        }
        static class OntologyStructuresFactory
        {
            public static string GetOntologyClassName(OntologyClass ontoClass)
            {
                UriNode inode = (UriNode)ontoClass.Resource;
                return inode.Uri.Segments.Last();
            }
            public static string GetOntologyPropertyName(OntologyProperty property)
            {
                UriNode inode = (UriNode)property.Resource;
                return inode.Uri.Segments.Last();
            }
            public static string GetParentClassName(OntologyClass ontoClass)
            {
                string name = "";
                if (ontoClass.SuperClasses.Any())
                    name = GetOntologyClassName(ontoClass.SuperClasses.First());
                return name;

            }

            public static OntologyPropertyStru NewOntologyPropertyStru(OntologyProperty property)
            {

                string type = "string";
                string name = GetOntologyPropertyName(property);
                bool isDataProperty = true;
                bool isNotEmpty = property.Ranges.Any();
                if (isNotEmpty)
                {
                    var ontoClass = property.Ranges.First();
                    if (property.IsOwlDatatypeProperty())
                    {
                        var uri = ontoClass.Resource.ToString();
                        var strArr = uri.Split("#");
                        type = strArr[1];
                        isDataProperty = true;
                    }
                    //if (property.isOwlObjectProperty())
                    else
                    {
                        type = GetOntologyClassName(ontoClass);
                        isDataProperty = false;
                    }
                    if (!property.IsOwlFunctionalProperty())
                        type = $"IEnumerable<{type}>";
                }

                OntologyPropertyStru propStru = new(name, type, isDataProperty, property);
                return propStru;
            }

            public static OntologyClassStru NewOntologyClassStru(OntologyClass ontoClass)
            {
                string classname = GetOntologyClassName(ontoClass);
                var classStru = new OntologyClassStru(classname, ontoClass);
                var properties = ontoClass.IsDomainOf;
                classStru.ParentClassName = GetParentClassName(ontoClass);
                foreach (var prop in properties)
                {
                    var propStru = NewOntologyPropertyStru(prop);
                    if (propStru.IsDataProperty)
                        classStru.OwlDataProperties.Add(propStru);
                    else
                        classStru.OwlObjectProperties.Add(propStru);
                }
                return classStru;

            }
        }

        struct OntologyClassStru(string name, OntologyClass baseNode)
        {
            public string Name = name;
            public string ParentClassName = "";
            public OntologyClass BaseNode = baseNode;
            public List<OntologyPropertyStru> OwlDataProperties = [];
            public List<OntologyPropertyStru> OwlObjectProperties = [];

        }
        struct OntologyPropertyStru(string name, string type, bool isDataPropertie, OntologyProperty baseNode)
        {
            public string Name = name;
            public string Type = type;
            public bool IsDataProperty = isDataPropertie;
            public bool IsObjectProperty = !isDataPropertie;
            public OntologyProperty BaseNode = baseNode;
        }
    }
}
