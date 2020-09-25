using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    public class Ensayo
    {
        public int ensayo;
        public string ensayo_string;                //Numero de Ensayo corregido
        public int  figuraexplorada, figuraelegida;
        public double tdeexplor, tderespuesta;
        public int resultado; //1 si es correcto, 0 si es incorrecto

        public Ensayo()
            {
                tdeexplor = 0;
            }

        


        public void guardaenBasedeDatos()
            { }
    }
}
