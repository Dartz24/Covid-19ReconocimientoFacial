using Luxand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Covid_19ReconocimientoFacial
{
    public partial class RegistroPaciente : Form
    {

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



        public RegistroPaciente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Capturar();
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
                        }
                        else
                        {
                            usuario = BuscarUsuario(name);
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
                    if (mouseX >= left && mouseX <= left + w && mouseY >= top && mouseY <= top + w)
                    {
                        pen = Pens.DarkBlue;
                        if (ProgramState.psRemember == programState)
                        {
                            if (FSDK.FSDKE_OK == FSDK.LockID(tracker, IDs[i]))
                            {
                                // get the user name
                                ////CamposBloqueados(false);
                                //Debug.WriteLine("desbloqueo campos");
                                //Debug.WriteLine("estaGuar: " + estaGuardado);

                                inputDatos inputDatos = new inputDatos();
                                bool nuevoUsuario = false;

                                if (usuario.Id <= 0)
                                {
                                    Debug.WriteLine("Nuevo usuario");
                                    nuevoUsuario = true;
                                }
                                else
                                {
                                    Debug.WriteLine("Nombre: " + usuario.Nombre + "\tCedula: " + usuario.Cedula);
                                    inputDatos.setDatos(usuario);
                                    nuevoUsuario = false;
                                }




                                if (DialogResult.OK == inputDatos.ShowDialog())
                                //if (estaGuardado)
                                {
                                    Debug.WriteLine("entró if");
                                    //userName = textBox_cedula.Text;
                                    usuario = inputDatos.usuario;

                                    //userName = inputName.userName;
                                    userName = usuario.Cedula;

                                    if (userName == null || userName.Length <= 0)
                                    {
                                        String s = "";
                                        FSDK.SetName(tracker, IDs[i], "");
                                        FSDK.PurgeID(tracker, IDs[i]);
                                    }
                                    else
                                    {
                                        FSDK.SetName(tracker, IDs[i], userName);
                                    }
                                    FSDK.UnlockID(tracker, IDs[i]);

                                    usuario.Imagen = frameImage;

                                    if (nuevoUsuario)
                                    {
                                        usuario = UsuarioDatos.insertUsuario(usuario);
                                        MessageBox.Show("Registro guardado correctamente");
                                    }
                                    else
                                    {
                                        Debug.WriteLine("id: " + usuario.Id);
                                        Debug.WriteLine("ced: " + usuario.Cedula);
                                        usuario = UsuarioDatos.updateUsuario(usuario);
                                        MessageBox.Show("Registro actualizado correctamente");
                                    }

                                    CargarUsuarioDB();

                                    //CamposBloqueados(true);
                                }
                                estaGuardado = false;
                            }
                        }
                    }
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








        /////metodos
        ///
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

        private void CargarUsuarioDB()
        {
            listaUsuarios.Clear();
            listaUsuarios = UsuarioDatos.CargarListaUsuario(listaUsuarios);
        }

    }
}
