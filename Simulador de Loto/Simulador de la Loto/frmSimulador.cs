using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace Simulado_de_la_Loto
{
    public partial class frmPantalla : Form
    {
        int cantidad = 0;
        int conteo = 0;
        frmLoading load = null;

 
        private string cadena = "Server=tcp:lottosoft.database.windows.net,1433;Initial Catalog=dbLoteria1;Persist Security Info=False;User ID=jarmandoo;Password=Azure123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection cn;

        public frmPantalla()
        {
            InitializeComponent();
            

        }

        private void GenerarJugada()
        {
            Random Num_Aleatorio = new Random(DateTime.Now.Millisecond);

            int num = Num_Aleatorio.Next(1, 38);

            if (string.IsNullOrEmpty(this.lblnum1.Text) || this.lblnum1.Text.Equals("0"))
            {
                this.lblnum1.Text = num.ToString();
            }
            else if (string.IsNullOrEmpty(this.lblnum2.Text) || this.lblnum2.Text.Equals("0"))
            {
                if (!this.lblnum1.Text.Equals(num.ToString()))
                {
                    this.lblnum2.Text = num.ToString();
                }
            }

            else if (string.IsNullOrEmpty(this.lblnum3.Text) || this.lblnum3.Text.Equals("0"))
            {
                if (!this.lblnum1.Text.Equals(num.ToString()) && !this.lblnum2.Text.Equals(num.ToString()))
                {
                    this.lblnum3.Text = num.ToString();
                }
            }

            else if (string.IsNullOrEmpty(this.lblnum4.Text) || this.lblnum4.Text.Equals("0"))
            {
                if (!this.lblnum1.Text.Equals(num.ToString()) && !this.lblnum2.Text.Equals(num.ToString()) &&
                    !this.lblnum3.Text.Equals(num.ToString()))
                {

                    this.lblnum4.Text = num.ToString();
                }
            }

            else if (string.IsNullOrEmpty(this.lblnum5.Text) || this.lblnum5.Text.Equals("0"))
            {
                if (!this.lblnum1.Text.Equals(num.ToString()) && !this.lblnum2.Text.Equals(num.ToString()) &&
                    !this.lblnum3.Text.Equals(num.ToString()) && !this.lblnum4.Text.Equals(num.ToString()))
                {
                    this.lblnum5.Text = num.ToString();
                }
            }

            else if (string.IsNullOrEmpty(this.lblnum6.Text) || this.lblnum6.Text.Equals("0"))
            {
                if (!this.lblnum1.Text.Equals(num.ToString()) && !this.lblnum2.Text.Equals(num.ToString()) &&
                    !this.lblnum3.Text.Equals(num.ToString()) && !this.lblnum4.Text.Equals(num.ToString()) &&
                    !this.lblnum4.Text.Equals(num.ToString()))
                {
                    this.lblnum6.Text = num.ToString();
                    this.timer1.Enabled = false;
                    cantidad = cantidad + 1;
                    this.lblCantidad.Text = "CANT: " + cantidad.ToString();
                    load.Close();
                }
            }        
        }

        private void GuardoJugada()
        {
            try
            {
                if (!this.lblnum1.Text.Equals("0") && !this.lblnum2.Text.Equals("0") && !this.lblnum3.Text.Equals("0") &&
                    !this.lblnum4.Text.Equals("0") && !this.lblnum5.Text.Equals("0") && !this.lblnum6.Text.Equals("0"))
                {
                    int[] Jugadas = new int[6];

                    Jugadas[0] = int.Parse(this.lblnum1.Text);
                    Jugadas[1] = int.Parse(this.lblnum2.Text);
                    Jugadas[2] = int.Parse(this.lblnum3.Text);
                    Jugadas[3] = int.Parse(this.lblnum4.Text);
                    Jugadas[4] = int.Parse(this.lblnum5.Text);
                    Jugadas[5] = int.Parse(this.lblnum6.Text);

                    string valor = (Jugadas[0].ToString() + " " + Jugadas[1].ToString() + " " + Jugadas[2].ToString() + " " +
                                    Jugadas[3].ToString() + " " + Jugadas[4].ToString() + " " + Jugadas[5].ToString());

                    string ruta = Directory.GetCurrentDirectory();
                    string directorio = ruta + "\\jugada.txt";

                    if (!File.Exists(directorio))
                    {
                        FileStream archivo = File.Create(directorio);
                        archivo.Close();
                    }

                    StreamWriter sw = new StreamWriter(directorio, true);
                    sw.Write(valor.ToString() + "      ");
                    sw.WriteLine(DateTime.Now);
                    sw.Close();

                    GuardarBaseDatos(valor);
                                  
                    LimpiarCampo();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }

        }

        private void GuardarBaseDatos(string resultados)
        {
            DateTime fecha = DateTime.Now;
            cn = new SqlConnection();
            cn.ConnectionString = cadena;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert Into tb_Resultados values ('" + resultados + "','" + fecha.ToString() + "') ";


            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        private void LimpiarCampo()
        {
            this.lblnum1.Text = "0";
            this.lblnum2.Text = "0";
            this.lblnum3.Text = "0";
            this.lblnum4.Text = "0";
            this.lblnum5.Text = "0";
            this.lblnum6.Text = "0";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            GuardoJugada();
            this.Close();
        }

        private void btnGeneraNumero_Click(object sender, EventArgs e)
        {
            if (this.lblnum1.Text.Equals("0") || this.lblnum2.Text.Equals("0") || this.lblnum3.Text.Equals("0") ||
                this.lblnum4.Text.Equals("0") || this.lblnum5.Text.Equals("0") || this.lblnum6.Text.Equals("0"))
            {
                load = new frmLoading();
                this.timer1.Enabled = true;
                load.ShowDialog();
            }
            else
            {
                GuardoJugada();
                btnGeneraNumero.PerformClick(); 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (conteo == 10)
            {
                GenerarJugada();
                conteo = 0;
            }
            conteo = conteo + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ruta = Directory.GetCurrentDirectory();
                string directorio = ruta + "\\jugada.txt";

                GuardoJugada();

                if (File.Exists(directorio))
                {
                    System.Diagnostics.Process.Start(directorio);
                }
                else
                {
                    MessageBox.Show("NO EXISTE JUGADA ANTERIOR GUARDADA.!!", this.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void frmPantalla_Load(object sender, EventArgs e)
        {
            MessageBox.Show("BIENVENIDO AL SISTEMA DE SIMULACION DE LOTO " + DateTime.Now.Date.ToString("dd/MM/yyyy") + ".!!", this.Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
