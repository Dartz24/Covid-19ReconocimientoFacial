﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDatos {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.6.0.0")]
    internal sealed partial class Configuracion : global::System.Configuration.ApplicationSettingsBase {
        
        private static Configuracion defaultInstance = ((Configuracion)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Configuracion())));
        
        public static Configuracion Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ingenieriadesarrollo.database.windows.net;Initial Catalog=Reconocimie" +
            "ntoFacial;User ID=bpaul573;Password=Desarrolloingenieria001")]
        public string ConexionBD {
            get {
                return ((string)(this["ConexionBD"]));
            }
        }
    }
}
