using CotizadorRojoBetabel.Models;
using CotizadorRojoBetabel.Views;
using LibreR.Controllers;
using LibreR.Embedded.Newtonsoft.Json;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CotizadorRojoBetabel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static OrmLiteConnectionFactory DbFactory;
        internal static Logger Log;

        private void Application_Startup(object sender, StartupEventArgs args)
        {
            // create logger instance
            var assembly = GetType().GetAssembly();

            Log = new Logger(string.Format("{0}", assembly.Name), string.Format(
                "{0}\r\nhttps://nutricion-rojo-betabel.negocio.site\r\nSalud y Sabor en Armonía.\r\ne-mail: developer.rojobetabel@gmail.com\r\nCONTACTO:\r\nCel: (642) 102-3512",
                assembly.Name
            ));

            // catch unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Current.DispatcherUnhandledException += CurrentDomain_UnhandledException;

            // show wait view
            ParentView.Current.Show();
            ParentView.Show_WaitView("Inicializando...");

            // continue
            LoadApp();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Message(e.Serialize());
            Console.WriteLine();
        }

        private void CurrentDomain_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Message(e.Serialize());
            Console.WriteLine();
        }

        private Task LoadApp()
        {
            return Task.Run(() =>
            {
                // set the default format for date time objects in the json serializer
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                };

                // load-create database
                var database = $"{Directory.GetCurrentDirectory()}\\Data\\local.db";
                if (!File.Exists(database))
                {
                    var stream = File.Create(database);
                    stream.Close();
                }

                DbFactory = new OrmLiteConnectionFactory($"Data Source={database};", SqliteDialect.Provider);

                using (var db = DbFactory.Open())
                {
                    db.CreateTableIfNotExists<Products>();
                }
            });
        }
    }
}
