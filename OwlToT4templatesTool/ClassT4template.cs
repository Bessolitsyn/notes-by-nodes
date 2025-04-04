using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool
{
    internal class ClassT4template
    {
        const string BASE_TEMPLATE = "template.tt_";
        string[] _templateContent;
        string _className = "";
        string _namespace = "";
        List<string> _properties = [];
        List<string> _methods = [];

        public ClassT4template()
        {
            _templateContent = System.IO.File.ReadAllLines(BASE_TEMPLATE);
        }

        public void SetNamespace(string nspace)
        {
            _namespace = nspace;
            for (int i = 0; i < _templateContent.Length; i++)
            {
                _templateContent[i] = _templateContent[i].Replace("@@namespaceDef", nspace);
            }

        }
        public void SetClassName(string className, string parentClassName = "", bool isAbstract = true, bool isPartial = true)
        {

            _className = className;

            var classDefenition = "class " + className;
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
        public void AddProperty(string name, string type, string getterAndSetter = "{ get; set; }", bool isPublic = true, bool isAbstract = true)
        {
            string property = type + " " + name + " " + getterAndSetter;
            //property = AddAbstractExp(method);
            if (isAbstract) property = AddAbstractExp(property);
            if (isPublic) property = AddPublicExp(property);
            _properties.Add(property);
        }

        public void AddMethod(string name, bool isPublic = true, string returnType = "void")
        {
            string method = returnType + " " + name + ";";
            method = AddAbstractExp(method);
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

        static string AddPartialExp(string str)
        {
            return "partial " + str;
        }
    }
}
