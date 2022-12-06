using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVER_tp
{
    class Cliente 
    {
        public string nombre;
        public List<double> Pago_con;
        public List<string> Que_lleva;
        
        public Cliente(List<double> pago_con, List<string> que_lleva, string nombre)
        {
            this.nombre = nombre;
            this.Pago_con = pago_con;
            this.Que_lleva = que_lleva;
        }
        /// <summary>
        /// calcula con cuanta plata pago
        /// </summary>
        /// <returns></returns>
        public double Total()
        {
            double total = 0;
            for (int i = 0; i < Pago_con.Count(); i++)
                total += Pago_con[i];
            return total;
        }
        

    }
}
