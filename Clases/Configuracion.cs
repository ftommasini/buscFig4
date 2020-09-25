using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    public class Configuracion
    {
        public string directorio;       //Direccion de guardado de los datos - Por defecto Server del CINTRA

        public bool tlimite;       //TRUE tiene un limite de tiempo - FALSE no limitado por tiempo
        public int valorlimite;    // Valor del limite de tiempo

        public bool confOrden;          // TRUE Elección de Orden - FALSE se ejecuta en azar
        public bool Familiarizacion;       // FALSE Familiarizacion desact - TRUE Familiarizacon activ 
        public bool boolPreguntas;          // True Activadas - False Desactivadas

        public bool FeedbackRespuesta;          // Feedback sonoro Respuesta
        public bool FeedbackEstado;           // Feedback sonoro Respuesta

        public int cantFig;        //Cantidad de Figuras
        public int cantRep;   //Cantidad de Repeticiones x Figura


        public int[] orden;                 // Cuando se defina repeticiones y cant Fig debemos hacer    ensayo_suj = new int[repeticiones*cantFig];
        public int[] ordenFamil;                 // Cuando se defina repeticiones y cant Fig debemos hacer    ensayo_suj = new int[repeticiones*cantFig];

        public int sujeto;



        public Configuracion()
               {
                    //directorio = @"\\\\CINTRA-M029\\DatosBuscFiguras";

                    tlimite = true; valorlimite = 60;
                    confOrden = false;
                    FeedbackRespuesta = false;
                    FeedbackEstado = true;
                    boolPreguntas = true;
                    sujeto = 1;

                    cantRep = 4;
                    cantFig = 4;
                    Familiarizacion = false;   // Inicia con Familiarizacion desactivado
               }

        public void CrearVectorOrden()
        {
            orden = new int[cantFig*cantRep];
            ordenFamil = new int[10];
        }
  

        

      


    
    }

       
}
