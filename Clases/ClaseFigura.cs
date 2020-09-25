using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1

{
    public class ClaseFigura
    {
        public string nombre;
        public int CantPuntos, color;

        public System.Windows.Point[] punto = new System.Windows.Point[20];

        public PointCollection PuntosdibujoOriginal = new PointCollection();
        public Polyline dibujoOriginal = new Polyline();

        private PointCollection PuntosdibujoEdicion = new PointCollection();
        public Polyline dibujoEdicion = new Polyline();

        private PointCollection PuntosdibujoMini = new PointCollection();
        public Polyline dibujoMini = new Polyline();


        public ClaseFigura()
            {
              // Definiciones de Características de la Figura
                dibujoOriginal.Stroke = System.Windows.Media.Brushes.Black;
                dibujoOriginal.StrokeThickness = 2;
                dibujoOriginal.FillRule = FillRule.EvenOdd;

                dibujoEdicion.Stroke = System.Windows.Media.Brushes.Black;
                dibujoEdicion.StrokeThickness = 1.5;
                dibujoEdicion.FillRule = FillRule.EvenOdd;

                dibujoMini.Stroke = System.Windows.Media.Brushes.Black;
                dibujoMini.StrokeThickness = 1;
                dibujoMini.FillRule = FillRule.EvenOdd;   
            }


        public void EditarNombre(string n)
            {
                nombre = n;
            }

        public void AgregarPunto(int i, double x, double y)
        {
            punto[i] = new System.Windows.Point(x, y);


            PuntosdibujoOriginal.Add(new System.Windows.Point(x,y));

            PuntosdibujoEdicion.Add(new System.Windows.Point(x/2, y/2));

            PuntosdibujoMini.Add(new System.Windows.Point(x/10, y/10));
        }

        public void BlanquearPuntos()
            {
                PuntosdibujoOriginal.Clear();
                PuntosdibujoEdicion.Clear();
                PuntosdibujoMini.Clear();
            }

        public void FinalizarFigura(int i)
            {    
            
                    //Puntos de Cierre
                punto[i] = punto[0]; 

                PuntosdibujoOriginal.Add(punto[0]);

                PuntosdibujoEdicion.Add(new System.Windows.Point(punto[0].X/2, punto[0].Y/2));      // Aca debemos editar el punto ya que esta en su tamaño original

                PuntosdibujoMini.Add(new System.Windows.Point(punto[0].X/10, punto[0].Y/10));


                    // Asigna Puntos a la Figura
                
                dibujoOriginal.Points = PuntosdibujoOriginal;

                dibujoEdicion.Points = PuntosdibujoEdicion;

                dibujoMini.Points = PuntosdibujoMini; 
            }

        public void EditarColor(string s) //"#C5D50019"
            {
                var converter = new System.Windows.Media.BrushConverter();  //Variable de color
                dibujoOriginal.Fill = (System.Windows.Media.Brush)converter.ConvertFromString(s);
                dibujoEdicion.Fill = (System.Windows.Media.Brush)converter.ConvertFromString(s);
                dibujoMini.Fill = (System.Windows.Media.Brush)converter.ConvertFromString(s);
            }
        public void EditarCantPuntos(int c)
            {
                CantPuntos = c;
            }
        public int returnCantPuntos()
            {
                return CantPuntos;
            }

        public string returnNombre()
        {
            return nombre;
        }
    }
}
