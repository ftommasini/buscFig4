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
using System.Text.RegularExpressions;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for EdicionFiguras.xaml
    /// </summary>
    public partial class EdicionFiguras : Window
    {
      //  VentConfiguracion conf;
        ClaseFigura[] Fig;
        TextBox[] coord = new TextBox[20];
        TextBlock[] textcoord = new TextBlock[20];
        private int[] coordAUX = new int[2];
        int CantFigReal;                // Este valor se carga al iniciar la ventana, toma el valor del slider de la ventana de configuracion


        public EdicionFiguras(ClaseFigura[] f)
        {
            InitializeComponent();
            Fig = f;
            coord = new TextBox[] {b1,b2,b3,b4,b5,b6,b7,b8,b9,b10,b11,b12,b13,b14,b15,b16,b17,b18,b19,b20};
            textcoord = new TextBlock[] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20 };
   //         StackEdicion.Children.Add(Fig[0].dibujoEdicion);
        }

        public void funcionCantFig(int selecc)
        {
            CantFigReal = selecc;

            comboBoxEdicion.Items.Clear();      // Limpia todo por si se abre de nuevo porque sino se agrega
            

            for (int i = 0; i < selecc; i++)  // Definimos acá la inicialización del ComboBox porque en el constructor todavía no esta inicializado la toma de datos
                comboBoxEdicion.Items.Add(Convert.ToString(i+1) + " - " + Fig[i].nombre);
        }

        private void comboBoxEdicion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            StackEdicion.Children.Clear();              //     Limpia el Stack
            StackEdicion.Children.Add(Fig[comboBoxEdicion.SelectedIndex].dibujoEdicion);    // Muestra Dibujo

            bNombre.Text = Fig[comboBoxEdicion.SelectedIndex].returnNombre();

            slCantPuntos.Value = Fig[comboBoxEdicion.SelectedIndex].returnCantPuntos();     //Pone la cantidad de puntos

            for (int i = 0; i < Fig[comboBoxEdicion.SelectedIndex].returnCantPuntos(); i++)     // Pone todos los puntos de la figura
                coord[i].Text = Fig[comboBoxEdicion.SelectedIndex].punto[i].ToString();



        }

        private void slCantPuntos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            coord = new TextBox[] { b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20 };
            textcoord = new TextBlock[] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20 };

            for (int i = 0; i < 20; i++)
            {
                if (i < Convert.ToUInt32(slCantPuntos.Value))
                {
                    coord[i].Visibility = Visibility.Visible;
                    textcoord[i].Visibility = Visibility.Visible;
                }
                else
                {
                    coord[i].Visibility = Visibility.Hidden;
                    textcoord[i].Visibility = Visibility.Hidden;
                } 
            }
        }



        private void botProbar_Click(object sender, RoutedEventArgs e)
        {
            GuardaEdicion(comboBoxEdicion.SelectedIndex); //Va a la funcion que edita todo

            StackEdicion.Children.Clear();              //     Limpia el Stack
            StackEdicion.Children.Add(Fig[comboBoxEdicion.SelectedIndex].dibujoEdicion);    // Muestra el nuevo Dibujo del usuario

        }

        private void GuardaEdicion(int index)
            {
                Fig[index].EditarNombre(bNombre.Text);

                Fig[index].EditarCantPuntos(Convert.ToInt32(slCantPuntos.Value)); // Edita la Nueva Cantidad de Puntos

                Fig[index].BlanquearPuntos();       // Blanquea la nueva cantidad de puntos

                int i = 0;
                for (i = 0; i < Fig[index].returnCantPuntos(); i++)       //Guarda los puntos modificados por el usuario
                {
                    ExtraeCoord(coord[i].Text);
                    Fig[index].AgregarPunto(i, coordAUX[0], coordAUX[1]);
                }
                Fig[index].FinalizarFigura(i);

                // Edita el color elegido

            }

        public void ExtraeCoord(string input)              // Esta función extrae las coordenadas desde el string de los TextBox
        {
            string[] numbers = Regex.Split(input, @"\D+");
            int i = 0;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    coordAUX[i] = int.Parse(value);
                }
                i++;
            }
        }

        private void botAceptar_Click(object sender, RoutedEventArgs e)
        {
            comboBoxEdicion.SelectedIndex = 0;
            this.Close();   
        }

        private void StackEdicion_MouseMove(object sender, MouseEventArgs e)
        {
            CoordMouse.Text = Convert.ToString((int)e.GetPosition(StackEdicion).X * 2) + ";" + Convert.ToString((int)e.GetPosition(StackEdicion).Y * 2);

        }

        private void StackEdicion_MouseLeave(object sender, MouseEventArgs e)
        {
            CoordMouse.Text = "Coordenada Mouse";
        }

        



    }
}
