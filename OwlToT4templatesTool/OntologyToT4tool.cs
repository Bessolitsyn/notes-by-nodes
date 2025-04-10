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
        
        public static void CreateAllT4Templates(string ontoDirectory, bool addRecord = false, bool replaceExistedFiles = false)
        {
            RecodArguments.Clean();
            OntologyGraph onto = new();
            FileLoader.Load(onto, ontoDirectory);
            CreateT4Templates(onto.AllClasses, addRecord, replaceExistedFiles);            
        }
        public static void CreateClassT4Templates(string ontoClass, string ontoDirectory, bool addRecord = false, bool replaceExistedFiles = false)
        {
            OntologyGraph onto = new();
            FileLoader.Load(onto, ontoDirectory);
            CreateT4Templates(onto.AllClasses.Where(owlc => owlc.ToString() == ontoClass), addRecord, replaceExistedFiles);


        }

        static void CreateT4Templates(IEnumerable<OntologyClass> clasess, bool addRecord = false, bool replaceExistedFiles = false)
        {
            List<ClassT4template> templates = [];
            foreach (var owlClass in clasess)
            {
                var ontoClassStru = OntologyStructuresFactory.NewOntologyClassStru(owlClass);
                templates.Add(CreateClassT4template(OntologyToT4toolExecuter.NameSpace, ontoClassStru, OntologyToT4toolExecuter.TemplatesDirectory, replaceExistedFiles));
                
            }
            if (addRecord)
                foreach (var tmpl in templates)
                {
                    CreateRecordT4template(tmpl, OntologyToT4toolExecuter.NameSpaceForRecords, OntologyToT4toolExecuter.TemplatesDirectoryForRecords, replaceExistedFiles);
                }
        }

        static ClassT4template CreateClassT4template(string nameSpace, OntologyClassStru ontologyClassStru, string sourceDirectory, bool replaceExistedFiles = false)
        {
            try
            {
                ClassT4template template = new(nameSpace);
                template.SetClassName(ontologyClassStru.Name, ontologyClassStru.ParentClassName);
                ontologyClassStru.OwlDataProperties.ForEach(prop => template.AddProperty(prop.Name, prop.Type));
                ontologyClassStru.OwlObjectProperties.ForEach(prop => template.AddPropertyAndMethodsToEdit(prop));
                template.SaveTemplateFiles(sourceDirectory, replaceExistedFiles);
                return template;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static ClassT4template CreateRecordT4template(ClassT4template template, string nameSpace, string sourceDirectory, bool replaceExistedFiles = false)
        {
            try
            {
                template.CreateRecordTemplate(nameSpace);                
                template.SaveRecordTemplateFiles(sourceDirectory, replaceExistedFiles);
                return template;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static class OntologyStructuresFactory
        {
            private static string classPostfix = "";
            public static string GetOntologyClassName(OntologyClass ontoClass)
            {
                UriNode inode = (UriNode)ontoClass.Resource;
                return inode.Uri.Segments.Last() + classPostfix;
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
                }

                OntologyPropertyStru propStru = new(name, type, isDataProperty, property.IsOwlFunctionalProperty(), property);
                return propStru;
            }

            public static OntologyClassStru NewOntologyClassStru(OntologyClass owlClass)
            {
                string classname = GetOntologyClassName(owlClass);
                var classStru = new OntologyClassStru(classname, owlClass);
                var properties = owlClass.IsDomainOf;
                classStru.ParentClassName = GetParentClassName(owlClass);
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
    }

    public struct OntologyClassStru(string name, OntologyClass baseNode)
    {
        public string Name = name;
        public string ParentClassName = "";
        public OntologyClass BaseNode = baseNode;
        public List<OntologyPropertyStru> OwlDataProperties = [];
        public List<OntologyPropertyStru> OwlObjectProperties = [];

    }
    public struct OntologyPropertyStru(string name, string type, bool isDataPropertie, bool isFunctional, OntologyProperty baseNode)
    {
        public string Name = name;
        public string Type = type;
        public bool IsFunctional = isFunctional;
        public bool IsDataProperty = isDataPropertie;
        public bool IsObjectProperty = !isDataPropertie;
        public OntologyProperty BaseNode = baseNode;
    }
}
