using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    class Azar
    {
            private int nfig;
            private int nrepetic;
            private int mismafigura = 2;
            private int diferenciamax = 3;
            private int[] historial;
            private int[] contador;
            private Random r;  
        

        public Azar(int a, int b)
        {
            nfig = a;
            nrepetic = b;

            historial= new int[nfig * nrepetic];
            contador = new int[nfig];

            r = new Random(DateTime.Now.Millisecond);     // hacemos c+1 para que cada sujeto tenga semilla diferente
        }


        public int[] etapas()
          {
              for (int n = 0; n < nfig*nrepetic; n++)
              {
                  int f;
                  f = genera();

                  if (n >= mismafigura)    // Primer filtro -> No se repite una figura mas de "mismafigura" veces
                  {
                      int aux = 1;
                      while (aux == 1)
                      {
                          for (int i = n - mismafigura; i < n; i++)
                          {
                              if (historial[i] == f)
                                  aux *= 1;
                              else
                                  aux *= 0;
                          }
                          if (aux == 1)
                              f = genera();
                          else
                              aux = 0;
                      }
                  }



                  contador[f]++;                                           // Segundo Filtro
                  while (contador.Max() - contador.Min() >= diferenciamax)
                  {
                      contador[f]--;
                      f = genera();
                      contador[f]++;
                  }


                  while (contador[f] == nrepetic + 1)              // Último filtro, todas las figuras se repiten igual cantidad de veces
                  {
                      contador[f]--;
                      f = genera();
                      contador[f]++;
                  }


                  historial[n] = f;          // Anade dato al historial     
              }
                   return historial;

          }

         public int genera()
          {
            int ran;
            ran = (int)r.Next(0, nfig);   //Crea variable azar, n=0 -> CUADRADO,  n=1 -> CIRCULO, n=2 -> TRIANGULO 

            return (ran);
          }
    }
}
