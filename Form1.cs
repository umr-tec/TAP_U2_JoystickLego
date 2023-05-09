using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
//Librerias externas de Aforge y NXTNet
using AForge;
using AForge.Controls;
using NxtNet;

namespace TAP_U2_JoystickLego
{
    public partial class Form1 : Form
    {
        //Variables globaes para el Joystick
        Joystick joystick;
        List<Joystick.DeviceInfo> devicesJoysticks 
            = Joystick.GetAvailableDevices();
        //Objeto blobal para el robot
        Nxt robot = new Nxt();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Mostrar los Joysticks disponibles
            foreach (Joystick.DeviceInfo joystick in devicesJoysticks)
            {
                comboBox1.Items.Add(string.Format("ID: {0}, Name: {1}...", joystick.ID, joystick.Name));
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();
            try
            {
                joystick = new Joystick(comboBox1.SelectedIndex);
                dialog = MessageBox.Show("¿Deseas activar el NXT?","JoystickLego", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    tmtJoystick.Enabled = true;
                }
                if (dialog == DialogResult.No)
                {
                    tmtJoystick.Enabled = false;
                }

            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void tmtJoystick_Tick(object sender, EventArgs e)
        {
            //Conocer el estatus del Joystick
            Joystick.Status status = joystick.GetCurrentStatus();
            //Cargar los valores del Joystick
            lblX.Text = status.XAxis.ToString();
            lblY.Text = status.YAxis.ToString();



            //1. Conectarnos al NXT
            if (status.IsButtonPressed(Joystick.Buttons.Button1))
            {
                //Hailitar la conexion al NXT
                try
                {
                    robot.Connect("COM4");
                    robot.PlayTone(800, 2000);
                    MessageBox.Show("Robot conectado");
                    tmrGame.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            if (status.IsButtonPressed(Joystick.Buttons.Button3))
            {
                robot.SetOutputState(MotorPort.PortA, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            if (status.IsButtonPressed(Joystick.Buttons.Button2))
            {
                try
                {
                    robot.Disconnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }           

        }

        private void tmrGame_Tick(object sender, EventArgs e)
        {
            Joystick.Status status = joystick.GetCurrentStatus();

            #region MovimientoJoystick
            //2. Programar los Joysticks
            if (status.XAxis.ToString() == "-1")
            {
                btnLeft.BackColor = Color.LightPink;
                robot.SetOutputState(MotorPort.PortA, 100, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 0, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            else
            {
                btnLeft.BackColor = Color.Transparent;
                robot.SetOutputState(MotorPort.PortA, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            if (status.XAxis.ToString() == "1")
            {
                btnRight.BackColor = Color.LightPink;
                robot.SetOutputState(MotorPort.PortA, 0, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 100, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            else
            {                
                btnRight.BackColor = Color.Transparent;
                robot.SetOutputState(MotorPort.PortA, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            if (status.YAxis.ToString() == "1")
            {
                btnDown.BackColor = Color.LightPink;
            }
            else
            {
                btnDown.BackColor = Color.Transparent;
            }
            if (status.YAxis.ToString() == "-1")
            {
                btnUp.BackColor = Color.LightPink;
                robot.SetOutputState(MotorPort.PortA, 100, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                robot.SetOutputState(MotorPort.PortC, 100, MotorModes.On, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
            }
            else
            {
                try
                {
                    btnUp.BackColor = Color.Transparent;
                    robot.SetOutputState(MotorPort.PortA, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                    robot.SetOutputState(MotorPort.PortC, 0, MotorModes.Brake, MotorRegulationMode.Speed, 100, MotorRunState.Running, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            #endregion
        }
    }
}
