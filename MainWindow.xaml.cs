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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;

  

namespace WpfApplication1
{

    public partial class MainWindow : Window

    {
        bool resultado, explora, eleccion, preg;
        double texplora, trespuesta, taux;
        DateTime tiempo;        //Lo declaramos ahora para no hacer perder tiempo a la fucnion en declararla cada vez q se llama;
        Prueba suj;
        Configuracion Config;
        ClaseFigura[] Figura;
        ClasePregunta[] Pregunta;

        System.Media.SoundPlayer player;
            //, SonidoRosa;

        System.IO.StreamWriter archivoRecorr;
        

        System.Windows.Threading.DispatcherTimer temporizador = new System.Windows.Threading.DispatcherTimer(); // este sera el temporizador que finaliza la prueba
        System.Windows.Threading.DispatcherTimer Tmouse = new System.Windows.Threading.DispatcherTimer();       // Y este es el timer de interrupcion que cada tantos ticks toma el dato de la posición del mouse
        System.Windows.Threading.DispatcherTimer segundero = new System.Windows.Threading.DispatcherTimer(); 
                   
        public int ensayoActual; //Este int contendra el ensayo actual que se esta explorando en ese momento
        public int ensayoActualFamil;
        public int s;  // S es el sujeto actual que va a explorar o está explorando
        public int interfaz_segundos;

        public int pAux,pnum;



        private PointCollection _points = new PointCollection();

        int ticks = 0;          // NUmero necesario para mostrar los milisegundos del recorrido
        int estado;             // Con esto sabemos si esta dentro o fuera de la figura, 0 es fuera, 1 es dentro
        int sel;
        System.Media.SoundPlayer pregSon = new System.Media.SoundPlayer(Properties.Resources.PreguntasDef);


        public MainWindow()
        {
            InitializeComponent();
        //    SonidoRosa = new System.Media.SoundPlayer("C:\Buscando_Figuras\Archivos_del_Programa\RosaMono.wav");
            //SonidoRosa.Source = new System.Media.SoundPlayer(Properties.Resources.son_Rosa);
            //SonidoRosa.Source = new Uri(@"son_Rosa.wav", UriKind.Relative);

          //  SonidoRosa.Source = new System.Media.SoundPlayer(Properties.Resources.son_Rosa);

            SonidoRosa.Source = new Uri("C://BuscandoFiguras4/RosaMono.wav"); //Ubicacion de Sonido de figura
            SonidoRosa.LoadedBehavior = MediaState.Stop; SonidoRosa.UnloadedBehavior = MediaState.Close;

            segundero.Interval = TimeSpan.FromSeconds(1);         //Activación del Temporizador del monitor de tiempo de exploracion
            segundero.Tick += new EventHandler(SegunderoInterfaz);

            Tmouse.Interval = TimeSpan.FromMilliseconds(1);          // Milisegundos de Guarda recorrido
            Tmouse.Tick += new EventHandler(Tmouse_Tick);


            Config = new Configuracion();
            Figura = new ClaseFigura[8];
            Pregunta = new ClasePregunta[3]; AsignaciondePreguntas();
            CargaDatosFiguras(ref Figura);

            EdicionFiguras Edic = new EdicionFiguras(Figura);
            VentConfiguracion c = new VentConfiguracion(Config, Edic, Figura);
            

            this.Visibility = Visibility.Hidden; //Oculta MainWindows para mostrar primero la Configuración.


            c.ShowDialog();

            if (c.SalidaExitosa == false)       // En caso de no Configurarse correctamente el programa se cierra
            {
                System.Windows.MessageBox.Show("La configuración no ha finalizado correctamente, el programa se cerrará.", "Configuración Errónea", MessageBoxButton.OK, MessageBoxImage.Information);
                System.Environment.Exit(1);    //Sale del programa automaticamente    
            }
            suj = new Prueba((Config.cantFig * Config.cantRep), c.pr_string);        //Creamos la cantidad de Sujetos y Ensayos Necesarios y Pasamos el nombre de prueba a la clase Prueba que contiene los datos de la prueba

            creacionDeCarpetas();  // Funcion de crear carpetas
  

          if (Config.sujeto == 1)            // Ventana que muestra el guardado de los nombres
             {
                 DatosSujetos inf = new DatosSujetos(Config.directorio, suj.NombrePrueba);
                  inf.ShowDialog();

                  if (inf.SalidaExitosa == false)       // En caso de no Configurarse correctamente el programa se cierra
                  {
                      System.Windows.MessageBox.Show("La configuración no ha finalizado correctamente, el programa se cerrará.", "Configuración Errónea", MessageBoxButton.OK, MessageBoxImage.Information);
                      System.Environment.Exit(1);    //Sale del programa automaticamente    
                  }
              }

                ensayoActual = 0; // inicializamos con 0 La posicion del ensayo
                ensayoActualFamil = 0; // inicializamos con 0 La posicion del ensayo de familiarizacion
           
            
            // TENEMOS TODO CREADO PASAMOS A LA INTERFAZ DE MAINWINDOWS


            this.Visibility = Visibility.Visible;
            Button_Salir.Visibility = Visibility.Hidden;
            label_ID.Content = suj.NombrePrueba;
            Panel_Sujeto.Content = "Sujeto: Sujeto " + Config.sujeto;
            

            // Funcion que espera al otro sujeto


            resultado = true; explora = false; eleccion = false; preg = false; // DESBLOQUEADA LA ETAPA DE EXPLORACIÓN
            // Cuando explora es verdad 

            if (Config.Familiarizacion == true)
               label_Estado.Content = "Listo para iniciar Familiarización";
            else
               label_Estado.Content = "Listo para iniciar la Prueba " + suj.NombrePrueba + " en Sujeto " + Config.sujeto;

            textTeclas.Text = "8 (NumPad) para iniciar la exploración.";

        }








        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.NumPad8 && resultado == true)                // Iniciar exploración 
            {
                IniciaExplora();
            }


            if (e.Key == Key.NumPad2 && explora == true)                // Finaliza exploración
            {
                ElijeResultados();               
            }


            if (e.Key == Key.A && eleccion == true && Config.cantFig >= 1)               
            {
                Seleccion(0);
            }
            if (e.Key == Key.S && eleccion == true && Config.cantFig >= 2)               
            {
                Seleccion(1);
            }
            if (e.Key == Key.D && eleccion == true && Config.cantFig >= 3)              
            {
                Seleccion(2);
            }
            if (e.Key == Key.F && eleccion == true && Config.cantFig >= 4)              
            {
                Seleccion(3);
            }
            if (e.Key == Key.G && eleccion == true && Config.cantFig >= 5)              
            {
                Seleccion(4);
            }
            if (e.Key == Key.H && eleccion == true && Config.cantFig >= 6)              
            {
                Seleccion(5);
            }
            if (e.Key == Key.J && eleccion == true && Config.cantFig >= 7)            
            {
                Seleccion(6);
            }
            if (e.Key == Key.K && eleccion == true && Config.cantFig >= 8)               
            {
                Seleccion(7);
            }

            if (e.Key == Key.Q && Config.Familiarizacion==true)
                {
                    ensayoActualFamil = 9;
                    label_Estado.Content="Familiarización CANCELADA. En el próximo ensayo comienza la Prueba.";
                }

            if (e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.I || e.Key == Key.P)
            {
                if(preg==true)
                {
                    Pregunta[pnum].sonido.Stop();       //Sin embargo no para!!! WTF
                   

                    if (e.Key == Key.D1)
                        Pregunta[pnum].respuestaInt = 1; 
                    if (e.Key == Key.D2)
                        Pregunta[pnum].respuestaInt = 2; 
                    if (e.Key == Key.D3)
                        Pregunta[pnum].respuestaInt = 3; 
                    if (e.Key == Key.D4)
                        Pregunta[pnum].respuestaInt = 4;
                    if (e.Key == Key.D5)
                        Pregunta[pnum].respuestaInt = 5; 

                    if (e.Key == Key.I)         //VERDADERO
                        Pregunta[pnum].respuestaBool = true;
                    if (e.Key == Key.P)         //FALSO
                        Pregunta[pnum].respuestaBool = false;



                    if (pnum==2)
                         Guardar(); 
                    else
                    {
                        pnum++;

                        if (Config.Familiarizacion == true)
                            label_Estado.Content = "Familiariz: Escuchando y esperando respuesta de la Pregunta " + (pnum + 1);
                        else
                            label_Estado.Content = "Escuchando y esperando respuesta de la Pregunta " + (pnum + 1);

                        Pregunta[pnum].sonido.Play();    //Se reproduce la nueva pregunta
                    }

                }
            }
        }



        public void IniciaExplora()
        {
            //labelInfo.Visibility = Visibility.Hidden;
            
            if (Config.FeedbackEstado==true)
            {
                player = new System.Media.SoundPlayer(Properties.Resources.son_inicia);
                player.Play();
            }

            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(690, 384);


            polylineObj.Visibility = Visibility.Visible;        // Se Muestra la Figura en pantalla

            if (Config.Familiarizacion==true)      // Solo entra a Familiarizacion si es igual a 1
            {
                polylineObj.Points = Figura[Config.ordenFamil[ensayoActualFamil]].PuntosdibujoOriginal;
               // System.Windows.Media.Brush a = "#ECA060";
               // polylineObj.Fill = "#ECA060";
                label_Estado.Content = "Familiariz: Explorando ensayo " + (ensayoActualFamil + 1) + " de 10";
                textTeclas.Text = "2 (NumPad) para finalizar la exploración                  Q: Para cancelar Familiarización";
            }
                
            else
            {
                 polylineObj.Points = Figura[Config.orden[ensayoActual]].PuntosdibujoOriginal;
                 label_Estado.Content = "Explorando ensayo " + (ensayoActual + 1) + " de " + (Config.cantFig * Config.cantRep);
                 textTeclas.Text = "2 (NumPad) para finalizar la exploración";
            }

            

            if(Config.Familiarizacion==false)
                    CreaArchivoRecorrido();


            taux = Tiempoinicial();     //Calculamos timepo inicial para caluclar el tiempo de exploracion
            temporizador.Tick += new EventHandler(tCumplido); //inicia temporizador de exploración
            temporizador.Interval = new TimeSpan(0, 0, Config.valorlimite);  //Valor de tiempo de teporizador
            if (Config.tlimite==true)
                     temporizador.Start();              // Solo si se seleccionó el temporizador se activa, sino no

            Tmouse.Start();     //Inicio de las interrupciones de toma de datos del mouse

            suj.en[ensayoActual].ensayo = ensayoActual + 1;         // Guardado de numero de ensayo en Clase Prueba
            if (ensayoActual + 1 < 10)
                suj.en[ensayoActual].ensayo_string = "0" + (ensayoActual + 1).ToString();
            else
                suj.en[ensayoActual].ensayo_string = (ensayoActual + 1).ToString();


            //// INTERFAZ////
            if (Config.Familiarizacion == false)
            {
                Panel_Ensayo.Content = "Ensayo: " + (ensayoActual + 1).ToString() + " de " + (Config.cantFig * Config.cantRep);
                Panel_FExplor.Content = "Figura Explorada: " + Figura[Config.orden[ensayoActual]].nombre;
                label_IDEnsayo.Content = suj.NombrePrueba + Config.sujeto + suj.en[ensayoActual].ensayo_string;
            }
            else
            {
                Panel_Ensayo.Content = "FAMILIARIZACIÓN: " + (ensayoActualFamil + 1).ToString() + " de 10";
                Panel_FExplor.Content = "Figura Explorada: " + Figura[Config.ordenFamil[ensayoActualFamil]].nombre;
            }
            Panel_FEleg.Content = "Figura Elegida: ";  // Restablecemos la interfaz en el nuevo ensayo
            Panel_Result.Content = "Resultado:";
            Panel_TRespuesta.Content = "Tiempo de Respuesta:";

            interfaz_segundos = 0;
            segundero.Start();

            /////////////////
            

            resultado = false; explora = true;
        }

        public void ElijeResultados()           //  DESPUES DE APRETAR "2" FINALIZA la exploración
        {
            Tmouse.Stop();      //Debemos parar el Tmouse para que no siga calculando intervalos
            ticks = 0;
            segundero.Stop();

            texplora = Calculatiempo(taux);
            temporizador.Stop();          //Debemos parar el temporizador para que no siga calculando intervalos - Si esta desactivado no importa no genera error
            taux = Tiempoinicial();         //Calculamos timepo inicial para calcular el tiempo de respuesta

            if (Config.Familiarizacion == false)
                    archivoRecorr.Close();      // Cerramos archivo de recorrido

            polylineObj.Visibility = Visibility.Hidden;   // Quitamos la figura de pantalla

            if (Config.FeedbackEstado == true)
            {
                player = new System.Media.SoundPlayer(Properties.Resources.son_opcion);    //Sonido de finalizacion de exploracion
                player.Play();
            }


            label_Estado.Content = "Familiariz: Esperando elección de Figura";
            textTeclas.Text = "- 1er Figura: A    - 2da Figura: S     - 3ra Figura: D     - 4ta Figura: F     - 5ta Figura: G     - 6ta Figura: H";

            

            explora = false; eleccion = true; preg = false;
        }

        public void Seleccion(int sel)
        {
     
            guardadoEnsayoenClasePrueba(sel);       // Guardado de datos primera parte

            if (Config.Familiarizacion == true)
            {
                Panel_FEleg.Content = "Figura Elegida: " + Figura[sel].nombre;
                if (Config.ordenFamil[ensayoActualFamil] == sel)
                    Panel_Result.Content = "Resultado: CORRECTO";
                else
                    Panel_Result.Content = "Resultado: INCORRECTO";
            }
            else
            {
                Panel_FEleg.Content = "Figura Elegida: " + Figura[sel].nombre;
                if (Config.orden[ensayoActual] == sel)
                    Panel_Result.Content = "Resultado: CORRECTO";
                else
                    Panel_Result.Content = "Resultado: INCORRECTO";
            }
                trespuesta = Calculatiempo(taux);

                Panel_TRespuesta.Content = "Tiempo de Respuesta: " + trespuesta.ToString("n2");

                if (Config.boolPreguntas == true)
                {
                    ///INICIA PREGUNTAS

                    eleccion = false; resultado = false; preg = true;

                    pnum = 0;
                    

                    if (Config.Familiarizacion== true)
                             label_Estado.Content = "Familiariz: Escuchando y esperando respuesta de la Pregunta " + (pnum+1);
                    else
                             label_Estado.Content = "Escuchando y esperando respuesta de la Pregunta " + (pnum + 1);

                    textTeclas.Text = "- Respuesta Numérica: 1 al 5 (No Numpad)              - Respuesta Discreta:           -- SI : I                -- NO : P";

                    //pregSon.PlaySync();     //Sonido de Preguntas
                    Pregunta[pnum].sonido.Play();

                }
                else
                { Guardar();  }
            
        }


      public void Guardar()
         {
             eleccion = false; resultado = true; preg = false;
               if (Config.Familiarizacion == false)   // Aca si se guardan los datos de prueba cuando la Familiarizacion es Falsa
            {


                guardarEnsayoenCSV();


                ensayoActual++;

                textTeclas.Text = "8 (NumPad) para inciar la exploración.";

                if (verificasalida() == true)           // VERIFICACION DE SALIDA
                {
                    label_Estado.Content = "Prueba " + suj.NombrePrueba + " de Sujeto " + Config.sujeto + " Finalizada. Gracias Totales";      //Muestra finalizacion de prueba
                    Thickness m = Button_Salir.Margin;
                    m.Left = 1010;                  
                    Button_Salir.Margin = m;
                    Button_Salir.Visibility = Visibility.Visible;
                }
                else
                    label_Estado.Content = "Listo para iniciar exploración de ensayo " + (ensayoActual + 1) + " de " + (Config.cantFig * Config.cantRep);

            }
            else                // FAMILIARIZACION verdadera
            {
                ensayoActualFamil++;
                if (ensayoActualFamil == 10)  //10 es la cantidad de ensayos que tiene familiarizacion
                {
                    Config.Familiarizacion = false; // Significa que terminamos la Familiarizon y pasamos al true
                    label_Estado.Content = "Listo para iniciar la Prueba " + suj.NombrePrueba + " en Sujeto " + Config.sujeto;
                }
                else
                   label_Estado.Content = "Familiariz: Listo para iniciar exploración de ensayo " + (ensayoActualFamil+1) + " de 10";

            }

            
        }

        public void guardadoEnsayoenClasePrueba(int n)
        {
            suj.en[ensayoActual].figuraexplorada = Config.orden[ensayoActual];     //Guarda la figura explorada en este ensayo
            suj.en[ensayoActual].figuraelegida = n;

            suj.en[ensayoActual].tdeexplor = texplora;
            suj.en[ensayoActual].tderespuesta = trespuesta;

            if (Config.orden[ensayoActual] == n)
            {
                suj.en[ensayoActual].resultado = 1;
                if (Config.FeedbackRespuesta == true)
                {
                    player = new System.Media.SoundPlayer(Properties.Resources.son_correcto);    //Sonido de finalizacion de exploracion
                    player.PlaySync();
                }
            }
            else
            {
                suj.en[ensayoActual].resultado = 0;
                if (Config.FeedbackRespuesta == true)
                {
                    player = new System.Media.SoundPlayer(Properties.Resources.son_incorrecto);    //Sonido de finalizacion de exploracion
                    player.PlaySync();
                }
            }

            if (Config.FeedbackRespuesta == false)
            {
                player = new System.Media.SoundPlayer(Properties.Resources.son_eleccion);    //Sonido de finalizacion de exploracion
                player.PlaySync();
            }

        }

        void guardarEnsayoenCSV()
        {

            System.IO.StreamWriter arch;
            string ubic = Config.directorio + "\\Suj" + Config.sujeto + "\\" + suj.NombrePrueba + "_" + Config.sujeto + ".csv";
            arch = new System.IO.StreamWriter(ubic, true);

            if (ensayoActual==0)        // Si es la primera entrada creamos el Header del archivo
                arch.WriteLine("ID;Ensayo;Sujeto;FiguraExplorada;FiguraElegida;TiempoExploracion;TiempoRespuesta;Resultado;Preg1;Preg2;Preg3");

            arch.WriteLine(suj.NombrePrueba + Config.sujeto + suj.en[ensayoActual].ensayo_string + ";" + suj.en[ensayoActual].ensayo_string + ";" + Config.sujeto + ";" + suj.en[ensayoActual].figuraexplorada + ";" + suj.en[ensayoActual].figuraelegida + ";" + suj.en[ensayoActual].tdeexplor.ToString("n2") + ";" + suj.en[ensayoActual].tderespuesta.ToString("n2") + ";" + suj.en[ensayoActual].resultado + ";" + Pregunta[0].respuestaInt + ";" + Pregunta[1].respuestaBool + ";" + Pregunta[2].respuestaInt);
            arch.Close();
        }

        void CreaArchivoRecorrido()
        {
            string aux;
            if ((ensayoActual + 1) < 10)
                aux = "0" + (ensayoActual + 1).ToString();
            else
                aux = (ensayoActual + 1).ToString();
            try
            {
                string ubic2 = Config.directorio + "\\Suj" + Config.sujeto + "\\Recorridos\\" + suj.NombrePrueba + "_" + Config.sujeto + "_" + aux + ".csv";
                archivoRecorr = new System.IO.StreamWriter(ubic2, true);
                archivoRecorr.WriteLine("Tmilisegundos;X;Y;Estado");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("El Sujeto 1 fue configurado incorrectamente, Sujeto 2 no tiene donde guardar sus archivos", "Mala Configuración de Sujeto 1", MessageBoxButton.OK, MessageBoxImage.Information);
                System.Environment.Exit(1);    //Sale del programa automaticamente    
                throw;
            }

        }


        public bool verificasalida()
        {
            if (ensayoActual == (Config.cantFig * Config.cantRep))
                return true;
            else
                return false;
        }



        double Tiempoinicial()
        {       
            tiempo = DateTime.Now; //Guarda tiempo del mouse
            double tiempototal = ((double)tiempo.Hour) * 3600f + ((double)tiempo.Minute) * 60f + ((double)tiempo.Second) + ((double)tiempo.Millisecond) / 1000f; 
            return (tiempototal);
        }

        double Calculatiempo(double inic)
        {
            tiempo = DateTime.Now; //Guarda tiempo del mouse
            double tiempototal = ((double)tiempo.Hour) * 3600f + ((double)tiempo.Minute) * 60f + ((double)tiempo.Second) + ((double)tiempo.Millisecond) / 1000f - inic; // Resta tiempo actual - tiempo inicial 
            return (tiempototal);
        }

        //  BordeFigura.Children.Add(Figura[0].dibujoOriginal);



        // System.Environment.Exit(1);    Sale del programa automaticamente      

        private void tCumplido(object sender, EventArgs e)  //Función que
        {
            ElijeResultados();
        }


        void CargaDatosFiguras(ref ClaseFigura[] f)
        {

            // Figura 1 - CUADRADO      
            f[0] = new ClaseFigura();
            f[0].EditarNombre("Cuadrado");
            f[0].EditarCantPuntos(4);
            f[0].AgregarPunto(0, 250, 100);
            f[0].AgregarPunto(1, 650, 100);
            f[0].AgregarPunto(2, 650, 500);
            f[0].AgregarPunto(3, 250, 500);
            f[0].AgregarPunto(4, 250, 100);
            f[0].FinalizarFigura(5);
            f[0].EditarColor("#0521D4");

            // Figura 2 - Triangulo      
            f[1] = new ClaseFigura();
            f[1].EditarNombre("Triangulo");
            f[1].EditarCantPuntos(3);
            f[1].AgregarPunto(0, 450, 100);
            f[1].AgregarPunto(1, 700, 500);
            f[1].AgregarPunto(2, 200, 500);
            f[1].AgregarPunto(3, 450, 100);
            f[1].FinalizarFigura(4);
            f[1].EditarColor("#9EF338");

            // Figura 3 - Rectangulo      
            f[2] = new ClaseFigura();
            f[2].EditarNombre("Rectangulo");
            f[2].EditarCantPuntos(4);
            f[2].AgregarPunto(0, 250, 240);
            f[2].AgregarPunto(1, 650, 240);
            f[2].AgregarPunto(2, 650, 360);
            f[2].AgregarPunto(3, 250, 360);
            f[2].AgregarPunto(4, 250, 240);
            f[2].FinalizarFigura(5);
            f[2].EditarColor("#D50019");

            // Figura 4 - Triangulito      
            f[3] = new ClaseFigura();
            f[3].EditarNombre("Triangulito");
            f[3].EditarCantPuntos(3);
            f[3].AgregarPunto(0, 450, 100);
            f[3].AgregarPunto(1, 520, 500);
            f[3].AgregarPunto(2, 380, 500);
            f[3].AgregarPunto(3, 450, 100);
            f[3].FinalizarFigura(4);
            f[3].EditarColor("#E86308");

            // Figura 5 - Rombo      
            f[4] = new ClaseFigura();
            f[4].EditarNombre("Rombo");
            f[4].EditarCantPuntos(4);
            f[4].AgregarPunto(0, 450, 100);
            f[4].AgregarPunto(1, 700, 300);
            f[4].AgregarPunto(2, 450, 500);
            f[4].AgregarPunto(3, 200, 300);
            f[4].AgregarPunto(4, 450, 100);
            f[4].FinalizarFigura(5);
            f[4].EditarColor("#A10019");


            // Figura 6 - Estrella      
            f[5] = new ClaseFigura();
            f[5].EditarNombre("Estrella");
            f[5].EditarCantPuntos(10);
            f[5].AgregarPunto(0, 450, 80);
            f[5].AgregarPunto(1, 512, 250);
            f[5].AgregarPunto(2, 700, 250);
            f[5].AgregarPunto(3, 544, 346);
            f[5].AgregarPunto(4, 600, 500);
            f[5].AgregarPunto(5, 450, 406);
            f[5].AgregarPunto(6, 300, 500);
            f[5].AgregarPunto(7, 354, 346);
            f[5].AgregarPunto(8, 200, 250);
            f[5].AgregarPunto(9, 388, 250);
            f[5].AgregarPunto(10, 450, 80);          
            f[5].FinalizarFigura(11);
            f[5].EditarColor("#BDBD20");


            // Figura 7 - Elefante      
            f[6] = new ClaseFigura();
            f[6].EditarNombre("Elefante");
            f[6].EditarCantPuntos(11);
            f[6].AgregarPunto(0, 240, 150);
            f[6].AgregarPunto(1, 370, 170);
            f[6].AgregarPunto(2, 460, 230);
            f[6].AgregarPunto(3, 560, 220);
            f[6].AgregarPunto(4, 660, 370);
            f[6].AgregarPunto(5, 800, 370);
            f[6].AgregarPunto(6, 800, 380);
            f[6].AgregarPunto(7, 40, 380);
            f[6].AgregarPunto(8, 40, 370);
            f[6].AgregarPunto(9, 160, 370);
            f[6].AgregarPunto(10, 190, 230);
            f[6].AgregarPunto(11, 240, 150);

            f[6].FinalizarFigura(12);
            f[6].EditarColor("#51320D");


            // Figura 8 - Dignidad      
            f[7] = new ClaseFigura();
            f[7].EditarNombre("Dignidad");
            f[7].EditarCantPuntos(14);
            f[7].AgregarPunto(0, 250, 240);
            f[7].AgregarPunto(1, 300, 100);
            f[7].AgregarPunto(2, 380, 70);
            f[7].AgregarPunto(3, 530, 80);
            f[7].AgregarPunto(4, 558, 120);
            f[7].AgregarPunto(5, 563, 200);
            f[7].AgregarPunto(6, 533, 290);
            f[7].AgregarPunto(7, 570, 420);
            f[7].AgregarPunto(8, 510, 470);
            f[7].AgregarPunto(9, 400, 500);
            f[7].AgregarPunto(10, 322, 450);
            f[7].AgregarPunto(11, 312, 312);
            f[7].AgregarPunto(12, 328, 400);
            f[7].AgregarPunto(13, 420, 420);
            f[7].AgregarPunto(14, 250, 240);
            f[7].FinalizarFigura(15);
            f[7].EditarColor("#E2D3C0");


        }


        private void polylineObj_MouseEnter(object sender, MouseEventArgs e)
        {
            SonidoRosa.LoadedBehavior = MediaState.Play;
            SonidoRosa.UnloadedBehavior = MediaState.Close;
            estado = 1;
        }

        private void polylineObj_MouseLeave(object sender, MouseEventArgs e)
        {
            SonidoRosa.LoadedBehavior = MediaState.Stop;
            SonidoRosa.UnloadedBehavior = MediaState.Close;
            estado = 0;
        }

          public void creacionDeCarpetas()
            {  if (Config.sujeto==1)
              {
                Directory.CreateDirectory(Config.directorio);   //Crea la carpeta principal de la prueba
                Directory.CreateDirectory(Config.directorio + "\\Suj1"); // Creamos la carpeta de ambos sujetos
                Directory.CreateDirectory(Config.directorio + "\\Suj2");
                Directory.CreateDirectory(Config.directorio + "\\Suj1\\Recorridos");  // Creamos Carpeta de Recorridos
                Directory.CreateDirectory(Config.directorio + "\\Suj2\\Recorridos");
              }        
            }

        private void Tmouse_Tick(object Sender, EventArgs e)
          {
            if (Config.Familiarizacion == false)
                archivoRecorr.WriteLine(ticks++ + ";" + Mouse.GetPosition(null).X + ";" + Mouse.GetPosition(null).Y + ";" + estado);            
          }

        private void SegunderoInterfaz(object Sender, EventArgs e)
        {
            Panel_TExplor.Content = "Tiempo Exploración: " + (interfaz_segundos+2);
            interfaz_segundos++;
        }

      

        void AsignaciondePreguntas()
        {
            Pregunta[0] = new ClasePregunta();
            Pregunta[0].numero = 1;
            Pregunta[0].nombre = "DificultadReconocimiento";
            Pregunta[0].tipo = false;
            Pregunta[0].sonido = new System.Media.SoundPlayer(Properties.Resources.Preg1);

            Pregunta[1] = new ClasePregunta();
            Pregunta[1].numero = 2;
            Pregunta[1].nombre = "SintioFeeling";
            Pregunta[1].tipo = true;
            Pregunta[1].sonido = new System.Media.SoundPlayer(Properties.Resources.Preg2);

            Pregunta[2] = new ClasePregunta();
            Pregunta[2].numero = 3;
            Pregunta[2].nombre = "DificultadFeeling";
            Pregunta[2].tipo = false;
            Pregunta[2].sonido = new System.Media.SoundPlayer(Properties.Resources.Preg3);
        }

        private void Button_Salir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("La prueba fue exitosa, todos los datos han sido guardados. El programa se cerrará.", "PRUEBA EXITOSA", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Environment.Exit(1); 
        }


    }     
 

}

