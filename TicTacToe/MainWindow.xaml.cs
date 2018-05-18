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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private Members

        private MarkType[] board;

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
            board = new MarkType[9];
            for(int i = 0 ; i < board.Length ; i++ )
            {
                board[i] = MarkType.Free;
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

            if( board[index]!= MarkType.Free)
            {
                //if Button already marked
                return;
            }

            board[index] = MarkType.Cross;

            button.Content = mPlayer1Turn ? "X" : "O"  ;

            if ( !mPlayer1Turn )
            {
                button.Foreground = Brushes.Red;
            }

            //mPlayer1Turn = !mPlayer1Turn;   //mPlayerTurn ^= true;

            CheckForWinner();

            if (!mGameEnded)
            {
                AIsTurn();
            }
            
        }

        private void AIsTurn()
        {
            List<int> list = RemainingIndexis(board);
            Random random = new Random();
            int index = random.Next(0, list.Count);
            MarkIndex( list[index] );

            CheckForWinner();
        }

        private List<int> RemainingIndexis(MarkType[] newBoard)
        {
            List<int> list = new List<int>();
            for(int i=0; i< newBoard.Length; i++)
            {
                if (newBoard[i] == MarkType.Free)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        private void MarkIndex(int index)
        {
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {

                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);

                if ( row == index/3 && column == index%3 )
                {
                    button.Foreground = Brushes.Red;
                    button.Content = "O";
                }
            });

            board[index] = MarkType.Nought;
        }

        private bool CanWin(MarkType[] board, bool isPlayer)
        {

            MarkType player = isPlayer ? MarkType.Cross : MarkType.Nought;

            if ((board[0] != MarkType.Free && ((board[0] & board[1] & board[2]) == player)) ||
                (board[3] != MarkType.Free && ((board[3] & board[4] & board[5]) == player)) ||
                (board[6] != MarkType.Free && ((board[6] & board[7] & board[8]) == player)) ||
                (board[0] != MarkType.Free && ((board[0] & board[3] & board[6]) == player)) ||
                (board[1] != MarkType.Free && ((board[1] & board[4] & board[7]) == player)) ||
                (board[2] != MarkType.Free && ((board[2] & board[5] & board[8]) == player)) ||
                (board[0] != MarkType.Free && ((board[0] & board[4] & board[8]) == player)) ||
                (board[2] != MarkType.Free && ((board[2] & board[4] & board[6]) == player))
                )
            {
                return true;
            }
            else return false;
            
        }

        private int MiniMax(MarkType[] newBoard, bool player)
        {
            List<int> emptyIndexis = RemainingIndexis(newBoard);
            return 0;
        }

        private void CheckForWinner()
        {
            //Check Horizontals

            if( board[0] != MarkType.Free && ((board[0] & board[1] & board[2]) == board[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }
            if (board[3] != MarkType.Free && ((board[3] & board[4] & board[5]) == board[3]))
            {
                mGameEnded = true;
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }
            if (board[6] != MarkType.Free && ((board[6] & board[7] & board[8]) == board[6]))
            {
                mGameEnded = true;
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }

            //Check Verticals
            if (board[0] != MarkType.Free && ((board[0] & board[3] & board[6]) == board[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
            }
            if (board[1] != MarkType.Free && ((board[1] & board[4] & board[7]) == board[1]))
            {
                mGameEnded = true;
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
            }
            if (board[2] != MarkType.Free && ((board[2] & board[5] & board[8]) == board[2]))
            {
                mGameEnded = true;
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
            }

            //Check Obliques
            if (board[0] != MarkType.Free && ((board[0] & board[4] & board[8]) == board[0]))
            {
                mGameEnded = true;
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }
            else if (board[2] != MarkType.Free && ((board[2] & board[4] & board[6]) == board[2]))
            {
                mGameEnded = true;
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
            }

            //Check Full Board
            if (!board.Any(result => result == MarkType.Free) && !mGameEnded )
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
