using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using ConsoleApp31;

namespace Semantic
{
    class Def_table
    {
        int tracker = 0;
        public List<string> Name = new List<string>();
        List<string> Type = new List<string>();
        List<string> Category = new List<string>();
        List<string> Parent = new List<string>();
        List<int> Reference = new List<int>();

        public List<Class_Def_table> objectName = new List<Class_Def_table>();

        //insert class name
        public bool Insert(string name, string type, string category, string parent, ref int reference)
        {
            if (!Duplication(name))
            {
                Name.Add(name);
                Type.Add(type);
                Category.Add(category);
                Parent.Add(parent);
                Reference.Add(tracker);

                reference = tracker;

                objectName.Add(new Class_Def_table());

                tracker = tracker + 1;

                return true;
            }
            else
            {
                return false;
            }
        }

        //checking class name duplication
        public bool Duplication(string name)
        {
            if (Name.Count != 0)
            {
                for (int i = 0; i < Name.Count; i++)
                {
                    if (Name[i] == name)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        //lookup in the definition table for a class
        public bool lookupDT(string name, ref string category, ref string parent)
        {
            for (int i = 0; i < Name.Count; i++)
            {
                if (Name[i] == name)
                {
                    category = Category[i];
                    parent = Parent[i];
                    return true;
                }
            }
            return false;
        }

        //lookup function in the class definition table
        public string lookupFUNC(string name, string PL, int CTaddress, ref string am)
        {
            //int CTaddress = ClsNametoClsAdd(ClassName);
            //if (CTaddress != -1)
            //{
            for (int i = 0; i < objectName[CTaddress].Name.Count; i++)
            {
                if (objectName[CTaddress].Name[i] == name && objectName[CTaddress].ParaMeterList[i] == PL)
                {
                    am = objectName[CTaddress].AccessModifier[i];
                    //tm = objectName[CTaddress].TypeModifier[i];

                    return objectName[CTaddress].ReturnType[i];
                }
            }
            return null;
            //}
            //else
            //{
            //    return null;
            //}
        }

        //convert from class name to a class address
        public int ClsNametoClsAdd(string ClassName)
        {
            int CTaddress = 0;
            for (int i = 0; i < Name.Count; i++)
            {
                if (Name[i] == ClassName)
                {
                    CTaddress = Reference[i];
                    return CTaddress;
                }
            }
            return -1;
        }

        //lookup in a specific class body for anything but a function
        public string lookupCDT(string name, int CTaddress)
        {
            //int CTaddress = ClsNametoClsAdd(ClassName);

            //if (CTaddress != -1)
            //{
                for (int i = 0; i < objectName[CTaddress].Name.Count; i++)
                {
                    if (objectName[CTaddress].Name[i] == name)
                    {
                        return objectName[CTaddress].Type[i];
                    }
                }
                return null;
            //}
            //else
            //{
            //    return null;
            //}
        }

    }

    class Class_Def_table
    {
        int tracker = 0;

        public List<string> Name = new List<string>();
        public List<string> Type = new List<string>();
        public List<string> AccessModifier = new List<string>();
        public List<string> ParaMeterList = new List<string>();
        public List<string> ReturnType = new List<string>();

        //insert function in a specific class body 
        public bool InsertForFunction(string name, string type, string accessModifier, string paraMeterList, string Rt)
        {
            if (!Duplication(name, paraMeterList))
            {
                Name.Add(name);
                Type.Add(type);
                AccessModifier.Add(accessModifier);
                ParaMeterList.Add(paraMeterList);
                ReturnType.Add(Rt);

                tracker = tracker + 1;

                return true;
            }
            else
            {
                return false;
            }
        }
        
        //check for the name of a function before inserting it to avoid duplication 
        public bool Duplication(string name, string paraMeterList)
        {
            if (Name.Count != 0)
            {
                for(int i=0; i < Name.Count;i++)
                {
                    if (Name[i] == name)
                    {
                        if (ParaMeterList[i] == null) 
                        {
                            Console.WriteLine("a variable already has a same name");
                            return true;
                        }
                        else if (ParaMeterList[i] == paraMeterList)
                        {
                            Console.WriteLine("a function with the same name is already declared");
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        
        //insert in a specific class body anything but the function 
        public bool Insert(string name, string type)
        {
            if (!Duplication(name))
            {
                Name.Add(name);
                Type.Add(type);
                AccessModifier.Add(null);
                ParaMeterList.Add(null);
                ReturnType.Add(null);

                tracker = tracker + 1;

                return true;
            }
            else
            {
                return false;
            }
        }
        
        //check for the name of the variable to inserting before inserting it to avoid duplication 
        public bool Duplication(string name)
        {
            if (Name.Count != 0)
            {
                for (int i=0; i < Name.Count;i++)
                {
                    if (Name[i] == name)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

    }

    class Func_table
    {
        int tracker = 0;
        int scopeValue = 0;
        List<string> Name = new List<string>();
        List<string> Type = new List<string>();
        List<int> Scope = new List<int>();
        public Stack<int> scope = new Stack<int>();

        //inserting the variable in the function table
        public bool Insert(string name, string type, int scope)
        {
            if (!Duplication(name))
            {
                Name.Add(name);
                Type.Add(type);
                Scope.Add(scope);

                tracker = tracker + 1;

                return true;
            }
            else
            {
                return false;
            }
        }

        //checking for the duplication of the variable
        public bool Duplication(string name)
        {
            if (Name.Count != 0)
            {
                int i = 0;
                while (i < Name.Count)
                {
                    if (Name[i] == name)
                    {
                        return true;
                    }
                    else
                    {
                        i++;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        //looking up variable in the function table
        public string lookupFT(string name, int scope, int CTaddress)
        {
            Def_table def_table = new Def_table();

            hm:
            for (int i = 0; i < Scope.Count; i++)
            {
                if (Scope[i] == scope && Name[i] == name)
                {
                    return Type[i];
                }
            }


            scope = scope - 1;
            if (scope>=0)
            {
                goto hm;
            }

            for (int i = 0; i < def_table.objectName[CTaddress].Name.Count; i++)
            {
                if (def_table.objectName[CTaddress].Name[i] == name)
                {
                    return def_table.objectName[CTaddress].Type[i];
                }
            }

            return null;
        }

        public void CreateScope()
        {
            scopeValue = scopeValue + 1;
            scope.Push(scopeValue);
        }

        public void DestroyScope()
        {
            scope.Pop();
        }

        public int GetCurrentScope()
        {
            int CurrentScope=0;

            if (scope.Count > 0)
            {
                CurrentScope = scope.Peek();
            }
            return CurrentScope;
        }
    }

    class compatibility
    {
        public string DualCompatibility(string t1, string t2, string t3)
        {
            switch (t3)
            {
                case "*":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "int";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else
                    {
                        return null;
                    }
                case "/":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else
                    {
                        return null;
                    }
                case "%":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else
                    {
                        return null;
                    }
                case "+":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "int";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "string" && t2 == "float")
                    {
                        return "string";
                    }
                    else if (t1 == "float" && t2 == "string")
                    {
                        return "string";
                    }
                    else if (t1 == "int" && t2 == "string")
                    {
                        return "string";
                    }
                    else if (t1 == "string" && t2 == "int")
                    {
                        return "string";
                    }
                    else if (t1 == "string" && t2 == "string")
                    {
                        return "string";
                    }
                    else if (t1 == "string" && t2 == "char")
                    {
                        return "string";
                    }
                    else if (t1 == "char" && t2 == "string")
                    {
                        return "string";
                    }
                    else
                    {
                        return null;
                    }
                case "-":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "int";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "float";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else
                    {
                        return null;
                    }

                case "&&":
                    if (t1 == "bool" && t2 == "bool")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }

                case "||":
                    if (t1 == "bool" && t2 == "bool")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }

                case "<":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case "<=":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case ">":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case ">=":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case "==":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "string" && t2 == "string")
                    {
                        return "bool";
                    }
                    else if (t1 == "char" && t2 == "char")
                    {
                        return "bool";
                    }
                    else if (t1 == "bool" && t2 == "bool")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case "!=":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "int")
                    {
                        return "bool";
                    }
                    else if (t1 == "int" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "bool";
                    }
                    else if (t1 == "string" && t2 == "string")
                    {
                        return "bool";
                    }
                    else if (t1 == "char" && t2 == "char")
                    {
                        return "bool";
                    }
                    else if (t1 == "bool" && t2 == "bool")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                case "=":
                    if (t1 == "int" && t2 == "int")
                    {
                        return "int";
                    }
                    else if (t1 == "float" && t2 == "float")
                    {
                        return "float";
                    }
                    else if (t1 == "string" && t2 == "string")
                    {
                        return "string";
                    }
                    else if (t1 == "char" && t2 == "char")
                    {
                        return "char";
                    }
                    else if (t1 == "bool" && t2 == "bool")
                    {
                        return "bool";
                    }
                    else
                    {
                        return null;
                    }
                default:
                    return null;
            }
        }

        public string UnaryCompatibility(string t1, string t2)
        {
            if (t1 == "!" && t2 == "bool")
            {
                return "bool";
            }
            else if (t1 == "++" && t2 == "int")
            {
                return "int";
            }
            else if (t1 == "--" && t2 == "int")
            {
                return "int";
            }
            else if (t1 == "++" && t2 == "int")
            {
                return "int";
            }
            else if (t1 == "--" && t2 == "int")
            {
                return "int";
            }
            else if (t1 == "++" && t2 == "float")
            {
                return "float";
            }
            else if (t1 == "--" && t2 == "float")
            {
                return "float";
            }
            else if (t1 == "float" && t2 == "++")
            {
                return "float";
            }
            else if (t1 == "float" && t2 == "--")
            {
                return "float";
            }
            else if (t1 == "int" && t2 == "++")
            {
                return "int";
            }
            else if (t1 == "int" && t2 == "--")
            {
                return "int*";
            }
            else
            {
                return null;
            }
        }
    }
}