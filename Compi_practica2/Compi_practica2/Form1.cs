using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;

using System.IO;
using System.Diagnostics;
using Irony.Ast;
using WINGRAPHVIZLib;

namespace Compi_practica2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            abrir();
        }

        private void analizarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Analizador.listaVariables.Clear();
            Analizador.listMetodos.Clear();
            Analizador.listaError.Clear();
            Analizador.impresion.Clear();
            Analizador ej2 = new Analizador();
            ejecucion ejecuta = new ejecucion();
            if (ej2.esCadenaValida(txtEntrada.Text, new Gramatica()))
            {
                MessageBox.Show("CADENA VALIDA");
                if (Analizador.padre.Root != null)
                {
                    // Graficas.ConstruirArbol(Analizador.padre.Root);
                    //txtSalida.Text = ej2.hacerOperaciones(Analizador.padre.Root).ToString();
                    // Graficas.GraficarArbol("AST.txt", "c:/Precedencia");
                     ej2.Recolectar_variables(Analizador.padre.Root).ToString();
                    string cadenita="";
                    string cadenita2 = "";
                    string impre = " ";
                    int x = 0;
                    bool existeMain = false;
                    
                    for (int i = 0; i < Analizador.listMetodos.Count; i++)
                    {
                        cadenita += Analizador.listMetodos[i].tipo.ToString()+" "+Analizador.listMetodos[i].nombre.ToString()+" "+Analizador.listMetodos[i].parame.Count.ToString()+"\r\n";
                        if (Analizador.listMetodos[i].nombre == "main")
                        {
                            x = i;
                            existeMain = true;
                        }
                      
                    }
                    for (int i = 0; i < Analizador.listaVariables.Count; i++)
                    {
                        cadenita += Analizador.listaVariables[i].tipo.ToString() + " " + Analizador.listaVariables[i].nombre.ToString() + " " + Analizador.listaVariables[i].valor.ToString()+ "\r\n";

                    }
                    for (int i = 0; i < Analizador.listaError.Count; i++)
                    {
                        cadenita2 += Analizador.listaError[i] + "\r\n";

                    }
                    for (int i = 0; i < Analizador.impresion.Count; i++)
                    {
                        impre += Analizador.impresion[i]+ "\r\n";
                    }




                    txtSalida.Text = cadenita;
                    txtSalida.Text += impre;
                    if (existeMain == true)
                    {
                        ejecuta.EjecucionMain(Analizador.listMetodos[x].arbolito);
                    }
                    

                    txtErrores.Text = cadenita2;
                    
                }
            }
            else
            {
                MessageBox.Show("CADENA INVALIDA");
                

            }
        }
        public static void generar_errores(ParseTree arbol)
        {
           
            foreach (ParserMessage p in arbol.ParserMessages )
            {
                if (p.Message.Contains("Invalid"))
                {

                }
                else
                {

                }
            }
            
        }

        private void abrir()
        {
            //cuadro abrir
            OpenFileDialog archivo = new OpenFileDialog();
            //inicia variable para leer el archivo
            System.IO.StreamReader leeArchivo = null;
            //Filtrar archivo
            archivo.Filter = "TXT (.txt)|*.txt";
            //visualizar ventana de archivos
            archivo.ShowDialog(this);

            try
            {
                //este codigo se utiliza para que se pueda pueda mostrar la informacion del archivo que queremos abrir 
                archivo.OpenFile();
                leeArchivo = System.IO.File.OpenText(archivo.FileName);
                txtEntrada.Text = leeArchivo.ReadToEnd();


            }
            catch (Exception)
            {

            }
        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
