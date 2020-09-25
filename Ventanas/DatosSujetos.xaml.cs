using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{

    public partial class DatosSujetos : Window
    {
        public bool SalidaExitosa;
        public string dirG;
        string nomPrueba;

        public DatosSujetos(string a, string nom)
        {
            InitializeComponent();
            nomPrueba = nom;
            dirG=a;                     // Cargamos la dirección del directorio
            SalidaExitosa = false;
        }

        private void button_Finalizar_Click(object sender, RoutedEventArgs e)
        {
            creacionDeArchivoDPersonales();
            SalidaExitosa = true;
            this.Close();
        }

        void creacionDeArchivoDPersonales()
        {
            string ubic;
            System.IO.StreamWriter arch;

            ubic = dirG + "\\" + nomPrueba + ".csv";
            arch = new System.IO.StreamWriter(ubic);
            arch.WriteLine("Prueba;Sujeto;Nombre;Apellido;Edad;Sexo;EstadoCivil;Ciudad;NivelEstudios");
            arch.WriteLine(nomPrueba + ";1;" + TextBox_Nom1.Text + ";" + TextBox_Apell1.Text + ";" + TextBox_Edad1.Text + ";" + TextBox_Sexo1.Text + ";" + TextBox_Civil1.Text + ";" + TextBox_Ciu1.Text + ";" + TextBox_Estu1.Text);
            arch.WriteLine(nomPrueba + ";2;" + TextBox_Nom2.Text + ";" + TextBox_Apell2.Text + ";" + TextBox_Edad2.Text + ";" + TextBox_Sexo2.Text + ";" + TextBox_Civil2.Text + ";" + TextBox_Ciu2.Text + ";" + TextBox_Estu2.Text);
            arch.Close();

        }


    }
}
