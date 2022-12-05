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
        public Maquina(Dictionary<string,double> productos)
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
        }
        public void Recibir_Cliente(Cliente cliente)
        {
            foreach (var item in cliente.Pago_con)
            {
                if (Billetes_Monedas[item] < 5 && item>=10)
                    Billetes_Monedas[item]++; //le sumo uno a la cantidad de billetes que me da el cliente
                else if(Billetes_Monedas[item] < 15 && item <=5)
                    Billetes_Monedas[item]++;
                else
                    throw new Exception("No aceptan mas billetes, no podemos atenderte.");
            }
            Calcular_Total_A_Pagar(cliente);
        }
        public void Calcular_Total_A_Pagar(Cliente cliente)
        {//busco el precio de los productos en el dic y los sumo
            double total=0;
            for (int i = 0; i < cliente.Que_lleva.Count(); i++)
            {
                if(Productos.ContainsKey(cliente.Que_lleva[i])) //si existe lo sumo
                total += Productos[cliente.Que_lleva[i]];
            }
            Vuelto_Greedy(total, cliente);
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
            Console.WriteLine("El total a pagar de "  + cliente.nombre + " es: " + total);
            Console.WriteLine("Paga con: " + cliente.Total());
            Console.WriteLine("El vuelto es: ");
            foreach (var item in Lo_que_le_voy_a_dar)
            {
                Console.WriteLine(item);
                Console.ReadLine();
            }
            //return Lo_que_le_voy_a_dar;
        }
    }
}
