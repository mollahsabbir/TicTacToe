using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private Members

        private MarkType[] mResults;

        private bool mPlayer1Turn;

        private bool mGameEnded;

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        #endregion
        

        private void NewGame()
        {
            mResults = new MarkType[9];
            for(int i = 0 ; i < mResults.Length ; i++ )
            {
                mResults[i] = MarkType.Free;
            }

            mPlayer1Turn = true;

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            mGameEnded = false;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            var button = (Button)sender;
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + (row * 3);

            if( mResults[index]!= MarkType.Free)
            {
                return;
            }

            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            button.Content = mPlayer1Turn ? "X" : "O"  ;

            if ( !mPlayer1Turn )
            {
                button.Foreground = Brushes.Red;
            }

            mPlayer1Turn = !mPlayer1Turn;   //mPlayerTurn ^= true;

            CheckForWinner();
        }

        private void CheckForWinner()
        {
            //Check Horizontals
            
            if( mResults[0] != MarkType.Free && ((mResults[0] & mResults[1] & mResults[2]) == mResults[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }
            if (mResults[3] != MarkType.Free && ((mResults[3] & mResults[4] & mResults[5]) == mResults[3]))
            {
                mGameEnded = true;
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }
            if (mResults[6] != MarkType.Free && ((mResults[6] & mResults[7] & mResults[8]) == mResults[6]))
            {
                mGameEnded = true;
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }

            //Check Verticals
            if (mResults[0] != MarkType.Free && ((mResults[0] & mResults[3] & mResults[6]) == mResults[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
            }
            if (mResults[1] != MarkType.Free && ((mResults[1] & mResults[4] & mResults[7]) == mResults[1]))
            {
                mGameEnded = true;
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
            }
            if (mResults[2] != MarkType.Free && ((mResults[2] & mResults[5] & mResults[8]) == mResults[2]))
            {
                mGameEnded = true;
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
            }

            //Check Obliques
            if (mResults[0] != MarkType.Free && ((mResults[0] & mResults[4] & mResults[8]) == mResults[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }
            if (mResults[2] != MarkType.Free && ((mResults[2] & mResults[4] & mResults[6]) == mResults[2]))
            {
                mGameEnded = true;
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
            }

            //Check Full Board
            if (!mResults.Any(result => result == MarkType.Free))
            {
                mGameEnded = true;

                //Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;                
                });
            }
        }
    }
}
