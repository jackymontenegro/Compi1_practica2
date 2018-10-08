using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Compi_practica2
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {
            //---------------------> Comentarios
            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/*", "*/");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);

            //---------------------> Definir Palabras Reservadas
            MarkReservedWords("main");
            MarkReservedWords("if");
            MarkReservedWords("else");
            MarkReservedWords("print");


            MarkReservedWords("do");
            MarkReservedWords("while");
            MarkReservedWords("imprimir");
            MarkReservedWords("return");


            MarkReservedWords("true");
            MarkReservedWords("false");
            MarkReservedWords("int");
            MarkReservedWords("float");

            MarkReservedWords("char*");
            MarkReservedWords("bool");

            //---------------------> (Opcional)Definir variables para palabras reservadas
            var principal = ToTerm("main");
            var si = ToTerm("if");
            var sino = ToTerm("else");
            var imprime = ToTerm("print");


            var hacer = ToTerm("do");
            var mientras = ToTerm("while");
            var imprimir = ToTerm("imprimir");
            var retornar = ToTerm("return");


            var verdadero = ToTerm("true");
            var falso = ToTerm("false");
            var entero = ToTerm("int");
            var flo = ToTerm("float");

            var caracter = ToTerm("char*");
            var booleano = ToTerm("bool");

            //---------------------> (Opcional)Definir variables para signos y mas
            var asignar = ToTerm("=");
            var pyc = ToTerm(";");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var alla = ToTerm("{");
            var clla = ToTerm("}");

            //------------------------> Operadores
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var multiplicacion = ToTerm("*");
            var division = ToTerm("/");
            var porcentaje = ToTerm("%");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var menorigual = ToTerm("<=");
            var mayorigual = ToTerm(">=");
            var comparacion = ToTerm("==");
            var or = ToTerm("||");
            var and = ToTerm("&&");
            var suma = ToTerm("+=");
            var resta = ToTerm("-=");
            var diferente = ToTerm("!=");
            var disminuir = ToTerm("--");
            var aumentar = ToTerm("++");
            var coma = ToTerm(",");

            //---------------------> No Terminales
            var INICIO = new NonTerminal("INICIO");
            var METODO = new NonTerminal("METODO");
            var SENTENCIAS = new NonTerminal("SENTENCIAS");
            var DECLARA = new NonTerminal("DECLARA");
            var DS = new NonTerminal("DS");
            var PARAMETROS = new NonTerminal("PARAMETROS");
            var PARA = new NonTerminal("PARA");
            var DECLARACIONES = new NonTerminal("DECLARACIONES");
            var IF = new NonTerminal("IF");
            var IFE = new NonTerminal("IFE");
            var VARIOS = new NonTerminal("VARIOS");
            var LLAMADA = new NonTerminal("LLAMADA");
            var VALORES = new NonTerminal("VALORES");
            var IMPRIMIR = new NonTerminal("IMPRIMIR");
            var WHILE = new NonTerminal("WHILE");
            var DOWHILE = new NonTerminal("DOWHILE");
            var TIPO = new NonTerminal("TIPO");
            var S = new NonTerminal("S");
            var EXPRE = new NonTerminal("EXPRE");
            var INCREMENTA = new NonTerminal("INCREMENTA");
            var DECREMENTA = new NonTerminal("DECREMENTA");
            var ASIGNACIONES = new NonTerminal("ASIGNACIONES");



            //---------------------> Terminales
            NumberLiteral num = new NumberLiteral("num");
            IdentifierTerminal id = TerminalFactory.CreateCSharpIdentifier("id");
            var tstring = new StringLiteral("tstring", "\"", StringOptions.AllowsDoubledQuote);
            var tchar = new StringLiteral("tchar", "'", StringOptions.AllowsDoubledQuote);



            //DECLARACION DE TERMINALES POR MEDIO DE ER.

            //  RegexBasedTerminal num = new RegexBasedTerminal("num", "([0-9]+|[)");


            //----------------------------------------------PRODUCCIONES-----------------------------------------------------------

            S.Rule = MakeStarRule(S, INICIO);

            INICIO.Rule = METODO
                              | IMPRIMIR
                              | DECLARACIONES + pyc
                              | LLAMADA + pyc;

            /* --------------------------------------------------------------------------------------- *
             *                                   CREACION DE METODOS                                   *
             * --------------------------------------------------------------------------------------- */

            METODO.Rule = entero + principal + apar + cpar + alla + VARIOS + clla
                               | TIPO + id + apar + PARAMETROS + cpar + alla + VARIOS + clla;

            /* --------------------------------------------------------------------------------------- *
             *                                DECLARACION DE VARIABLES                                 *
             * --------------------------------------------------------------------------------------- */

            TIPO.Rule = entero | caracter | booleano | flo;

            DECLARACIONES.Rule = TIPO + id + DECLARA
                                | TIPO + id + asignar + EXPRE + DECLARA
                                | id + ASIGNACIONES + EXPRE;

            ASIGNACIONES.Rule = suma
                                | asignar
                                | resta;


            DECLARA.Rule = DECLARA + coma + DS
                                | DS
                                | Empty;

            DS.Rule = id
                                | id + asignar + EXPRE;

            PARAMETROS.Rule = PARAMETROS + coma + PARA
                                | PARA
                                | Empty;

            PARA.Rule = TIPO + id;

            /* --------------------------------------------------------------------------------------- *
             *                                      SENTENCIAS                                         *
             * --------------------------------------------------------------------------------------- */

            IMPRIMIR.Rule = imprime + apar + EXPRE + cpar + pyc;

            VARIOS.Rule = MakeStarRule(VARIOS, SENTENCIAS);

            SENTENCIAS.Rule = DECLARACIONES + pyc
                                | retornar + EXPRE + pyc
                                | IF
                                | IFE
                                | WHILE
                                | DOWHILE
                                | IMPRIMIR
                                | INCREMENTA + pyc
                                | DECREMENTA + pyc
                                | LLAMADA + pyc;

            LLAMADA.Rule = id + apar + VALORES + cpar;

            VALORES.Rule = VALORES + coma + EXPRE
                                | EXPRE;

            IF.Rule = si + apar + EXPRE + cpar + alla + VARIOS + clla;

            IFE.Rule = si + apar + EXPRE + cpar + alla + VARIOS + clla + sino + alla + VARIOS + clla;

            WHILE.Rule = mientras + apar + EXPRE + cpar + alla + VARIOS + clla;

            DOWHILE.Rule = hacer + alla + VARIOS + clla + mientras + apar + EXPRE + cpar;

            INCREMENTA.Rule = EXPRE + mas + mas;

            DECREMENTA.Rule = EXPRE + menos + menos;

            /* --------------------------------------------------------------------------------------- *
             *                       OPERACIONES ARITMETICAS Y RELACIONALES                            *
             * --------------------------------------------------------------------------------------- */

            EXPRE.Rule = EXPRE + or + EXPRE
                    | EXPRE + and + EXPRE
                    | EXPRE + comparacion + EXPRE
                    | EXPRE + diferente + EXPRE
                    | EXPRE + mayor + EXPRE
                    | EXPRE + mayorigual + EXPRE
                    | EXPRE + menor + EXPRE
                    | EXPRE + menorigual + EXPRE
                    | EXPRE + mas + EXPRE
                    | EXPRE + menos + EXPRE
                    | EXPRE + multiplicacion + EXPRE
                    | EXPRE + division + EXPRE
                    | EXPRE + porcentaje + EXPRE
                    | apar + EXPRE + cpar
                    | num
                    | menos + num
                    | id
                    | verdadero
                    | falso
                    | tstring
                    | LLAMADA
                    | INCREMENTA
                    | DECREMENTA;


            //---------------------> No Terminal Inicial
            this.Root = S;

            //---------------------> Definir Asociatividad
            RegisterOperators(1, Associativity.Left, or);                 //OR
            RegisterOperators(2, Associativity.Left, and);                 //AND
            RegisterOperators(3, Associativity.Left, comparacion, diferente);           //IGUAL, DIFERENTE
            RegisterOperators(4, Associativity.Left, mayor, menor, mayorigual, menorigual); //MAYORQUES, MENORQUES
            RegisterOperators(5, Associativity.Left, mas, menos);           //MAS, MENOS
            RegisterOperators(6, Associativity.Left, multiplicacion, division);        //POR, DIVIDIR
            RegisterOperators(2, Associativity.Left, porcentaje);                 //POTENCIA
            RegisterOperators(7, Associativity.Right, "!");                 //NOT


            //---------------------> Manejo de Errores
            SENTENCIAS.ErrorRule = SyntaxError + SENTENCIAS;
            METODO.ErrorRule = SyntaxError + clla;
            //CASOS.ErrorRule = SyntaxError + CASO;*/

            //---------------------> Eliminacion de caracters, no terminales
            this.MarkPunctuation(apar, cpar, pyc, alla, clla, asignar, coma);
           // this.MarkPunctuation("print", "if", "else", "do", "while");
           // this.MarkTransient(SENTENCIA);
            //this.MarkTransient(CASO);
        }

    }
}

             /* --------------------------------------------------------------------------------------- *
             *                                DECLARACION DE VARIABLES                                 *
             * --------------------------------------------------------------------------------------- */
             /*
//VARIABLES PARA PARAMETROS 
VARIABLES_PARAMETROS.Rule = VARIABLES_PARAMETROS + coma + DECLARAR
                                    | DECLARAR;

            //VARIABLES DECLARADAS DENTRO DE LOS METODOS
            VARIABLES_METODOS.Rule =VARIABLES_METODOS + VARIABLEM
                        |VARIABLEM;
            
            VARIABLEM.Rule = LISTA_DECLARAR + pyc
                        | LISTA_DECLARAR_ASIGNAR + pyc;

            //DECLARACION DE UNA VARIABLE int a
            DECLARAR.Rule = TIPO + id
                            |Empty;

            //DECLARACION DE UNA VARIABLE CON SU ASIGNACION int a = 0
            DECLARAR_ASIGNAR.Rule = MakeStarRule(DECLARAR_ASIGNAR, TIPO_ASIGNACION);

//DECLARACION UNA LISTA DE VARIABLES CON VALOR ASIGNADO A CADA UNA  int a = 0, b = 1
//LISTA_DECLARAR_ASIGNAR.Rule = MakeStarRule(LISTA_DECLARAR_ASIGNAR, LISTA_TIPO_ASIGNACION);
LISTA_DECLARAR_ASIGNAR.Rule =  LISTA_TIPO_ASIGNACION;

            //DECLARACION UNA LISTA DE VARIABLES  int a , b 
            //LISTA_DECLARAR.Rule = MakeStarRule(LISTA_DECLARAR, LISTA_TIPOS);
            LISTA_DECLARAR.Rule = LISTA_TIPOS;

            //DATOS QUE PUEDEN SER ASIGNADOS 5+5 , 5 , "HOLA", TRUE/FALSE
            DATO.Rule = EXPRE
                    | tstring
                    | BOL;


            BOL.Rule = verdadero
                    | falso;

            //TIPO DE DATO
            TIPO.Rule = entero
                    | booleano
                    | flo
                    | caracter;

            //ASIGNACION SEGUN TIPO DE DATO 
            TIPO_ASIGNACION.Rule = entero + id + asignar + EXPRE
                                  | booleano + id + asignar + BOL
                                  | flo + id + asignar + EXPRE
                                  | caracter + id + asignar + tstring;

            //ASIGNACION SEGUN TIPO DE DATO LISTA  
            LISTA_TIPO_ASIGNACION.Rule = booleano + LISTA_BOOLS
                                  | caracter + LISTA_CHARS
                                  | flo + LISTA_FLOATS
                                  | entero + LISTA_INTS;

            //SEPARACION DE COMAS ENTRE VARIABLES
            LISTA_BOOLS.Rule = LISTA_BOOLS + coma + LISTA_BOOL
                                  | LISTA_BOOL;

            LISTA_FLOATS.Rule = LISTA_FLOATS + coma + LISTA_FLOAT
                                  | LISTA_FLOAT;

            LISTA_INTS.Rule = LISTA_INTS + coma + LISTA_INT
                                  | LISTA_INT;

            LISTA_CHARS.Rule = LISTA_CHARS + coma + LISTA_CHAR
                                  | LISTA_CHAR;

            //VARIABLES DECLARADAS CON VALOR ASIGNADO
            LISTA_BOOL.Rule = id + asignar + BOL;

            LISTA_CHAR.Rule = id + asignar + tstring;

            LISTA_FLOAT.Rule = id + asignar + EXPRE;

            LISTA_INT.Rule = id + asignar + EXPRE;

            //VARIABLES DECLARADAS
            LISTA_TIPOS.Rule = TIPO + LISTA_TIPO;

            //SEPARACION DE COMAS ENTRE VARIABLES
            LISTA_TIPO.Rule = LISTA_TIPO + coma + id
                            | id;
*/
