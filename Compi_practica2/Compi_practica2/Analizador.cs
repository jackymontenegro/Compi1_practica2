using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Irony.Ast;
using WINGRAPHVIZLib;

namespace Compi_practica2
{

    class Analizador
    {
        string va = " ";
        string tipo;

        bool declaracion;
        string decla = " ";
        bool global;

        ParseTreeNode cuerpo;
        ParseTreeNode sentencia;

        public static List<Nodo_principal> listMetodos = new List<Nodo_principal>();
        public static List<Nodo_principal> listaVariables = new List<Nodo_principal>();
        public static List<Nodo_principal> listParametros;
        public static List<string> impresion = new List<string>();
        public static List<string> listaError = new List<string>();

        private static int contador;
        private static String grafo;
        public static ParseTree padre;
        public bool esCadenaValida(string cadenaEntrada, Grammar gramatica)
        {

            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);
            padre = arbol;

            if (arbol.Root == null)
            {
                MessageBox.Show("cagadales con parser");
               
            }
            else
            {
                //   Console.WriteLine(5 + 5);
                generarImagen(arbol.Root);

            }

            return arbol.Root != null;
        }

        public string Recolectar_variables(ParseTreeNode raiz)
        {

            string nada = "";
            string Inicio = raiz.ToString();


            switch (Inicio)
            {
                case "S":
                    {
                        Console.WriteLine(Inicio);
                        for (int i = 0; i < raiz.ChildNodes.Count; i++)
                        {
                            //cuerpo = raiz.ChildNodes[i];
                            string nodo = Recolectar_variables(raiz.ChildNodes[i]);


                        }

                        return nada;
                    }
                case "INICIO":
                    {
                        global = true;
                        string nodo = Recolectar_variables(raiz.ChildNodes[0]);
                        return nodo;

                    }
                case "METODO":
                    {
                        global = false;
                        if (raiz.ChildNodes.Count != 4)
                        {
                            listParametros = new List<Nodo_principal>();
                            string titulo = raiz.ChildNodes[1].ToString();
                            string varios = Recolectar_variables(raiz.ChildNodes[2]);
                            cuerpo = raiz.ChildNodes[2];
                            //  Console.WriteLine(cuerpo.Term.Name);
                            //   Console.WriteLine("METODO " + titulo+"\n"+varios);
                            Nodo_principal metodo = new Nodo_principal(titulo.Replace(" (Keyword)", ""), "int", cuerpo, listParametros);
                            listMetodos.Add(metodo);


                        }
                        else
                        {
                            listParametros = new List<Nodo_principal>();
                            string tip = Recolectar_variables(raiz.ChildNodes[0]).Replace(" (Keyword)", "");
                            string titulo = raiz.ChildNodes[1].ToString();
                            string parametros = Recolectar_variables(raiz.ChildNodes[2]);
                            string varios = Recolectar_variables(raiz.ChildNodes[3]);
                            cuerpo = raiz.ChildNodes[3];
                            //   Console.WriteLine(cuerpo.Term.Name);
                            //  Console.WriteLine("METODO " + titulo+parametros+"\n"+varios);
                            Nodo_principal metodo = new Nodo_principal(titulo.Replace(" (id)", ""), tip, cuerpo, listParametros);
                            listMetodos.Add(metodo);

                        }
                        return nada;
                    }
                case "DECLARACIONES":
                    {

                        string tp = "";
                        bool exi = false;
                        if (raiz.ChildNodes.Count == 3)
                        {

                            string compara = raiz.ChildNodes[0].Term.Name;
                            if (compara == "id")
                            {
                                declaracion = true;
                                string tipo_asignacion = Recolectar_variables(raiz.ChildNodes[1]);
                                for(int i = 0; i < listaVariables.Count; i++)
                                {
                                    if (raiz.ChildNodes[0].ToString().Replace(" (id)", "") == listaVariables[i].nombre)
                                    {
                                        tp = listaVariables[i].tipo;
                                        exi = true;
                                    }
                                }

                                this.tipo = tp;
                                string valor_asignado = Recolectar_variables(raiz.ChildNodes[2]);

                               
                                if(exi == true)
                                {
                                    asignacionvalor(raiz.ChildNodes[0].ToString().Replace(" (id)", ""), tipo_asignacion, valor_asignado);
                                }
                                else
                                {
                                    listaError.Add("ERROR: variable NO EXISTE");
                                }
                                

                                declaracion = false;
                                //METODO PARA VERIFICAR VALOR ASIGNADO
                                Console.WriteLine(raiz.ChildNodes[0].ToString() + tipo_asignacion + valor_asignado);

                            }
                            else
                            {
                                declaracion = true;
                                string tipo = Recolectar_variables(raiz.ChildNodes[0]);
                                this.tipo = tipo.Replace(" (Keyword)", "");
                                string muchasVariables = Recolectar_variables(raiz.ChildNodes[2]);
                                Nodo_principal nodo = new Nodo_principal("SIN", raiz.ChildNodes[1].ToString().Replace(" (id)", ""), tipo.Replace(" (Keyword)", ""));
                                if (global == true)
                                {
                                    // listaVariables.Add(nodo);
                                    agregarVariable(nodo);
                                }

                                declaracion = false;
                                //   Console.WriteLine(tipo + raiz.ChildNodes[1].ToString() +muchasVariables);
                                this.tipo = " ";
                                //AQUI DEBE DE IR UN METODO QUE VERIFIQUE QUE SE INGRESO CORRECTAMENTE EL VALOR DE LA VARIABLE
                            }
                            return nada;


                        }
                        else
                        {
                            declaracion = true;
                            string tipo = Recolectar_variables(raiz.ChildNodes[0]);
                            this.tipo = tipo.Replace(" (Keyword)", "");
                            string expre = Recolectar_variables(raiz.ChildNodes[2]);
                            string muchasVariables = Recolectar_variables(raiz.ChildNodes[3]);
                            Nodo_principal nodo = new Nodo_principal(expre, raiz.ChildNodes[1].ToString().Replace(" (id)", ""), tipo.Replace(" (Keyword)", ""));
                            if (global == true)
                            {
                                // listaVariables.Add(nodo);
                                agregarVariable(nodo);
                            }
                            declaracion = false;
                            Console.WriteLine(tipo + raiz.ChildNodes[1].ToString() + "=" + expre + muchasVariables);
                            this.tipo = " ";
                        }
                        return nada;
                    }
                case "LLAMADA":
                    {
                        string valores = Recolectar_variables(raiz.ChildNodes[1]);

                        return raiz.ChildNodes[0].ToString() + valores;
                    }
                case "IMPRIMIR":
                    {
                        declaracion = true;
                        string expre = Recolectar_variables(raiz.ChildNodes[1]);
                        //  Console.WriteLine(raiz.ChildNodes[0].ToString() + expre);

                        string cade = " ";
                        string[] separators = { "+" };
                        string value = expre;
                        string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in words)
                        {
                            cade += word.Replace("SIN" +
                                "", "");

                        }
                        impresion.Add(cade);
                        declaracion = false;
                        return raiz.ChildNodes[0].ToString() + expre;
                    }
                case "TIPO":
                    {

                        string tipo_var = raiz.ChildNodes[0].ToString().Replace(" (Key symbol)", "");
                        return tipo_var;
                    }
                case "VARIOS":
                    {

                        for (int i = 0; i < raiz.ChildNodes.Count; i++)
                        {
                            sentencia = raiz.ChildNodes[i];
                            string nodo = Recolectar_variables(raiz.ChildNodes[i]);
                            va += nodo;

                        }
                        return va;
                    }
                case "PARAMETROS":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {

                            string parametros = Recolectar_variables(raiz.ChildNodes[0]);
                            string para = Recolectar_variables(raiz.ChildNodes[1]);

                            return parametros + para;

                        }
                        else if (raiz.ChildNodes.Count == 1)
                        {
                            string para = Recolectar_variables(raiz.ChildNodes[0]);
                            return para;
                        }

                        return nada;
                    }
                case "ASIGNACIONES":
                    {
                        if(raiz.ChildNodes.Count == 0)
                        {
                            return "=";
                        }
                        else
                        {
                            string tipo_asignacion = raiz.ChildNodes[0].ToString().Replace(" (Key symbol)", "");
                            return tipo_asignacion;
                        }
                       
                    }
                case "EXPRE":
                    {
                        if (raiz.ChildNodes.Count == 3)
                        {
                            string opera1 = Recolectar_variables(raiz.ChildNodes[0]);
                            string opera2 = Recolectar_variables(raiz.ChildNodes[2]);
                            string signo = raiz.ChildNodes[1].ToString().Replace(" (Key symbol)", "");
                            if (declaracion == true && (this.tipo == "int" || this.tipo == "float"))
                            {
                                string resultado = Convert.ToString(opera(opera1, opera2, signo));

                                return resultado;
                            }/*else if ((this.tipo == "int" || this.tipo == "float")&&decla=="char")
                            {
                                listaError.Add("ERROR: variable " + opera1 +"y"+opera2 + " NO ES COMPATIBLE");
                                return opera1 + signo + opera2;
                            }*/
                            else
                            {
                                return opera1 + signo + opera2;
                            }

                        } else if (raiz.ChildNodes.Count == 2)
                        {
                            string signo = raiz.ChildNodes[0].ToString().Replace(" (Key symbol)", "");
                            string num = raiz.ChildNodes[1].ToString().Replace(" (num)", "");
                            return signo + num;
                        }
                        else if (raiz.ChildNodes.Count == 1)
                        {
                            //string cosa = raiz.ChildNodes[0].Token.Text;
                            string cosa2 = raiz.ChildNodes[0].Term.Name;
                            //  Console.WriteLine(cosa+cosa2);
                            switch (cosa2)
                            {
                                case "EXPRE":
                                    {
                                        string nodo = Recolectar_variables(raiz.ChildNodes[0]);
                                        return nodo;
                                    }

                                case "num":
                                    {

                                        return raiz.ChildNodes[0].ToString().Replace(" (num)", "");
                                    }
                                case "id":
                                    {
                                        if (declaracion == true)

                                        {

                                            string val = traeValorID(raiz.ChildNodes[0].ToString().Replace(" (id)", ""));

                                            if (decla == "float")
                                            {
                                                return val;
                                            } else if (decla == "int")
                                            {
                                                return val;
                                            }
                                            else if (decla == "char*")
                                            {
                                                return val;
                                            } else
                                            {
                                                listaError.Add("ERROR: variable " + val + " NO ES COMPATIBLE");
                                                return val;
                                                //  return raiz.ChildNodes[0].ToString().Replace(" (id)", "");
                                            }

                                        }


                                        else

                                        {
                                            return raiz.ChildNodes[0].ToString().Replace(" (id)", "");
                                        }
                                    }
                                case "true":
                                    {
                                        return raiz.ChildNodes[0].ToString().Replace(" (Keyword)", "");
                                    }
                                case "false":
                                    {
                                        return raiz.ChildNodes[0].ToString().Replace(" (Keyword)", "");
                                    }
                                case "tstring":
                                    {
                                        return raiz.ChildNodes[0].ToString().Replace(" (tstring)", "");
                                    }
                                case "LLAMADA":
                                    {
                                        string nodo = Recolectar_variables(raiz.ChildNodes[0]);
                                        return nodo;
                                    }
                                case "INCREMENTA":
                                    {
                                        string nodo = Recolectar_variables(raiz.ChildNodes[0]);
                                        return nodo;
                                    }
                                case "DECREMENTA":
                                    {
                                        string nodo = Recolectar_variables(raiz.ChildNodes[0]);
                                        return nodo;
                                    }
                            }
                        }
                        return nada;
                    }
                case "DECLARA":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {

                            string mu = Recolectar_variables(raiz.ChildNodes[0]);
                            string muchasVar = Recolectar_variables(raiz.ChildNodes[1]);
                            return mu + muchasVar;



                        }
                        else
                        {
                            if (raiz.ChildNodes.Count == 0)
                            {
                                return "";
                            }
                            else
                            {
                                string muchasVar = Recolectar_variables(raiz.ChildNodes[0]);
                                return muchasVar;
                            }
                        }
                        return nada;

                    }
                case "DS":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            string expre = Recolectar_variables(raiz.ChildNodes[1]);
                            Nodo_principal nodo = new Nodo_principal(expre, raiz.ChildNodes[0].ToString().Replace(" (id)", ""), tipo.Replace(" (Keyword)", ""));
                            if (global == true)
                            {
                                // listaVariables.Add(nodo);
                                agregarVariable(nodo);
                            }
                            return raiz.ChildNodes[0].ToString() + expre;
                        }
                        else
                        {
                            Nodo_principal nodo = new Nodo_principal("SIN", raiz.ChildNodes[0].ToString().Replace(" (id)", ""), tipo.Replace(" (Keyword)", ""));
                            if (global == true)
                            {
                                // listaVariables.Add(nodo);
                                agregarVariable(nodo);
                            }
                            return raiz.ChildNodes[0].ToString();
                        }
                    }
                case "SENTENCIAS":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {

                            string retorna = raiz.ChildNodes[0].ToString();
                            string expre = Recolectar_variables(raiz.ChildNodes[1]);

                            return retorna + expre;

                        } else
                        {
                            string sentencia = Recolectar_variables(raiz.ChildNodes[0]);
                            return sentencia;
                        }
                        return nada;

                    }
                case "PARA":
                    {
                        string tipo = Recolectar_variables(raiz.ChildNodes[0]);
                        Nodo_principal parametro = new Nodo_principal("SIN", raiz.ChildNodes[1].ToString().Replace(" (id)", ""), tipo.Replace(" (Keyword)", ""));
                        bool es = false;
                        for (int i = 0; i < listParametros.Count; i++)
                        {
                            if (listParametros[i].nombre == parametro.nombre)
                            {
                                es = true;
                            }
                        }

                        if (es == false)
                        {
                            listParametros.Add(parametro);
                            return tipo + raiz.ChildNodes[1].ToString();
                        }
                        else
                        {
                            listaError.Add("ERROR: variable ya existe como parametro");
                            return tipo + raiz.ChildNodes[1].ToString();
                        }


                    }
                case "IF":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[1]);
                        string varios = Recolectar_variables(raiz.ChildNodes[2]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios;

                    }
                case "IFE":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[1]);
                        string varios = Recolectar_variables(raiz.ChildNodes[2]);
                        string varios2 = Recolectar_variables(raiz.ChildNodes[4]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios + raiz.ChildNodes[3].ToString() + "\n" + varios2;

                    }
                case "WHILE":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[1]);
                        string varios = Recolectar_variables(raiz.ChildNodes[2]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios;

                    }
                case "DOWHILE":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[3]);
                        string varios = Recolectar_variables(raiz.ChildNodes[1]);
                        return raiz.ChildNodes[0].ToString() + varios + raiz.ChildNodes[2].ToString() + expre;

                    }
                case "INCREMENTA":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[0]);
                        string resul = Convert.ToString(Convert.ToDouble(expre) + 1);
                        return resul;

                    }
                case "DECREMENTA":
                    {
                        string expre = Recolectar_variables(raiz.ChildNodes[0]);
                        string resul = Convert.ToString(Convert.ToDouble(expre) - 1);
                        return resul;

                    }
                case "VALORES":
                    {
                        if (raiz.ChildNodes.Count != 1)
                        {
                            string valores = Recolectar_variables(raiz.ChildNodes[0]);
                            string expre = Recolectar_variables(raiz.ChildNodes[1]);
                            return valores + expre;
                        }
                        else
                        {
                            string expre = Recolectar_variables(raiz.ChildNodes[0]);
                            return expre;
                        }

                        return nada;

                    }

            }


            return null;
        }

        public void asignacionvalor(string a, string b,string c)
        {

            for(int i = 0; i < listaVariables.Count; i++)
            {
                if(listaVariables[i].nombre == a)
                {
                    listaVariables[i].valor = c;
                }
            }

        }

        public void agregarVariable(Nodo_principal nodo)
        {
            bool add = false;

           for (int i = 0; i < listaVariables.Count; i++)
            {
                if(listaVariables[i].nombre == nodo.nombre)
                {
                    add = true;
                }
            }

            if (add == false)
            {


                if (nodo.tipo == "int")
                {

                    if(nodo.valor == "SIN")
                    {
                        listaVariables.Add(nodo);
                    }
                    else
                    {
                        bool t = IsNumeric(nodo.valor);
                        if (t == true)
                        {
                            double a = Convert.ToDouble(nodo.valor);
                            string b = Convert.ToString(Math.Truncate(a));
                            Nodo_principal nodo1 = new Nodo_principal(b, nodo.nombre, nodo.tipo);
                            listaVariables.Add(nodo1);
                            decla = " ";
                        }
                    }

                    
                }
                else if (nodo.tipo == "char*")
                {
                    string cade = " ";
                    string[] separators = { "+" };
                    string value = nodo.valor;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        cade += word;

                    }
                    // Console.WriteLine(cade);
                    Nodo_principal nodo1 = new Nodo_principal(cade, nodo.nombre, nodo.tipo);
                    listaVariables.Add(nodo1);
                    decla = " ";


                }
                else if (nodo.tipo == "bool")
                {
                    if (nodo.valor == "true" || nodo.valor == "false"|| nodo.valor == "SIN")
                    {
                        listaVariables.Add(nodo);
                        decla = " ";
                    }

                }
                else if (nodo.tipo == "float")
                {

                    if (nodo.valor == "SIN")
                    {
                        listaVariables.Add(nodo);
                    }
                    else
                    {
                        bool t = IsNumeric(nodo.valor);
                        if (t == true)
                        {
                            double a = Convert.ToDouble(nodo.valor);
                            string b = Convert.ToString(a);
                            Nodo_principal nodo1 = new Nodo_principal(b, nodo.nombre, nodo.tipo);
                            listaVariables.Add(nodo1);
                            decla = " ";
                        }
                    }
                   
                }

            }
            else
            {
                listaError.Add("ERROR: NOMBRE DE VARIABLE YA USADO.");
            }
        }

        public bool IsNumeric(object Expression)

        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;

        }

       

        public string traeValorID(string id)
        {
            int i = 0;
            string numero= "";
            if (listaVariables.Count != 0)
            {
                for (i = 0; i < listaVariables.Count; i++)
                {
                    if (listaVariables[i].nombre == id)
                    {
                        decla = listaVariables[i].tipo;
                       return numero = listaVariables[i].valor;
                       
                    }
                }

                if (i == listaVariables.Count)
                {
                    listaError.Add("ERROR: variable " + id + " no creada.");
                    return "0";
                }
            }
            else
            {
                listaError.Add("ERROR: variable " + id + " no creada.");
                return "0";
            }
            return "0";
        }
        public double opera(string ope1, string ope2, string sig)
        {
            bool ban1 = IsNumeric(ope1);
            bool ban2 = IsNumeric(ope2);
            double operando1=0;
            double operando2=0;

            if (ban1 == true)
            {
                operando1 = Convert.ToDouble(ope1);

            }
            else
            {
                int i = 0;

                if(listaVariables.Count != 0)
                {
                    for (i = 0; i < listaVariables.Count; i++)
                    {
                        if (listaVariables[i].nombre == ope1)
                        {

                            operando1 = Convert.ToDouble(listaVariables[i].valor);
                            break;
                        }
                    }

                    if (i == listaVariables.Count)
                    {
                            listaError.Add("ERROR: variable " + ope1 + " no creada.");
                    }
                }
                else
                {
                    listaError.Add("ERROR: variable " + ope1 + " no creada.");
                }
     
            }

            if(ban2 == true)
            {
                operando2 = Convert.ToDouble(ope2);
            }
            else
            {
                int i=0;
                if (listaVariables.Count != 0)
                {
                    for (i = 0; i < listaVariables.Count; i++)
                    {
                        if (listaVariables[i].nombre == ope2)
                        {
                            operando2 = Convert.ToDouble(listaVariables[i].valor);
                            break;
                        }
                    }
                    if (i == listaVariables.Count)
                    {
                        listaError.Add("ERROR: variable " + ope2 + " no creada.");
                    }
                }
                else
                {
                    listaError.Add("ERROR: variable " + ope2 + " no creada.");
                }
            }

            
            
            switch (sig)
            {
                case "+":
                    {
                        return operando1 + operando2;
                    }
                case "-":
                    {
                        return operando1 - operando2;
                    }
                case "*":
                    {
                        return operando1 * operando2;
                    }
                case "/":
                    {
                        return operando1 / operando2;
                    }
                case "%":
                    {
                        return operando1 % operando2;
                    }
            }
            return 0.0;
        }



        public double hacerOperaciones(ParseTreeNode raiz)
        {
            string Inicio = raiz.ToString();
            
            switch (Inicio)
            {
                case "S":
                    {
                        double resultado = hacerOperaciones(raiz.ChildNodes[0]);
                        return resultado;
                    }

                case "EXPRE":
                    {

                        /* if(raiz.ChildNodes.Count == 0)
                         {
                             double resultado = hacerOperaciones(hijos[0]);
                             return resultado;
                         }
                         else */
                        if (raiz.ChildNodes.Count == 3)
                        {
                            double operando1 = hacerOperaciones(raiz.ChildNodes[0]);
                            double operando2 = hacerOperaciones(raiz.ChildNodes[2]);
                            string operador = raiz.ChildNodes[1].ToString().Replace(" (Key symbol)", "");

                            switch (operador)
                            {
                                case "+":
                                    {
                                        return operando1 + operando2;
                                    }
                                case "-":
                                    {
                                        return operando1 - operando2;
                                    }
                                case "*":
                                    {
                                        return operando1 * operando2;
                                    }
                                case "/":
                                    {
                                        return operando1 / operando2;
                                    }
                                case "%":
                                    {
                                        return operando1 % operando2;
                                    }
                            }

                            return 0.0;
                        }
                        else
                        {

                            if (raiz.ChildNodes.Count == 2)
                            {
                                string val = "-" + raiz.ChildNodes[1].ToString().Replace(" (num)", "");

                                return Convert.ToDouble(val);

                            }
                            else
                            {

                                if (raiz.ChildNodes[0].ToString() == "EXPRE")
                                {
                                    double resultado = hacerOperaciones(raiz.ChildNodes[0]);
                                    return resultado;
                                }
                                else
                                {
                                    String val = raiz.ChildNodes[0].ToString().Replace(" (num)", ""); ;

                                    return Convert.ToDouble(val);
                                }


                            }
                        }
                    }

            }
            return 0.0;

        }
        public static void generarImagen(ParseTreeNode raiz)
        {
            String grafoDOT = getDOT(raiz);
            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            // MessageBox.Show(grafoDOT);  
            //img.Save("C:\\Users\\ROBIN SALVATIERRA\\Dropbox\\OLC2\\OLC2_P1\\Proyecto1_200915428\\Proyecto1_200915428\\entradas\\robast.png");
            img.Save("robast.png");

        }


        //graficar arbol
        public static String getDOT(ParseTreeNode raiz)
        {

            grafo = "digraph G{"+ "graph[bgcolor = black];"+ "edge[color = white];" ;
             
            grafo += "nodo0[style=filled,label=\"" + escapar(raiz.ToString()) + "\",color = white,shape=egg];\n";
            contador = 1;
            recorrerAST("nodo0", raiz);
            grafo += "}";
            return grafo;
        }



        private static void recorrerAST(String padre, ParseTreeNode hijos)
        {

            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {

                String nombrehijo = "nodo" + contador.ToString();
                grafo += nombrehijo + "[style=filled,label=\"" + escapar(hijo.ToString()) + "\",color = white,shape=egg];\n";
                grafo += padre + "->" + nombrehijo + ";\n";
                contador++;
                recorrerAST(nombrehijo, hijo);
            }
        }

        private static String escapar(String cadena)
        {

            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");
            return cadena;
        }

    }
}
/*
 *         public double hacerOperaciones(ParseTreeNode raiz)
        {
            string Inicio = raiz.ToString();
            ParseTreeNode[] hijos = null;
            if (raiz.ChildNodes.Count > 0)
            {
                hijos = raiz.ChildNodes.ToArray();
            }
            switch (Inicio)
            {
                case "S":
                    {
                        double resultado = hacerOperaciones(hijos[0]);
                        return resultado;
                    }

                case "EXPRE":
                    {


                        if (raiz.ChildNodes.Count == 3)
                        {
                            double operando1 = hacerOperaciones(hijos[0]);
double operando2 = hacerOperaciones(hijos[2]);
string operador = hijos[1].ToString().Replace(" (Key symbol)", "");

                            switch (operador)
                            {
                                case "+":
                                    {
                                        return operando1 + operando2;
                                    }
                                case "-":
                                    {
                                        return operando1 - operando2;
                                    }
                                case "*":
                                    {
                                        return operando1* operando2;
                                    }
                                case "/":
                                    {
                                        return operando1 / operando2;
                                    }
                                case "%":
                                    {
                                        return operando1 % operando2;
                                    }
                            }

                            return 0.0;
                        }
                        else
                        {

                            if (raiz.ChildNodes.Count == 2)
                            {
                                string val = "-" + hijos[1].ToString().Replace(" (num)", "");

                                return Convert.ToDouble(val);

                            }
                            else
                            {

                                if (hijos[0].ToString() == "EXPRE")
                                {
                                    double resultado = hacerOperaciones(hijos[0]);
                                    return resultado;
                                }
                                else
                                {
                                    String val = hijos[0].ToString().Replace(" (num)", ""); ;

                                    return Convert.ToDouble(val);
                                }


                            }
                        }
                    }

            }
            return 0.0;

        }
  */