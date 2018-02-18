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
using System.IO;
using System.Text.RegularExpressions;

namespace MIllionare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Question> questions = new List<Question>();
        int curQuestionPos = 0;
        private int[] int_rand = new int[4];
        Random rnd = new Random();
        List<Button> answers = new List<Button>();
        List<TextBlock> ans_text = new List<TextBlock>();

        public void FillList()
        {
            StreamReader read = new StreamReader("Questions.txt",Encoding.Default);
            string parse = null;
             while(!read.EndOfStream)
             {
                 parse += read.ReadLine();
                 parse += "\n";
             }
             read.Close();
             string pattern = @"\<\d+\.(?<quest>.+)\>\n\^(?<true>(\w+-?\s?)+)\n\*(?<ans1>(\w+-?\s?)+)\n\*(?<ans2>(\w+-?\s?)+)\n\*(?<ans3>(\w+-?\s?)+)\n";
            Regex reg = new Regex(pattern, RegexOptions.Multiline);
             MatchCollection colect = reg.Matches(parse);

             Question temp_q;
            foreach(Match match in colect)
            {
                    temp_q = new Question(
                    match.Groups["quest"].Value, match.Groups["ans1"].Value,
                    match.Groups["ans2"].Value, match.Groups["ans3"].Value,
                    match.Groups["true"].Value, match.Groups["true"].Value);
                    questions.Add(temp_q);
            }

            answers.Add(but1);
            answers.Add(but2);
            answers.Add(but3);
            answers.Add(but4);

            ans_text.Add(ans1);
            ans_text.Add(ans2);
            ans_text.Add(ans3);
            ans_text.Add(ans4);

            ShuffleQuestionList(questions);
        }


        private void ShuffleQuestionList(List<Question> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Question value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
    

        void RandomQuestionsPos()
        {
            for (int i = 0; i < int_rand.Count(); i++)
            {
                int_rand[i] = 0;
            }
            int temp;
            for (int i = 0; i < int_rand.Count(); i++)
            {
                while (int_rand[i] == 0)
                {
                    temp = rnd.Next(1, 5);
                    for (int k = 0; k < int_rand.Count(); k++)
                    {
                        if (int_rand[k] == temp)
                            break;
                        else if (int_rand[k] != 0)
                            continue;
                        else
                        {
                            int_rand[i] = temp;
                            break;
                        }
                    }
                }
            }
        }
        private void NextQuestion()
        {
            RandomQuestionsPos();
            Question_block.Text = questions[curQuestionPos].question;
            ans_text[0].Text = questions[curQuestionPos].answers[int_rand[0] - 1];
            ans_text[1].Text = questions[curQuestionPos].answers[int_rand[1] - 1];
            ans_text[2].Text = questions[curQuestionPos].answers[int_rand[2] - 1];
            ans_text[3].Text = questions[curQuestionPos].answers[int_rand[3] - 1];
        }

        public MainWindow()
        {
            InitializeComponent();
            FillList();
            NextQuestion();

        }

        private void answer_Click(object sender, RoutedEventArgs e)
        {
            ShowAllButtons();
            Button but = sender as Button;
            int pos=0;
            for (int i = 0; i < 4; i++)
            {
                if (answers[i] == but)
                {
                    pos = i;
                    break;
                }
            }

            if (ans_text[pos].Text == questions[curQuestionPos].true_answer)
            {
                if (curQuestionPos == 14)
                {
                    MessageBox.Show("You win 1 000 000$!!!", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                    curQuestionPos = 0;
                    NextQuestion();
                    FiftyFifty.IsEnabled = true;
                    Call.IsEnabled = true;
                    Zal.IsEnabled = true;
                    TrueOne.IsEnabled = true;
                    ListBox.SelectedIndex = 14;
                    ShuffleQuestionList(questions);
                }
                else
                {
                    curQuestionPos++;
                    NextQuestion();
                    ListBox.SelectedIndex--;
                }
            }
            else
            {
                curQuestionPos = 0;
                FiftyFifty.IsEnabled = true;
                Call.IsEnabled = true;
                Zal.IsEnabled = true;
                TrueOne.IsEnabled = true;
                ListBox.SelectedIndex = 14;
                MessageBox.Show("You lost",":(", MessageBoxButton.OK, MessageBoxImage.Information);
                ShuffleQuestionList(questions);
                NextQuestion();
            }
        }
       
        private void ShowAllButtons()
        {
            for (int i = 0; i < 4; i++)
			{
                answers[i].Visibility = Visibility.Visible;
			}
            
        }
        private void fiftyfifty_Click(object sender, RoutedEventArgs e)
        {
            if (FiftyFifty.IsEnabled)
            {
                int pos1 = 0;
                int pos2 = 0;

                do
                {
                    pos1 = rnd.Next(1, 5);
                }
                while (ans_text[pos1 - 1].Text == questions[curQuestionPos].true_answer);


                for (int i = 0; i < 4; i++)
                {
                    if (ans_text[i].Text == questions[curQuestionPos].true_answer)
                    {
                        pos2 = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (i == pos1 - 1 || i == pos2)
                        continue;
                    else
                        answers[i].Visibility = Visibility.Hidden;
                }
                FiftyFifty.IsEnabled = false;
            }
            
        }

        private void Zal_Click(object sender, RoutedEventArgs e)
        {
            if (Zal.IsEnabled)
            {
                Auditorium wnd = new Auditorium();
                wnd.Owner = this;
                int pos = 0;
                int true_proc = rnd.Next(40, 60);
                int proc1 = true_proc - 24, proc2 = true_proc - 32, proc3 = true_proc - 17;
                for (int i = 0; i < 4; i++)
                {
                    if (ans_text[i].Text == questions[curQuestionPos].true_answer)
                    {
                        pos = i;
                        break;
                    }
                }
                double temp_proc = 0;
                if (pos == 0)
                {
                    temp_proc = true_proc - wnd.A_rect.Height;
                    wnd.A_percent.Text = true_proc.ToString() + " %";
                    wnd.A_rect.Height = true_proc ;
                    wnd.A_rect.Margin = new Thickness(wnd.A_rect.Margin.Left, wnd.A_rect.Margin.Top - temp_proc, wnd.A_rect.Margin.Right, wnd.A_rect.Margin.Bottom); 

                    temp_proc = proc3 - wnd.B_rect.Height;
                    wnd.B_percent.Text = proc3.ToString() + " %";
                    wnd.B_rect.Height = proc3;
                    wnd.B_rect.Margin = new Thickness(wnd.B_rect.Margin.Left, wnd.B_rect.Margin.Top - temp_proc, wnd.B_rect.Margin.Right, wnd.B_rect.Margin.Bottom);

                    temp_proc = proc2 - wnd.C_rect.Height;
                    wnd.C_percent.Text = proc2.ToString() + " %";
                    wnd.C_rect.Height = proc2;
                    wnd.C_rect.Margin = new Thickness(wnd.C_rect.Margin.Left, wnd.C_rect.Margin.Top - temp_proc, wnd.C_rect.Margin.Right, wnd.C_rect.Margin.Bottom);

                    temp_proc = proc1 - wnd.D_rect.Height;
                    wnd.D_percent.Text = proc1.ToString() + " %";
                    wnd.D_rect.Height = proc1;
                    wnd.D_rect.Margin = new Thickness(wnd.D_rect.Margin.Left, wnd.D_rect.Margin.Top - temp_proc, wnd.D_rect.Margin.Right, wnd.D_rect.Margin.Bottom); 

                }
                else if (pos == 1)
                {
                    temp_proc = proc2 - wnd.A_rect.Height;
                    wnd.A_percent.Text = proc2.ToString() + " %";
                    wnd.A_rect.Height = proc2;
                    wnd.A_rect.Margin = new Thickness(wnd.A_rect.Margin.Left, wnd.A_rect.Margin.Top - temp_proc, wnd.A_rect.Margin.Right, wnd.A_rect.Margin.Bottom);

                    temp_proc = true_proc - wnd.B_rect.Height;
                    wnd.B_percent.Text = true_proc.ToString() + " %";
                    wnd.B_rect.Height = true_proc;
                    wnd.B_rect.Margin = new Thickness(wnd.B_rect.Margin.Left, wnd.B_rect.Margin.Top - temp_proc, wnd.B_rect.Margin.Right, wnd.B_rect.Margin.Bottom);

                    temp_proc = proc3 - wnd.C_rect.Height;
                    wnd.C_percent.Text = proc3.ToString() + " %";
                    wnd.C_rect.Height = proc3;
                    wnd.C_rect.Margin = new Thickness(wnd.C_rect.Margin.Left, wnd.C_rect.Margin.Top - temp_proc, wnd.C_rect.Margin.Right, wnd.C_rect.Margin.Bottom);

                    temp_proc = proc1 - wnd.D_rect.Height;
                    wnd.D_percent.Text = proc1.ToString() + " %";
                    wnd.D_rect.Height = proc1;
                    wnd.D_rect.Margin = new Thickness(wnd.D_rect.Margin.Left, wnd.D_rect.Margin.Top - temp_proc, wnd.D_rect.Margin.Right, wnd.D_rect.Margin.Bottom);
                }
                else if (pos == 2)
                {
                    temp_proc = proc3 - wnd.A_rect.Height;
                    wnd.A_percent.Text =  proc3.ToString() + " %";
                    wnd.A_rect.Height = proc3;
                    wnd.A_rect.Margin = new Thickness(wnd.A_rect.Margin.Left, wnd.A_rect.Margin.Top - temp_proc, wnd.A_rect.Margin.Right, wnd.A_rect.Margin.Bottom);

                    temp_proc = proc1 - wnd.B_rect.Height;
                    wnd.B_percent.Text =  proc1.ToString() + " %";
                    wnd.B_rect.Height = proc1;
                    wnd.B_rect.Margin = new Thickness(wnd.B_rect.Margin.Left, wnd.B_rect.Margin.Top - temp_proc, wnd.B_rect.Margin.Right, wnd.B_rect.Margin.Bottom);

                    temp_proc = true_proc - wnd.C_rect.Height;
                    wnd.C_percent.Text =  true_proc.ToString() + " %";
                    wnd.C_rect.Height = true_proc;
                    wnd.C_rect.Margin = new Thickness(wnd.C_rect.Margin.Left, wnd.C_rect.Margin.Top - temp_proc, wnd.C_rect.Margin.Right, wnd.C_rect.Margin.Bottom);

                    temp_proc = proc2 - wnd.D_rect.Height;
                    wnd.D_percent.Text =  proc2.ToString() + " %";
                    wnd.D_rect.Height = proc2;
                    wnd.D_rect.Margin = new Thickness(wnd.D_rect.Margin.Left, wnd.D_rect.Margin.Top - temp_proc, wnd.D_rect.Margin.Right, wnd.D_rect.Margin.Bottom);
                }
                else if (pos == 3)
                {
                    temp_proc = proc2 - wnd.A_rect.Height;
                    wnd.A_percent.Text =  proc2.ToString() + " %";
                    wnd.A_rect.Height = proc2;
                    wnd.A_rect.Margin = new Thickness(wnd.A_rect.Margin.Left, wnd.A_rect.Margin.Top - temp_proc, wnd.A_rect.Margin.Right, wnd.A_rect.Margin.Bottom);

                    temp_proc = proc1 - wnd.B_rect.Height;
                    wnd.B_percent.Text =  proc1.ToString() + " %";
                    wnd.B_rect.Height = proc1;
                    wnd.B_rect.Margin = new Thickness(wnd.B_rect.Margin.Left, wnd.B_rect.Margin.Top - temp_proc, wnd.B_rect.Margin.Right, wnd.B_rect.Margin.Bottom);

                    temp_proc = proc3 - wnd.C_rect.Height;
                    wnd.C_percent.Text =  proc3.ToString() + " %";
                    wnd.C_rect.Height = proc3;
                    wnd.C_rect.Margin = new Thickness(wnd.C_rect.Margin.Left, wnd.C_rect.Margin.Top - temp_proc, wnd.C_rect.Margin.Right, wnd.C_rect.Margin.Bottom);

                    temp_proc = true_proc - wnd.D_rect.Height;
                    wnd.D_percent.Text =  true_proc.ToString() + " %";
                    wnd.D_rect.Height = true_proc;
                    wnd.D_rect.Margin = new Thickness(wnd.D_rect.Margin.Left, wnd.D_rect.Margin.Top - temp_proc, wnd.D_rect.Margin.Right, wnd.D_rect.Margin.Bottom);
                }
                wnd.Show();             
                Zal.IsEnabled = false;
            }
        }

        private void Call_Click(object sender, RoutedEventArgs e)
        {
            if (Call.IsEnabled)
            {
                int pos = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (ans_text[i].Text == questions[curQuestionPos].true_answer)
                    {
                        pos = i;
                        break;
                    }
                }
                MessageBox.Show("Ваш друг Колька считает, что ответ: " + ans_text[pos].Text, "Call", MessageBoxButton.OK, MessageBoxImage.Information);
                Call.IsEnabled = false;
            }
        }

        private void TrueOne_Click(object sender, RoutedEventArgs e)
        {
            if (TrueOne.IsEnabled)
            {
                int pos = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (ans_text[i].Text == questions[curQuestionPos].true_answer)
                    {
                        pos = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (i == pos)
                        continue;
                    else
                        answers[i].Visibility = Visibility.Hidden;
                }
                TrueOne.IsEnabled = false;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox.SelectedIndex = 14 - curQuestionPos;
        }



        
    
        
    }

    
}
