using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void analizar()
        {
            string entrada = cadenaEntrada.Text;
            bool digito = false, letra = false, guion = false;

            if(entrada[entrada.Length -1] == ';')
            {

                if (entrada[0] == '-')
                {
                    digito = true;
                }
                if (entrada[0] == '+')
                {
                    digito = true;
                }
                if (char.IsDigit(entrada[0]))
                    digito = true;

                if (digito == true){

                    bool numeroNegativo = false, numeroExiste = false, punto = false, numeroError = false;

                    if (entrada[0] == '-')
                    {
                        Estados.Text += "\n1, ";
                        numeroNegativo = true;
                    }
                    else
                    {
                        Estados.Text += "\n1, ";
                    }

                    for (int contador = 1; contador < entrada.Length - 1; contador++) {
                        if ((entrada[contador] == '.') == punto){
                            numeroError = true;
                        }

                        if ((entrada[contador] == '-') == numeroNegativo)
                            numeroError = true;

                        if ((punto && (numeroExiste == false)) || (numeroNegativo && (numeroExiste == false)))
                            numeroError = true;

                        if (char.IsNumber(entrada[contador]) == !punto)
                        {
                            Estados.Text += "2, ";
                            numeroExiste = true;
                        }
                        else if (char.IsNumber(entrada[contador]) == punto)
                        {
                            Estados.Text += "4, ";
                        }

                        if ((entrada[contador] == '.') == numeroExiste)
                        {
                            punto = true;
                            Estados.Text += "4, ";
                        }

                    }

                    if (numeroError)
                    {
                        Tokens.Text += " \nExpresion Valida\n";

                        if (punto && numeroNegativo)
                            Componentes.Text += "\n" + entrada.ToString() + ": Numero Real, Negativo\n";
                        else if (punto && !numeroNegativo)
                            Componentes.Text += "\n" + entrada.ToString() + ": Numero Real, Positivo\n";
                        if (!punto && numeroNegativo)
                            Componentes.Text += "\n" + entrada.ToString() + ": Numero Entero, Negativo\n";
                        else if (!punto && !numeroNegativo)
                        {
                            Componentes.Text += "\n" + entrada.ToString() + ": Numero Entero, Positivo\n";
                        }

                        Estados.Text += " 3";
                    }
                    else if (numeroError)
                    {
                        Tokens.Text += " \n Expresion Invalida\n";
                    }
                }

                /* Letras                      */


                if ((entrada[0] == '_') || char.IsLetter(entrada[0]))
                {
                    bool expresionAritmetica=false;
                    int inicio = 0;
                    if (entrada[0] == '_')
                    {
                        Estados.Text += "15, ";
                        Componentes.Text += entrada[0].ToString() + " <- Guion bajo    ";
                        inicio = 1;
                    }
                    guion = true;
                    letra = false;
                    for (int contador = inicio; contador < entrada.Length - 1; contador++)
                    {
                        char[] opr = new char[] { '*', '/', '+', '-', '^' };
                        bool operadorBool = false;
                        for (int i = 0; i < 5; i++)
                        {
                            if (entrada[contador] == opr[i])
                            {
                                operadorBool = true;
                                Estados.Text += "11, ";
                                Componentes.Text += opr[i].ToString() + "   <- Operador   ";
                            }
                            else if(char.IsLetter(entrada[contador]))
                            {
                                while ((char.IsLetter(entrada[contador])) || (char.IsDigit(entrada[contador])))
                                {
                                    if (char.IsLetter(entrada[contador]))
                                    {
                                        while (char.IsLetter(entrada[contador]))
                                        {
                                            Estados.Text += "6, ";
                                            Componentes.Text += entrada[contador];
                                            contador++;
                                        }
                                    }
                                    if (char.IsDigit(entrada[contador]))
                                    {
                                        while (char.IsDigit(entrada[contador]))
                                        {
                                            Estados.Text += "14, ";
                                            Componentes.Text += entrada[contador];
                                            contador++;
                                        }
                                    }
                                }
                                Componentes.Text += "  <-Variable  ";

                            }
                            /*/                                                                 */
                            if (char.IsDigit(entrada[contador]))
                            {
                                bool NotEntero = false;
                                while (char.IsDigit(entrada[contador]))
                                {
                                    Estados.Text += "14, ";
                                    Componentes.Text += entrada[contador];
                                    contador++;

                                    if(entrada[contador] == '.')
                                    {
                                        NotEntero = true;
                                        Estados.Text += "10, ";
                                        Componentes.Text += entrada[contador];
                                        contador++;
                                        while (char.IsDigit(entrada[contador]))
                                        {
                                            Estados.Text += "14, ";
                                            Componentes.Text += entrada[contador];
                                            contador++;
                                        }
                                        Componentes.Text += "  <-Real   ";
                                    }
                                }
                                if (!NotEntero)
                                {
                                    Componentes.Text += "  <-Entero    ";
                                }
                                
                            }
                        }

                    }
                }
                else
                {
                    Tokens.Text += entrada + " <- Variables y operaciones necesitan empezar con lentra o _ ;  ";
                }
            }
            else
            {
                Tokens.Text += entrada + " <- Falta ;  ";
            }
        }

        public bool operadores(string cadena,int posicion)
        {
            char[] opr = new char[] { '*', '/', '+', '-', '^' };
            bool operadorBool = false;
            for(int contador = posicion; contador < cadena.Length-2;contador++)
            {
                for(int i = 0; i < 5; i++)
                {
                    if (cadena[contador] == opr[i])
                        operadorBool = true;
                }
                if (operadorBool)
                {
                    if (cadena[contador] == cadena[contador + 1])
                        return true;
                }
                operadorBool = false;
            }
            return false;
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            analizar();
            Tokens.Text += " \n";
            Componentes.Text += "\n ";
            cadenaEntrada.Text += "\n ";
            Estados.Text += " \n";
        }
        public void error(string _cadena)
        {
            Tokens.Text += "Expresion no valida";
            Componentes.Text += _cadena + "  -> entrada invalida  ";
            cadenaEntrada.Text += " ";
            Estados.Text += "Estado invalido ";
        }
        private void CadenaEntrada_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Tokens.Text = " ";
            Componentes.Text = " ";
            cadenaEntrada.Text = " ";
            Estados.Text = " ";
        }
    }
}
