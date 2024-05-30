using System.Windows;
using System.Windows.Input;
using Tema2.Commands;
using Tema2.Models;

namespace Tema2.ViewModels
{
    public class GameStatisticsVM : BaseNotification
    {
        private int _whiteWins;
        public int WhiteWins
        {
            get => _whiteWins;
            set
            {
                if (_whiteWins != value)
                {
                    _whiteWins = value;
                    NotifyPropertyChanged(nameof(WhiteWins)); 
                }
            }
        }

        private int _redWins;
        public int RedWins
        {
            get => _redWins;
            set
            {
                if (_redWins != value)
                {
                    _redWins = value;
                    NotifyPropertyChanged(nameof(RedWins)); 
                }
            }
        }

        private int _maxRemainingPieces;
        public int MaxRemainingPieces
        {
            get => _maxRemainingPieces;
            set
            {
                if (_maxRemainingPieces != value)
                {
                    _maxRemainingPieces = value;
                    NotifyPropertyChanged(nameof(MaxRemainingPieces)); 
                }
            }
        }

        public ICommand ShowStatisticsCommand { get; private set; }

        public GameStatisticsVM()
        {
            ShowStatisticsCommand = new SimpleCommand(ExecuteShowStatistics);
            ReloadStatistics(); // initializarea statisticilor la început
        }

        private void ExecuteShowStatistics()
        {
            ReloadStatistics(); // reîncarcarea și afisarea statisticilor la comandă
            string statistics = $"Număr de victorii Jucători Albi: {WhiteWins}\nNumăr de victorii Jucători Roșii: {RedWins}\nMaxim piese rămase pe tablă: {MaxRemainingPieces}";
            MessageBox.Show(statistics, "Statistici", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ReloadStatistics()
        {
            var stats = GameStatistics.GetStatistics();
            WhiteWins = stats.whiteWins;
            RedWins = stats.redWins;
            MaxRemainingPieces = stats.maxRemainingPieces;
        }
    }

}
