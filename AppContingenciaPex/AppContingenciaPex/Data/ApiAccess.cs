using AppContingenciaPex.Entity;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppContingenciaPex.Data
{
    public class ApiAccess
    {
        public async Task<String> Service(string url)
        {
            string result;
            try
            {
                using (var client = new HttpClient(new NativeMessageHandler()))
                {
                    client.BaseAddress = new Uri(Constantes.RestUrl);

                    var response = await client.GetAsync(url);

                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception EX)
            {
                var error = EX.Message;
                return null;
            }


            try
            {
                JsonConvert.DeserializeObject(result);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<String> ServicePost(string url, object ks)
        {
            string result;
            try
            {

                var jsonreq = JsonConvert.SerializeObject(ks);
                var conten = new StringContent(jsonreq, Encoding.UTF8, "text/json");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constantes.RestUrl);
                    var response = await client.PostAsync(url, conten);
                    result = response.Content.ReadAsStringAsync().Result;
                }



            }
            catch (Exception)
            {
                return null;
            }

            try
            {
                JsonConvert.DeserializeObject(result);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        //

        public async Task<Cliente> clientelocal (string placa)
        {
            if (placa=="ANB082")
            {
                var clie = new Cliente();

                clie.placa = "ANB082";
                clie.cliente = "Oswaldo Espinoza Coral";
                clie.saldo = 20.00;
                clie.tipo = "PREPAGO";
                clie.estado = "INACTIVO";

                return clie;
            }

            return null;
        }


        public async Task<Contingencia> contilocal(string placa,string monto,string agente,string titulo)
        {
            if (placa == "ANB082")
            {
                var cont = new Contingencia();

                cont.titulo =titulo;
                cont.placa = placa;
                cont.codigo = "1002";
                cont.fecha = "2018-05-06 23:11:10";
                cont.agente = agente;
                cont.monto = "S/ "+ monto;

                return cont;
            }

            return null;
        }

        //public async Task<Respuesta> EnviarComprobante(Comprobante Comp)
        //{
        //    string url = string.Format("/api/comprobante");
        //    string result = await ServicePost(url, Comp);

        //    return JsonConvert.DeserializeObject<Respuesta>(result);

        //}

        public async Task<Cliente> ObtenerCliente(string placa)
        {
            string url = string.Format(Constantes.CarpUrl + "/cliente?placa={0}", placa);
            string result = await Service(url);
            try
            {
                return JsonConvert.DeserializeObject<Cliente>(result);
            }
            catch (Exception ex)
            {
                return new Cliente();
            }
           
        }

        public async Task<Contingencia> InsertaContingencia(string placa,string idmidia,string monto,string agente,string titulo)
        {
            string url = string.Format(Constantes.CarpUrl + "/contingencia?placa={0}&idmidia={1}&monto={2}&agente={3}&titulo={4}", placa,idmidia,monto,agente,titulo);
            string result = await Service(url);
            try
            {
                return JsonConvert.DeserializeObject<Contingencia>(result);
            }
            catch (Exception ex)
            {
                return new Contingencia();
            }

        }


        //public async Task<Respuesta> Compuesta(List<Juntar> Compu)
        //{
        //    string url = string.Format("/api/Compuesta");
        //    string result = await ServicePost(url, Compu);

        //    return JsonConvert.DeserializeObject<Respuesta>(result);

        //}



    }
}
