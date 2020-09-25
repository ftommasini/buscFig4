using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    public class Prueba
    {
     //   public string nombre;
    //    public int edad;

        public string NombrePrueba;
        public string[] nom = new string[2];
        public int[] edad = new int[2];

        public Ensayo[] en, famil;

        public Prueba(int cant, string s)
            {

                famil = new Ensayo[10];                 //  Creamos solo 10 ensayos para FAMILIARIZACIÓN
                for (int i = 0; i < 10; i++)          //Inicializamos todos los ensayos de familiarizacion
                {
                    famil[i] = new Ensayo();
                }
                en = new Ensayo[cant];                  //  Creamos slos ensayos que corresponden a la prueba
                for (int i = 0; i < cant; i++)          //Inicializamos todos los ensayos
                {
                    en[i] = new Ensayo();
                }

                NombrePrueba = s;
                

                
            }
    }
}
