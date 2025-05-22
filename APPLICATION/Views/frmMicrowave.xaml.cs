using System.Windows;
using APPLICATION.Models;
using APPLICATION.Interfaces;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;
using System.Data.SqlTypes;

namespace APPLICATION.View
{
    /// <summary>
    /// Lógica interna para frmMicrowave.xaml
    /// </summary>
    public partial class frmMicrowave : Window
    {
        private IAppliance _appliance;

        public frmMicrowave()
        {
            InitializeComponent();

            _appliance = new Microwave();

            Text_Timer.Text = FormatTimespanToMinutes();
        }

        #region Events

        private void Border_Sair_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        #region Numpad/Keyboard

        private void Text_Timer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Microwave)_appliance).Progress != EProgress.StandBy)
            {
                ShowMessage("Não é possível alterar timer durante aquecimento!\n" +
                            "\t- Cancele o aquecimento atual.");
            }

            Text_Timer_Edit.Text = FormatTimespanToSeconds();
            UpdateVisibilityTextTimerEdit(true);
        }

        private void Text_Timer_Edit_KeyDown(object sender, KeyEventArgs e)
        {       
            if(e.Key == Key.Delete || e.Key == Key.Back)
            {
                e.Handled = false;
            }
            else if((e.Key >= Key.D0 && 
                     e.Key <= Key.D9) ||
                    (e.Key >= Key.NumPad0 &&
                     e.Key <= Key.NumPad9))
            {
                if(Text_Timer_Edit.Text.Length >= 4)
                    e.Handled = true;                
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Border_Key_Number_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (((Microwave)_appliance).Progress != EProgress.StandBy)
            {
                ShowMessage("Não é possível alterar timer durante aquecimento!\n" +
                            "\t- Cancele o aquecimento atual.");
            }
            
            Border origin = sender as Border;

            if(origin.Child is TextBlock textBlock)
            {
                UpdateVisibilityTextTimerEdit(true);

                UpdateTextTimerEdit(textBlock.Text);
            }
        }

        #endregion Numpad/Keyboard

        #region Actions Keys

        private void Border_Key_LessPower_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Microwave)_appliance).LessPower();

            ShowPotential(((Microwave)_appliance).Potential);
        }

        private void Border_Key_MorePower_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Microwave)_appliance).MorePower();

            ShowPotential(((Microwave)_appliance).Potential);            
        }


        private void Border_ActionKey_Pause_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(((Microwave)_appliance).Progress == EProgress.InProgress)
            {
                ((Microwave)_appliance).Pause();

                Key_Pause.Text = "Cancelar";
            }
            else if(((Microwave)_appliance).Progress == EProgress.Paused)
            {
                ((Microwave)_appliance).Cancel();

                Key_Pause.Text = "Pausar";
            }
        }

        private void Border_ActionKey_Start_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (((Microwave)_appliance).Progress != EProgress.InProgress)
            {
                if(((Microwave)_appliance).Progress == EProgress.StandBy ||
                   ((Microwave)_appliance).Progress == EProgress.Finished)
                {
                    if (!ValidateTimer(out TimeSpan newTimer, out string erroMessage))
                    {
                        ShowMessage(erroMessage);
                        return;
                    }

                    ((Microwave)_appliance).SetTimer(newTimer);
                }

                ((Microwave)_appliance).Start();

                UpdateTimerAndProgress();

                if (((Microwave)_appliance).Progress == EProgress.Paused)
                {
                    Key_Pause.Text = "Pausar";
                }
            }
        }

        #endregion Actions Keys

        #endregion Events

        private string FormatTimespanToMinutes()
        {
            return ((Microwave)_appliance).Timer.ToString(@"mm\:ss");
        }

        private string FormatTimespanToSeconds()
        {
            return ((Microwave)_appliance).Timer.TotalSeconds.ToString();
        }


        private void UpdateVisibilityTextTimerEdit(bool IsEnable)
        {
            Text_Timer_Edit.Visibility = IsEnable ? Visibility.Visible : Visibility.Hidden;
            Text_Timer.Visibility = !IsEnable ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateTextTimerEdit(string value)
        {
            if(Text_Timer_Edit.Text.Length < 4)
            {
                Text_Timer_Edit.Text += value;
            }
        }

        private async void UpdateTimerAndProgress()
        {
            while (((Microwave)_appliance).WarmingUp)
            {
                await Task.Delay(1000);

                if(((Microwave)_appliance).Progress == EProgress.InProgress)
                {
                    Text_Progress.Text = ((Microwave)_appliance).ProgressText;
                    Text_Timer.Text = FormatTimespanToMinutes();
                    DoEvents();
                }
                else if(((Microwave)_appliance).Progress == EProgress.StandBy)
                {
                    Text_Progress.Text = string.Empty;
                }
            }

            if(((Microwave)_appliance).Progress == EProgress.Finished)
            {
                Text_Progress.Text = ((Microwave)_appliance).ProgressText;
                Text_Timer.Text = FormatTimespanToMinutes();
                DoEvents();
            }
        }
        
        private async void ShowPotential(int Potential)
        {
            if (Text_Timer.Visibility == Visibility.Visible)
            {
                string valueOriginal = Text_Timer.Text;

                Text_Timer.Text = Potential.ToString();

                DoEvents();

                await Task.Delay(1000);

                Text_Timer.Text = valueOriginal;
            }
            else
            {
                string valueOriginal = Text_Timer_Edit.Text;

                Text_Timer_Edit.Text = Potential.ToString();

                DoEvents();

                await Task.Delay(1000);

                Text_Timer_Edit.Text = valueOriginal;
            }
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Microwave Benner",
                            MessageBoxButton.OK, MessageBoxImage.Hand);
        }


        private bool ValidateTimer(out TimeSpan newTimer, out string errorMessage)
        {
            bool result = false;
            errorMessage = string.Empty;
            newTimer = new TimeSpan();

            if (Text_Timer_Edit.Visibility == Visibility.Visible)
            {
                if (Text_Timer_Edit.Text.Length == 0)
                {
                    errorMessage += "Obrigatório informar tempo!";
                    return result;
                }

                if (Convert.ToInt64(Text_Timer_Edit.Text) <= 0)
                {
                    errorMessage += "Tempo mínimo permitido 1 Segundos!";
                    return result;
                }

                if (Convert.ToInt64(Text_Timer_Edit.Text) > 120)
                {
                    errorMessage += "Tempo máximo permitido 120 Segundos!";
                    return result;
                }
                
                newTimer = TimeSpan.FromSeconds(Convert.ToInt64(Text_Timer_Edit.Text));

                result = true;
            }
            else
            {
                if (Text_Timer.Text.Replace(":", "").Length == 0)
                {
                    errorMessage += "Obrigatório informar tempo!";
                    return result;
                }

                if (Convert.ToInt64(Text_Timer.Text.Replace(":", "")) == 0)
                {
                    errorMessage += "Obrigatório informar tempo válido!";
                    return result;
                }

                string[] hourMinute = Text_Timer.Text.Split(':');

                newTimer = new TimeSpan(0, Convert.ToInt32(hourMinute[0]), Convert.ToInt32(hourMinute[1]));

                if (newTimer.TotalSeconds <= 0)
                {
                    errorMessage += "Tempo mínimo permitido 1 Segundos!";
                    return result;
                }

                if (newTimer.TotalSeconds > 120)
                {
                    errorMessage += "Tempo máximo permitido 120 Segundos!";
                    return result;
                }

                result = true;
            }

            return result;
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render,
                                                  new Action(delegate { }));
        }
    }
}
