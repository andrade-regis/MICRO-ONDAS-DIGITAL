using APPLICATION.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace APPLICATION.Models
{
    public class Microwave : IAppliance
    {
        public string ProgressText {  get; private set; }
        public EProgress Progress { get; private set; }

        public TimeSpan Timer { get; private set; }
        public int Potential { get; private set; }

        public bool WarmingUp { get; private set; }

        public Microwave()
        {
            Progress = EProgress.StandBy;
            
            UpdateMicrowaveState(Progress);
        }

        public void SetTimer(TimeSpan newTimer)
        {
            if(Progress != EProgress.StandBy &&
               Progress != EProgress.Finished)
            {
                throw new Exception("Não é permitido modificar o timer no estado atual!");
            }

            if(newTimer.TotalSeconds > 120 ||
               newTimer.TotalSeconds <= 0)
            {
                throw new Exception("Somente valores entre 1 e 120 são permitidos!");
            }

            Timer = newTimer;
        }

        public async Task Start()
        {
            if (WarmingUp)
                WarmingUp = true;

            UpdateMicrowaveState(EProgress.InProgress);

            while (WarmingUp)
            {
                if(Timer.TotalMilliseconds >= 0)
                {
                    ProgressText += WriteProgress('*');

                    await Task.Delay(1000);

                    Timer = Timer.Subtract(TimeSpan.FromSeconds(1));
                }
                else
                {
                    WarmingUp = false;
                    UpdateMicrowaveState(EProgress.Finished);
                }
            }
        }

        public void Pause()
        {
            if (WarmingUp)
                WarmingUp = false;

            UpdateMicrowaveState(EProgress.Paused);
        }

        public void Cancel()
        {
            if (WarmingUp)
                WarmingUp = false;

            UpdateMicrowaveState(EProgress.StandBy);
        }

        public void LessPower()
        {
            if (Potential > 1)
            {
                Potential--;
            }
            else
            {
                throw new Exception("Potência já se encontra no minimo permitido!");
            }
        }

        public void MorePower()
        {
            if (Potential < 10)
            {
                Potential++;
            }
            else
            {
                throw new Exception("Potência já se encontra no máximo permitido!");
            }
        }

        public string WriteProgress(char character)
        {
            string textProgress = string.Empty;

            if (Timer.Seconds > 0)
                textProgress = CalculateProgressString(character);
            else
                textProgress = "Aquecimento Concluído";

            return textProgress;
        }        


        public void UpdateMicrowaveState(EProgress progress)
        {
            switch (progress)
            {
                case EProgress.StandBy:
                    {
                        Timer = TimeSpan.FromSeconds(90);

                        Potential = 10;

                        Progress = progress;
                    }
                    break;
                case EProgress.InProgress:
                    {
                        Progress = progress;
                    }
                    break;
                case EProgress.Paused:
                    {
                        Progress = progress;
                    }
                    break;
            }
        } 

        private string CalculateProgressString(char character)
        {
            string progressString = string.Empty;

            for (int counter = 0; counter < Potential; counter++)
            {
                progressString += character;
            }

            return string.Concat(progressString, " ");
        }

        
    }
}
