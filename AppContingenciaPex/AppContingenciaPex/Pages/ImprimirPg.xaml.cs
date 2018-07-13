using AppContingenciaPex.Service;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;

using System;
using System.Collections.Generic;
using AppContingenciaPex.Entity;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace AppContingenciaPex.Pages
{
    public partial class ImprimirPg : ContentPage
    {

        private MPosControllerPrinter _printer;
        private MposConnectionInformation _connectionInfo;
        private static SemaphoreSlim _printSemaphore = new SemaphoreSlim(1, 1);

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

            LabeValorTitulo.Text = "* " + conti.titulo + " *";
            LabeValorAgente.Text = conti.agente;

        }

        void buttimpr_Clicked(object sender, System.EventArgs e)
        {
            buttimpr.IsEnabled = false;
            Imprimir();
            Navigation.PopAsync();
        }

        void buttregre_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Imprimir()
        {
             conexion();

            _printer = await OpenPrinterService(_connectionInfo);

            if (_printer == null)
                return;

            try
            {
                await _printSemaphore.WaitAsync();

                // note : Page mode and transaction mode cannot be used together between IN and OUT.
                // When "setTransaction" function called with "MPOS_PRINTER_TRANSACTION_IN", print data are stored in the buffer.
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);
                // Printer Setting Initialize
                await _printer.directIO(new byte[] { 0x1b, 0x40 });

           
                await _printer.printText("LAMSAC\n\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = true,Width = MPosFontSizeWidth.MPOS_FONT_SIZE_WIDTH_1  });

                await _printer.printText(LabeValorTitulo.Text+"\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER , Bold = true});
                await _printer.printText("TARIFA :" + LabeValorPeaje.Text+"\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER });
                await _printer.printText("FECHA: " + LabeValorFecha.Text+"\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER });
                await _printer.printText("PLACA: " + LabeValorPlaca.Text +"\n\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER });

                await _printer.printText("VINCI Highways\n", new MPosFontAttribute() { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER , Reverse=true});

                // Feed to tear-off position (Manual Cutter Position)
                await _printer.directIO(new byte[] { 0x1b, 0x4a, 0xaf });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "OK");
            }
            finally
            {
                // Printer starts printing by calling "setTransaction" function with "MPOS_PRINTER_TRANSACTION_OUT"
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_OUT);
                // If there's nothing to do with the printer, call "closeService" method to disconnect the communication between Host and Printer.
                _printSemaphore.Release();
            }

        }


        private  void conexion()
        {
            var connectionInfo = new MposConnectionInformation();

            connectionInfo.BluetoohDeviceId = "SPP-R310";
            connectionInfo.MacAddress = "74:F0:7D:E6:B6:38";
            connectionInfo.IntefaceType = MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH;
            _connectionInfo = connectionInfo;
        }

        async Task<MPosControllerPrinter> OpenPrinterService(MposConnectionInformation connectionInfo)
        {
            if (connectionInfo == null)
                return null;

            if (_printer != null)
                return _printer;

         

            _printer = MPosDeviceFactory.Current.createDevice(MPosDeviceType.MPOS_DEVICE_PRINTER) as MPosControllerPrinter;

            switch (connectionInfo.IntefaceType)
            {
                case MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH:
                case MPosInterfaceType.MPOS_INTERFACE_WIFI:
                case MPosInterfaceType.MPOS_INTERFACE_ETHERNET:
                    _printer.selectInterface((int)connectionInfo.IntefaceType, connectionInfo.Address);
                    _printer.selectCommandMode((int)MPosCommandMode.MPOS_COMMAND_MODE_BYPASS);
                    break;
                default:
                    await DisplayAlert("Connection Fail", "Not Supported Interface", "OK");
                    return null;
            }

            await _printSemaphore.WaitAsync();

            try
            {
                var result = await _printer.openService();
                if (result != (int)MPosResult.MPOS_SUCCESS)
                {
                    _printer = null;
                    await DisplayAlert("Connection Fail", "openService failed. (" + result.ToString() + ")", "OK");
                }
            }
            finally
            {
                _printSemaphore.Release();
            }

            return _printer;
        }



    }
}
