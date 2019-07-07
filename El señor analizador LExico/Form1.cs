using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public int contador;
        public bool BoolVariable;
        public bool BoolNumero;
        public Form1()
        {
            InitializeComponent();
        }
        public void analizar()
        {
            contador = 0;
            string entrada = cadenaEntrada.Text;
            switch(QEstados(entrada))
            {
                case -1: { Tokens.Text = "\n***********Cadena Invalida************\n ";
                           Estados.Text += "Se movio a un estado invalido ";
                        Componentes.Text += "** No se reconoce el componente ****";                       
                         } break;
                case 1:
                    {
                        Tokens.Text = "\n***********Cadena Invalida************\n ";
                        Estados.Text += " 1 a 6 Error";
                        Componentes.Text += "**Error***";
                    }
                    break;
                case 2:
                    {
                        Tokens.Text = "\n***********Cadena Valida************\n ";
                        Estados.Text += " -q6-";
                    }
                    break;
                case 3:
                    {
                        Tokens.Text = "\n***********Cadena Invalida************\n ";
                        Estados.Text += " 3 a 6 Error";
                        Componentes.Text += "**Error***";
                    }
                    break;
                case 4:
                    {
                        Tokens.Text = "\n***********Cadena Valida************\n ";
                        Estados.Text += " -q6-";
                        Componentes.Text += " Real****";
                    }
                    break;
                case 5:
                    {
                        Tokens.Text = "\n***********Cadena Valida************\n ";
                        Estados.Text += " -q6-";
                        Componentes.Text += " Variable****";
                    }
                    break;
                case 8:
                    {
                        Tokens.Text = "\n***********Cadena Valida************\n ";
                        Estados.Text += " -q6-";
                        Componentes.Text += " Variable****";
                    }
                    break;
                case 9:
                    {
                        Tokens.Text = "\n***********Cadena Valida************\n ";
                        Estados.Text += " -q6-";
                        Componentes.Text += " Expresion****";
                    }
                    break;
            }
                
        }

        public int QEstados(string entrada)
        {
            Regex MasMenos = new Regex(@"^(\+|\-)$");  //   + o  -
            //Regex Digito = new Regex(@"^[0-9]+$");   //   D
            Regex PuntoDecimal = new Regex(@"^[0-9]*\.[0-9]+$");  //punto decimal
            Regex Letras = new Regex(@"^([a-z]|[A-Z])+$");  //letra
            //Regex GuionLetras = new Regex(@"^(_|([a-z]|[A-Z]))*([a-z]|[A-Z])+(_|([a-z]|[A-Z]))*$");
            //Regex OpLogicosArit = new Regex(@"^(\*|\/|\\|\-|\+|\=|\=\=|\&\&|\|\||\!\=|\<|\>|\<\=|\>\=)$");  //Operadores logicos y aritmeticos 
            Regex Final = new Regex(@"^.*;$");          //;
            Regex Dummi = new Regex(@"^;.+$");
            //bool aux = false;

            if ((Final.IsMatch(entrada) && lastChar(entrada)) && !(entrada.Length == 1) && !Dummi.IsMatch(entrada))
            {
                

                if (MasMenos.IsMatch(entrada[contador].ToString()))
                {
                    Estados.Text += "q1, ";
                    contador++;
                    if (lastChar(entrada) && (entrada[contador] == ';'))
                    {
                        return -1;
                    }

                    if (digitoEvalua(entrada,"q2, "))
                    {
                        Componentes.Text +=  ": con signo";
                        return 2;
                    }

                    if((entrada[contador] == '.') && lastChar(entrada))
                    {
                        Componentes.Text += entrada[contador].ToString();
                        contador++;
                        Estados.Text += "q3, ";
                        if(entrada[contador] == ';')
                        {
                            return -1;
                        }
                        if (digitoEvalua(entrada, "q5, "))
                        {
                            Componentes.Text += " : con signo "; //Checar los mensajes

                            return 4;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                

                }
                if (digitoPuntoEvalua(entrada,"q2, ","q3, ", "q5, "))
                {
                    if((entrada[contador] == ';'))
                    {
                        return 2;
                    }
                    else
                    {
                        return -1;
                    }
                    
                }


                if(Letras.IsMatch(entrada[contador].ToString()) && lastChar(entrada)) 
                {
                    if (variables(entrada,"q9, ","q4, ", "q10, ", "q17, "))
                    {
                        return 5;
                    }
                    
                    if(entrada[contador] == '=')                //Evalua el igual para la asignacion
                    {
                        Estados.Text += "q11, ";
                        Componentes.Text += entrada[contador].ToString() + " <: Asignacion: ";
                        contador++;
                        if (Expresiones(entrada))
                        {
                            if(entrada[contador] == ';')
                                return 9;
                            else
                                return -1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    
                    else
                    {
                        return -1;
                    }

                }
                if((entrada[contador] == '_') && lastChar(entrada))
                {
                    if (!GuionEvalua(entrada, "q7, "))
                    {
                        if (variables(entrada, "q9, ", "q4, ", "q10, ", "q7, "))
                        {
                            return 5;
                        }
                        if (entrada[contador] == '=')                    //Evalua el igual para la asignacion
                        {
                            Estados.Text += "q11, ";
                            Componentes.Text += entrada[contador].ToString() + " <: Asignacion: ";
                            contador++;
                            if (Expresiones(entrada))
                            {
                                if (entrada[contador] == ';')
                                    return 9;
                                else
                                    return -1;
                            }
                            else
                            {
                                return -1;
                            }
                        }

                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }

                else
                {
                    return -1;
                }
            }
            Tokens.Text += "\n Falta de  \";\" ";
            return -1;
        }
        /*
        public bool Expresion(string entrada)  //Calcula la expresion 
        {
            bool operador = false;
            while (lastChar(entrada) && !operador)  //Trabajando
            {
                operador = false;
                if (variablesAndNumeros(entrada, "q16, ", "q17, ", "q18, ", "q13, ", "q15, "))
                {
                    operador = true;
                }
               //QUitado Operador
            }
            return true;
        }
        */
        public bool lastChar(string entrada)
        {
            if(contador < entrada.Length)
            {
                return true;
            }
            return false;
        }
        /// /// ////////////////////////////////////////     Inicio     Evaluacion de   Digitos sin punto ////////////////////////////////////////////////
        public bool digitoEvalua(string entrada,string Q)
        {
            Regex Digito = new Regex(@"^([0-9])+$");   //   D
            
            if (lastChar(entrada))
            {
                BoolNumero = false;
                while (Digito.IsMatch(entrada[contador].ToString()))
                {
                    Componentes.Text += entrada[contador].ToString();
                    Estados.Text += Q;
                    contador++;
                    BoolNumero = true;
                }

                if (lastChar(entrada) && ((entrada[contador] == ';') || BoolNumero))
                {
                    return true;
                }
            }
            return false;
        }

        /// /// /// ////////////////////////////////////////     Inicio     Evaluacion de   Guion ////////////////////////////////////////////////

        public bool GuionEvalua(string entrada,string Q)
        {
            
            Regex guion = new Regex(@"^_+$");   //   D
            
            if (lastChar(entrada))
            {
                while (guion.IsMatch(entrada[contador].ToString()))
                {
                    Componentes.Text += entrada[contador].ToString();
                    Estados.Text += Q;
                    contador++;

                    
                }
                if (lastChar(entrada) && (entrada[contador] == ';'))
                {
                    return true ;
                }
                          
            }
            return false;
        }
        /// /// ////////////////////////////////////////     Fin     Evaluacion de   Guion ////////////////////////////////////////////////

        /// ////////////////////////////////////////     Inicio     Evaluacion de Digitos y puntos ////////////////////////////////////////////////
        public bool digitoPuntoEvalua(string entrada, string Q,string Q2, string Q3)
        {
            if (lastChar(entrada))
            {
                if(digitoEvalua(entrada, Q))
                {
                    if (entrada[contador] == '.')
                    {
                        Componentes.Text += entrada[contador].ToString();
                        Estados.Text += Q2;
                        contador++;
                        if (entrada[contador] == ';')
                        {
                            return false;
                        }
                        if (digitoEvalua(entrada, Q3))
                        {
                            BoolNumero = true;
                            Componentes.Text += " <: Real: ";
                            return true;

                        }

                    }

                    BoolNumero = true;
                    Componentes.Text += " <: Entero: ";
                    return true;
                }
                
            }
            return false;
        }
        /// ////////////////////////////////////////      Fin    Evaluacion de Digitos y puntos ////////////////////////////////////////////////
        /// 
        /// ////////////////////////////////////////      Inicio Letras ////////////////////////////////////////////////
        public bool letrasEvalua(string entrada, string Q)
        {
            Regex Letras = new Regex(@"^([a-z]|[A-Z])+$");  //letra


            while ((contador < entrada.Length) && (Letras.IsMatch(entrada[contador].ToString())))
            {
                Componentes.Text += entrada[contador].ToString();
                Estados.Text += Q;
                contador++;
            }
            if (lastChar(entrada) && (entrada[contador] == ';'))
            {
                return true;
            }
            else
                return false;
        }

        /// ////////////////////////////////////////      Inicio Variables     ////////////////////////////////////////////////
        public bool variables(string entrada, string QDigito, string QLetra, string QGuion, string GuionFuera)
        {
            bool aux = true;
            Regex Letras = new Regex(@"^([a-z]|[A-Z])+$");  //letra
            Regex Digito = new Regex(@"^([0-9])+$");   //   D
            Regex guion = new Regex(@"^_+$");   //   Guion
            Regex OpLogicosArit = new Regex(@"^(\*|\/|\^|\-|\+|<|>)$");  //Operadores logicos y aritmeticos 
            Regex OpLogicosArit2 = new Regex(@"^(\=\=|\&\&|\|\||\!\=|\<\=|\>\=)$$");  //Operadores logicos y aritmeticos 
            if (lastChar(entrada) && (Letras.IsMatch(entrada[contador].ToString()) || (guion.IsMatch(entrada[contador].ToString()))))
            {
                while (lastChar(entrada) && guion.IsMatch(entrada[contador].ToString()))
                {
                    Componentes.Text += entrada[contador].ToString();
                    Estados.Text += GuionFuera;
                    contador++;
                }
                if (lastChar(entrada) && Letras.IsMatch(entrada[contador].ToString()))
                {
                    while (aux)
                    {

                        //Revisar de mejor manera el como evaluar que entro y valido alguna validacion    _1a1a

                        if (lastChar(entrada) && letrasEvalua(entrada, QLetra))
                        {
                            BoolVariable = true;
                            Componentes.Text += " <: Variable: ";
                            return true;
                        }
                        if (lastChar(entrada) && digitoEvalua(entrada, QDigito))
                        {
                            BoolVariable = true;
                            Componentes.Text += " <: Variable: ";
                            return true;
                        }
                        if (lastChar(entrada) && GuionEvalua(entrada, QGuion))
                        {
                            BoolVariable = true;
                            Componentes.Text += " <: Variable: ";
                            return true;
                        }
                        if (lastChar(entrada) && (entrada[contador] == ';'))
                        {
                            BoolVariable = true;
                            Componentes.Text += " <: Variable: ";
                            return true;
                        }
                        if (lastChar(entrada) && (entrada[contador] == '='))
                        {
                            aux = false;
                        }
                        if (lastChar(entrada) && !(Letras.IsMatch(entrada[contador].ToString())) && !(Digito.IsMatch(entrada[contador].ToString())) && !(guion.IsMatch(entrada[contador].ToString())))
                        {
                            aux = false;
                        }

                    }
                    BoolVariable = true;
                    Componentes.Text += " <: Variable: ";
                }
                

            }
            return false;
        }
        /*
        public bool variablesAndNumeros(string entrada, string QDigito, string QDigito2, string QDigito3, string QLetra, string QGuion)
       */

        public bool Expresiones(string entrada)
        {
            bool aux = false;
            if (lastChar(entrada) && !operadorEvalua(entrada, "q11, "))
            {
                while (!operadorEvalua(entrada, "q11, ")  && !aux)
                {
                    BoolNumero = false;
                    BoolVariable = false;

                    if (lastChar(entrada) && variables(entrada, "q8, ", "q13, ", "q14, ", "q15, "))  //Arreglar el problema de los estados con los guiones
                    {
                        if (lastChar(entrada) && (entrada[contador] == ';'))
                            return true;
                    }
                    if (lastChar(entrada) && digitoPuntoEvalua(entrada, "q16, ", "q17, ", "q18, "))
                    {
                        if (lastChar(entrada) && (entrada[contador] == ';'))
                            return true;
                    }
                    //Se supone que lo anterior ya sirve checar como evaluar la doble repeticion del operador y algun modo de erro en caso de 

                    if (BoolNumero || BoolVariable)
                    {
                        if (lastChar(entrada) && operadorEvalua(entrada, "q11, "))
                        {
                            aux = false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }


        /// ////////////////////////////////////////      Inicio Operador     ////////////////////////////////////////////////
        public bool operadorEvalua(string entrada,string Qoperador)
        {
            Regex OpLogicosArit = new Regex(@"^(\*|\/|\^|\-|\+|<|>)$");  //Operadores logicos y aritmeticos 
            Regex OpLogicosArit2 = new Regex(@"^(\=\=|\&\&|\|\||\!\=|\<\=|\>\=)$$");  //Operadores logicos y aritmeticos 
            string doble = "";
            if (contador <entrada.Length -2)
            {
                doble = entrada[contador].ToString() + entrada[contador + 1].ToString() ;

                if (lastChar(entrada) && OpLogicosArit2.IsMatch(doble))
                {

                    Estados.Text += Qoperador;
                    Componentes.Text += doble + " <:Operador Logico: ";
                    contador += 2;
                    return true;
                }
            }
            if (lastChar(entrada) && OpLogicosArit.IsMatch(entrada[contador].ToString())){
                
                Estados.Text += Qoperador;
                Componentes.Text += entrada[contador].ToString() + " <:Operador Aritmetico: ";
                contador++; 
                return true;
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
            cadenaEntrada.Text += "";
            Estados.Text += "Estado invalido ";
        }
        private void CadenaEntrada_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            Tokens.Text = "";
            Componentes.Text = "";
            Estados.Text= "";
            cadenaEntrada.Text = "";
            cadenaEntrada.Focus();
        }
    }
}
