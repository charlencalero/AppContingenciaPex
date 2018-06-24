using System;
using System.Collections.Generic;
using AppContingenciaPex.Entity;
using Xamarin.Forms;

namespace AppContingenciaPex.Pages
{
    public partial class ImprimirPg : ContentPage
    {
        public ImprimirPg()
        {
            InitializeComponent();
        }

        public ImprimirPg(Contingencia conti)
        {
            InitializeComponent();

            LabeValorFecha.Text = conti.fecha;
            LabeValorPeaje.Text = conti.monto;
            LabeValorPlaca.Text = conti.placa;
            LabeValorCodigo.Text = conti.codigo;
            LabeValorTitulo.Text ="* " + conti.titulo+" *";
            LabeValorAgente.Text = conti.agente;

        }

        void buttimpr_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        void buttregre_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
