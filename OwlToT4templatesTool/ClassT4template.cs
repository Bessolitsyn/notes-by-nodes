using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static OwlToT4templatesTool.OntologyToT4tool;

namespace OwlToT4templatesTool
{
    internal class ClassT4template
    {
#if DEBUG
        readonly string BASE_TEMPLATE = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\OwlToT4templatesTool\\template.tt_";
        readonly string BASE_TEMPLATE2 = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\OwlToT4templatesTool\\template_r.tt_";
#else
        readonly string BASE_TEMPLATE = "template.tt_";
        readonly string BASE_TEMPLATE2 = "template_r.tt_";
#endif
        string[] _templateContent;
        string[] _template2Content;
        string _className = "";
        string _parentClassName = "";
        string _classnamespace = "";
        string _recordName = "";
        string _recordnamespace = "";
        string _recordnamePostfix= "Dataset";
        List<string> _properties = [];
        //List<string> _argumentsForRecords= [];
        List<string> _methods = [];

        public ClassT4template(string nspace)
        {
            _templateContent = System.IO.File.ReadAllLines(BASE_TEMPLATE);
            _classnamespace = nspace;
            SetNamespace();

        }

        void SetNamespace()
        {
            for (int i = 0; i < _templateContent.Length; i++)
            {
                _templateContent[i] = _templateContent[i].Replace("@@namespaceDef", _classnamespace);
            }
        }
        public void SetClassName(string className, string parentClassName = "", bool isAbstract = true, bool isPartial = true)
        {
            _className = className;
            _parentClassName = parentClassName;
            var classDefenition = $"class { _className}";
            if (!String.IsNullOrEmpty(_parentClassName))
            { 
                classDefenition +=" : " + _parentClassName;
                classDefenition += $", I{_className}";
            }
            else
                classDefenition += $" : I{_className}";


            var interfaceDefenition = $"interface I{_className}";
            if (isPartial) classDefenition = AddPartialExp(classDefenition);
            if (isAbstract) classDefenition = AddAbstractExp(classDefenition);
            classDefenition = AddPublicExp(classDefenition);
            interfaceDefenition = AddPublicExp(interfaceDefenition);

            for (int i = 0; i < _templateContent.Length; i++)
            {
                _templateContent[i] = _templateContent[i].Replace("@@classDef", classDefenition);
                _templateContent[i] = _templateContent[i].Replace("@@interfaceDef", interfaceDefenition);
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
            RecodArguments.AddRecordArgumentToDicionary(_className,type + " " + name);
        }

        public void AddPropertyAndMethodsToEdit(OntologyPropertyStru propertyStru, bool addProtectedFieldForProperty = true)
        {

            AddPublicAbstractProperty(propertyStru);
            if (addProtectedFieldForProperty)
            {
                AddProtectedFieldForProperty(propertyStru);
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

        void AddPublicAbstractProperty(OntologyPropertyStru propertyStru)
        {
            string type = propertyStru.IsFunctional ? propertyStru.Type : $"IEnumerable<{propertyStru.Type}>";
            string abstract_property = type + " " + propertyStru.Name + " " + "{ get; }";
            abstract_property = AddAbstractExp(abstract_property);
            abstract_property = AddPublicExp(abstract_property);
            _properties.Add(abstract_property);
            RecodArguments.AddRecordArgumentToDicionary(_className, (propertyStru.IsFunctional ? "string " : $"string[] ") + propertyStru.Name);
        }

        void AddProtectedFieldForProperty(OntologyPropertyStru propertyStru)
        {
            string type = propertyStru.IsFunctional ? propertyStru.Type : $"List<{propertyStru.Type}>";
            string name = propertyStru.Name.Replace("Is", "is").Replace("Has", "has");
            string init = !propertyStru.IsFunctional ? " = []" : "";
            string protected_field = type + " " + name + init + ";";
            protected_field = AddProtectedExp(protected_field);
            _properties.Add(protected_field);

        }
        void AddMethodsForNotFunctionalDataProperty(OntologyPropertyStru propertyStru, bool isPublic = false, bool isAbstract = false)
        {
            string noun = propertyStru.Name.Replace("Is", "").Replace("Of", "").Replace("Has", "");
            string methodName = "AddInto" + noun;
            string args = propertyStru.Type + " item";
            AddMethod(methodName, args, isPublic, isAbstract);
            methodName = "RemoveFrom" + noun;
            AddMethod(methodName, args, isPublic, isAbstract);
        }
        void AddMethodsForFunctionalDataProperty(OntologyPropertyStru propertyStru, bool isPublic = false, bool isAbstract = false)
        {
            string noun = propertyStru.Name.Replace("Is", "").Replace("Of", "").Replace("Has", "");
            string methodName = "Set" + noun;
            string args = propertyStru.Type + " item";
            AddMethod(methodName, args, isPublic, isAbstract);
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

        public void SaveRecordTemplateFiles(string path = "", bool replaceExistedFiles = false)
        {
            string pathTempl = path + $"\\{_recordName}.tt";
            string pathArg = path + $"\\{_recordName}.arg";
            List<string> args = [];            
            
            if (!string.IsNullOrEmpty(_parentClassName))
                args.AddRange(RecodArguments.GetRecordArguments(_parentClassName).Select(a=>a+","));
            
            args.AddRange(RecodArguments.GetRecordArguments(_className).Select(a => a + ","));
            args[args.Count - 1] = args[args.Count - 1].Replace(",", "");

            if (!File.Exists(pathArg) || (File.Exists(pathArg) && replaceExistedFiles))
            {
                System.IO.File.WriteAllLines(pathTempl, _template2Content);
                System.IO.File.WriteAllLines(pathArg, args);
            }
        }

        public void CreateRecordTemplate(string namespase, bool isAbstract = false, bool isPartial = true)
        {
            _recordnamespace = namespase.Trim();
            _template2Content = System.IO.File.ReadAllLines(BASE_TEMPLATE2);
            SetRecordNamespace();
            SetRecordName(_className, _parentClassName, isAbstract, isPartial);


            void SetRecordNamespace()
            {
                for (int i = 0; i < _template2Content.Length; i++)
                {
                    _template2Content[i] = _template2Content[i].Replace("@@namespaceDef", _classnamespace);
                }
            }

            void SetRecordName(string recordName, string parentRecordName = "", bool isAbstract = false, bool isPartial = true)
            {
                
                _recordName = recordName + _recordnamePostfix;
                var classDefinition = "record " + _recordName;
                var parentDefinition = "";
                if (!String.IsNullOrEmpty(parentRecordName))
                    parentDefinition = " : " + RecodArguments.GetBaseRecordConstructor(_parentClassName, _recordnamePostfix);
                if (isPartial) classDefinition = AddPartialExp(classDefinition);
                if (isAbstract) classDefinition = AddAbstractExp(classDefinition);
                classDefinition = AddPublicExp(classDefinition);

                for (int i = 0; i < _template2Content.Length; i++)
                {
                    _template2Content[i] = _template2Content[i].Replace("@@classDef", classDefinition);
                    _template2Content[i] = _template2Content[i].Replace("@@className", _recordName);
                    _template2Content[i] = _template2Content[i].Replace("@@parentclassName", parentDefinition);
                }
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
