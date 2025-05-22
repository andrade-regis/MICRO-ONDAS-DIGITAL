using System;
using System.Threading.Tasks;

namespace APPLICATION.Interfaces
{
    public interface IAppliance
    {
        Task Start();

        void Pause();

        void Cancel();

        void MorePower();

        void LessPower();

        void SetTimer(TimeSpan newTimer);

        string WriteProgress(char character);
    }
}
