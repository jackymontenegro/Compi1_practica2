using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;
using System.IO;
using System.Diagnostics;

namespace Compi_practica2
{
    class Nodo_principal
    {
        public String valor = " ";
        public String tipo = " ";
        public string nombre = "";
        public ParseTreeNode arbolito;
        public List<Nodo_principal> parame;

        //PARA LOS PARAMETROS LISTA DE LISTAS

        public Nodo_principal( string val,string nom, string tip)
        {

            this.valor = val;
            this.nombre = nom;
            this.tipo = tip;

        }

        public Nodo_principal(string nom, string tip, ParseTreeNode arbol,List<Nodo_principal> parametros)
        {
            this.nombre = nom;
            this.tipo = tip;
            this.arbolito = arbol;
            this.parame = parametros;

        }
        public Nodo_principal(string nom,List<Nodo_principal> variables )
        {
            this.nombre = nom;
            this.parame = variables;
        }

       // public static List<List<Nodo_principal>> lista;
    }

}
