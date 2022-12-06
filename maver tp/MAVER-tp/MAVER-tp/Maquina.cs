using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVER_tp
{
    class Maquina
    {
        Dictionary<double, int> Billetes_Monedas; //denominacion - cantidad
        Dictionary<string, double> Productos;
        double vuelto;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="productos"></param>
        public Maquina(Dictionary<string, double> productos)
        {
            this.Billetes_Monedas = new Dictionary<double, int>(); //inicializo el dic
            Billetes_Monedas.Add(100, 3);
            Billetes_Monedas.Add(50, 3);
            Billetes_Monedas.Add(20, 3);
            Billetes_Monedas.Add(10, 3);
            Billetes_Monedas.Add(5, 10);
            Billetes_Monedas.Add(2, 10);
            Billetes_Monedas.Add(1, 10);
            Billetes_Monedas.Add(0.5, 10);
            Billetes_Monedas.Add(0.25, 10);

            this.Productos = productos;
            this.vuelto = 0;
        }
        /// <summary>
        /// verifica que se cumplan las condiciones de los billetes
        /// </summary>
        /// <param name="cliente"></param>
        public void Recibir_Cliente(Cliente cliente)
        {
            foreach (var item in cliente.Pago_con)
            {
                if (Billetes_Monedas[item] < 5 && item >= 10)
                    Billetes_Monedas[item]++; //le sumo uno a la cantidad de billetes que me da el cliente
                else if (Billetes_Monedas[item] < 15 && item <= 5)
                    Billetes_Monedas[item]++;
                else
                    throw new Exception("No aceptan mas billetes, no podemos atenderte.");
            }
            Calcular_Total_A_Pagar(cliente);
        }
        /// <summary>
        /// calcula cuanto tiene qe pagar el cliente
        /// </summary>
        /// <param name="cliente"></param>
        public void Calcular_Total_A_Pagar(Cliente cliente)
        {//busco el precio de los productos en el dic y los sumo
            double total = 0;
            for (int i = 0; i < cliente.Que_lleva.Count(); i++)
            {
                if (Productos.ContainsKey(cliente.Que_lleva[i])) //si existe lo sumo
                    total += Productos[cliente.Que_lleva[i]];
            }
            Vuelto_Greedy(total, cliente);
            this.vuelto = total;
        }
        /// <summary>
        /// Greedy 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public void Vuelto_Greedy(double total, Cliente cliente)
        {
            double vuelto = cliente.Total() - total; //calculo cuanto le tengo que devolver
            List<double> Lo_que_le_voy_a_dar = new List<double>();
            while (vuelto > 0)
            {
                foreach (var item in Billetes_Monedas.ToList())
                {
                    if (item.Key <= vuelto && item.Value > 1)
                    {
                        Lo_que_le_voy_a_dar.Add(item.Key);
                        Billetes_Monedas[item.Key] = item.Value - 1;
                        vuelto = vuelto - item.Key; //actualizo el vuelto
                        break;
                    }
                }
            }
            Console.WriteLine("El total a pagar de " + cliente.nombre + " es: " + total);
            Console.WriteLine("Paga con: " + cliente.Total());
            Console.WriteLine("El vuelto es: ");
            foreach (var item in Lo_que_le_voy_a_dar)
            {
                Console.WriteLine(item);
                //Console.ReadLine();
            }

        }
        public void Vuelto_Dinamico(Cliente cliente)
        {
            this.vuelto = 10;
            List<int> Billetes_ordenados = new List<int>();
            List<int> Cant_Billetes = new List<int>();
            Billetes_ordenados = Ordenar_Dic();
            int[,] matriz_dinamica = new int[Billetes_ordenados.Count(), Convert.ToInt32(this.vuelto*100)]; //multiplico *100 para poder poner la matriz en int
            int i, j;

            for(i=0; i< Billetes_ordenados.Count(); i++)
            {
                for (j = 0; j < Convert.ToInt32(this.vuelto * 100); j++)
                {
                    matriz_dinamica[0, j] = int.MaxValue-1;
                    matriz_dinamica[i, 0] = 0;
                    if(Billetes_ordenados[i]>j && i>0) //si el "billete es mayor al vuelto"
                    {
                        matriz_dinamica[i, j] = matriz_dinamica[i - 1, j]; 
                    }
                    else if(i>0)
                    {
                        if (matriz_dinamica[i - 1, j] > matriz_dinamica[i, j - Billetes_ordenados[i]] + 1)
                        {
                            matriz_dinamica[i, j] = matriz_dinamica[i, j - Billetes_ordenados[i]] + 1;
                        }
                        else
                            matriz_dinamica[i, j] = matriz_dinamica[i - 1, j];
                    }
                }
            }

            for (int k = 0; k < Billetes_ordenados.Count(); k++)
            {
                for (int s = 0; s < Convert.ToInt32(this.vuelto * 100); s++)
                {
                    Console.Write(matriz_dinamica[k, s] + " ");
                }
                Console.WriteLine();
            }

            for (i=0;i< Billetes_ordenados.Count(); i++)
                Cant_Billetes.Add(0);
            
            i = Billetes_ordenados.Count()-1;
            j = Convert.ToInt32(this.vuelto * 100)-1;
            
            while(j>0) //me copio en una lista nueva la cantidad de monedas que use
            {
                if (i > 1 && matriz_dinamica[i, j] == matriz_dinamica[i - 1, j])
                    i--;
                else
                {
                    Cant_Billetes[i]++;
                    j= j - Billetes_ordenados[i];
                }
            }

        }
        /// <summary>
        /// Ordena los billetes de menor a mayor
        /// </summary>
        /// <returns></returns>
        public List<int> Ordenar_Dic()
        {
            int cant = this.Billetes_Monedas.Count();
            List<int> Billetes_ordenados = new List<int>();
            foreach (var item in Billetes_Monedas)
            {
                if(item.Value > 1)
                    Billetes_ordenados.Add(Convert.ToInt32(item.Key*100));
            }
            for (int n = 0; n < cant; n++)
            {
                for (int i = 0; i < cant - 1; i++)
                {
                    if (Billetes_ordenados[i] < Billetes_ordenados[i + 1])
                    {
                        int aux = Billetes_ordenados[i];
                        Billetes_ordenados[i] = Billetes_ordenados[i + 1];
                        Billetes_ordenados[i + 1] = aux;
                    }
                }
            }
            return Billetes_ordenados;

        }

    }
    
}
