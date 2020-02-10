using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp31
{
    class Lexical
    {

        static List<string> CP = new List<string>();
        static List<string> VP = new List<string>();
        static List<int> LINE = new List<int>();

        static List<string> Tokens = new List<string>();

        static void Main(string[] args)
        {
            int line = 1;
            string temp = null;

            //reading the text file

            string text = File.ReadAllText("C:/Users/Anthony/Desktop/mytextfolder/testfile3.txt");

            int i = 0;
            

            while (i < text.Length)
            {
                //checking for line space
                if (text[i] == ' ')
                {
                    if(temp!=null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;
                    i++;
                }
                //checking for carriage return 
                else if (text[i] == '\r')
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;
                    line++;
                    i++;
                }
                //checking for line change 
                else if (text[i] == '\n')
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;
                    i++;



                }
                //checking for tab space
                else if (text[i] == (char)9)
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;
                    i++;



                }
                //checking for punctuators
                else if (text[i] == '}' || text[i] == '{' || text[i] == '(' || text[i] == ')' || text[i] == '[' || text[i] == ']' || text[i] == ':' || text[i] == ';' || text[i] == ',')
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;

                    if (text[i] == ':' && text[i + 1] == ':')
                    {
                        temp = text[i].ToString() + text[i + 1].ToString();
                        Console.WriteLine("(" + temp + "," + "," + line + ")");
                        string listOBJ = ("(" + temp + "," + "," + line + ")");

                        CP.Add(temp);
                        VP.Add(null);
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        temp = null;
                        i = i + 2;
                    }
                    else if (text[i] == ',')
                    {
                        Console.WriteLine("," + " ," + line + ")");
                        string listOBJ = ("," + " ," + line + ")");

                        CP.Add(",");
                        VP.Add(",");
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        temp = null;
                        i = i + 1;
                    }
                    else if (text[i] == '(' && text[i+1] != ')' )
                    {
                        int j = i;
                        string tempc = null;
                        
                        while (text[i + 1] != ')' && text[i + 1] != '\r')
                        {
                            tempc += text[i + 1];
                            i++;
                        }

                        //RE for the integer constant 
                        Regex reg2 = new Regex("^[+-]{0,1}[0-9]{1,20}$");

                        //Regex reg3 = new Regex("provide the RE for the float constant");
                        Regex reg_4 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");

                        if (reg2.IsMatch(tempc))
                        {
                            Console.WriteLine("((," + " " + "," + line + ")");
                            string listOBJ = ("((," + " " + "," + line + ")");

                            CP.Add("(");
                            VP.Add(null);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);

                            Console.WriteLine("(const," + tempc + "," + line + ")");
                            string listOBJy = ("(const," + tempc + "," + line + ")");

                            CP.Add("const");
                            VP.Add(tempc);
                            LINE.Add(line);

                            Tokens.Add(listOBJy);
                            tempc = null;
                            i++;
                        }
                        else if (reg_4.IsMatch(tempc))
                        {
                            Console.WriteLine("((," + " " + "," + line + ")");
                            string listOBJ = ("((," + " " + "," + line + ")");

                            CP.Add("(");
                            VP.Add(null);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);

                            Console.WriteLine("(const," + tempc + "," + line + ")");
                            string listOBJz = ("(const," + tempc + "," + line + ")");

                            CP.Add("const");
                            VP.Add(tempc);
                            LINE.Add(line);

                            Tokens.Add(listOBJz);
                            tempc = null;
                            i++;
                        }
                        else
                        {
                            i = j;
                            Console.WriteLine("((," + " " + "," + line + ")");
                            string listOBJ = ("((," + " " + "," + line + ")");

                            CP.Add("(");
                            VP.Add(null);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            tempc = null;
                            i++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("(" + text[i] + "," + "," + line + ")");
                        string listOBJ = ("(" + text[i] + "," + "," + line + ")");

                        CP.Add(text[i].ToString());
                        VP.Add(null);
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        i++;
                    }
                }

                //checking for operators
                else if (text[i] == '+' || text[i] == '-' || text[i] == '*' || text[i] == '/' || text[i] == '%' || text[i] == '<' || text[i] == '>' || text[i] == '=' || text[i] == '!' || text[i] == '&' || text[i] == '|')
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;

                    Regex reg17 = new Regex("^[+-]{0,1}[0-9]{1,20}$");

                    switch (text[i])
                    {
                        case '+':
                            //for the ++
                            if (text[i + 1] == '+')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(INC_DEC," + temp + "," + line + ")");
                                string listOBJh = ("(INC_DEC," + temp + "," + line + ")");

                                CP.Add("INC_DEC");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJh);
                                temp = null;
                                i = i + 2;
                            }
                            //for the =+7
                            else if (i - 1 >= 0 && text[i - 1] == '=' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                string tempa = null;
                                int j = i;
                                while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                {
                                    tempa += text[i + 1];
                                    i++;

                                }
                                Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                string listOBJf = ("(const," + text[j] + tempa + "," + line + ")");

                                CP.Add("const");
                                VP.Add(text[j] + tempa);
                                LINE.Add(line);

                                Tokens.Add(listOBJf);
                                i = i + 1;
                            }
                            //for the (\n)+7
                            else if (i - 1 >= 0 && text[i - 1] == '\n' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                string tempa = null;
                                int j = i;
                                while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                {
                                    tempa += text[i + 1];
                                    i++;
                                }
                                Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                string listOBJqq = ("(const," + text[j] + tempa + "," + line + ")");

                                CP.Add("const");
                                VP.Add(text[j] + tempa);
                                LINE.Add(line);

                                Tokens.Add(listOBJqq);
                                i = i + 1;
                            }
                            //for the =+     7
                            else if (i - 1 >= 0 && text[i - 1] == '=' && text[i + 1] == ' ')
                            {
                                int j = i;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }

                                string tempa = null;

                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJ1 = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJ1);
                                    i = i + 1;
                                }
                            }
                            //for the (\n)+     7
                            else if (i - 1 >= 0 && text[i - 1] == '\n' && text[i + 1] == ' ')
                            {
                                int j = i;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }

                                string tempa = null;

                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJj = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJj);
                                    i = i + 1;
                                }
                            }
                            //for the =      +7 and for the (\n)      +7
                            else if (i - 1 >= 0 && text[i - 1] == ' ' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                int j = i;
                                while (text[i - 1] == ' ' && text[i - 1] != '\r')
                                {
                                    i--;
                                }

                                string tempa = null;
                                if (text[i - 1] == '=')
                                {
                                    i = j;
                                    if (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                        {
                                            tempa += text[i + 1];
                                            i++;
                                        }
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJm = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJm);
                                    i = i + 1;
                                }
                                else if (text[i - 1] == '\n')
                                {
                                    i = j;
                                    if (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                        {
                                            tempa += text[i + 1];
                                            i++;
                                        }
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJg = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJg);
                                    i = i + 1;
                                }
                            }
                            //for the =     +     7 and \n      +     7 
                            else if (i - 1 >= 0 && text[i - 1] == ' ' && text[i + 1] == ' ')
                            {
                                int j = i;
                                string equalstoORcarragereturn = null;
                                while (text[i - 1] == ' ')
                                {
                                    i--;
                                }
                                if (text[i - 1] == '=' || text[i - 1] == '\n')
                                {
                                    equalstoORcarragereturn = text[i - 1].ToString();
                                }
                                i = j;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }
                                string tempa = null;
                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                }
                                if ((equalstoORcarragereturn == '='.ToString() && tempa != null))
                                {
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJk = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJk);
                                    i = i + 1;
                                }
                                else if (equalstoORcarragereturn == '\n'.ToString() && tempa != null)
                                {
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJs = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJs);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(PM," + text[i] + "," + line + ")");
                                string listOBJr = ("(PM," + text[i] + "," + line + ")");

                                CP.Add("PM");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJr);
                                i = i + 1;
                            }
                            break;

                        case '-':
                            //for the --
                            if (text[i + 1] == '-')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(INC_DEC," + temp + "," + line + ")");
                                string listOBJd = ("(INC_DEC," + temp + "," + line + ")");

                                CP.Add("INC_DEC");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJd);
                                temp = null;
                                i = i + 2;
                            }
                            //for the =-7
                            else if (text[i - 1] == '=' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                string tempa = null;
                                int j = i;
                                while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                {
                                    tempa += text[i + 1];
                                    i++;
                                }
                                Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                string listOBJn = ("(const," + text[j] + tempa + "," + line + ")");

                                CP.Add("const");
                                VP.Add(text[j] + tempa);
                                LINE.Add(line);

                                Tokens.Add(listOBJn);
                                i = i + 1;
                            }
                            //for the (\n)-7
                            else if (text[i - 1] == '\n' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                string tempa = null;
                                int j = i;
                                while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                {
                                    tempa += text[i + 1];
                                    i++;
                                }
                                Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                string listOBJc = ("(const," + text[j] + tempa + "," + line + ")");

                                CP.Add("const");
                                VP.Add(text[j] + tempa);
                                LINE.Add(line);

                                Tokens.Add(listOBJc);
                                i = i + 1;
                            }
                            //for the =-     7
                            else if (text[i - 1] == '=' && text[i + 1] == ' ')
                            {
                                int j = i;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }

                                string tempa = null;

                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJl = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJl);
                                    i = i + 1;
                                }
                            }
                            //for the (\n)-     7
                            else if (text[i - 1] == '\n' && text[i + 1] == ' ')
                            {
                                int j = i;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }

                                string tempa = null;

                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJu = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJu);
                                    i = i + 1;
                                }
                            }
                            //for the =      -7 and for the (\n)      -7
                            else if (text[i - 1] == ' ' && reg17.IsMatch(text[i + 1].ToString()))
                            {
                                int j = i;
                                while (text[i - 1] == ' ' && text[i - 1] != '\r')
                                {
                                    i--;
                                }

                                string tempa = null;
                                if (text[i - 1] == '=')
                                {
                                    i = j;
                                    if (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                        {
                                            tempa += text[i + 1];
                                            i++;
                                        }
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJr = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJr);
                                    i = i + 1;
                                }
                                else if (text[i - 1] == '\n')
                                {
                                    i = j;
                                    if (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                        {
                                            tempa += text[i + 1];
                                            i++;
                                        }
                                    }
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJa = ("(const," + text[j] + tempa + "," + line + ")");

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJa);
                                    i = i + 1;
                                }
                            }
                            //for the =     -     7 and \n      -     7 
                            else if (text[i - 1] == ' ' && text[i + 1] == ' ')
                            {
                                int j = i;
                                string equalstoORcarragereturn = null;
                                while (text[i - 1] == ' ')
                                {
                                    i--;
                                }
                                if (text[i - 1] == '=' || text[i - 1] == '\n')
                                {
                                    equalstoORcarragereturn = text[i - 1].ToString();
                                }
                                i = j;
                                while (text[i + 1] == ' ')
                                {
                                    i++;
                                }
                                string tempa = null;
                                if (reg17.IsMatch(text[i + 1].ToString()))
                                {
                                    while (reg17.IsMatch(text[i + 1].ToString()) && i + 1 < text.Length - 1)
                                    {
                                        tempa += text[i + 1];
                                        i++;
                                    }
                                }
                                if ((equalstoORcarragereturn == '='.ToString() && tempa != null))
                                {
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJ1 = "(const," + text[j] + tempa + "," + line + ")";

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJ1);
                                    i = i + 1;
                                }
                                else if (equalstoORcarragereturn == '\n'.ToString() && tempa != null)
                                {
                                    Console.WriteLine("(const," + text[j] + tempa + "," + line + ")");
                                    string listOBJ2 = "(const," + text[j] + tempa + "," + line + ")";

                                    CP.Add("const");
                                    VP.Add(text[j] + tempa);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJ2);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(PM," + text[i] + "," + line + ")");
                                string listOBJ3 = ("(PM," + text[i] + "," + line + ")");

                                CP.Add("PM");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ3);
                                i = i + 1;
                            }
                            break;

                        case '*':

                            Console.WriteLine("(MDM," + text[i] + "," + line + ")");
                            string listOBJ = ("(MDM," + text[i] + "," + line + ")");

                            CP.Add("MDM");
                            VP.Add(text[i].ToString());
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            i++;
                            break;

                        case '/':
                            if (text[i + 1] == '/')
                            {
                                string tempb = null;
                                while (text[i + 2] != '\r')
                                {
                                    tempb += text[i + 2];
                                    i++;
                                }
                                Console.WriteLine("(single_line_commments," + tempb + "," + line + ")");
                                string listOBJ5 = ("(single_line_commments," + tempb + "," + line + ")");

                                CP.Add("single_line_commments");
                                VP.Add(tempb);
                                LINE.Add(line);

                                Tokens.Add(listOBJ5);
                                i = i + 2;
                            }
                            else if (text[i + 1] == '*')
                            {
                                string tempb = null;
                            han:
                                while (text[i + 2] != '*')
                                {
                                    if (text[i + 2] == '\r')
                                    {
                                        line++;
                                    }
                                    tempb += text[i + 2];
                                    i++;
                                }
                                if (text[i + 3] == '/')
                                {
                                    Console.WriteLine("(multiple_line_commments," + tempb + "," + line + ")");
                                    string listOBJ5 = ("(multiple_line_commments," + tempb + "," + line + ")");

                                    CP.Add("multiple_line_commments");
                                    VP.Add(tempb);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJ5);
                                    i = i + 4;
                                }
                                else
                                {
                                    tempb += text[i + 2];
                                    i++;
                                    goto han;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(MDM," + text[i] + "," + line + ")");
                                string listOBJ5 = ("(MDM," + text[i] + "," + line + ")");

                                CP.Add("MDM");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ5);
                                i++;
                            }
                            break;

                        case '%':

                            Console.WriteLine("(MDM," + text[i] + "," + line + ")");
                            string listOBJ6 = ("(MDM," + text[i] + "," + line + ")");

                            CP.Add("MDM");
                            VP.Add(text[i].ToString());
                            LINE.Add(line);

                            Tokens.Add(listOBJ6);
                            i++;
                            break;

                        case '<':
                            if (text[i + 1] == '=')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(ROP," + temp + "," + line + ")");
                                string listOBJ7 = ("(ROP," + temp + "," + line + ")");


                                CP.Add("ROP");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ7);
                                temp = null;
                                i = i + 2;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("(ROP," + text[i] + "," + line + ")");
                                string listOBJ8 = ("(ROP," + text[i] + "," + line + ")");

                                CP.Add("ROP");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ8);
                                i = i + 1;
                                break;
                            }
                        case '=':
                            if (text[i + 1] == '=')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(Comparsion_opr," +temp+ "," + line + ")");
                                string listOBJ9 = ("(Comparsion_opr," +temp+ "," + line + ")");

                                CP.Add("Comparsion_opr");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ9);
                                temp = null;
                                i = i + 2;
                            }
                            else
                            {
                                Console.WriteLine("(" + text[i] + ", " + "," + line + ")");
                                string listOBJ10 = ("(" + text[i] + ", " + "," + line + ")");

                                CP.Add(text[i].ToString());
                                VP.Add(null);
                                LINE.Add(line);

                                Tokens.Add(listOBJ10);
                                temp = null;
                                i = i + 1;
                            }
                            break;

                        case '>':
                            if (text[i + 1] == '=')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(ROP," + temp + "," + line + ")");
                                string listOBJ11 = ("(ROP," + temp + "," + line + ")");

                                CP.Add("ROP");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ11);
                                temp = null;
                                i = i + 2;
                            }
                            else
                            {
                                Console.WriteLine("(ROP," + text[i] + "," + line + ")");
                                string listOBJ11 = ("(ROP," + text[i] + "," + line + ")");

                                CP.Add("ROP");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ11);
                                i = i + 1;
                            }
                            break;

                        case '!':
                            if (text[i + 1] == '=')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(Comparsion_opr," + temp + "," + line + ")");
                                string listOBJ12 = ("(Comparsion_opr," + temp + "," + line + ")");

                                CP.Add("Comparsion_opr");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ12);
                                temp = null;
                                i = i + 2;
                            }
                            else
                            {
                                //for the !
                                Console.WriteLine("(!," + text[i] + "," + line + ")");
                                string listOBJh11 = ("(!," + text[i] + "," + line + ")");

                                CP.Add("!");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJh11);
                                temp = null;
                                i = i + 1;
                                break;
                            }
                            break;

                        case '&':
                            if (text[i + 1] == '&')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(&&," + temp + "," + line + ")");
                                string listOBJ14 = ("(&&," + temp + "," + line + ")");

                                CP.Add("&&");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ14);
                                temp = null;
                                i = i + 2;
                            }
                            else
                            {
                                Console.WriteLine("(invalid lexeme," + text[i] + "," + line + ")");
                                string listOBJ16 = "(invalid lexeme," + text[i] + "," + line + ")";


                                CP.Add("invalid lexeme");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ16);
                                i = i + 1;
                            }
                            break;

                        case '|':
                            if (text[i + 1] == '|')
                            {
                                temp = text[i].ToString() + text[i + 1].ToString();
                                Console.WriteLine("(||," + temp + "," + line + ")");
                                string listOBJ15 = ("(||," + temp + "," + line + ")");

                                CP.Add("||");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ15);
                                temp = null;
                                i = i + 2;
                            }
                            else
                            {
                                Console.WriteLine("(invalid lexeme," + text[i] + line + ")");
                                string listOBJ17 = ("(invalid lexeme," + text[i] + line + ")");

                                CP.Add("invalid lexeme");
                                VP.Add(text[i].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ17);
                                i = i + 1;
                            }
                            break;
                    }
                }
                //checking for double quotes
                else if (text[i] == '\"')
                {
                    if (temp != null)
                    {
                        string yo = myfunction(temp, line);
                        Tokens.Add(yo);
                    }
                    temp = null;
                    int m = i;
                    i++;
                //string testing = "a++\\a=b=c*\"\\n\\"+=56"b=c

                backtofindingthedoublequote:
                    while (text[i] != '"' && text[i] != '\r')
                    {
                        temp += text[i];
                        i++;
                    }

                    if (text[i] == '\r')
                    {
                        Console.WriteLine("(invalid lexeme," + text[m] + temp + "," + line + ")");
                        string listOBJ = ("(invalid lexeme," + text[m] + temp + "," + line + ")");

                        CP.Add("invalid lexeme");
                        VP.Add(text[m]+temp);
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        temp = null;
                        line++;
                    }

                    if (text[i] == '\"' && text[i - 1] != '\\')
                    {
                        Console.WriteLine("(const," + temp + "," + line + ")");
                        string listOBJ = ("(const," + temp + "," + line + ")");

                        CP.Add("const");
                        VP.Add(temp);
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        temp = null;
                    }
                    else if (text[i] == '\"' && text[i - 1] == '\\' && text[i - 2] != '\\')
                    {
                        temp += text[i];
                        i++;
                        goto backtofindingthedoublequote;
                    }
                    else 
                    if (text[i] == '\"' && text[i - 1] == '\\' && text[i - 2] == '\\')
                    {
                        Console.WriteLine("(const," + temp + "," + line + ")");
                        string listOBJ = ("(const," + temp + "," + line + ")");

                        CP.Add("const");
                        VP.Add(temp);
                        LINE.Add(line);

                        Tokens.Add(listOBJ);
                        temp = null;
                    }
                    i++;
                }
                //checking for single quote
                else if (text[i] == '\'')
                {
                    if (temp != null)
                    {

                        string[] KW = {"return","while","class","for","this","void","int","float","Main","if","else","char","bool","break","continue"
                                       ,"array","string","base", "sealed", "virtual", "override", "inheritance", "public", "private"
                                       ,"protected", "new","static","object"};

                        //RE for the identifier without _
                        Regex reg21 = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9_]{0,20}$");


                        //RE for the identifier with _
                        Regex reg22 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");


                        //RE for the integer constant 
                        Regex reg23 = new Regex("^[+-]{0,1}[0-9]{1,20}$");


                        //RE for the float constant 
                        Regex reg_4 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");


                        //CHECK FOR KEYWORDS
                        for (int z = 0; z < KW.Length; z++)
                        {
                            if (temp == KW[z])
                            {
                                Console.WriteLine("(" + KW[z] + "," + "," + line + ")");

                                string listOBJ = ("(" + KW[z] + "," + "," + line + ")");

                                CP.Add(KW[z]);
                                VP.Add(null);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                                goto yo7;
                            }
                        }

                        //checking for identifier without the _
                        if (reg21.IsMatch(temp))
                        {
                            Console.WriteLine("(ID," + temp + "," + line + ")");
                            string listOBJ = ("(ID," + temp + "," + line + ")");

                            CP.Add("ID");
                            VP.Add(temp);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            temp = null;
                        }
                        //checking for the identifier with the _
                        else if (reg22.IsMatch(temp))
                        {
                            Console.WriteLine("(ID," + temp + "," + line + ")");
                            string listOBJ = ("(ID," + temp + "," + line + ")");

                            CP.Add("ID");
                            VP.Add(temp);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            temp = null;
                        }
                        //checking for the integer constant
                        else if (reg23.IsMatch(temp))
                        {
                            Console.WriteLine("(const," + temp + "," + line + ")");
                            string listOBJ = ("(const," + temp + "," + line + ")");

                            CP.Add("const");
                            VP.Add(temp);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            temp = null;
                        }
                        else if (reg_4.IsMatch(temp))
                        {
                            Console.WriteLine("(const," + temp + "," + line + ")");
                            string listOBJ = ("(const," + temp + "," + line + ")");

                            CP.Add("const");
                            VP.Add(temp);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            temp = null;
                        }
                        else
                        {
                            Console.WriteLine("(invalid lexeme," + temp + "," + line + ")");
                            string listOBJ = ("(invalid lexeme," + temp + "," + line + ")");

                            CP.Add("invalid lexeme");
                            VP.Add(temp);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            temp = null;
                        }
                    }
                yo7:
                    temp = null;

                    Regex regex = new Regex("^[]+-/*!@#$%^&()_a-zA-Z0-9]{1}$");
                    Regex regex2 = new Regex("^\\[nrt0b\\]{1}$");

                    if (text[i + 1] == '\\')
                    {
                        string tempe = null;
                        tempe = text[i + 1].ToString() + text[i + 2].ToString();
                        if ((tempe == "\\n") || tempe == "\\t".ToString() || tempe == "\\b".ToString() || tempe == "\\r".ToString() && text[i + 3] == '\'')
                        {
                            Console.WriteLine("(const," + tempe + "," + line + ")");

                            string listOBJ = ("(const," + tempe + "," + line + ")");

                            CP.Add("const");
                            VP.Add(tempe);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            tempe = null;
                        }
                        else
                        {
                            Console.WriteLine("(invalid lexeme," + text[i] + tempe + text[i + 3] + "," + line + ")");
                            string listOBJ = ("(invalid lexeme," + text[i] + tempe + text[i + 3] + "," + line + ")");

                            CP.Add("invalid_lexeme");
                            VP.Add(text[i]+tempe+text[i+3]);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            tempe = null;
                        }
                        i = i + 4;
                    }
                    else if (text[i + 1] != '\\')
                    {
                        string tempe = text[i + 1].ToString();
                        if (regex.IsMatch(tempe.ToString()) && text[i + 2] == '\'')
                        {
                            Console.WriteLine("(const," + tempe + "," + line + ")");

                            string listOBJ = ("(const," + tempe + "," + line + ")");

                            CP.Add("const");
                            VP.Add(tempe);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            tempe = null;
                        }
                        else
                        {
                            Console.WriteLine("(invalid lexeme," + text[i] + tempe + text[i + 2] + "," + line + ")");
                            string listOBJ = ("(invalid lexeme," + text[i] + tempe + text[i + 2] + "," + line + ")");

                            CP.Add("invalid_lexeme");
                            VP.Add(text[i] + tempe + text[i + 2]);
                            LINE.Add(line);

                            Tokens.Add(listOBJ);
                            tempe = null;
                        }
                        i = i + 3;
                    }
                }
                //check for the end marker
                else if(text[i]=='$')
                {
                    Console.WriteLine("(" + text[i]+",,"+line + ")");
                    string listOBJ = ("(" + text[i]+",,"+line + ")");

                    CP.Add(text[i].ToString());
                    VP.Add(null);
                    LINE.Add(line);

                    break;
                }
                //keep on adding the word to the temp here until a break is encountered
                else
                {
                    string holder = null;
                    //regex for identifier without the _ 
                    Regex reg_1 = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");

                    //regex for identifier with the _ 
                    Regex reg_2 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");

                    //regex for integer_constant 
                    Regex reg_3 = new Regex("^[+-]{0,1}[0-9]{1,20}$");

                    //regex for float_constant
                    Regex reg_4 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");

                    //for checking point in the temp and in the text[i] just received 
                    if (temp != null)
                    {
                        bool pointPresense = false;
                        for (int u = 0; u < temp.Length; u++)
                        {
                            if (temp[u] == '.')
                            {
                                pointPresense = true;
                            }
                        }
                        if (text[i] == '.' && pointPresense)
                        {
                            
                            if (temp[0] == '.' && (reg_1.IsMatch(temp[1].ToString())) || (reg_2.IsMatch(temp[1].ToString())))
                            {
                                Console.WriteLine("(.," + temp[0] + "," + line + ")");

                                string listOBJ = ("(.," + temp[0] + "," + line + ")");

                                CP.Add(".");
                                VP.Add(temp[0].ToString());
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                string temp1 = null;
                                temp1 = temp;
                                temp = null;
                                for (int r = 1; r < temp1.Length; r++)
                                {
                                    temp += temp1[r];
                                }
                                Console.WriteLine("(ID," + temp + "," + line + ")");

                                string listOBJ1 = ("(ID," + temp + "," + line + ")");

                                CP.Add("ID");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ1);

                                temp = null;
                            }
                            //checking the entire for the while float
                            else if (reg_4.IsMatch(temp))
                            {
                                Console.WriteLine("(const," + temp + "," + line + ")");


                                string listOBJ = ("(const," + temp + "," + line + ")");

                                CP.Add("const");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                            //ID
                            else if (reg_1.IsMatch(temp.ToString()))
                            {
                                Console.WriteLine("(ID," + temp + "," + line + ")");

                                string listOBJ = ("ID," + temp + "," + line + ")");

                                CP.Add("ID");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                            //ID
                            else if (reg_2.IsMatch(temp.ToString()))
                            {
                                Console.WriteLine("(ID," + temp + "," + line + ")");

                                string listOBJ = ("ID," + temp + "," + line + ")");

                                CP.Add("ID");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                            //INT_CONSTANT
                            else if (reg_3.IsMatch(temp.ToString()))
                            {
                                Console.WriteLine("(const," + temp + "," + line + ")");

                                string listOBJ = ("(const," + temp + "," + line + ")");

                                CP.Add("const");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }

                            
                            //printing of the error lexeme with one more check
                            else
                            {
                                int vatc = 0;
                                holder = null;
                                while(vatc<temp.Length)
                                {
                                    if ((reg_1.IsMatch(temp[vatc].ToString()) || reg_2.IsMatch(temp[vatc].ToString())))
                                    {
                                        
                                        holder += temp[vatc];
                                        if (vatc+1<temp.Length && temp[vatc+1] == '.')
                                        {
                                            Console.WriteLine("getting here ");
                                            Console.WriteLine("(ID," + holder + "," + line + ")");

                                            string listOBJ45 = ("ID," + holder + "," + line + ")");

                                            CP.Add("ID");
                                            VP.Add(holder);
                                            LINE.Add(line);

                                            Tokens.Add(listOBJ45);

                                            holder = null;
                                        }
                                        vatc++;
                                    }
                                    else if (temp[vatc] == '.')
                                    {
                                        Console.WriteLine("(.," + " ," + line + ")");

                                        string listOBJ453 = ("(.," + " ," + line + ")");

                                        CP.Add(".");
                                        VP.Add(null);
                                        LINE.Add(line);

                                        Tokens.Add(listOBJ453);

                                        vatc++;
                                    }
                                    else if ((reg_1.IsMatch(temp[vatc].ToString()) || reg_2.IsMatch(temp[vatc].ToString())))
                                    {
                                        Console.WriteLine("(ID," + holder + "," + line + ")");

                                        string listOBJ45 = ("ID," + holder + "," + line + ")");

                                        CP.Add("ID");
                                        VP.Add(holder);
                                        LINE.Add(line);

                                        Tokens.Add(listOBJ45);

                                        holder = null;

                                        vatc++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (holder.Length > 0)
                                {
                                    if (reg_1.IsMatch(holder)|| reg_2.IsMatch(holder))
                                    {
                                        Console.WriteLine("(ID," + holder + "," + line + ")");

                                        string listOBJ = ("(ID," + holder + "," + line + ")");

                                        CP.Add("ID");
                                        VP.Add(holder);
                                        LINE.Add(line);

                                        Tokens.Add(listOBJ);
                                        
                                    }
                                    else
                                    {
                                        Console.WriteLine("(invalid lexeme," + holder + "," + line + ")");

                                        string listOBJ = ("(invalid lexeme," + holder + "," + line + ")");

                                        CP.Add("invalid lexeme");
                                        VP.Add(holder);
                                        LINE.Add(line);

                                        Tokens.Add(listOBJ);
                                    }
                                }

                                temp = null;
                            }
                        }
                    }
                    temp += text[i];
                    i++;
                    if (i >= text.Length)
                    {
                        if (temp != null)
                        {
                            Regex reg = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");


                            //RE for the identifier with _

                            Regex reg1 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");


                            //RE for the integer constant 
                            Regex reg2 = new Regex("^[+-]{0,1}[0-9]{1,20}$");


                            //RE for the float constant 
                            //Regex reg3 = new Regex("provide the RE for the float constant");
                            Regex reg_5 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");


                            //CHECK FOR KEYWORDS
                            string[] KW = {"return","while","class","for","this","void","int","float","Main","if","else","char","bool","break","continue"
                           ,"array","string","base", "sealed", "virtual", "override", "inheritance", "public", "private"
                           ,"protected", "new","static","object"};

                            for (int z = 0; z < KW.Length; z++)
                            {
                                if (temp == KW[z])
                                {
                                    Console.WriteLine("(" + KW[z] + "," + "," + line + ")");

                                    string listOBJ = ("(" + KW[z] + "," + "," + line + ")");

                                    CP.Add(KW[z]);
                                    VP.Add(null);
                                    LINE.Add(line);

                                    Tokens.Add(listOBJ);

                                    temp = null;
                                    goto yo;
                                }
                            }
                            //checking for identifier without the _
                            if (reg.IsMatch(temp))
                            {
                                Console.WriteLine("(ID," + temp + "," + line + ")");

                                string listOBJ = ("(ID," + temp + "," + line + ")");

                                CP.Add("ID");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);
                                temp = null;
                            }
                            //checking for the identifier with the _
                            else if (reg1.IsMatch(temp))
                            {
                                Console.WriteLine("(ID," + temp + "," + line + ")");

                                string listOBJ = ("(ID," + temp + "," + line + ")");

                                CP.Add("ID");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);
                                temp = null;
                            }
                            //checking for the integer constant
                            else if (reg2.IsMatch(temp))
                            {
                                Console.WriteLine("(const," + temp + "," + line + ")");

                                string listOBJ = ("(const," + temp + "," + line + ")");

                                CP.Add("const");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                            //checking for the float constant
                            else if (reg_5.IsMatch(temp))
                            {
                                Console.WriteLine("(const," + temp + "," + line + ")");

                                string listOBJ = ("(const," + temp + "," + line + ")");

                                CP.Add("const");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                            else
                            {
                                Console.WriteLine("(invalid lexeme," + temp + "," + line + ")");

                                string listOBJ = ("(invalid lexeme," + temp + "," + line + ")");

                                CP.Add("invalid lexeme");
                                VP.Add(temp);
                                LINE.Add(line);

                                Tokens.Add(listOBJ);

                                temp = null;
                            }
                        yo:
                            break;
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(@"C:\Users\Anthony\Desktop\mytextfolder\result.txt", Tokens);
            Console.ReadKey();
            Syntax s = new Syntax(CP, VP,LINE);
        }

        static public string myfunction(string temp, int line)
        {
            string[] KW = {"return","while","class","for","this","void","int","float","Main","if","else","char","bool","break","continue"
                           ,"array","string","base", "sealed", "virtual", "override", "inheritance", "public", "private"
                           ,"protected", "new","static","object"};

            //RE for the identifier without _
            Regex reg = new Regex("^[a-zA-Z]{1,20}[a-zA-Z0-9]{0,20}$");


            //RE for the identifier with _
            Regex reg1 = new Regex("^[_a-zA-Z]{1,20}[a-zA-Z0-9]{1,20}$");


            //RE for the integer constant 
            Regex reg2 = new Regex("^[+-]{0,1}[0-9]{1,20}$");


            //RE for the float constant 
            //Regex reg3 = new Regex("provide the RE for the float constant");
            Regex reg_4 = new Regex("^[+-]{0,1}[0-9]{0,20}(.)[0-9]{1,20}$");

            string listOBJ = null;

            //CHECK FOR KEYWORDS

            for (int z = 0; z < KW.Length; z++)
            {
                if (temp == KW[z])
                {
                    Console.WriteLine("(" + KW[z] + "," + "," + line + ")");

                    listOBJ = ("(" + KW[z] + "," + "," + line + ")");

                    CP.Add(KW[z]);
                    VP.Add(null);
                    LINE.Add(line);

                    temp = null;
                    goto yo;
                }
            }

            if (reg.IsMatch(temp))
            {
                Console.WriteLine("(ID," + temp + "," + line + ")");
                listOBJ = ("(ID," + temp + "," + line + ")");

                CP.Add("ID");
                VP.Add(temp);
                LINE.Add(line);

                temp = null;
            }
            //checking for the identifier with the _
            else if (reg1.IsMatch(temp))
            {
                Console.WriteLine("(ID," + temp + "," + line + ")");
                listOBJ = ("(ID," + temp + "," + line + ")");

                CP.Add("ID");
                VP.Add(temp);
                LINE.Add(line);

                temp = null;
            }
            //checking for the integer constant
            else if (reg2.IsMatch(temp))
            {
                Console.WriteLine("(const," + temp + "," + line + ")");
                listOBJ = ("(const," + temp + "," + line + ")");

                CP.Add("const");
                VP.Add(temp);
                LINE.Add(line);

                temp = null;
            }
            else if (reg_4.IsMatch(temp))
            {
                Console.WriteLine("(const," + temp + "," + line + ")");
                listOBJ = ("(const," + temp + "," + line + ")");

                CP.Add("const");
                VP.Add(temp);
                LINE.Add(line);

                temp = null;
            }
            else if (reg.IsMatch(temp[0].ToString()) || reg1.IsMatch(temp[0].ToString()))
            {
                int inde = 0;
                string idcoll = null;
                while (inde < temp.Length)
                {
                    if (reg.IsMatch(temp[inde].ToString())|| reg1.IsMatch(temp[inde].ToString()))
                    {
                        idcoll += temp[inde];
                        if(inde+1>=temp.Length)
                        {
                            Console.WriteLine("(ID," + idcoll + "," + line + ")");
                            listOBJ = ("(ID," + idcoll + "," + line + ")");

                            CP.Add("ID");
                            VP.Add(idcoll);
                            LINE.Add(line);

                            idcoll = null;
                        }
                    }
                    else if(temp[inde]=='.')
                    {
                        Console.WriteLine("(ID," + idcoll + "," + line + ")");
                        listOBJ = ("(ID," + idcoll + "," + line + ")");

                        CP.Add("ID");
                        VP.Add(idcoll);
                        LINE.Add(line);

                        idcoll = null;

                        Console.WriteLine("(.," + "   ," + line + ")");
                        listOBJ = ("(.," + "   ," + line + ")");

                        CP.Add(".");
                        VP.Add(null);
                        LINE.Add(line);
                    }
                    else
                    {
                        break;
                    }
                    inde++;
                }
            }
            else if (temp[0] == '.' && reg.IsMatch(temp[1].ToString()) || reg1.IsMatch(temp[1].ToString()))
            {
                Console.WriteLine("(.," + temp[0] + "," + line + ")");

                string listOBJL20D = ("(.," + temp[0] + "," + line + ")");

                CP.Add(".");
                VP.Add(temp[0].ToString());
                LINE.Add(line);

                string temp1 = null;
                temp1 = temp;
                temp = null;
                for (int r = 1; r < temp1.Length; r++)
                {
                    temp += temp1[r];
                }

                if (reg.IsMatch(temp.ToString()) || reg1.IsMatch(temp.ToString()))
                {
                    Console.WriteLine("(ID," + temp + "," + line + ")");

                    string listOBJ1 = ("(ID," + temp + "," + line + ")");

                    CP.Add("ID");
                    VP.Add(temp);
                    LINE.Add(line);

                    Tokens.Add(listOBJ);

                    temp = null;

                }
            }
        yo:
            return listOBJ;
        }
    }
}