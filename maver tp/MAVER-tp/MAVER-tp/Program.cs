using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MAVER_tp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Dictionary<string, double> productos = new Dictionary<string, double>();

            List<double> Lista_billetes1= new List<double>(); //me creo una lista de billetes
            List<double> Lista_billetes2 = new List<double>();
            List<double> Lista_billetes3 = new List<double>();
            List<double> Lista_billetes4 = new List<double>();

            List<string> Que_lleva_1 = new List<string>();
            List<string> Que_lleva_2 = new List<string>();
            List<string> Que_lleva_3 = new List<string>();
            List<string> Que_lleva_4 = new List<string>();


            productos.Add("coca", 30); //inicializo el dic
            productos.Add("fanta", 20);
            productos.Add("agua", 10);
            productos.Add("sprite", 30);
            productos.Add("jugo", 15);
            productos.Add("papas", 13);
            productos.Add("doritos", 50);
            productos.Add("galletitas", 7.5);
            productos.Add("m&m", 3.25);
            productos.Add("palitos", 3);

            Lista_billetes1.Add(100); // total a pagar = 123
            Lista_billetes1.Add(20); // pago con = 125
            Lista_billetes1.Add(5);

            Lista_billetes2.Add(100); //  total a pagar = 50
                                     //   pago con = 100

            Lista_billetes3.Add(50); //   total a pagar = 220.5 
            Lista_billetes3.Add(50); //   pago con = 250
            Lista_billetes3.Add(50); 
            Lista_billetes3.Add(50);
            Lista_billetes3.Add(50);

            Lista_billetes4.Add(5);  // total a pagar = 3.25
                                     // pago con = 5

            Que_lleva_1.Add("doritos"); //lo que leva sol, suma 123
            Que_lleva_1.Add("doritos");
            Que_lleva_1.Add("fanta");
            Que_lleva_1.Add("palitos");

            Que_lleva_2.Add("doritos"); //lo que lleva valen, suma = 50

            Que_lleva_3.Add("galletitas"); //lo que lleva ro, suma 220.5
            Que_lleva_3.Add("papas");
            Que_lleva_3.Add("doritos");
            Que_lleva_3.Add("doritos");
            Que_lleva_3.Add("coca");
            Que_lleva_3.Add("coca");
            Que_lleva_3.Add("coca");
            Que_lleva_3.Add("agua");

            Que_lleva_4.Add("m&m"); //lo que lleva fabricio, suma 3.25

            Cliente Sol = new Cliente(Lista_billetes1, Que_lleva_1, "Sol"); //me creo mis clientes con lo que llevan y como pagan
            Cliente Valen = new Cliente(Lista_billetes2, Que_lleva_2, "Valentina");
            Cliente Rosario = new Cliente(Lista_billetes3, Que_lleva_3, "Rosario");
            Cliente Fabricio = new Cliente(Lista_billetes4, Que_lleva_4, "Fabricio");

            Maquina maquina = new Maquina(productos);

            try
            {
                maquina.Recibir_Cliente(Sol);
            } catch(Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Greedy(maquina.vuelto, Sol);
            try
            {
                maquina.Recibir_Cliente(Sol);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Dinamico(Sol);




            try
            {
                maquina.Recibir_Cliente(Valen);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Greedy(maquina.vuelto, Valen);

            try
            {
                maquina.Recibir_Cliente(Valen);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Dinamico(Valen);



            try
            {
                maquina.Recibir_Cliente(Rosario); //a rosario le va a salir la exeppcion de los billetes
              
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Greedy(maquina.vuelto, Rosario);

            try
            {
                maquina.Recibir_Cliente(Rosario); //a rosario le va a salir la exeppcion de los billetes

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Dinamico(Rosario);



            try
            {
                maquina.Recibir_Cliente(Fabricio);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Greedy(maquina.vuelto, Fabricio);
            try
            {
                maquina.Recibir_Cliente(Fabricio);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            maquina.Vuelto_Dinamico(Fabricio);



            Console.ReadLine();
            Console.Clear();
            //------------------------ FIN GREEDY ------------------------------




















            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
