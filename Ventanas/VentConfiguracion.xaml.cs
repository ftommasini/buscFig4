using System;
using System.IO;
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
using System.Windows.Forms;

namespace WpfApplication1
{
   
    public partial class VentConfiguracion : Window
    {
        private Configuracion config;
        private EdicionFiguras Edicion;
        private ClaseFigura[] Fig;

        public int pr;          // NUMERO DE PRUEBA
        public string pr_string;   // NUMERO DE PRUEBA CON STRING CORREGIDO (0 AGREGADOS)

        public bool SalidaExitosa;

        public int[] DisminuyeFiguras;

        StackPanel[] Miniaturas = new StackPanel[8];
        System.Windows.Controls.Label[] nombr = new System.Windows.Controls.Label[8];

        System.Windows.Controls.ComboBox[] Selecc = new System.Windows.Controls.ComboBox[36];     // Comboboxs de la etapa lista
        System.Windows.Controls.Label[] lab = new System.Windows.Controls.Label[36];

        System.Windows.Controls.ComboBox[] Familia = new System.Windows.Controls.ComboBox[10];


        public VentConfiguracion(Configuracion c, EdicionFiguras ed, ClaseFigura[] f)
        {
            config = c;
            InitializeComponent();
            LabelDirectorio.Content = "Seleccione un Directorio";
            Miniaturas = new StackPanel[] { Mini1, Mini2, Mini3, Mini4, Mini5, Mini6, Mini7, Mini8 };
            nombr = new System.Windows.Controls.Label[] { n1,n2,n3,n4,n5,n6,n7,n8 };
            Edicion = ed;
            Fig = f;
            Selecc = new System.Windows.Controls.ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9, comboBox10, comboBox11, comboBox12, comboBox13, comboBox14, comboBox15, comboBox16, comboBox17, comboBox18, comboBox19, comboBox20, comboBox21, comboBox22, comboBox23, comboBox24, comboBox25, comboBox26, comboBox27, comboBox28, comboBox29, comboBox30, comboBox31, comboBox32, comboBox33, comboBox34, comboBox35, comboBox36 };
            lab = new System.Windows.Controls.Label[] { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, l16, l17, l18, l19, l20, l21, l22, l23, l24, l25, l26, l27, l28, l29, l30, l31, l32, l33, l34, l35, l36 };
            Familia = new System.Windows.Controls.ComboBox[] { comboBox1fam, comboBox2fam, comboBox3fam, comboBox4fam, comboBox5fam, comboBox6fam, comboBox7fam, comboBox8fam, comboBox9fam, comboBox10fam};
            Label_nombre.Visibility = Visibility.Hidden;
            
            

            conf1.Visibility = Visibility.Hidden; conf2.Visibility = Visibility.Hidden;  //Ocultamos las Tabs
            conf3.Visibility = Visibility.Hidden; conf4.Visibility = Visibility.Hidden;
            conf5.Visibility = Visibility.Hidden; conf6.Visibility = Visibility.Hidden;

            for (int i = 0; i < 8; i++)
            {
                Miniaturas[i].Children.Add(Fig[i].dibujoMini);      // Imprime todos los mini poligonos
                nombr[i].Content = Fig[i].nombre;
            }

            Finalizar.Visibility = Visibility.Hidden;

            Sig4.Visibility = Visibility.Hidden;
            Sig5.Visibility = Visibility.Hidden;

            SalidaExitosa=false;
        }

        private void buttonDirectorio_Click(object sender, RoutedEventArgs e)
        {
            string direct;
            FolderBrowserDialog dir = new FolderBrowserDialog(); //Elije directorio
            dir.ShowDialog();
            direct = dir.SelectedPath;     

            LabelDirectorio.Content = direct;
            existenciaDirectorio(direct);
        }

         public void existenciaDirectorio(string carpeta)
            {
                if (Directory.Exists(carpeta))   // Esto se pone para el caso en que no se elija directorio no se muestre el visibility
                    Finalizar.Visibility = Visibility.Visible;
                else
                {
                    LabelEstadoDirectorio.Content = "NO HA SELECCIONADO NINGUN DIRECTORIO, por favor elija un directorio válido para continuar";
                   // LabelEstadoDirectorio.Background = (System.Windows.Media.Brush)converter.ConvertFromString("#A9D90000"); //Rojo
                    Finalizar.Visibility = Visibility.Hidden;
                }
               

             // Iniciamos comprobacion de Nombre de prueba

                 pr = 1;
                 correccionDeString(pr);
                 

                 while (Directory.Exists(carpeta + "\\" + pr_string)) // Crea el nombre de la prueba (inicia con 001)
                 {
                     pr++;
                     correccionDeString(pr);
                 }

                 if (config.sujeto==2)
                    pr--;       // Si el sujeto es el 2do solo debemos restarle 1 porque la carpeta ya fue creada

                 correccionDeString(pr); 
                

                 config.directorio = carpeta + "\\" + pr_string;     // Asignamos acá la dirección de la prueba

                 if (pr > 1)
                 {
                     LabelEstadoDirectorio.Content = "EXISTEN registros anteriores. Esta será la prueba Número : " + pr_string;
                     //LabelEstadoDirectorio.Background = (System.Windows.Media.Brush)converter.ConvertFromString("#5B208B20"); //Verde
                     Label_nombre.Visibility = Visibility.Visible;
                     Label_nombre.Content = "PRUEBA N° " + pr_string;
                 }
                 else
                 { if (config.sujeto==1)
                    {
                     LabelEstadoDirectorio.Content = "En este directorio NO SE ENCUENTRAN registros anteriores. Esta será la prueba Número : 001.";
                     // LabelEstadoDirectorio.Background = (System.Windows.Media.Brush)converter.ConvertFromString("#83EBE516"); //Amarillo
                     Label_nombre.Visibility = Visibility.Visible;
                     Label_nombre.Content = "PRUEBA N° 1";
                     Finalizar.Visibility = Visibility.Visible;
                    }
                 else
                    {
                     LabelEstadoDirectorio.Content = "En este directorio NO SE ENCUENTRAN registros anteriores. Seleccione otra carpeta";
                     //  LabelEstadoDirectorio.Background = (System.Windows.Media.Brush)converter.ConvertFromString("#83EBE516"); //Amarillo
                     Label_nombre.Visibility = Visibility.Hidden;
                     Finalizar.Visibility = Visibility.Hidden;
                    }
                 }
                    

             }

        void correccionDeString(int num)
         {
             if (num < 10)
                 pr_string = "00" + num.ToString();
             else
             {
                 if (num < 100)
                     pr_string = "0" + num.ToString();
                 else
                     pr_string = num.ToString();
             }

         }
                



        private void Sig1_Click(object sender, RoutedEventArgs e)
        {
            conf2.IsSelected = true;            //Pasa a la siguiente pestaña           
        }

        private void Atr2_Click(object sender, RoutedEventArgs e)
        {
            conf1.IsSelected = true;
        }

        private void Sig2_Click(object sender, RoutedEventArgs e)
        {
                 conf3.IsSelected = true; 
        }

        private void Atr3_Click(object sender, RoutedEventArgs e)
        {
            conf2.IsSelected = true;  
        }

        private void Sig3_Click(object sender, RoutedEventArgs e)
        {
            AsignaDisminuyeFiguras();       //Asignación de la cantidad de figuras para organizar el orden

            for (int i = 0; i < 36 ; i++)
            { 
                if (i< Convert.ToInt32(slCantFig.Value * slRepetFigura.Value))  // Muestra la cantidad correcta de 
                {
                    Selecc[i].Visibility = Visibility.Visible;
                    lab[i].Visibility = Visibility.Visible;
                }
                else
                {
                    Selecc[i].Visibility = Visibility.Hidden;
                    lab[i].Visibility = Visibility.Hidden;
                }
            }

            if (slCantFig.Value * slRepetFigura.Value <= 36)
            {
                if (radioLista.IsChecked == true)
                {
                    if (radioFam_Act.IsChecked == true)
                        conf4.IsSelected = true;
                    else
                        conf5.IsSelected = true;
                }
                else
                {
                    if (radioFam_Act.IsChecked == true)
                        conf4.IsSelected = true;
                    else
                        conf6.IsSelected = true;
                }
            }
            else
                System.Windows.MessageBox.Show("La Cantidad de Ensayos total por Sujeto debe menor o igual al 36, por favor corrija a Cantidad de Figuras o la Cantidad de Repeticiones de Figura", "Error: Cantidad de Ensayos superada", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Atr4_Click(object sender, RoutedEventArgs e)
        {
            conf3.IsSelected = true;
        }

        private void Sig4_Click(object sender, RoutedEventArgs e)
        {
            if (radioLista.IsChecked == true)
                    conf5.IsSelected = true;
            else
                conf6.IsSelected = true;
        }

        private void Atr5_Click(object sender, RoutedEventArgs e)
        {
            if (radioFam_Act.IsChecked == true)
                conf4.IsSelected = true;
            else
                conf3.IsSelected = true;
        }

        private void Sig5_Click(object sender, RoutedEventArgs e)
        {
            conf6.IsSelected = true;
        }

        private void Atr6_Click(object sender, RoutedEventArgs e)
        {
            if (radioLista.IsChecked == true)
                conf5.IsSelected = true;
            else
                conf3.IsSelected = true;
        }



        private void slCantFig_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)   
        {
            Miniaturas = new StackPanel[] { Mini1, Mini2, Mini3, Mini4, Mini5, Mini6, Mini7, Mini8 };
            nombr = new System.Windows.Controls.Label[] { n1, n2, n3, n4, n5, n6, n7, n8 };

            for (int i = 0; i < 8; i++)
                {
                    if (i < Convert.ToUInt32(slCantFig.Value))
                    {
                        Miniaturas[i].Visibility = Visibility.Visible;
                        //nombr[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Miniaturas[i].Visibility = Visibility.Hidden;
                        //nombr[i].Visibility = Visibility.Hidden;
                    }
                }

        }

        private void buttonEdic_Click(object sender, RoutedEventArgs e)
        {
            Edicion.funcionCantFig(Convert.ToInt32(slCantFig.Value));
            buttonEdic.Visibility = Visibility.Hidden;
            Edicion.ShowDialog();
        }

        public void AsignaDisminuyeFiguras()        //Esta función crea el vector y lo inicializa para la cantidad de figuras a la hora de hacer la lista
        {
            DisminuyeFiguras = new int[Convert.ToInt32(slCantFig.Value)];
            for (int i = 0; i < Convert.ToInt32(slCantFig.Value); i++)
                DisminuyeFiguras[i] = Convert.ToInt32(slRepetFigura.Value); 
        }

        private void comboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            Selecc[Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag)].Items.Clear();

            for (int i = 0; i < Convert.ToInt32(slCantFig.Value); i++)
            {
                    Selecc[Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag)].Items.Add(Convert.ToString(i + 1) + " - " + Fig[i].nombre + " ("+ Convert.ToString(DisminuyeFiguras[i]) + " restantes)");       
             }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i=Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag);
            if (Selecc[i].SelectedIndex != -1)
            {
                if (DisminuyeFiguras[Selecc[i].SelectedIndex] != 0)
                {
                    DisminuyeFiguras[Selecc[i].SelectedIndex]--; //Se le resta 1
                    Selecc[i].IsEnabled = false;
                }
                else
                {
                    System.Windows.MessageBox.Show("La figura elegida ya no está disponible, por favor elija otra figura.", "Error de Selección de Figura", MessageBoxButton.OK, MessageBoxImage.Information);
                    Selecc[Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag)].SelectedIndex = -1;
                }
            }


            for (int j = 0; j < slCantFig.Value * slRepetFigura.Value; j++)      //Muestra El boton siguiente cuando se hayan competado todas las opciones
            {   if (Selecc[j].SelectedIndex == -1)
                    break;
            if (j == (slCantFig.Value * slRepetFigura.Value - 1) && Selecc[j].SelectedIndex != -1)
                Sig5.Visibility = Visibility.Visible;
            }
        }

        private void buttonLimpiar_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 36; i++)
            {
                Selecc[i].SelectedIndex = -1;
                Selecc[i].IsEnabled = true;
            }

            Sig5.Visibility = Visibility.Hidden; //Oculta El boton siguiente
        }



        private void comboBoxFamilia_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            Familia[Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag)].Items.Clear();

            for (int i = 0; i < Convert.ToInt32(slCantFig.Value); i++)
            {
                Familia[Convert.ToInt32(((System.Windows.Controls.ComboBox)sender).Tag)].Items.Add(Convert.ToString(i + 1) + " - " + Fig[i].nombre );
            }
        }

        private void comboBoxFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            for (int j = 0; j < 10; j++)      //Muestra El boton siguiente cuando se hayan competado todas las opciones
            {
                if (Familia[j].SelectedIndex == -1)
                    break;
                if (j == 9 && Familia[j].SelectedIndex != -1)
                    Sig4.Visibility = Visibility.Visible;
            }
        }

        private void buttonLimpiarFamilia_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Familia[i].SelectedIndex = -1;
                Familia[i].IsEnabled = true;
            }

            Sig4.Visibility = Visibility.Hidden; //Oculta El boton siguiente
        }



        private void Finalizar_Click(object sender, RoutedEventArgs e)  // Guarda todas las configuraciones y cierra VentConfiguración
        {
            // El directorio ya se guarda en la clase Configuración cuando lo elegimos por lo que no hace falta hacerlo de nuevo


      /*      try
            {
                config.edad[0] = Convert.ToInt32(textBoxEdad1.Text);
                if (config.etapa == 2)
                    config.edad[1] = Convert.ToInt32(textBoxEdad2.Text);  // Corregir porque si se ingresan caracteres da error

            }
            catch (Exception)
            {
                config.edad[0] = 0;
                if (config.etapa == 2)
                    config.edad[1] = 0;
                // MostrarError("Error: " + ex.Message)
            }*/
                      
                   


            if (radioConLim.IsChecked == true)          // Configuramos Tipo de limite tiempo
            {
                config.tlimite = true;

                try
                {
                    config.valorlimite = Convert.ToInt32(textBoxSegundos.Text);
                }
                catch (Exception)
                {
                    config.tlimite = false;
                }
                
            }
            else
                config.tlimite = false;


            config.cantFig = Convert.ToInt32(slCantFig.Value);      // Configuramos cant de Fig

            config.cantRep = Convert.ToInt32(slRepetFigura.Value);        // Configuramos cant de rep



            config.CrearVectorOrden();       //Aca creamos el Vector Orden de Familiariz y prueba con la cantidad Total de Ensayos que se harán

            if (radioFam_Act.IsChecked == true)
            {
                GeneracionListaFamil();          //Función generación de la lista
            }


            if (radioLista.IsChecked == true)          // Configuramos Orden de Aparicion de Figuras
            {
                config.confOrden = true;
                GeneracionLista();          //Función generación de la lista
            }
            else
            {
                config.confOrden = false;
                GeneracionAzar();          //Función generación de la lista
            }


            if (radioFeed_1_Act.IsChecked == true)          // Configuramos Feedback Respuesta
            {
                config.FeedbackRespuesta = true;
            }
            else
                config.FeedbackRespuesta = false;


            if (radioFeed_2_Act.IsChecked == true)          // Configuramos Feedback Estado
            {
                config.FeedbackEstado = true;
            }
            else
                config.FeedbackEstado = false;

            if (radioPreg_Act.IsChecked == true)          // Configuramos Preguntas
            {
                config.boolPreguntas = true;
            }
            else
                config.boolPreguntas = false;


            if (radioFam_Act.IsChecked == true)          // Configuramos Orden de Aparicion de Figuras
            {
                config.Familiarizacion = true;
            }

            SalidaExitosa = true;

            this.Close();

        }

        public void GeneracionListaFamil()
        {
            for (int i = 0; i < 10; i++)
                config.ordenFamil[i] = Familia[i].SelectedIndex;
        }



        public void GeneracionLista()
        {
            for (int i = 0; i < config.cantFig * config.cantRep; i++)
                config.orden[i] = Selecc[i].SelectedIndex;              
        }

        public void GeneracionAzar()
        {
            Azar a = new Azar(config.cantFig, config.cantRep);
            config.orden = a.etapas();       
        }

        private void button_Sujeto_Click(object sender, RoutedEventArgs e)
        {
            if (config.sujeto == 1)
            {
                config.sujeto = 2;
                button_Sujeto.Content = "SUJETO 2";
                Label_directorio.Content = "Seleccione la carpeta creada recientemente por el programa del Sujeto 1:";

            }
            else
            {
                config.sujeto = 1;
                button_Sujeto.Content = "SUJETO 1";
                Label_directorio.Content = "Seleccione el directorio donde se guardarán los datos del ensayo:";
            }
        }










    } 
}
