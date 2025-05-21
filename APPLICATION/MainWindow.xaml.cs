using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APPLICATION
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _timerText;
        private TimeSpan _timer;
        private int potential;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events

        private void Border_Sair_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        #region Numpad/Keyboard

        private void Border_Key_One_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Two_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Three_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Four_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Five_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Six_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Seven_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Eight_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Nine_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_Zero_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        #endregion Numpad/Keyboard

        #region Actions Keys

        private void Border_Key_LessPower_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_Key_MorePower_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }


        private void Border_ActionKey_Pause_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_ActionKey_Start_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        #endregion Actions Keys

        #endregion Events

        private void Set_Timer_Text(string value)
        {

        }

    }
}
