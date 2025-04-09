using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OwlToT4templatesTool.OntologyToT4tool;

namespace OwlToT4templatesTool
{
    internal class ClassT4template
    {
#if DEBUG
        readonly string BASE_TEMPLATE = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\OwlToT4templatesTool\\template.tt_";
#else
        readonly string BASE_TEMPLATE = "template.tt_";
#endif
        string[] _templateContent;
        string _className = "";
        string _namespace = "";
        List<string> _properties = [];
        List<string> _methods = [];

        public ClassT4template(string nspace)
        {
            _templateContent = System.IO.File.ReadAllLines(BASE_TEMPLATE);
            _namespace = nspace;
            SetNamespace();

        }

        void SetNamespace()
        {
            for (int i = 0; i < _templateContent.Length; i++)
            {
                _templateContent[i] = _templateContent[i].Replace("@@namespaceDef", _namespace);
            }
        }
        public void SetClassName(string className, string parentClassName = "", bool isAbstract = true, bool isPartial = true)
        {
            _className = className;
            var classDefenition = "class " + _className;
            if (!String.IsNullOrEmpty(parentClassName))
                classDefenition = classDefenition + " : " + parentClassName;
            if (isPartial) classDefenition = AddPartialExp(classDefenition);
            if (isAbstract) classDefenition = AddAbstractExp(classDefenition);
            classDefenition = AddPublicExp(classDefenition);

            for (int i = 0; i < _templateContent.Length; i++)
            {
                _templateContent[i] = _templateContent[i].Replace("@@classDef", classDefenition);
                _templateContent[i] = _templateContent[i].Replace("@@className", _className);
            }
        }
        public void AddProperty(string name, string type, string getterAndSetter = "{ get; set; }", bool isPublic = true, bool isAbstract = false)
        {
            string property = type + " " + name + " " + getterAndSetter;
            //property = AddAbstractExp(method);
            if (isAbstract) property = AddAbstractExp(property);
            if (isPublic) property = AddPublicExp(property);
            _properties.Add(property);
        }

        public void AddPropertyAndMethodsToEdit(OntologyPropertyStru propertyStru, bool addProtectedFieldForProperty = true)
        {
            string type = propertyStru.IsFunctional ? propertyStru.Type : $"IEnumerable<{propertyStru.Type}>";
            AddPublicAbstractProperty(propertyStru, type);
            if (addProtectedFieldForProperty)
            {
                AddProtectedFieldForProperty(propertyStru, type);
            }
            if (!propertyStru.IsFunctional)
            {
                AddMethodsForNotFunctionalDataProperty(propertyStru);
            }
            else
            {
                AddMethodsForFunctionalDataProperty(propertyStru);
            }
        }

        void AddPublicAbstractProperty(OntologyPropertyStru propertyStru, string type)
        {
            string abstract_property = type + " " + propertyStru.Name + " " + "{ get; }";
            abstract_property = AddAbstractExp(abstract_property);
            abstract_property = AddPublicExp(abstract_property);
            _properties.Add(abstract_property);
        }

        void AddProtectedFieldForProperty(OntologyPropertyStru propertyStru, string type)
        {
            string name = propertyStru.Name.Replace("Is", "is").Replace("Has", "has");
            string init = !propertyStru.IsFunctional ? " = []" : "";
            string protected_field = type + " " + name + init +";";
            protected_field = AddProtectedExp(protected_field);
            _properties.Add(protected_field);

        }
        void AddMethodsForNotFunctionalDataProperty(OntologyPropertyStru propertyStru)
        {
            string noun = propertyStru.Name.Replace("Is", "").Replace("Of", "").Replace("Has", "");
            string methodName = "AddInto" + noun;
            string args = propertyStru.Type + " item";
            AddMethod(methodName, args);
            methodName = "RemoveFrom" + noun;
            AddMethod(methodName, args);
        }
        void AddMethodsForFunctionalDataProperty(OntologyPropertyStru propertyStru)
        {
            string noun = propertyStru.Name.Replace("Is", "").Replace("Of", "").Replace("Has", "");
            string methodName = "Set" + noun;
            string args = propertyStru.Type + " item";
            AddMethod(methodName, args);
        }

        public void AddMethod(string name, string args = "", bool isPublic = true, bool isAbstract = true, string returnType = "void")
        {
            string method = returnType + " " + name + "(" + args + ");";
            if (isAbstract) method = AddAbstractExp(method);
            if (isPublic) method = AddPublicExp(method);
            _methods.Add(method);

        }

        public void SaveTemplateFiles(string path = "", bool replaceExistedFiles = false)
        {

            string pathTempl = path + $"\\{_className}.tt";
            string pathProp = path + $"\\{_className}.prp";
            string pathMeth = path + $"\\{_className}.mth";
            if (!File.Exists(pathTempl) || (File.Exists(pathTempl) && replaceExistedFiles))
            {
                System.IO.File.WriteAllLines(pathTempl, _templateContent);
                System.IO.File.WriteAllLines(pathProp, _properties);
                System.IO.File.WriteAllLines(pathMeth, _methods);
            }
        }

        static string AddAbstractExp(string str)
        {
            return "abstract " + str;
        }

        static string AddPublicExp(string str)
        {
            return "public " + str;
        }

        static string AddProtectedExp(string str)
        {
            return "protected " + str;
        }

        static string AddPartialExp(string str)
        {
            return "partial " + str;
        }
    }
}
