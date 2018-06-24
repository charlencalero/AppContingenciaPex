using System;
using System.Collections.Generic;
using AppContingenciaPex.Data;
using Xamarin.Forms;

namespace AppContingenciaPex.Pages
{
    public partial class BuscarPg : ContentPage
    {
        ApiAccess api = new ApiAccess();
        double saldo = 0;
        Double montopeaje = 5.3;
        string agente = "MAXIMO HUAMAN";

        public BuscarPg()
        {
            InitializeComponent();
        }

        private void Limpiar()
        {
            LabeValorCliente.Text = "";
            LabeValorTipo.Text = "";
            LabeValorSaldo.Text = "";
            LabeValorEstado.Text = "";
            LabeValorPlaca.Text = "";
            TextPlaca.Text = "";
        }

        private async void buscarcplaca (string placa)
        {
            ButtBuscar.IsEnabled = false;

            Limpiar();

            var clie = await api.clientelocal(placa.ToUpper());
         
            ButtBuscar.IsEnabled = true;
           

            if(clie==null)
            {
             await   DisplayAlert("SYSTEM", "No se encontraron Datos", "ok");

                return;
            }

            LabeValorCliente.Text = clie.cliente;
            LabeValorTipo.Text = clie.tipo;
            saldo = clie.saldo;
            LabeValorSaldo.Text = "S/ " + clie.saldo.ToString();
            LabeValorEstado.Text = clie.estado;
            LabeValorPlaca.Text = clie.placa;

        }

     private   async void  buttpase_Clicked(object sender, System.EventArgs e)
        {
         


            if (saldo<montopeaje)
            {
               await DisplayAlert("SYSTEM", "Lo Sentimos su Saldo Actual es muy Bajo", "ok");
            }
            else
            {
                buttpase.IsEnabled = false;

                // enviar al webservice
                var conti = await api.contilocal(LabeValorPlaca.Text,montopeaje.ToString(),agente,"COBRO REGISTRADO");


                Limpiar();
                buttpase.IsEnabled = true;

                if (conti == null)
                {
                    await DisplayAlert("SYSTEM", "No se encontraron Datos", "ok");
                    return;
                }

                // retornar parametros
             await   Navigation.PushAsync(new ImprimirPg(conti));
            }
         }

        public async void buttfuga_Clicked(object sender, System.EventArgs e)
        {


            if (LabeValorPlaca.Text=="")
            {
                await DisplayAlert("SYSTEM", "No se Ubico ninguna Placa", "ok");
            }
            else
            {
                buttfuga.IsEnabled = false;
                // enviar al webservice
                var conti = await api.contilocal(LabeValorPlaca.Text, montopeaje.ToString(), agente, "FUGA REGISTRADO");

                buttfuga.IsEnabled = true;
                Limpiar();

                if (conti == null)
                {
                    await DisplayAlert("SYSTEM", "No se encontraron Datos", "ok");
                    return;
                }

                // retornar parametros
              await  Navigation.PushAsync(new ImprimirPg(conti));
            }
        }
        public async void buttpromesa_Clicked(object sender, System.EventArgs e)
        {
            if (montopeaje > saldo && LabeValorPlaca.Text != "")
            {
              await  DisplayAlert("SYSTEM", "Lo Sentimos su Saldo no es muy Bajo, realize un Pase", "ok");
            }
            else
            {
                buttpromesa.IsEnabled = false;
                // enviar al webservice
                var conti = await api.contilocal(LabeValorPlaca.Text, montopeaje.ToString(), agente, "PROMESA REGISTRADO");
              
                buttpromesa.IsEnabled = true;
                Limpiar();

                if (conti == null)
                {
                    await DisplayAlert("SYSTEM", "No se encontraron Datos", "ok");
                    return;
                }

                // retornar parametros
              await  Navigation.PushAsync(new ImprimirPg(conti));
            }
        }

        void buttbuscar_Clicked(object sender, System.EventArgs e)
        {
            
            buscarcplaca(TextPlaca.Text);
        }
    }
}
