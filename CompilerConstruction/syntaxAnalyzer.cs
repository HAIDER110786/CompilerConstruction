 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Semantic;
using ICG;

namespace ConsoleApp31
{
    class Syntax
    {
        Def_table def_table = new Def_table();
        Func_table funct_table = new Func_table();
        compatibility compatibility = new compatibility();
        Intermediate_code_generation ICG = new Intermediate_code_generation();


        //RE for the identifier without _
        Regex reg = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");


        //RE for the identifier with _
        Regex reg1 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");

        List<string> cp;
        List<string> vp;
        List<int> lineno;
        static int ind = 0;

        public Syntax(List<string> cp, List<string> vp, List<int> lineno)
        {
            File.WriteAllText("C:/Users/Anthony/Desktop/outputFile.txt", String.Empty);
            this.cp = cp;
            this.vp = vp;
            this.lineno = lineno;
            if (Start())
            {
                if (this.cp[ind] == "$")
                {
                    Console.WriteLine("No Syntax Error");
                }
            }
            Console.ReadKey();
        }

        public bool Start()
        {
            if (Class())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 40);
                return false;
            }
        }

        public bool Class()
        {
            //first set checking
            if (cp[ind] == "class" || cp[ind] == "sealed")
            {
                string category = null;
                if (Sealed(ref category))
                {
                    if (cp[ind] == "class")
                    {
                        string type = cp[ind];
                        ind++;
                        string name = null, parent = null;
                        if (Class_id(ref name, ref parent))
                        {
                            int CTaddress = 0;
                            if (def_table.Insert(name, type, category, parent, ref CTaddress))
                            {
                                if (cp[ind] == "{")
                                {
                                    ind++;
                                    if (Class_body(CTaddress))
                                    {
                                        if (cp[ind] == "}")
                                        {
                                            ind++;
                                            if (any_class_other_than_the_main_class())
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 69);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 75);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 81);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Redeclaration error");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 87);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 93);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 99);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 105);
                return false;
            }
        }

        public bool Sealed(ref string Category)
        {
            //first set checking
            if (cp[ind] == "sealed")
            {
                Category = "sealed";
                ind++;
                return true;
            }
            //follow set
            else if (cp[ind] == "class")
            {
                Category = "general";
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 125);
                return false;
            }
        }

        public bool Class_id(ref string name, ref string parent)
        {
            //first set checking
            if (cp[ind] == "ID")
            {
                name = vp[ind];
                ind++;
                if (Inheritance(ref parent))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 141);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 147);
                return false;
            }
        }

        public bool Inheritance(ref string parent)
        {
            if (cp[ind] == ":")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    parent = vp[ind];
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 163);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "{")
            {
                parent = "object";
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 174);
                return false;
            }
        }

        public bool Class_body(int CTaddress)
        {
            if (cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool" || cp[ind] == "public" || cp[ind] == "private" ||
                cp[ind] == "protected" || cp[ind] == "ID" || cp[ind] == "class" || cp[ind] == "sealed")
            {
                if (C_B(CTaddress))
                {
                    if (cp[ind] == "public")
                    {
                        ind++;
                        if (cp[ind] == "static")
                        {
                            ind++;
                            if (cp[ind] == "void")
                            {
                                ind++;
                                if (cp[ind] == "Main")
                                {
                                    ind++;
                                    if (cp[ind] == "(")
                                    {
                                        funct_table.CreateScope();
                                        ind++;
                                        if (def_table.objectName[CTaddress].InsertForFunction("Main", null, "public", null, null))
                                        {
                                            if (cp[ind] == ")")
                                            {
                                                ind++;
                                                if (cp[ind] == "{")
                                                {
                                                    ind++;
                                                    if (MST(CTaddress))
                                                    {
                                                        if (cp[ind] == "}")
                                                        {
                                                            funct_table.DestroyScope();
                                                            ind++;
                                                            if (C_B(CTaddress))
                                                            {
                                                                return true;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Error on line:" + 217);
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("problem in the } of the Main method");
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Error on line:" + 229);
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("problem in the { of the Main method");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(") must follow the )");
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("there can't be 2 mains");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("( must follow the Main keyword");
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("there must be a main method");
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Main return type must be void");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Main method must be static");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("there must be a main method");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 277);
                    return false;
                }
            }

            else if(cp[ind]=="}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 283);
                return false;
            }
        }

        public bool C_B(int CTaddress)
        {
            //checking for function
            if (cp[ind] == "private" || cp[ind] == "protected" || (cp[ind] == "public" && cp[ind + 1] != "static") ||
                ((cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool" || cp[ind] == "ID") && cp[ind + 1] == "ID" && cp[ind + 2] == "(")
                || cp[ind] == "void")
            {
                if (function(CTaddress))
                {
                    if (C_B(CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 305);
                    return false;
                }
            }

            //declaration checking
            else if ((cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "float" || cp[ind] == "bool" || cp[ind] == "ID") && cp[ind + 1] == "ID"
                && ((cp[ind + 2] == "=" && cp[ind + 3] != "new") || cp[ind + 2] == ";" || cp[ind + 2] == ","))
            {
                if (DEC(CTaddress))
                {
                    if (C_B(CTaddress))
                    {
                        return true;
                    }
                    else
                    { 
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 374);
                    return false;
                }
            }
            //array_dec
            else if ((cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool") && cp[ind + 1] == "[")
            {
                if (Array_dec())
                {
                    if (C_B(CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 465);
                    return false;
                }
            }

            //class inside the class
            else if (cp[ind] == "sealed" || cp[ind] == "class")
            {
                if (any_class_other_than_the_main_class())
                {
                    if (C_B(CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 343);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 349);
                    return false;
                }
            }

            //object declaration
            else if (cp[ind] == "ID")
            {
                if (OBJ_DEC(CTaddress))
                {
                    if (C_B(CTaddress))
                    {
                        return true;
                    }
                    else
                    {

                        Console.WriteLine("Error on line:" + 384);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 390);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }

            //follow set
            else if (cp[ind] == "public" && cp[ind + 1] == "static")
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        //all the above ahamdulillah checked semantically as well

        //function fully checked alhamdulillah
        public bool function(int CTaddress)
        {
            string am = null;
            if (AM(ref am))
            {
                string rt = null;
                if (RT(ref rt))
                {
                    if (cp[ind] == "ID")
                    {
                        string name = vp[ind];
                        ind++;
                        if (cp[ind] == "(")
                        {
                            string pl = null;
                            ind++;

                            funct_table.CreateScope();
                            if (PL(ref pl))
                            {
                                ICG.Output(CTaddress.ToString()+"_"+name+"_"+pl);
                                string type = pl + "->" + rt;
                                if (def_table.objectName[CTaddress].InsertForFunction(name, type, am, pl, rt))
                                {
                                    if (cp[ind] == ")")
                                    {
                                        ind++;
                                        if (cp[ind] == "{")
                                        {
                                            ind++;
                                            if (MST(CTaddress))
                                            {
                                                if (cp[ind] == "}")
                                                {
                                                    ICG.Output("endP");
                                                    ICG.Output("");
                                                    funct_table.DestroyScope();

                                                    ind++;
                                                    return true;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Error on line:" + 468);
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error on line:" + 474);
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 480);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 486);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Function name redeclaration");
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 492);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 498);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 504);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 510);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 516);
                return false;
            }
        }

        public bool AM(ref string am)
        {
            if (cp[ind] == "public" || cp[ind] == "private" || cp[ind] == "protected")
            {
                am = cp[ind];
                ind++;
                return true;
            }
            //follow set
            else if (cp[ind] == "void" || cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool")
            {
                am = "private";
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 535);
                return false;
            }
        }

        public bool RT(ref string rt)
        {
            if (cp[ind] == "void")
            {
                rt = cp[ind];
                ind++;
                return true;
            }
            else if (DT(ref rt))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 553);
                return false;
            }
        }


        public bool DT(ref string rt)
        {
            if (cp[ind] == "int" || cp[ind] == "bool" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "ID")
            {

                if (cp[ind] != "ID")
                {
                    rt = cp[ind];
                }
                else if (cp[ind] == "ID")
                {
                    rt = vp[ind];
                }
                string rt2ndpart = null;

                ind++;
                if (in1(ref rt2ndpart))
                {
                    rt = rt + rt2ndpart;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 570);
                    return false;
                }
            }
            else
            {
                //Console.WriteLine("Error on line:" +576);
                return false;
            }
        }

        public bool in1(ref string rt2ndpart)
        {
            if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "]")
                {
                    rt2ndpart = "[]";
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 593);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "ID")
            {
                rt2ndpart = null;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 604);
                return false;
            }
        }

        public bool PL(ref string pl)
        {
            string type = null;
            if (DT(ref type))
            {
                pl = type;
                if (cp[ind] == "ID")
                {
                    if (funct_table.Insert(vp[ind], type, funct_table.GetCurrentScope()))
                    {
                        ind++;
                        if (PL1(ref pl,type))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 1067);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1073);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")")
            {
                pl = "void";
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 1084);
                return false;
            }
        }

        public bool PL1(ref string pl,string type)
        {
            if (cp[ind] == ",")
            {
                ind++;
                type = null;
                if (DT(ref type))
                {
                    pl = pl + "," + type;
                    if (cp[ind] == "ID")
                    {
                        if (funct_table.Insert(vp[ind], type, funct_table.GetCurrentScope()))
                        {
                            ind++;
                            if (PL1(ref pl,type))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 1105);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Redeclaration error");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1111);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1117);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")")
            {
                pl = null;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 1128);
                return false;
            }
        }
        //above all functions checked 
        
        public bool OBJ_DEC(int CTaddress)
        {
            //first set checking
            if (cp[ind] == "ID")
            {
                string ID=vp[ind];
                ind++;
                string category = null;
                string parent = null;

                if (def_table.lookupDT(ID, ref category, ref parent))
                {
                    if (category != "sealed")
                    {
                        if (OL(ID, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 388);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("sealed class cannot be inherited");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("type error" + 388);
                    Console.WriteLine("error on the line :"+lineno[ind]);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 394);
                return false;
            }
        }

        public bool OL(string ID, int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                if (funct_table.scope.Count == 0)
                { 
                    if (def_table.objectName[CTaddress].Insert(vp[ind], ID))
                    {
                        ind++;
                        if (cp[ind] == "=")
                        {
                            ind++;
                            if (cp[ind] == "new")
                            {
                                ind++;
                                if (cp[ind] == "ID")
                                {
                                    string parent = null, category = null;

                                    def_table.lookupDT(vp[ind], ref category, ref parent);

                                    if (vp[ind] == ID || parent == ID)
                                    {
                                        ind++;
                                        if (cp[ind] == "(")
                                        {
                                            ind++;
                                            //string Tf = null;
                                            //if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                                            //{
                                            if (cp[ind] == ")")
                                            {
                                                ind++;
                                                if (cp[ind] == ";")
                                                {
                                                    ind++;
                                                    return true;
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error on line:" + 425);
                                                return false;
                                            }
                                            //}
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 437);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("type of object and the new instance of the object do not match");
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 437);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 449);
                                return false;
                            }
                        }
                        else if (cp[ind] == ";")
                        {
                            ind++;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 455);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else if(funct_table.scope.Count>0)
                {
                    if (funct_table.Insert(vp[ind],ID,funct_table.GetCurrentScope()))
                    {
                        ind++;
                        if (cp[ind] == "=")
                        {
                            ind++;
                            if (cp[ind] == "new")
                            {
                                ind++;
                                if (cp[ind] == "ID")
                                {
                                    string parent = null, category = null;

                                    def_table.lookupDT(vp[ind], ref category, ref parent);

                                    if (vp[ind] == ID || parent == ID)
                                    {
                                        ind++;
                                        if (cp[ind] == "(")
                                        {
                                            ind++;
                                            //string Tf = null;
                                            //if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                                            //{
                                            if (cp[ind] == ")")
                                            {
                                                ind++;
                                                if (cp[ind] == ";")
                                                {
                                                    Console.WriteLine(cp[ind+1]);
                                                    ind++;
                                                    return true;
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error on line:" + 425);
                                                return false;
                                            }
                                            //}
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 437);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("type of object and the new instance of the object do not match");
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 437);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 449);
                                return false;
                            }
                        }
                        else if (cp[ind] == ";")
                        {
                            ind++;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 455);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "]")
                {
                    ind++;
                    if (cp[ind] == "ID")
                    {
                        ind++;
                        if (cp[ind] == "=")
                        {
                            ind++;
                            if (cp[ind] == "new")
                            {
                                ind++;
                                if (cp[ind] == "ID")
                                {
                                    ind++;
                                    if (cp[ind] == "[")
                                    {
                                        ind++;
                                        if (OL1())
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 486);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 492);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 498);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 504);
                                return false;
                            }
                        }
                        else if (cp[ind] == ";")
                        {
                            ind++;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 510);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 516);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 522);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 528);
                return false;
            }
        }

        public bool OL1()
        {
            if (OE())
            {
                if (cp[ind] == "]")
                {
                    ind++;
                    if (A2())
                    {
                        if (A6())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 548);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 554);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 560);
                    return false;
                }
            }
            else if (cp[ind] == "]")
            {
                ind++;
                if (cp[ind] == "{")
                {
                    ind++;
                    if (A2point0())
                    {
                        if (cp[ind] == "}")
                        {
                            ind++;
                            if (A6())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 581);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 587);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 593);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 599);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 605);
                return false;
            }
        }

        private bool OE()
        {
            throw new NotImplementedException();
        }

        public bool any_class_other_than_the_main_class()
        {
            string category = null;
            if (Sealed(ref category))
            {
                if (cp[ind] == "class")
                {
                    string type = cp[ind];
                    ind++;
                    string name = null, parent = null;
                    if (Class_id(ref name, ref parent))
                    {
                        int CTaddress = 0;
                        if (def_table.Insert(name, type, category, parent, ref CTaddress))
                        {
                            if (cp[ind] == "{")
                            {
                                ind++;
                                if (C_B(CTaddress))
                                {
                                    if (cp[ind] == "}")
                                    {
                                        ind++;
                                        if(any_class_other_than_the_main_class())
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("error on line: "+1089);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 387);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 366);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 399);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("redeclaration error");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 404);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 384);
                    return false;
                }
            }

            //follow set start
            else if (cp[ind] == "$" || cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "float" || cp[ind] == "public" || cp[ind] == "private" || cp[ind] == "protected"
                || cp[ind] == "char" || cp[ind] == "ID" || cp[ind] == "bool" || cp[ind] == "class" || cp[ind] == "sealed" || cp[ind] == "void" || cp[ind] == "}")
            {
                return true;
            }

            else
            {
                Console.WriteLine("Error on line:" + 390);
                return false;
            }
        }

        public bool MST(int CTaddress)
        {
            //checking the first set
            if (cp[ind] == "public" || cp[ind] == "private" || cp[ind] == "protected" || cp[ind] == "void" || cp[ind] == "string" || cp[ind] == "bool"
                || cp[ind] == "int" || cp[ind] == "float" || cp[ind] == "ID" || cp[ind] == "object" || cp[ind] == "if" || cp[ind] == "while"
                || cp[ind] == "for" || cp[ind] == "INC_DEC")
            {
                if (SST(CTaddress))
                {
                    if (MST(CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 308);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 314);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 325);
                return false;
            }
        }

        public bool SST(int CTaddress)
        {
            if (cp[ind] == "while")
            {
                if (while_st(CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "for")
            {
                if (for_st(CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "object")
            {
                if (Object_Array_Dec())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "if")
            {
                if (if_else_st(CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                if (INC_DEC_BEFORE_ID(CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if ((cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "float" || cp[ind] == "bool" || cp[ind] == "ID") && cp[ind + 1] == "ID"
                && ((cp[ind + 2] == "=" && cp[ind + 3] != "new") || cp[ind + 2] == ";" || cp[ind + 2] == ","))
            {

                if (DEC(CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 374);
                    return false;
                }
            }
            else if ((cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "float" || cp[ind] == "bool") && cp[ind + 1] == "["
                && cp[ind + 2] == "]" && cp[ind + 3] == "ID" && cp[ind + 4] != "(")
            {
                if (Array_dec())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 386);
                    return false;
                }
            }
            //else if (
            //        cp[ind] == "private" || cp[ind] == "protected" || (cp[ind] == "public" && cp[ind + 1] != "static") || cp[ind] == "void" ||
            //        (cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool" || cp[ind] == "ID") &&
            //        ((cp[ind + 1] == "ID" && cp[ind + 2] == "(") ||
            //        (cp[ind + 1] == "[" && cp[ind + 2] == "]" && cp[ind + 3] == "ID" && cp[ind + 4] == "(")))
            //{
            //    if (function())
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Error on line:" + 425);
            //        return false;
            //    }
            //}
            else if ((cp[ind] == "ID" && cp[ind + 1] == "ID" && cp[ind + 2] == "=" && cp[ind + 3] == "new") || (cp[ind] == "ID" && cp[ind + 1] == "["))
            {
                if (OBJ_DEC(CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 863);
                    return false;
                }
            }
            else if (cp[ind] == "ID")
            {
                if (hm(CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 865);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 887);
                return false;
            }
        }

        public bool hm(int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                string ID = vp[ind];
                ind++;
                if (H1(ID,CTaddress))
                {
                    if (cp[ind] == ";")
                    {
                        ind++;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 906);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 912);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 918);
                return false;
            }
        }

        public bool H1(string ID,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                
                if (Tf != null && Tf != "int" && Tf != "string" && Tf != "float" && Tf != "string" && Tf != "bool" && (reg.IsMatch(Tf) || reg1.IsMatch(Tf)))
                {
                    ind++;

                    string category = null, parent = null;
                    if (def_table.lookupDT(Tf, ref category, ref parent))
                    {
                        CTaddress = def_table.ClsNametoClsAdd(Tf);
                        if (CTaddress == -1)
                        {
                            if (cp[ind] == "ID")
                            {
                                ID = vp[ind];
                                ind++;
                                if (H1(ID, CTaddress))
                                {
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 937);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 943);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("problem with the class name");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("no class of such name exists");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("type error");
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (H2())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 919);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 925);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 931);
                    return false;
                }
            }
            else if (cp[ind] == "(")
            {
                ind++;
                string Tf=null;
                if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        ind++;
                        if (H3(ID,Tf,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 978);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                ind++;

                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);

                if (Tf != null)
                {
                    string Tn = null;
                    if (OE(ref Tn, CTaddress))
                    {
                        if (compatibility.DualCompatibility(Tf, Tn, "=") != null)
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("type error");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("the variable doesnt exist");
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                string operatorr = vp[ind];
                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                if (Tf != null)
                {
                    if (cp[ind]=="INC_DEC")
                    {
                        if (compatibility.UnaryCompatibility(Tf, operatorr) != null)
                        {
                            ind++;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("error in the inc dec in the hm part");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 991);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("variable doesnt exist");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 996);
                return false;
            }
        }

        public bool PLIFW_FUN_CALL(ref string Tf, int CTaddress)
        {
            string curr_pl = null;
            if (OE(ref curr_pl,CTaddress))
            {
                Tf = curr_pl;
                if (PLIFW_FUN_CALL1(ref Tf,CTaddress,curr_pl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")")
            {
                Tf = "void";
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PLIFW_FUN_CALL1(ref string Tf, int CTaddress, string curr_pl)
        {
            if (cp[ind] == ",")
            {
                ind++;
                if (OE(ref curr_pl, CTaddress))
                {
                    Tf = Tf + "," + curr_pl;
                    if (PLIFW_FUN_CALL1(ref Tf, CTaddress,curr_pl))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool H2()
        {
            if (cp[ind] == ".")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (H1())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1049);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1055);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                ind++;
                if (OE())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1068);
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                if (vp[ind] == "++" || vp[ind] == "--")
                {
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1080);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1086);
                return false;
            }
        }

        private bool H1()
        {
            throw new NotImplementedException();
        }

        public bool H3(string ID,string Tf,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                string am = null;
                Tf = def_table.lookupFUNC(ID, Tf, CTaddress, ref am);

                for (int i = 0; i < def_table.objectName[CTaddress].Name.Count(); i++)
                {
                    Console.WriteLine(def_table.objectName[CTaddress].Name[i]);
                }

                string category = null, parent = null;
                if (def_table.lookupDT(Tf, ref category, ref parent))
                {
                    if (def_table.ClsNametoClsAdd(Tf) != -1)
                    {
                        ind++;
                        CTaddress = def_table.ClsNametoClsAdd(Tf);
                        if (cp[ind] == "ID")
                        {
                            ID = vp[ind];
                            ind++;
                            if (H1(ID, CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 1105);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 1111);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("probelm converting line to address");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("some problem with the name if the class");
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ";")
            {
                string am = null, tm = null;
                if (def_table.lookupFUNC(ID, Tf, CTaddress, ref am)!=null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("No function like this occurs");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1122);
                return false;
            }
        }
        //checked

        //i am here right now
        public bool hmOE(ref string Tf,int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                string ID = vp[ind];
                ind++;
                if (H11(ref Tf,ID,CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 912);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "MDM" || cp[ind] == "PM" || cp[ind] == "ROP" || cp[ind] == "&&" || cp[ind] == "||" || cp[ind] == "," || cp[ind] == ";"
                || cp[ind] == ")"
                || cp[ind] == "}" || cp[ind] == "]")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 918);
                return false;
            }
        }

        public bool H11(ref string Tf,string ID,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                if (Tf != null)
                {
                    CTaddress = def_table.ClsNametoClsAdd(Tf);
                    if (CTaddress != -1)
                    {
                        ind++;
                        if (cp[ind] == "ID")
                        {
                            ID = vp[ind];
                            ind++;
                            if (H11(ref Tf, ID, CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 937);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 943);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("no class of such name exists");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "INC_DEC")
                    {
                        if (vp[ind] == "++" || vp[ind] == "--")
                        {
                            ind++;
                            if (cp[ind] == "]")
                            {
                                ind++;
                                if (H22())
                                {
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 919);
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (cp[ind] == "]")
                    {
                        ind++;
                        if (H22())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 919);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 925);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 931);
                    return false;
                }
            }
            else if (cp[ind] == "(")
            {
                ind++;
                if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        ind++;
                        if (H33(ref Tf,ID,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 978);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                string operatorr = "=";
                ind++;
                string Tn = null;
                if (OE(ref Tn,CTaddress))
                {
                    if (compatibility.DualCompatibility(Tf, Tn, operatorr) != null)
                    {
                        Tf = compatibility.DualCompatibility(Tf, Tn, operatorr);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                if (vp[ind] == "++" || vp[ind] == "--")
                {
                    if (compatibility.UnaryCompatibility(Tf, vp[ind]) != null)
                    {
                        Tf = compatibility.UnaryCompatibility(Tf, vp[ind]);
                        ind++;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 991);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "MDM" || cp[ind] == "PM" || cp[ind] == "ROP" || cp[ind] == "&&" || cp[ind] == "||" || cp[ind] == "," || cp[ind] == ";" || cp[ind] == ")"
                || cp[ind] == "}" || cp[ind] == "]")
            {
                Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 996);
                return false;
            }
        }

        public bool H22()
        {
            if (cp[ind] == ".")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (H11())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1049);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1055);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                ind++;
                if (OE())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1068);
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                if (vp[ind] == "++" || vp[ind] == "--")
                {
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1080);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "MDM" || cp[ind] == "PM" || cp[ind] == "ROP" || cp[ind] == "&&" || cp[ind] == "||" || cp[ind] == "," || cp[ind] == ";" || cp[ind] == ")"
                || cp[ind] == "}" || cp[ind] == "]")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 1086);
                return false;
            }
        }

        private bool H11()
        {
            throw new NotImplementedException();
        }

        public bool H33(ref string Tf,string ID,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                string am = null;
                Tf = def_table.lookupFUNC(ID, Tf, CTaddress,ref am);
                CTaddress = def_table.ClsNametoClsAdd(Tf);
                if (CTaddress != -1)
                {
                    ind++;
                    if (cp[ind] == "ID")
                    {
                        ind++;
                        if (H11(ref Tf,ID,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 1105);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1111);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("no class of such name exist");
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "MDM" || cp[ind] == "PM" || cp[ind] == "ROP" || cp[ind] == "&&" || cp[ind] == "||" || cp[ind] == "," || cp[ind] == ";" || cp[ind] == ")"
                || cp[ind] == "}" || cp[ind] == "]")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 1122);
                return false;
            }
        }
        //checked all the above

        public bool Array_dec()
        {
            if (DT_with_no_extension())
            {
                if (cp[ind] == "[")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (cp[ind] == "ID")
                        {
                            ind++;
                            if (P1())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 431);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 437);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 443);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 449);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 653);
                return false;
            }
        }

        public bool P1()
        {
            if (cp[ind] == "=")
            {
                ind++;
                if (cp[ind] == "new")
                {
                    ind++;
                    if (DT_with_no_extension())
                    {
                        if (cp[ind] == "[")
                        {
                            ind++;
                            if (P2())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 484);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 490);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 496);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 502);
                    return false;
                }
            }
            else if (P6())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 508);
                return false;
            }
        }

        public bool P2()
        {
            if (cp[ind] == "]")
            {
                ind++;
                if (P3())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 524);
                    return false;
                }
            }
            else if (cp[ind] == "const")
            {
                ind++;
                if (cp[ind] == "]")
                {
                    ind++;
                    if (P4())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 540);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 546);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 552);
                return false;
            }
        }

        public bool P3()
        {
            if (cp[ind] == "{")
            {
                ind++;
                if (P7())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 570);
                    return false;
                }
            }
            else
            {
                //Console.WriteLine("Error on line:" +582);
                return false;
            }
        }

        public bool P4()
        {
            if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else if (P3())
            {
                return true;
            }
            else if (cp[ind] == ",")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (P1())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 600);
                return false;
            }
        }

        public bool P5()
        {
            if (cp[ind] == ",")
            {
                ind++;
                if (OE())
                {
                    if (P5())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 618);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 624);
                    return false;
                }
            }
            else if (P8())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 643);
                return false;
            }
        }

        public bool P6()
        {
            if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else if (cp[ind] == ",")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (P1())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 664);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 670);
                return false;
            }
        }

        public bool P7()
        {
            if (OE())
            {
                if (P5())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 637);
                    return false;
                }
            }
            else if (P8())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool P8()
        {
            if (cp[ind] == "}")
            {
                ind++;
                if (P6())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool if_else_st(int CTaddress)
        {
            if (cp[ind] == "if")
            {
                ind++;
                if (cp[ind] == "(")
                {
                    ind++;
                    string Tn = null, tn = null;
                    //this small tn would be used in the ICG
                    if (OE(ref Tn,CTaddress))
                    {
                        if (cp[ind] == ")")
                        {
                            string L1 = ICG.CreateLabel();

                            ICG.Output("if (" + tn + " == false jmp " + L1);

                            ind++;
                            funct_table.CreateScope();
                            if (body(CTaddress))
                            {
                                funct_table.DestroyScope();
                                if (OELSE(CTaddress,L1))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool OELSE(int CTaddress,string L1)
        {
            if (cp[ind] == "else")
            {
                string L2 = ICG.CreateLabel();
                ICG.Output("jmp " + L2);

                ind++;
                funct_table.CreateScope();
                if (body(CTaddress))
                {
                    ICG.Output(L1 + ":");
                    funct_table.DestroyScope();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}" || cp[ind] == "ID" || cp[ind] == "INC_DEC" || cp[ind] == "int" || cp[ind] == "string" || cp[ind] == "char" || cp[ind] == "bool" || cp[ind] == "void" ||
                cp[ind] == "if" || cp[ind] == "while" || cp[ind] == "for" || cp[ind] == "public" || cp[ind] == "private" || cp[ind] == "protected" || cp[ind] == "object")
            {
                ICG.Output(L1 + ":");
                return true;
            }
            else
            {
                return false;
            }
        }
        //checked all the above

        public bool for_st(int CTaddress)
        {
            if (cp[ind] == "for")
            {
                ind++;
                if (cp[ind] == "(")
                {
                    funct_table.CreateScope();
                    ind++;
                    if (C1(CTaddress))
                    {
                        if (C2(CTaddress))
                        {
                            if (cp[ind] == ";")
                            {
                                ind++;
                                if (C3(CTaddress))
                                {
                                    if (cp[ind] == ")")
                                    {
                                        ind++;
                                        if (body(CTaddress))
                                        {
                                            funct_table.DestroyScope();
                                            return true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error on line:" + 391);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 397);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 403);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 409);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 415);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 421);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 427);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 433);
                return false;
            }
        }

        public bool C1(int CTaddress)
        {
            if (cp[ind] == "int" || cp[ind] == "float")
            {
                if (DEC_FOR_LOOP(CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 448);
                    return false;
                }
            }
            else if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else if (cp[ind] == "ID")
            {
                if (Assignment_statement(CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 466);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 472);
                return false;
            }
        }

        public bool DEC_FOR_LOOP(int CTaddress)
        {
            string VarType = null;
            if (DT_with_no_extension_for_FOR_LOOP(ref VarType))
            {
                if (cp[ind] == "ID")
                {
                    string name = vp[ind];
                    bool insertionResult = false;

                    if (funct_table.scope.Count < 0)
                    {
                        insertionResult = def_table.objectName[CTaddress].Insert(name, VarType);
                    }
                    else
                    {
                        insertionResult = funct_table.Insert(name, VarType, funct_table.GetCurrentScope());
                    }
                    

                    if (insertionResult)
                    {
                        ind++;
                        if (L2(VarType, CTaddress))
                        {
                            if (L3(VarType, CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 564);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 570);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 576);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1042);
                return false;
            }
        }

        public bool DT_with_no_extension_for_FOR_LOOP(ref string VarType)
        {
            if (cp[ind] == "int" || cp[ind] == "float")
            {
                VarType = cp[ind];
                ind++;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 2738);
                return false;
            }
        }

        public bool Assignment_statement(int CTaddress)
        {
            if (AS(CTaddress))
            {
                if (cp[ind] == ";")
                {
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 488);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 494);
                return false;
            }
        }

        public bool DT_with_no_extension()
        {
            if (cp[ind] == "int" || cp[ind] == "bool" || cp[ind] == "string" || cp[ind] == "float")
            {
                ind++;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 710);
                return false;
            }
        }

        public bool DT_with_no_extension_for_declaration(ref string VarType)
        {
            if (cp[ind] == "int" || cp[ind] == "bool" || cp[ind] == "string" || cp[ind] == "float" || cp[ind] == "ID")
            {
                VarType = cp[ind];
                ind++;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 2789);
                return false;
            }
        }

        public bool DEC(int CTaddress)
        {
            string VarType = null;
            if (DT_with_no_extension_for_declaration(ref VarType))
            {
                if (cp[ind] == "ID")
                {
                    string name = vp[ind];
                    bool insertionResult = false;

                    if (funct_table.scope.Count < 0)
                    {
                        insertionResult = def_table.objectName[CTaddress].Insert(name, VarType);
                    }
                    else
                    {
                        insertionResult = funct_table.Insert(name, VarType, funct_table.GetCurrentScope());
                    }




                    if (insertionResult)
                    {
                        ind++;
                        if (L2(VarType,CTaddress))
                        {
                            if (L3(VarType,CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 564);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 570);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 576);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1042);
                return false;
            }
        }

        public bool L2(string VarType,int CTaddress)
        {
            if (cp[ind] == "=")
            {
                ind++;
                if (funct_table.scope.Count > 0)
                {
                    string Tn = null;
                    if (OE(ref Tn, CTaddress))
                    {
                        if (compatibility.DualCompatibility(VarType, Tn, "=") != null)
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("type error in the L2 or in other words error in the declaration in one of the function part");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 598);
                        return false;
                    }
                }
                else if (funct_table.scope.Count <= 0)
                {
                    string Tf = null;
                    if (E(ref Tf, CTaddress))
                    {
                        if (compatibility.DualCompatibility(VarType, Tf, "=") != null)
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(VarType);
                            Console.WriteLine(Tf);
                            Console.WriteLine("type error in the declaration in the class body");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 2899);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("error on line " + 2597);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "," || cp[ind] == ";")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +609);
                return false;
            }
        }

        public bool L3(string VarType, int CTaddress)
        {
            if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else if (cp[ind] == ",")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    string name = vp[ind];
                    string type = VarType;

                    bool insertionResult = false;

                    if (funct_table.scope.Count < 0)
                    {
                        insertionResult = def_table.objectName[CTaddress].Insert(name, VarType);
                    }
                    else
                    {
                        insertionResult = funct_table.Insert(name, VarType, funct_table.GetCurrentScope());
                    }




                    if (insertionResult)
                    { 
                        if (L2(VarType,CTaddress))
                        {
                            if (L3(VarType,CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 635);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 641);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("redeclaration error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 653);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 2492);
                return false;
            }
        }

        public bool C2(int CTaddress)
        {
            string Tn = null;
            if (OE(ref Tn, CTaddress))
            {
                //if (Tn=="bool")
                //{
                return true;
                //}
                //else
                //{
                //    Console.WriteLine("type error in the second part of the for loop");
                //    return false;
                //}
            }
            //follow set
            else if (cp[ind] == ";")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IDBI(ref string Tf,int CTaddress)
        {
            if (cp[ind] == "INC_DEC")
            {
                if (vp[ind] == "++" || vp[ind] == "--")
                {
                    string operand = null;
                    operand = vp[ind];
                    ind++;
                    if (cp[ind] == "ID")
                    {
                        string ID = vp[ind];
                        ind++;
                        if (FA1(ref Tf,ID,CTaddress))
                        {
                            if (compatibility.UnaryCompatibility(Tf, operand) != null)
                            {
                                Tf = compatibility.UnaryCompatibility(Tf, operand);
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("type error");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +742);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +754);
                    return false;
                }
            }
            //follow set
            else if (cp[ind]== ")" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == "PM" || cp[ind] == "MDM" 
                || cp[ind] == "ROP" || cp[ind] == "||" || cp[ind] == "&&")
            {
                return true;
            }
            else
            { 
                Console.WriteLine("Error on line:" +760);
                return false;
            }
        }

        public bool FA1(ref string Tf, string ID, int CTaddress)
        {
            if (cp[ind] == "(")
            {
                ind++;
                if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        string am = null;
                        string tm = null;

                        if (def_table.lookupFUNC(ID, Tf, CTaddress, ref am) != null)
                        {
                            Tf = def_table.lookupFUNC(ID, Tf, CTaddress, ref am);

                            if (def_table.ClsNametoClsAdd(Tf) != -1)
                            {

                                CTaddress = def_table.ClsNametoClsAdd(Tf);
                                //RE for the identifier without _
                                Regex reg = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");


                                //RE for the identifier with _              
                                Regex reg1 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");

                                if (Tf != null && (reg.IsMatch(Tf)) || (reg1.IsMatch(Tf)) && (Tf != "int" && Tf != "string" && Tf != "float" && Tf != "char"
                                    && Tf != "bool"))
                                {
                                    ind++;
                                    if (cp[ind] == ".")
                                    {
                                        ind++;
                                        if (cp[ind] == "ID")
                                        {
                                            ID = vp[ind];

                                            ind++;
                                            if (FA1(ref Tf, ID, CTaddress))
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error on line:" + 1156);
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("error on line " + 2644);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 1168);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("the type is not of a class");
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("no class name with exists");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("no function of such name exists");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1180);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == ".")
            {
                Tf = funct_table.lookupFT(ID,funct_table.GetCurrentScope(), CTaddress);
                //AT THIS POINT THE Tf CONTAINS THE TYPE OF THE ID WHICH MUST BE A CLASS

                //RE for the identifier without _
                Regex reg = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");

                //RE for the identifier with _
                Regex reg1 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");

                if (Tf != null && (reg.IsMatch(Tf)) || (reg1.IsMatch(Tf)) && (Tf != "int" && Tf != "string" && Tf != "float" && Tf != "char" && Tf != "bool"))
                {
                    CTaddress = def_table.ClsNametoClsAdd(Tf);

                    if (CTaddress != -1)
                    {
                        ind++;
                        if (cp[ind] == "ID")
                        {
                            ID = vp[ind];
                            ind++;

                            if (FA1(ref Tf, ID, CTaddress))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 1196);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("error on line" + 2720);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("no such class name exist");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("type error. the typeis not that of a class");
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (FA2(ref Tf,ID,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 1221);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1227);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1233);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == "PM" || cp[ind] == "MDM"
                || cp[ind] == "ROP" || cp[ind] == "||" || cp[ind] == "&&")
            {
                Tf = def_table.lookupCDT(ID, CTaddress);
                if (Tf != null)
                {
                    return true;
                }
                else  
                {
                    Console.WriteLine("type error");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1244);
                return false;
            }
        }
        
        public bool FA2(ref string Tf,string ID,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (FA1(ref Tf,ID,CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1263);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1269);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ")" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == "PM" || cp[ind] == "MDM"
                || cp[ind] == "ROP" || cp[ind] == "||" || cp[ind] == "&&")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 1280);
                return false;
            }
        }

        public bool INC_DEC_BEFORE_ID(int CTaddress)
        {
            if(cp[ind]=="INC_DEC")
            {
                string Tf = null;
                if(IDBI(ref Tf,CTaddress))
                {
                    if (cp[ind] == ";")
                    {
                        ind++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool C3(int CTaddress)
        {
            string Tn = null;
            //checking for the assignment and the inc_dec
            if (AS_ID_for_C3(CTaddress))
            {
                return true;
            }
            else if (IDBI(ref Tn,CTaddress))
            {
                return true;
            }
            //follow set
            else if (cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" + 721);
                return false;
            }
        }

        public bool AS_ID_for_C3(int CTaddress)
        {
            if(cp[ind]=="ID")
            {
                string ID = vp[ind];
                ind++;
                if(F1_Fac1(ID,CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool F1_Fac1(string ID,int CTaddress)
        {
            if (cp[ind] == "(")
            {
                string Tf = null;
                ind++;
                if (PLIFW_FUN_CALL(ref Tf, CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        string am = null;
                        Tf = def_table.lookupFUNC(ID, Tf, CTaddress, ref am);
                        CTaddress = def_table.ClsNametoClsAdd(Tf);
                        ind++;
                        if (cp[ind] == ".")
                        {
                            ind++;
                            if (cp[ind] == "ID")
                            {
                                ID = vp[ind];
                                ind++;
                                if (F1_Fac1(ID, CTaddress))
                                {
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 909);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 915);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 921);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 927);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 933);
                    return false;
                }
            }
            else if (cp[ind] == ".")
            {
                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                CTaddress = def_table.ClsNametoClsAdd(Tf);

                ind++;
                if (cp[ind] == "ID")
                {
                    ID = vp[ind];
                    ind++;
                    if (F1_Fac1(ID, CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 949);
                        return false;
                    }
                }
                else
                {


                    Console.WriteLine("Error on line:" + 955);
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (F2_Fac2())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 974);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 980);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 986);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                ind++;
                string Tn = null;
                if (OE(ref Tn, CTaddress))
                {
                    if (compatibility.DualCompatibility(Tf, Tn, "=") != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 999);
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                string Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                string operatorr = vp[ind];
                ind++;
                if (compatibility.UnaryCompatibility(Tf, operatorr)!=null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Type error");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool F2_Fac2()
        {
            if (cp[ind] == ".")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (F1_Fac1())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 1023);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1029);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                ind++;
                if (OE())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1042);
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                if (vp[ind] == "++")
                {
                    ind++;
                    return true;
                }
                else if (vp[ind] == "--")
                {
                    ind++;
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 1403);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 1048);
                return false;
            }
        }

        private bool F1_Fac1()
        {
            throw new NotImplementedException();
        }

        public bool AS(int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                string ID = vp[ind];
                ind++;
                if (F1(ID,CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool F1(string ID,int CTaddress)
        {
            if (cp[ind] == "(")
            {
                ind++;
                string Tf=null;
                if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        string am = null;
                        string category = null;
                        string parent = null;

                        Tf=def_table.lookupFUNC(ID, Tf, CTaddress, ref am);
                        def_table.lookupDT(Tf, ref category, ref parent);

                        CTaddress = def_table.ClsNametoClsAdd(Tf);

                        if (CTaddress != -1)
                        {

                            ind++;
                            if (cp[ind] == ".")
                            {
                                ind++;
                                if (cp[ind] == "ID")
                                {
                                    ID = vp[ind];
                                    ind++;
                                    if (F1(ID,CTaddress))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error on line:" + 909);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" + 915);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" + 921);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("no class of such name exist");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +927);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +933);
                    return false;
                }
            }
            else if (cp[ind] == ".")
            {
                string Tf = null;
                Tf=funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                CTaddress=def_table.ClsNametoClsAdd(Tf);

                ind++;
                if (cp[ind] == "ID")
                {
                    ID = vp[ind];
                    ind++;
                    if (F1(ID,CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +949);
                        return false;
                    }
                }
                else
                {


                    Console.WriteLine("Error on line:" +955);
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (F2())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +974);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +980);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +986);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {

                string operatorr = "=";

                string Tf = null;

                Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);

                ind++;
                string Tn = null;
                if (OE(ref Tn,CTaddress))
                {
                    if (compatibility.DualCompatibility(Tf, Tn, operatorr)!=null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +999);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool F2()
        {
            if (cp[ind] == ".")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (F1())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +1023);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +1029);
                    return false;
                }
            }
            else if (cp[ind] == "=")
            {
                ind++;
                if (OE())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +1042);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +1048);
                return false;
            }
        }

        private bool F1()
        {
            throw new NotImplementedException();
        }

        public bool while_st(int CTaddress)
        {
            if (cp[ind] == "while")
            {
                string L1 = ICG.CreateLabel();
                ICG.Output(L1 + ":");

                ind++;
                if (cp[ind] == "(")
                {
                    ind++;
                    string Tn = null, tn = null;
                    //this small tn would be used later on in the ICG
                    if (OE(ref Tn,CTaddress))
                    {
                        if (cp[ind] == ")")
                        {
                            string L2 = ICG.CreateLabel();
                            ICG.Output("if(" + tn + "==false) jmp" + L2);

                            ind++;
                            funct_table.CreateScope();
                            if (body(CTaddress))
                            {
                                funct_table.DestroyScope();

                                ICG.Output("jmp" + L1);
                                ICG.Output(L2 + ":");

                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on while body");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error in the Error in the ) before the while keyword the while keyword");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error in the OE");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error in the ( after the while keyword");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error in the while keyword");
                return false;
            }
        }

        public bool body(int CTaddress)
        {
            if (SST(CTaddress))
            {
                return true;
            }
            else if (cp[ind] == "{")
            {
                ind++;
                if (MST(CTaddress))
                {
                    if (cp[ind] == "}")
                    {
                        ind++;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +438);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +444);
                    return false;
                }
            }
            else if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2131);
                return false;
            }
        }

        public bool OE(ref string Toe,int CTaddress)
        {
            if (cp[ind] == "ID" || cp[ind] == "const" || cp[ind] == "!" || cp[ind] == "(" || cp[ind] == "INC_DEC")
            {
                if (AE(ref Toe, CTaddress))
                {
                    if (OE_dash(ref Toe,CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +474);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +480);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2037);
                return false;
            }
        }

        public bool OE_dash(ref string Toedash,int CTaddress)
        {
            if (vp[ind] == "||")
            {
                string operand = vp[ind];
                ind++;
                string Tae = null;
                if (AE(ref Tae, CTaddress))
                {
                    if (compatibility.DualCompatibility(Toedash, Tae, operand) != null)
                    {
                        Toedash = compatibility.DualCompatibility(Toedash, Tae, operand);
                        if (OE_dash(ref Toedash, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 504);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +510);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +521);
                return false;
            }
        }

        public bool AE(ref string Tae,int CTaddress)
        {
            if (cp[ind] == "ID" || cp[ind] == "const" || cp[ind] == "!" || cp[ind] == "(" || cp[ind] == "INC_DEC")
            {
                if (RE(ref Tae, CTaddress))
                {
                    if (AE_dash(ref Tae, CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +538);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +544);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +550);
                return false;
            }
        }

        public bool AE_dash(ref string Taedash,int CTaddress)
        {
            if (vp[ind] == "&&")
            {
                string operand = vp[ind];
                ind++;
                string Tre = null;
                if (RE(ref Tre, CTaddress))
                {
                    if (compatibility.DualCompatibility(Taedash, Tre, operand) != null)
                    {
                        Taedash = compatibility.DualCompatibility(Taedash, Tre, operand);
                        if (AE_dash(ref Taedash, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 568);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +574);
                    return false;
                }
            }
            //follow set
            else if (vp[ind] == "||" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +585);
                return false;
            }
        }

        public bool RE(ref string Tre,int CTaddress)
        {
            if (cp[ind] == "ID" || cp[ind] == "const" || cp[ind] == "!" || cp[ind] == "(" || cp[ind] == "INC_DEC")
            {
                if (E(ref Tre, CTaddress))
                {
                    if (RE_dash(ref Tre, CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +602);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +608);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +614);
                return false;
            }
        }

        public bool RE_dash(ref string Tredash,int CTaddress)
        {
            if (cp[ind] == "ROP")
            {
                string operand = vp[ind];
                ind++;
                string Te = null;
                if (E(ref Te, CTaddress))
                {
                    if(compatibility.DualCompatibility(Tredash,Te,operand)!=null)
                    {
                        Tredash= compatibility.DualCompatibility(Tredash, Te, operand);
                        if (RE_dash(ref Tredash, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 632);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +638);
                    return false;
                }
            }
            //follow set
            else if (vp[ind] == "||" || vp[ind] == "&&" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +649);
                return false;
            }
        }

        public bool E(ref string Te,int CTaddress)
        {
            if (cp[ind] == "ID" || cp[ind] == "const" || cp[ind] == "!" || cp[ind] == "(" || cp[ind] == "INC_DEC")
            {
                if (T(ref Te, CTaddress))
                {
                    if (E_dash(ref Te, CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +666);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +672);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +678);
                return false;
            }
        }

        public bool E_dash(ref string Tedash,int CTaddress)
        {
            if (cp[ind] == "PM")
            {
                string operand = vp[ind];
                ind++;
                string Tre = null;
                if (RE(ref Tre,CTaddress))
                {
                    if (compatibility.DualCompatibility(Tedash, Tre,operand) != null)
                    {
                        Tedash = compatibility.DualCompatibility(Tedash, Tre, operand);
                        if (E_dash(ref Tedash, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 696);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +702);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "ROP" || vp[ind] == "&&" || vp[ind] == "||" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +713);
                return false;
            }
        }

        public bool T(ref string Tt,int CTaddress)
        {
            if (cp[ind] == "ID" || cp[ind] == "const" || cp[ind] == "!" || cp[ind] == "(" || cp[ind] == "INC_DEC")
            {
                if (F(ref Tt,CTaddress))
                {
                    if (T_dash(ref Tt,CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +730);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +736);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +742);
                return false;
            }
        }

        public bool T_dash(ref string Ttdash,int CTaddress)
        {
            if (cp[ind] == "MDM")
            {
                string operand = vp[ind];
                ind++;
                string Tre=null;
                if (RE(ref Tre, CTaddress))
                {
                    if (compatibility.DualCompatibility(Ttdash, Tre, operand) != null)
                    {
                        Ttdash= compatibility.DualCompatibility(Ttdash, Tre, operand);
                        if (T_dash(ref Ttdash, CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 760);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Type error");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +766);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "PM" || cp[ind] == "ROP" || vp[ind] == "&&" || vp[ind] == "||" || cp[ind] == ";" || cp[ind] == "," || cp[ind] == "]" || cp[ind] == "}" || cp[ind] == ")")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +777);
                return false;
            }
        }

        public bool F(ref string Tf,int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                if (hmOE(ref Tf,CTaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (cp[ind] == "const")
            {
                Tf = vp[ind];
                //RE for the integer constant 
                Regex reg2 = new Regex("^[+-]{0,1}[0-9]{1,20}$");


                //RE for the float constant 
                //Regex reg3 = new Regex("provide the RE for the float constant");
                Regex reg_4 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");

                if(reg2.IsMatch(Tf))
                {
                    Tf = "int";
                }
                else if (reg_4.IsMatch(Tf))
                {
                    Tf = "float";
                }

                ind++;
                return true;
            }
            else if (cp[ind] == "(")
            {
                ind++;
                if (OE(ref Tf, CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        ind++;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 807);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" + 813);
                    return false;
                }
            }
            else if (cp[ind] == "INC_DEC")
            {
                //compatibility is checked inside the IDBI
                if (IDBI(ref Tf, CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" + 826);
                    return false;
                }
            }
            else if (cp[ind] == "!")
            {
                ind++;
                if (fn_call(ref Tf,CTaddress))
                {
                    Tf = compatibility.UnaryCompatibility(Tf, "!");
                    if (Tf != null)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("type error");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" + 832);
                return false;
            }
        }

        public bool fn_call(ref string Tf,int CTaddress)
        {
            if (cp[ind] == "ID")
            {
                string ID = vp[ind];
                ind++;
                if (Pr1(ref Tf,ID,CTaddress))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +3527);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +3533);
                return false;
            }
        }

        public bool Pr1(ref string Tf,string  ID,int  CTaddress)
        {
            if (cp[ind] == "(")
            {
                ind++;
                if (PLIFW_FUN_CALL(ref Tf,CTaddress))
                {
                    if (cp[ind] == ")")
                    {
                        ind++;
                        if (Pr2(ref Tf,ID,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +3554);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +3560);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +3566);
                    return false;
                }
            }
            else if (cp[ind] == ".")
            {
                ind++;
                Tf = funct_table.lookupFT(ID, funct_table.GetCurrentScope(), CTaddress);
                CTaddress = def_table.ClsNametoClsAdd(Tf);
                if (CTaddress != -1)
                {
                    if (cp[ind] == "ID")
                    {
                        ID = vp[ind];
                        ind++;
                        if (Pr1(ref Tf,ID,CTaddress))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" + 3582);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" + 3588);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("no class of such name exist");
                    return false;
                }
            }
            else if (cp[ind] == "[")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (cp[ind] == ".")
                        {
                            ind++;
                            if (cp[ind] == "ID")
                            {
                                ind++;
                                if (Pr1())
                                {
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Error on line:" +3613);
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +3619);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +3625);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +3631);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +3637);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +3643);
                return false;
            }
        }

        private bool Pr1()
        {
            throw new NotImplementedException();
        }

        public bool Pr2(ref string Tf,string ID,int CTaddress)
        {
            if (cp[ind] == ".")
            {
                string am = null;
                Tf = def_table.lookupFUNC(ID, Tf, CTaddress, ref am);
                CTaddress = def_table.ClsNametoClsAdd(Tf);
                ind++;
                if (cp[ind] == "ID")
                {
                    ID = vp[ind];
                    ind++;
                    if (Pr1(ref Tf,ID,CTaddress))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +3662);
                        return false;
                    }

                }
                else
                {
                    Console.WriteLine("Error on line:" +3668);
                    return false;
                }
            }
            //follow set of F
            else if (cp[ind] == "MDM" || cp[ind] == "PM" || cp[ind] == "ROP" || cp[ind] == "&&" || cp[ind] == "||" || cp[ind] == "," || cp[ind] == ";" || cp[ind] == ")"
                || cp[ind] == "}" || cp[ind] == "]")
            {
                string am = null;
                Tf = def_table.lookupFUNC(ID, Tf, CTaddress, ref am);
                if (Tf != null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("type error");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +3679);
                return false;
            }
        }
        //checked till here

        public bool Object_Array_Dec()
        {
            if (cp[ind] == "object")
            {
                ind++;
                if (cp[ind] == "[")
                {
                    ind++;
                    if (cp[ind] == "]")
                    {
                        ind++;
                        if (cp[ind] == "ID")
                        {
                            ind++;
                            if (A1())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +2357);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2363);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2369);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2375);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2381);
                return false;
            }
        }

        public bool A1()
        {
            if (cp[ind] == "=")
            {
                ind++;
                if (cp[ind] == "new")
                {
                    ind++;
                    if (cp[ind] == "object")
                    {
                        ind++;
                        if (cp[ind] == "[")
                        {
                            ind++;
                            if (fact1())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +2411);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2417);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2423);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2429);
                    return false;
                }
            }
            else if(A6())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2435);
                return false;
            }
        }
        
        public bool fact1()
        {
            if (cp[ind] == "]")
            {
                ind++;
                if (cp[ind] == "{")
                {
                    ind++;
                    if (A2point0())
                    {
                        if (cp[ind] == "}")
                        {
                            ind++;
                            if (A6())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +2459);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2465);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2471);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2477);
                    return false;
                }
            }
            else if (OE())
            {
                if (cp[ind] == "]")
                {
                    ind++;
                    if (A2())
                    {
                        if (A6())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2494);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2500);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2506);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2512);
                return false;
            }
        }

        public bool A2point0()
        {
            if (A3())
            {
                return true;
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2530);
                return false;
            }
        }

        public bool A2()
        {
            if (cp[ind] == "{")
            {
                ind++;
                if (A2point0())
                {
                    if (cp[ind] == "}")
                    {
                        ind++;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2549);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2555);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == ";" || cp[ind] == ",")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2566);
                return false;
            }
        }

        public bool A3()
        {
            if (OE())
            {
                if (A4())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +2622);
                    return false;
                }
            }
            else if (cp[ind] == "new")
            {
                ind++;
                if (fact2())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +2594);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2600);
                return false;
            }
        }

        public bool fact2()
        {
            if (cp[ind] == "object")
            {
                ind++;
                if (cp[ind] == "[")
                {
                    ind++;
                    if (fact3())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2619);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2625);
                    return false;
                }
            }
            else if (DT_with_no_extension())
            {
                if (cp[ind] == "[")
                {
                    ind++;
                    if (fact4())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2640);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2646);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2652);
                return false;
            }
        }

        public bool fact3()
        {
            if (cp[ind] == "]")
            {
                ind++;
                if (cp[ind] == "{")
                {
                    ind++;
                    if (A3())
                    {
                        if (cp[ind] == "}")
                        {
                            ind++;
                            if (A4())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +2676);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2682);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2688);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2694);
                    return false;
                }
            }
            else if (OE())
            {
                if (cp[ind] == "]")
                {
                    ind++;
                    if (A5())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2709);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2715);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2721);
                return false;
            }
        }

        public bool fact4()
        {
            if (cp[ind] == "]")
            {
                ind++;
                if (cp[ind] == "{")
                {
                    ind++;
                    if (A8())
                    {
                        if (cp[ind] == "}")
                        {
                            ind++;
                            if (A4())
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Error on line:" +2745);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2751);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2757);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2763);
                    return false;
                }
            }
            else if (OE())
            {
                if (cp[ind] == "]")
                {
                    ind++;
                    if (A7())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2778);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2784);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2790);
                return false;
            }
        }

        public bool A4()
        {
            if (cp[ind] == ",")
            {
                ind++;
                if (A3())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +2806);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2817);
                return false;
            }
        }

        public bool A5()
        {
            if (cp[ind] == "{")
            {
                ind++;
                if (A3())
                {
                    if (cp[ind] == "}")
                    {
                        ind++;
                        if (A4())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2838);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2844);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2850);
                    return false;
                }
            }
            else if (A4())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2860);
                return false;
            }
        }

        public bool A6()
        {
            if (cp[ind] == ";")
            {
                ind++;
                return true;
            }
            else if (cp[ind] == ",")
            {
                ind++;
                if (cp[ind] == "ID")
                {
                    ind++;
                    if (A1())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2884);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2890);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error on line:" +2896);
                return false;
            }
        }

        public bool A7()
        {
            if (cp[ind] == "{")
            {
                ind++;
                if (A8())
                {
                    if (cp[ind] == "}")
                    {
                        ind++;
                        if (A4())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error on line:" +2917);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2923);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2929);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2940);
                return false;
            }
        }

        public bool A8()
        {
            if (OE())
            {
                if (A9())
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error on line:" +2955);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +2966);
                return false;
            }
        }

        public bool A9()
        {
            if (cp[ind] == ",")
            {
                ind++;
                if (OE())
                {
                    if (A9())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error on line:" +2984);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Error on line:" +2990);
                    return false;
                }
            }
            //follow set
            else if (cp[ind] == "}")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error on line:" +3001);
                return false;
            }
        }
    }
}