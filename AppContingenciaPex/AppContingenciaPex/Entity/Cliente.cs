using System;
namespace AppContingenciaPex.Entity
{
    public class Cliente
    {
        public Cliente()
        {
        }

        public Cliente(string placa, string cliente, double saldo, string tipo, string estado,string idmidia)
        {
            this.placa = placa;
            this.cliente = cliente;
            this.saldo = saldo;
            this.tipo = tipo;
            this.estado = estado;
            this.idmidia = idmidia;
        }

        public string placa { get; set; }
        public string cliente { get; set; }
        public double saldo { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public string idmidia { get; set; }


    }
}
