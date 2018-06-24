using System;
namespace AppContingenciaPex.Entity
{
    public class Contingencia
    {
        public Contingencia()
        {
        }

        public Contingencia(string  titulo,string codigo, string monto, string fecha, string agente, string placa)
        {
            this.codigo = codigo;
            this.monto = monto;
            this.fecha = fecha;
            this.agente = agente;
            this.placa = placa;
            this.titulo = titulo;
        }

        public string titulo { get; set; }
        public string codigo { get; set; }
        public string monto { get; set; }
        public string fecha { get; set; }
        public string agente { get; set; }
        public string placa { get; set; }

    }
}
