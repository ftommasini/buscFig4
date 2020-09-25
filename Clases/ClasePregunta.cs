using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1
{
    public class ClasePregunta
    {
        public int numero;
        public string nombre;
        public System.Media.SoundPlayer sonido;
        public bool tipo; // tipo true es "verdadero falso". tipo false es del 1 al 5.
        public bool respuestaBool;
        public int respuestaInt;

        public ClasePregunta()
        {
        }
    }
}
