using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PintarPalabras
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        //bueno pues iniciemos
        //hare toda la progra en esta clase, talvez no sea lo mas optimo pero es para darles una idea de la funcionalidad


/*-----------------------METODOS O FUNCIONES-------------------------------*/
        public string TipoReservada(string texto)//para verificar si es una palabra reservada
        {
            string tipo = "";
            switch (texto.ToLower())
            {
                case ("planificador"):
                    tipo = "RESERVADA";
                    break;
                case ("año"):
                    tipo = "RESERVADA";
                    break;

                case ("mes"):
                    tipo = "RESERVADA";
                    break;
                case ("dia"):
                    tipo = "RESERVADA";
                    break;
                case ("descripción"):
                    tipo = "RESERVADA";
                    break;
                case ("imagen"):
                    tipo = "RESERVADA";
                    break;
                default:
                    tipo = "CADENA";
                    break;
            }
            return tipo;
        }
        public void PINTAR(int inicio, int fin, string tipo)//para pintar el texto
        {
            richTextBox1.SelectionStart = inicio;//inicio de seleccion para pintar
            richTextBox1.SelectionLength = fin;//fin de la seleccion para pintar

            //el color que debe utilizar al pintar
            switch (tipo)
            {
                case ("ERROR"):
                    richTextBox1.SelectionColor = Color.Red;
                    break;
                case ("NUMERO"):
                    richTextBox1.SelectionColor = Color.Purple;
                    break;
                case ("CADENA"):
                    richTextBox1.SelectionColor = Color.Orange;
                    break;
                case ("RESERVADA"):
                    richTextBox1.SelectionColor = Color.Blue;
                    break;
                case ("SIMBOLO"):
                    richTextBox1.SelectionColor = Color.Black;
                    break;
                default:break;
            }

        }

/*-----------------------CLASES-------------------------------*/
        //lo mas facil es usar objetos para guardar la informacion de los tokes fila, columna, contenido, tipo, etc....
        class MiToken
        {
            public string TIPO { set; get; }//tipo de token , numero, letra, signo, reservada, id.
            public string TOKEN { set; get; }//el contenido del token
            public int FILA { set; get; }//fila del token
            public int COLUMNA { set; get; }//columna

        }

/*----------------------listas para guardar tokens-----------*/

        ArrayList ListTokens = new ArrayList();//aca guardare los tokens
        ArrayList ListError = new ArrayList();//aca guardare los tokens error






/*----------------------------metodo de analisis lexico-------------*/

        public void ALexico(string Texto_A)
        {
            ListTokens.Clear();
            ListError.Clear();

            //variables
            char[] texto = Texto_A.ToCharArray();//obtener el text analizado
            int estado = 0;
            int fila = 1;
            int contadorpos = 1;
            int columna =1;
            string token = "";


            for (int i = 0; i < texto.Length; i++)//recorrer el texto
            {
                

                switch (estado)
                {
                    case 0:

                        if (char.IsNumber(texto[i]))//si es numero
                        {
                            columna = contadorpos;
                            estado = 1;
                            token = token + texto[i].ToString();
                            //ademas aca verifica si es fin de cadena para guardarlo
                            if (i+1==texto.Length)
                            {
                                ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = "NUMERO", TOKEN = texto[i].ToString() });
                                //cada vez que guardamos algo debemos pintar tambien
                                //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                                PINTAR(i, i + 1, "NUMERO");
                            }
                            contadorpos++;

                        }
                        else if (char.IsLetter(texto[i]))//si es letra
                        {
                            columna = contadorpos;
                            estado = 2; token = token + texto[i].ToString();
                            //ademas aca verifica si es fin de cadena para guardarlo
                            if (i + 1 == texto.Length)
                            {
                                ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = TipoReservada(token), TOKEN = texto[i].ToString() });
                                //cada vez que guardamos algo debemos pintar tambien
                                //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                                PINTAR(i, i + 1, TipoReservada(token));
                                estado = 0;
                            }
                            contadorpos++;
                        }
                        else if (texto[i].ToString().Equals("{")|| texto[i].ToString().Equals("}")|| texto[i].ToString().Equals(":"))//si es simbolo
                        {
                            columna = contadorpos;
                            estado = 3;
                            //en realidad no es necesario ir al estado 3 ya que puedes guardar desde aca el simbolo. :v
                            ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = "SIMBOLO", TOKEN = texto[i].ToString() });
                            //cada vez que guardamos algo debemos pintar tambien
                            //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                            PINTAR(i, i + 1, "SIMBOLO");//+1 debido a que es solo un caracter
                            contadorpos++;
                            estado = 0;

                        }
                        else if (texto[i].ToString().Equals(" "))//aca si hay espacio
                        {
                            //cambiar la columna
                            contadorpos++;
                        }
                        else if (texto[i].ToString().Equals("\n"))//aca un salto de linea
                        {
                            contadorpos = 1;
                            fila++;
                            //reiniciar columna y aumentar fila
                        }
                        else//errores
                        {
                            columna = contadorpos;
                            //guardar el error 
                            ListError.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = "ERROR", TOKEN = texto[i].ToString() });
                            //cada vez que guardamos algo debemos pintar tambien
                            //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                            PINTAR(i, i+1,"ERROR");//+1 debido a que es solo un caracter
                            contadorpos++;
                            estado = 0;
                            
                        }
                        break;









                    case 1://los numeros

                        if (char.IsNumber(texto[i]))//si es numero
                        {
                            estado = 1;
                            token = token + texto[i].ToString();
                            //ademas aca verifica si es fin de cadena para guardarlo
                            if (i + 1 == texto.Length)
                            {
                                ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = "NUMERO", TOKEN = token});
                                //cada vez que guardamos algo debemos pintar tambien
                                //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                                PINTAR((i-token.Length+1),i+1, "NUMERO");
                            }
                            contadorpos++;

                        }
                        
                        else//cualquier otra cosa
                        {
                            //primero guardar lo concatenado hasta ahora
                            ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = "NUMERO", TOKEN = token });
                            //cada vez que guardamos algo debemos pintar tambien
                            //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                            PINTAR((i - token.Length), i + 1, "NUMERO");

                            //reiniciamos token
                            token = "";


                            //regresamos una posicion
                            i--;
                            contadorpos--;
                            estado = 0;
                            
                        }
                        break;

















                    case 2:
                        if (char.IsLetter(texto[i]))//si es letra
                        {
                            estado = 2;
                            token = token + texto[i].ToString();
                            //ademas aca verifica si es fin de cadena para guardarlo
                            if (i + 1 == texto.Length)
                            {
                                ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = TipoReservada(token), TOKEN = token });
                                //cada vez que guardamos algo debemos pintar tambien
                                //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                                PINTAR((i - token.Length + 1), i + 1, TipoReservada(token));
                            }
                            contadorpos++;

                        }

                        else//cualquier otra cosa
                        {
                            //primero guardar lo concatenado hasta ahora
                            ListTokens.Add(new MiToken { COLUMNA = columna, FILA = fila, TIPO = TipoReservada(token), TOKEN = token });
                            //cada vez que guardamos algo debemos pintar tambien
                            //para pintar mandamos la posicion inicial de ¨i¨ y la posicion final, ademas el tipo para saber el color
                            PINTAR((i - token.Length), i + 1, TipoReservada(token));

                            //reiniciamos token
                            token = "";


                            //regresamos una posicion
                            i--;
                            contadorpos--;
                            estado = 0;

                        }
                        break;





                    case 3: break;
                }


            }
            
        }




/*---------------------------------------------------------------------*/
        public void html_Tokens(ArrayList listado)
        {
            string nombredoc = "Tokens.html";
            FileStream fs = new FileStream(nombredoc, FileMode.Create);
            StreamWriter Archivo = new StreamWriter(fs, System.Text.Encoding.Default);
            Archivo.Write("<!DOCTYPE html> \n");
            Archivo.Write("<html> \n");
            Archivo.Write("<head> \n");
            Archivo.Write("<title>Listado de Tokens</title> \n");
            Archivo.Write("<link rel=stylesheet href=\"style.css\"> \n");
            Archivo.Write("</head> \n");
            Archivo.Write("<body> \n");
            Archivo.Write("<center> \n");
            Archivo.Write("<h2>Listado de Tokens<br></h2> \n");
            Archivo.Write("</center> \n");
            Archivo.Write("<table align=center> \n");
            Archivo.Write("<tr> \n");
            Archivo.Write("<td><h3>#</h3></td> \n");//numero
            Archivo.Write("<td><h3>LEXEMA</h3></td> \n");//lexema
            Archivo.Write("<td><h3>FILA</h3></td> \n");//fila
            Archivo.Write("<td><h3>COLUMNA</h3></td> \n");//columna
            Archivo.Write("<td><h3>TIPO</h3></td> \n");//tipo
            Archivo.Write("</tr> \n");
            Archivo.Write("<center> \n");
            for (int i = 0; i < listado.Count; i++)
            {
                Archivo.Write("<tr> \n");
                Archivo.Write("<td><p>" + (i + 1) + " <p></td>\n");//numero
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).TOKEN+ "<p></td>\n");//lexema
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).FILA + "<p></td>\n");//fila
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).COLUMNA + "<p></td>\n");//columna
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).TIPO + "</p></td>\n");//tipo
                Archivo.Write("</tr> \n");
            }

            Archivo.Write("</body> \n");
            Archivo.Write("</html> \n");
            Archivo.Close();
            System.Diagnostics.Process.Start(nombredoc);



        }
        public void html_Errores(ArrayList listado)
        {
            string nombredoc = "Errores.html";
            FileStream fs = new FileStream(nombredoc, FileMode.Create);
            StreamWriter Archivo = new StreamWriter(fs, System.Text.Encoding.Default);
            Archivo.Write("<!DOCTYPE html> \n");
            Archivo.Write("<html> \n");
            Archivo.Write("<head> \n");
            Archivo.Write("<title>Listado de Errores</title> \n");
            Archivo.Write("<link rel=stylesheet href=\"style.css\"> \n");
            Archivo.Write("</head> \n");
            Archivo.Write("<body> \n");
            Archivo.Write("<center> \n");
            Archivo.Write("<h2>Listado de Errores<br></h2> \n");
            Archivo.Write("</center> \n");
            Archivo.Write("<table align=center> \n");
            Archivo.Write("<tr> \n");
            Archivo.Write("<td><h3>#</h3></td> \n");//numero
            Archivo.Write("<td><h3>CARACTER</h3></td> \n");//lexema
            Archivo.Write("<td><h3>FILA</h3></td> \n");//fila
            Archivo.Write("<td><h3>COLUMNA</h3></td> \n");//columna
            Archivo.Write("</tr> \n");
            Archivo.Write("<center> \n");
            for (int i = 0; i < listado.Count; i++)
            {
                Archivo.Write("<tr> \n");
                Archivo.Write("<td><p>" + (i + 1) + " <p></td>\n");//numero
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).TOKEN + "<p></td>\n");//lexema
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).FILA + "<p></td>\n");//fila
                Archivo.Write("<td><p>" + ((MiToken)listado[i]).COLUMNA + "<p></td>\n");//columna
                Archivo.Write("</tr> \n");
            }

            Archivo.Write("</body> \n");
            Archivo.Write("</html> \n");
            Archivo.Close();
            System.Diagnostics.Process.Start(nombredoc);



        }

        public void generar_html(ArrayList List_lex, ArrayList List_err)
        {
            if (List_err.Count > 0 && List_lex.Count > 0)
            {
                html_Errores(List_err);
                html_Tokens(List_lex);
            }
            else if (List_lex.Count > 0)
            {
                html_Tokens(List_lex);
            }
            else if (List_err.Count > 0)
            {
                html_Errores(List_err);
                
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //mandar ha analizar y pintar
            ALexico(richTextBox1.Text.ToString());

            //generar html
            generar_html(ListTokens,ListError);

            //restablecer la escritura a color negro.
            richTextBox1.SelectionStart = richTextBox1.Text.ToString().Length;
            richTextBox1.SelectionColor = Color.Black;
        }
    }
}
