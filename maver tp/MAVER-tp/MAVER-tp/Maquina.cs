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
        public double vuelto;
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
            //Vuelto_Greedy(total, cliente);
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
            //this.vuelto = 49.5
            List<int> Billetes_ordenados = new List<int>();
            List<int> Cant_Billetes = new List<int>();
            List<int> Billetes_value = new List<int>();
            double total = 0;
            int i, j;
            for (i = 0; i < cliente.Que_lleva.Count(); i++)
            {
                if (Productos.ContainsKey(cliente.Que_lleva[i])) //si existe lo sumo
                    total += Productos[cliente.Que_lleva[i]];
            }
            this.vuelto = cliente.Total() - total;
            Billetes_ordenados = Ordenar_Dic(ref Billetes_value); 
            int[,] matriz_dinamica = new int[Billetes_ordenados.Count(), Convert.ToInt32(this.vuelto*100)+1]; //multiplico *100 para poder poner la matriz en int
          

            for(i=0; i< Billetes_ordenados.Count(); i++)
            {
                for (j = 0; j < Convert.ToInt32(this.vuelto * 100)+1; j++)
                {
                    matriz_dinamica[0, j] = int.MaxValue-1;
                    matriz_dinamica[i, 0] = 0;
                    if (Billetes_value[i] > 0)
                    {
                        if (Billetes_ordenados[i] > j && i > 0) //si el "billete es mayor al vuelto"
                        {
                            matriz_dinamica[i, j] = matriz_dinamica[i - 1, j];
                        }
                        else if (i > 0)
                        {
                            matriz_dinamica[i, j] = Math.Min(matriz_dinamica[i - 1, j], matriz_dinamica[i, j - Billetes_ordenados[i]] + 1);
                        }
                    }
                    else
                        matriz_dinamica[i, j] = matriz_dinamica[i - 1, j];
                }
            }


            for (i=0;i< Billetes_ordenados.Count(); i++)
                Cant_Billetes.Add(0);
            
            i = Billetes_ordenados.Count()-1;
            j = Convert.ToInt32(this.vuelto * 100);
            
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

            foreach(var item in Billetes_Monedas)
            {
                for(i= Cant_Billetes.Count()-1; i>0;i--)
                {
                    if(item.Key*100 == Billetes_ordenados[i])
                    {
                        if (item.Value - Cant_Billetes[i] < 1 && i != 0) //si lo que tengo menos lo que me dio la matriz es menor a 1
                        {
                            Cant_Billetes[i - 1]+= Convert.ToInt32((Cant_Billetes[i] - item.Value)/2); //le sumo la mitad del de arriba
                            Cant_Billetes[i]-= (Cant_Billetes[i] - item.Value);
                        }
                    }
                }
            }

            
            Console.WriteLine("El total a pagar de {0} es: {1} ", cliente.nombre, total);
            Console.WriteLine("Paga con: " + cliente.Total());
            Console.WriteLine("El vuelto es: ");
            double aux = 0;

            for (i = 0; i < Cant_Billetes.Count(); i++)
            {
                if(Cant_Billetes[i]!=0)
                {
                    aux = Convert.ToDouble(Billetes_ordenados[i]) / 100;
                    if (i<4)
                        Console.WriteLine("{0} billetes de ${1} ", Cant_Billetes[i], aux.ToString("N2"));
                    else
                        Console.WriteLine("{0} monedas de ${1} ", Cant_Billetes[i], aux.ToString("N2"));


                }


            }
        }
        /// <summary>
        /// Ordena los billetes de menor a mayor
        /// </summary>
        /// <returns></returns>
        public List<int> Ordenar_Dic(ref List<int> Billetes_value)
        {
            int cant = this.Billetes_Monedas.Count();
            List<int> Billetes_ordenados = new List<int>();

            foreach (var item in Billetes_Monedas)
            {
                if(item.Value > 1)
                {
                    Billetes_ordenados.Add(Convert.ToInt32(item.Key * 100));

                }
            }

            Billetes_ordenados.OrderBy(num=>num);
            

            for(int i=0;i< Billetes_ordenados.Count(); i++) //me copio los value con una lista nueva
            {
                foreach (var item in Billetes_Monedas)
                {
                    if (Billetes_ordenados[i]==item.Key*100)
                    {
                        Billetes_value.Add(item.Value-1); //para tener 1 siempre
                        break;
                    }
                }
                    
            }
            
                return Billetes_ordenados;

        }

    }

}
