using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;


namespace Compi_practica2
{
    class ejecucion
    {

        string va = " ";
        string tipo;
        string nada = "";

        bool declaracion;
        string decla = " ";
        bool global;

        public static List<Nodo_principal> ambito = new List<Nodo_principal>();
        public static List<Nodo_principal> listaVar = new List<Nodo_principal>();

        public string EjecucionMain(ParseTreeNode raiz)
        {

            string Inicio = raiz.ToString();

            switch (Inicio)
            {
                case "VARIOS":
                    {
                        Console.WriteLine(Inicio);
                        for (int i = 0; i < raiz.ChildNodes.Count; i++)
                        {
                            //cuerpo = raiz.ChildNodes[i];
                            string nodo = EjecucionMain(raiz.ChildNodes[i]);


                        }

                        return "";
                    }
                case "SENTENCIAS":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {

                            string retorna = raiz.ChildNodes[0].ToString();
                            string expre = EjecucionMain(raiz.ChildNodes[1]);

                            return retorna + expre;

                        }
                        else
                        {
                            string sentencia = EjecucionMain(raiz.ChildNodes[0]);
                            return sentencia;
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
                                string tipo_asignacion = EjecucionMain(raiz.ChildNodes[1]);
                                for (int i = 0; i < listaVar.Count; i++)
                                {
                                    if (raiz.ChildNodes[0].ToString().Replace(" (id)", "") == listaVar[i].nombre)
                                    {
                                        tp = listaVar[i].tipo;
                                        exi = true;
                                    }
                                }

                                this.tipo = tp;
                                string valor_asignado = EjecucionMain(raiz.ChildNodes[2]);


                                if (exi == true)
                                {
                                    asignacionvalor(raiz.ChildNodes[0].ToString().Replace(" (id)", ""), tipo_asignacion, valor_asignado);
                                }
                                else
                                {
                                    Analizador.listaError.Add("ERROR: variable NO EXISTE");
                                }


                                declaracion = false;
                                //METODO PARA VERIFICAR VALOR ASIGNADO
                                Console.WriteLine(raiz.ChildNodes[0].ToString() + tipo_asignacion + valor_asignado);

                            }
                            else
                            {
                                declaracion = true;
                                string tipo = EjecucionMain(raiz.ChildNodes[0]);
                                this.tipo = tipo.Replace(" (Keyword)", "");
                                string muchasVariables = EjecucionMain(raiz.ChildNodes[2]);
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
                            string tipo = EjecucionMain(raiz.ChildNodes[0]);
                            this.tipo = tipo.Replace(" (Keyword)", "");
                            string expre = EjecucionMain(raiz.ChildNodes[2]);
                            string muchasVariables = EjecucionMain(raiz.ChildNodes[3]);
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
                        string valores = EjecucionMain(raiz.ChildNodes[1]);

                        return raiz.ChildNodes[0].ToString() + valores;
                    }
                case "IMPRIMIR":
                    {
                        declaracion = true;
                        string expre = EjecucionMain(raiz.ChildNodes[1]);
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
                        Analizador.impresion.Add(cade);
                        declaracion = false;
                        return raiz.ChildNodes[0].ToString() + expre;
                    }
                case "TIPO":
                    {

                        string tipo_var = raiz.ChildNodes[0].ToString().Replace(" (Key symbol)", "");
                        return tipo_var;
                    }
                case "ASIGNACIONES":
                    {
                        if (raiz.ChildNodes.Count == 0)
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
                            string opera1 = EjecucionMain(raiz.ChildNodes[0]);
                            string opera2 = EjecucionMain(raiz.ChildNodes[2]);
                            string signo = raiz.ChildNodes[1].ToString().Replace(" (Key symbol)", "");
                            if (declaracion == true && (this.tipo == "int" || this.tipo == "float"))
                            {
                                string resultado = Convert.ToString(opera(opera1, opera2, signo));

                                return resultado;
                            }/*else if ((this.tipo == "int" || this.tipo == "float")&&decla=="char")
                            {
                                Analizador.listaError.Add("ERROR: variable " + opera1 +"y"+opera2 + " NO ES COMPATIBLE");
                                return opera1 + signo + opera2;
                            }*/
                            else
                            {
                                return opera1 + signo + opera2;
                            }

                        }
                        else if (raiz.ChildNodes.Count == 2)
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
                                        string nodo = EjecucionMain(raiz.ChildNodes[0]);
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
                                            }
                                            else if (decla == "int")
                                            {
                                                return val;
                                            }
                                            else if (decla == "char*")
                                            {
                                                return val;
                                            }
                                            else
                                            {
                                                Analizador.listaError.Add("ERROR: variable " + val + " NO ES COMPATIBLE");
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
                                        string nodo = EjecucionMain(raiz.ChildNodes[0]);
                                        return nodo;
                                    }
                                case "INCREMENTA":
                                    {
                                        string nodo = EjecucionMain(raiz.ChildNodes[0]);
                                        return nodo;
                                    }
                                case "DECREMENTA":
                                    {
                                        string nodo = EjecucionMain(raiz.ChildNodes[0]);
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

                            string mu = EjecucionMain(raiz.ChildNodes[0]);
                            string muchasVar = EjecucionMain(raiz.ChildNodes[1]);
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
                                string muchasVar = EjecucionMain(raiz.ChildNodes[0]);
                                return muchasVar;
                            }
                        }
                        return nada;

                    }
                case "DS":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            string expre = EjecucionMain(raiz.ChildNodes[1]);
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
                case "IF":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[1]);
                        string varios = EjecucionMain(raiz.ChildNodes[2]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios;

                    }
                case "IFE":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[1]);
                        string varios = EjecucionMain(raiz.ChildNodes[2]);
                        string varios2 = EjecucionMain(raiz.ChildNodes[4]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios + raiz.ChildNodes[3].ToString() + "\n" + varios2;

                    }
                case "WHILE":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[1]);
                        string varios = EjecucionMain(raiz.ChildNodes[2]);
                        return raiz.ChildNodes[0].ToString() + expre + "\n" + varios;

                    }
                case "DOWHILE":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[3]);
                        string varios = EjecucionMain(raiz.ChildNodes[1]);
                        return raiz.ChildNodes[0].ToString() + varios + raiz.ChildNodes[2].ToString() + expre;

                    }
                case "INCREMENTA":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[0]);
                        string resul = Convert.ToString(Convert.ToDouble(expre) + 1);
                        return resul;

                    }
                case "DECREMENTA":
                    {
                        string expre = EjecucionMain(raiz.ChildNodes[0]);
                        string resul = Convert.ToString(Convert.ToDouble(expre) - 1);
                        return resul;

                    }
                case "VALORES":
                    {
                        if (raiz.ChildNodes.Count != 1)
                        {
                            string valores = EjecucionMain(raiz.ChildNodes[0]);
                            string expre = EjecucionMain(raiz.ChildNodes[1]);
                            return valores + expre;
                        }
                        else
                        {
                            string expre = EjecucionMain(raiz.ChildNodes[0]);
                            return expre;
                        }

                        return nada;

                    }

            }



            return null;
        }
        public void asignacionvalor(string a, string b, string c)
        {

            for (int i = 0; i < listaVar.Count; i++)
            {
                if (listaVar[i].nombre == a)
                {
                    listaVar[i].valor = c;
                }
            }

        }

        public void agregarVariable(Nodo_principal nodo)
        {
            bool add = false;

            for (int i = 0; i < listaVar.Count; i++)
            {
                if (listaVar[i].nombre == nodo.nombre)
                {
                    add = true;
                }
            }

            if (add == false)
            {


                if (nodo.tipo == "int")
                {

                    if (nodo.valor == "SIN")
                    {
                        listaVar.Add(nodo);
                    }
                    else
                    {
                        bool t = IsNumeric(nodo.valor);
                        if (t == true)
                        {
                            double a = Convert.ToDouble(nodo.valor);
                            string b = Convert.ToString(Math.Truncate(a));
                            Nodo_principal nodo1 = new Nodo_principal(b, nodo.nombre, nodo.tipo);
                            listaVar.Add(nodo1);
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
                    listaVar.Add(nodo1);
                    decla = " ";


                }
                else if (nodo.tipo == "bool")
                {
                    if (nodo.valor == "true" || nodo.valor == "false" || nodo.valor == "SIN")
                    {
                        listaVar.Add(nodo);
                        decla = " ";
                    }

                }
                else if (nodo.tipo == "float")
                {

                    if (nodo.valor == "SIN")
                    {
                        listaVar.Add(nodo);
                    }
                    else
                    {
                        bool t = IsNumeric(nodo.valor);
                        if (t == true)
                        {
                            double a = Convert.ToDouble(nodo.valor);
                            string b = Convert.ToString(a);
                            Nodo_principal nodo1 = new Nodo_principal(b, nodo.nombre, nodo.tipo);
                            listaVar.Add(nodo1);
                            decla = " ";
                        }
                    }

                }

            }
            else
            {
                Analizador.listaError.Add("ERROR: NOMBRE DE VARIABLE YA USADO.");
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
            string numero = "";
            if (listaVar.Count != 0)
            {
                for (i = 0; i < listaVar.Count; i++)
                {
                    if (listaVar[i].nombre == id)
                    {
                        decla = listaVar[i].tipo;
                        return numero = listaVar[i].valor;

                    }
                }

                if (i == listaVar.Count)
                {
                    Analizador.listaError.Add("ERROR: variable " + id + " no creada.");
                    return "0";
                }
            }
            else
            {
                Analizador.listaError.Add("ERROR: variable " + id + " no creada.");
                return "0";
            }
            return "0";
        }
        public double opera(string ope1, string ope2, string sig)
        {
            bool ban1 = IsNumeric(ope1);
            bool ban2 = IsNumeric(ope2);
            double operando1 = 0;
            double operando2 = 0;

            if (ban1 == true)
            {
                operando1 = Convert.ToDouble(ope1);

            }
            else
            {
                int i = 0;

                if (listaVar.Count != 0)
                {
                    for (i = 0; i < listaVar.Count; i++)
                    {
                        if (listaVar[i].nombre == ope1)
                        {

                            operando1 = Convert.ToDouble(listaVar[i].valor);
                            break;
                        }
                    }

                    if (i == listaVar.Count)
                    {
                        Analizador.listaError.Add("ERROR: variable " + ope1 + " no creada.");
                    }
                }
                else
                {
                    Analizador.listaError.Add("ERROR: variable " + ope1 + " no creada.");
                }

            }

            if (ban2 == true)
            {
                operando2 = Convert.ToDouble(ope2);
            }
            else
            {
                int i = 0;
                if (listaVar.Count != 0)
                {
                    for (i = 0; i < listaVar.Count; i++)
                    {
                        if (listaVar[i].nombre == ope2)
                        {
                            operando2 = Convert.ToDouble(listaVar[i].valor);
                            break;
                        }
                    }
                    if (i == listaVar.Count)
                    {
                        Analizador.listaError.Add("ERROR: variable " + ope2 + " no creada.");
                    }
                }
                else
                {
                    Analizador.listaError.Add("ERROR: variable " + ope2 + " no creada.");
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
    }
}


