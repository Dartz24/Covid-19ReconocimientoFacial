using Luxand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail;
using System.Windows.Forms;

namespace Covid_19ReconocimientoFacial
{
    public partial class ReconocimientoFacial : Form
    {
        public ReconocimientoFacial()
        {
            InitializeComponent();
        }


        #region Variables

        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        ProgramState programState = ProgramState.psRecognize;

        String cameraName;
        bool needClose = false;
        string userName;
        String TrackerMemoryFile = "tracker.dat";
        int mouseX = 0;
        int mouseY = 0;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        #endregion

        bool estaGuardado = false;
        List<Usuario> listaUsuarios = new List<Usuario>();
        Usuario usuario = new Usuario();



        private void button1_Click(object sender, EventArgs e)
        {
            Capturar();
        }

        /// <summary>
        /// metodos
        /// </summary>


        private void CargarUsuarioDB()
        {
            listaUsuarios.Clear();
            listaUsuarios = UsuarioDatos.CargarListaUsuario(listaUsuarios);
        }

        private Usuario BuscarUsuario(string name)
        {
            //Debug.WriteLine("name: " + name);
            foreach (Usuario item in listaUsuarios)
            {
                if (item.Cedula.Equals(name))
                {
                    //Debug.WriteLine("encontro: ced: " + item.Cedula);
                    return item;
                }
            }
            //Debug.WriteLine("no encontro");
            return null;
        }


        private void CarcarCampos(Usuario usuario)
        {
            textBox_id.Text = usuario.Id.ToString();
            textBox_cedula.Text = usuario.Cedula;
            textBox_nombre.Text = usuario.Nombre;
            textBox_apellido.Text = usuario.Apellido;
            textBox_telefono.Text = usuario.Telefono;
            textBox_email.Text = usuario.Email;
           
           
        }

        private void LimpiarCampos()
        {
            textBox_id.Text = null;
            textBox_cedula.Text = null;
            textBox_nombre.Text = null;
            textBox_apellido.Text = null;
            textBox_telefono.Text = null;
            textBox_email.Text = null;
        }

        private void Iniciar()
        {

            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("gyYgVWQTSzjiuGB/hH8dKgg0QrrIuhoHdfUCzD9rY+vru3WRZsaezTX6YWj9osdI/cmxY1NSdLkyWuugMPCxUG7/xNLegHLeaUpzVyKpDkaWL8tJIUsIL7xv9bhmgifPbAyTDuxF3VGxXmHkv/L/MStf9kdXV/A1vVvT93QC4vQ="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();

            #region Camaras
            //VERIFICAR CAMARAS, OBTENER LOS NOMBRE Y LA CANTIDAD DE CAMARAS.
            string[] cameraList;
            int count;
            FSDKCam.GetCameraList(out cameraList, out count);

            if (0 == count)
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            cameraName = cameraList[0];

            #endregion

            FSDKCam.VideoFormatInfo[] formatList;
            FSDKCam.GetVideoFormatList(ref cameraName, out formatList, out count);

            int VideoFormat = 0; // choose a video format
            pictureBox1.Width = formatList[VideoFormat].Width;
            pictureBox1.Height = formatList[VideoFormat].Height;

            //Debug.WriteLine("Width: " + pictureBox1.Width);
            //Debug.WriteLine("Height: " + pictureBox1.Height);

            //this.Width = formatList[VideoFormat].Width + 75;
            //this.Height = formatList[VideoFormat].Height + 150;
            //Debug.WriteLine("Width: " + pictureBox1.Width);
            //Debug.WriteLine("Height: " + pictureBox1.Height);


        }


        private void Capturar()
        {
            this.button1.Enabled = false;
            int cameraHandle = 0;

            int r = FSDKCam.OpenVideoCamera(ref cameraName, ref cameraHandle);
            Debug.WriteLine("cameraName: " + cameraName);
            Debug.WriteLine("cameraHandle: " + cameraHandle);
            Debug.WriteLine("r: " + r);


            if (r != FSDK.FSDKE_OK)
            {
                MessageBox.Show("Error opening the first camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            int tracker = 0;
            // creating a Tracker
            if (FSDK.FSDKE_OK != FSDK.LoadTrackerMemoryFromFile(ref tracker, TrackerMemoryFile)) // try to load saved tracker state
                FSDK.CreateTracker(ref tracker); // if could not be loaded, create a new tracker

            int err = 0; // set realtime face detection parameters
            FSDK.SetTrackerMultipleParameters(tracker, "HandleArbitraryRotations=false; DetermineFaceRotationAngle=false; InternalResizeWidth=100; FaceDetectionThreshold=5;", ref err);

            while (!needClose)
            {
                Int32 imageHandle = 0;
                if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle, ref imageHandle)) // grab the current frame from the camera
                {
                    Application.DoEvents();
                    continue;
                }

                FSDK.CImage image = new FSDK.CImage(imageHandle);


                long[] IDs;
                long faceCount = 0;
                FSDK.FeedFrame(tracker, 0, image.ImageHandle, ref faceCount, out IDs, sizeof(long) * 256); // maximum of 256 faces detected
                Array.Resize(ref IDs, (int)faceCount);

                // make UI controls accessible (to find if the user clicked on a face)
                Application.DoEvents();

                Image frameImage = image.ToCLRImage();
                Graphics gr = Graphics.FromImage(frameImage);
                //pictureBox2.Image = frameImage;

                if (IDs.Length == 0)
                {
                    LimpiarCampos();
                }

                for (int i = 0; i < IDs.Length; ++i)
                {
                    //Debug.WriteLine("Cant: " + IDs.Length);
                    FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
                    FSDK.GetTrackerFacePosition(tracker, 0, IDs[i], ref facePosition);

                    int left = facePosition.xc - (int)(facePosition.w * 0.6);
                    int top = facePosition.yc - (int)(facePosition.w * 0.5);
                    int w = (int)(facePosition.w * 1.2);

                    String name;
                    int res = FSDK.GetAllNames(tracker, IDs[i], out name, 65536); // maximum of 65536 characters

                    //DETECCION

                    if (FSDK.FSDKE_OK == res)
                    { // draw name

                        if (name.Length <= 0)
                        {
                            name = "Desconocido";
                            usuario = new Usuario();
                            LimpiarCampos();
                        }
                        else
                        {
                            usuario = BuscarUsuario(name);
                            CarcarCampos(usuario);
                            name = usuario.Nombre + " " + usuario.Apellido;
                            
                        }

                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;

                        gr.DrawString(name, new Font("Century Gothic", 16, FontStyle.Bold),
                            new SolidBrush(Color.LightGreen),
                            facePosition.xc, top + w + 5, format);
                    }


                    //REGISTRO
                    Pen pen = Pens.DarkGreen;
                    //if (mouseX >= left && mouseX <= left + w && mouseY >= top && mouseY <= top + w)
                    //{
                    //    pen = Pens.DarkBlue;
                    //    if (ProgramState.psRemember == programState)
                    //    {
                    //        if (FSDK.FSDKE_OK == FSDK.LockID(tracker, IDs[i]))
                    //        {
                    //            // get the user name
                    //            ////CamposBloqueados(false);
                    //            //Debug.WriteLine("desbloqueo campos");
                    //            //Debug.WriteLine("estaGuar: " + estaGuardado);

                    //            inputDatos inputDatos = new inputDatos();
                    //            bool nuevoUsuario = false;

                    //            if (usuario.Id <= 0)
                    //            {
                    //                Debug.WriteLine("Nuevo usuario");
                    //                nuevoUsuario = true;
                    //            }
                    //            else
                    //            {
                    //                Debug.WriteLine("Nombre: " + usuario.Nombre + "\tCedula: " + usuario.Cedula);
                    //                inputDatos.setDatos(usuario);
                    //                nuevoUsuario = false;
                    //            }




                    //            if (DialogResult.OK == inputDatos.ShowDialog())
                    //            //if (estaGuardado)
                    //            {
                    //                Debug.WriteLine("entró if");
                    //                //userName = textBox_cedula.Text;
                    //                usuario = inputDatos.usuario;

                    //                //userName = inputName.userName;
                    //                userName = usuario.Cedula;

                    //                if (userName == null || userName.Length <= 0)
                    //                {
                    //                    String s = "";
                    //                    FSDK.SetName(tracker, IDs[i], "");
                    //                    FSDK.PurgeID(tracker, IDs[i]);
                    //                }
                    //                else
                    //                {
                    //                    FSDK.SetName(tracker, IDs[i], userName);
                    //                }
                    //                FSDK.UnlockID(tracker, IDs[i]);

                    //                usuario.Imagen = frameImage;

                    //                if (nuevoUsuario)
                    //                {
                    //                    usuario = Usuario_Datos.insertUsuario(usuario);
                    //                    MessageBox.Show("Registro guardado correctamente");
                    //                }
                    //                else
                    //                {
                    //                    Debug.WriteLine("id: " + usuario.Id);
                    //                    Debug.WriteLine("ced: " + usuario.Cedula);
                    //                    usuario = Usuario_Datos.updateUsuario(usuario);
                    //                    MessageBox.Show("Registro actualizado correctamente");
                    //                }

                    //                CargarUsuarioDB();

                    //                //CamposBloqueados(true);
                    //            }
                    //            estaGuardado = false;
                    //        }
                    //    }
                    //}

                    gr.DrawRectangle(pen, left, top, w, w);
                }
                programState = ProgramState.psRecognize;

                // display current frame
                pictureBox1.Image = frameImage;
                GC.Collect(); // collect the garbage after the deletion
            }

            FSDK.SaveTrackerMemoryToFile(tracker, TrackerMemoryFile);
            FSDK.FreeTracker(tracker);


            FSDKCam.CloseVideoCamera(cameraHandle);
            FSDKCam.FinalizeCapturing();

        }

        private void ReconocimientoFacial_FormClosing(object sender, FormClosingEventArgs e)
        {
            needClose = true;
        }

        private void ReconocimientoFacial_Load(object sender, EventArgs e)
        {
            CargarUsuarioDB();
            Iniciar();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Capturar();
        }

        private void cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //maximizar
            WindowState = FormWindowState.Maximized;
            pictureBox4.Visible = false;
            restaurar.Visible = true;
        }

        private void minimizar_Click(object sender, EventArgs e)
        {
            //minimizar
            WindowState = FormWindowState.Minimized;
        }

        private void restaurar_Click(object sender, EventArgs e)
        {
            //Restaurar
            WindowState = FormWindowState.Normal;
            restaurar.Visible = false;
            pictureBox4.Visible = true;
        }

        private void Email()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587; //puertto smtp google y de gmail
            smtp.Timeout = 50;
            smtp.Host = "smtp.live.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            #region Credencial
            smtp.Credentials = new System.Net.NetworkCredential
            {
                UserName = "dartzgotico@hotmail.com", //Usuario Hotmail
                Password = "19956dartz"   //Contraseña
            };
            #endregion


            MailAddress from = new MailAddress("dartzgotico@hotmail.com", "Sistema Informatico Reconocimiento Facial Covid-19", Encoding.UTF8);


            var listamails = textBox_email.Text.Split(';');

            using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
            {
                message.From = from;
                message.Subject = "Centro Salud N#2";
                message.IsBodyHtml = true;
                message.Priority = System.Net.Mail.MailPriority.High;
                message.Body = CuerpoMensajeHTML();

                ///creo una vista cuerpo del mensaje                

                AlternateView h2 = AlternateView.CreateAlternateViewFromString(CuerpoMensajeHTML(), Encoding.UTF8
                    , MediaTypeNames.Text.Html);

                // pongo la imagen en el html creado


                LinkedResource img =
                    new LinkedResource(@"C:\Imagenes\11.png",
                                        MediaTypeNames.Image.Jpeg);
                img.ContentId = "imagen";

                // Lo incrustamos en la vista HTML...

                h2.LinkedResources.Add(img);

                message.AlternateViews.Add(h2);





                //envio a todod los destinatarios
                foreach (var item in listamails)
                {


                    try
                    {
                        message.To.Add(item);
                        smtp.Send(message);
                        message.To.Clear();

                    }
                    catch (Exception e)
                    {

                        MessageBox.Show(e.Message);
                        // LabelMensaje.Text = e.Message;
                    }

                }


                MessageBox.Show("CORREOS ENVIADOS SATISFACTORIAMENTE");
               // LabelMensaje.Text = "CORREOS ENVIADOS SATISFACTORIAMENTE";
            }



            //    SmtpMailoMail = new SmtpMail("TryIt");
            //    SmtpClientoSmtp = new SmtpClient();//crea objeto de la clase SmtpClient
            //    oMail.From = new MailAddress("alex39garces@gmail.com"); // email desde donde se enviara el mensaje
            //    oMail.To.Add(new MailAddress("alex39garces@gmail.com"));// configuracion de email de destino del mensaje
            //    oMail.Subject = "alerta sistema ecu-911";// configuracion del asunto de recibo del mensaje
            //    List<string> archivo = new List<string>();
            //    oMail.TextBody = "una persona buscada ha sido identificada su nombre es: " +
            //    visitorName1 + "\tla ciudad es:\t" + town.Text + "\tsu GPS es:\t" +
            //    geolocation.Text + "el nivel de acierto es" + acierto.ToString() + "%" + "\tla calle es:\t" + street.Text;
            //    Windows.Storage.StorageFile file = currentIdPhotoFile; //se realiza la carga de la foto de la persona buscada para enviarla conjuntamente con las coordenadas
            //    stringattfile = file.Path;
            //    AttachmentoAttachment = awaitoMail.AddAttachmentAsync(attfile);//carga el archivo a enviar
            //    SmtpServeroServer = new SmtpServer("smtp.gmail.com");// configuracion del servidor Gmail SMTP
            //    oServer.User = "alex39garces@gmail.com";// usuario y password requeridos para la
            //    autenticacion SMTP
            //    oServer.Password = "aleks9166garces21";//clave de la cuenta de correo electronico
            //    oServer.Port = 465;// utiliza el puerto 465
            //    oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
            //    awaitoSmtp.SendMailAsync(oServer, oMail);





        }


        private string CuerpoMensajeHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table align='center'>");
            sb.Append("<tr><td colspan='2' style='background:#F3C145 ' align='center'> Notificación Centro Salud</td></tr>" +
                              "<center><img align='center' src='cid:imagen'/></center>");
            sb.Append("<tr><td colspan='2'align='center'  style='background:#F3C145'>Datos Personales</td></tr>");
            sb.Append("  <tr>");
            sb.Append("<td>CÉDULA: </td>");
            sb.Append("<td>" + textBox_cedula.Text + " </td>");
            sb.Append("  </tr>");

            sb.Append("  <tr>");
            sb.Append("<td>PACIENTE: </td>");
            string nombres;
            nombres = textBox_nombre.Text + " " + textBox_apellido.Text;
            sb.Append("<td>" + nombres+" </td>");
            sb.Append("  </tr>");


            sb.Append("  <tr>");
            sb.Append("<td>MOTIVO: </td>");
            sb.Append("<td>El Paciente esta fuera de su asilamiento, por favor comuniquenos con nosotros lo mas pronto posible</td>");
            sb.Append("  </tr>");

            sb.Append("  <tr>");
            sb.Append("<td>ULTIMA UBICACIÓN : </td>");
            sb.Append("<td>" + textBoxUbicacion.Text + " </td>");
            sb.Append("  </tr>");

            // sb.Append("<td> </td>");

            sb.Append(" </table>");


            return sb.ToString();

        }


        void Audio()
        {
            var dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sonido/2.wav");
            SoundPlayer Player = new SoundPlayer();
            // Player.SoundLocation = "C:/2.wav";
            Player.SoundLocation = dir;
             Player.Play();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Audio();
            Email();
            
        }
    }
}
