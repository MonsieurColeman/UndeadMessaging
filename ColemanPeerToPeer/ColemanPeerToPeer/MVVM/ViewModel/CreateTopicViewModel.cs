
/*
    This file contaions the GUI logic for the Create Topic Window
*/

using ColemanPeerToPeer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColemanPeerToPeer
{
    /// <summary>
    /// Interaction logic for CreateTopic.xaml
    /// </summary>
    public partial class CreateTopicView : Window
    {
        public CreateTopicView()
        {
            InitializeComponent();
        }

        private void Win_Btn_CreateTopic(object sender, RoutedEventArgs e)
        //Topic creation behavior resulting from button event 
        {
            //do nothing if nothing is in the textbox
            if (String.IsNullOrWhiteSpace(newTopicTextbox.Text))
                return;

            //truncate string
            string newTopicName = newTopicTextbox.Text;
            if (newTopicName.Length > 15)
                newTopicName = newTopicTextbox.Text.Substring(0, 15);

            //make sure the proposed topic name actually has letters
            //Note: errors occur when it does not have letters
            if (!Regex.IsMatch(newTopicName, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show(GlobalStrings.inputValidation_TopicCreation);
                return;
            }

            Client.CreateTopic(GlobalStrings.tag_TopicCreation+newTopicName);
            this.Close();
        }

        private void Win_Btn_CancelTopicCreation(object sender, RoutedEventArgs e)
        //A button behavior that closes the window
        {
            this.Close();
        }
    }
}

/*
 Maintenance History

0.7 Added Topic Creation functionality
0.8 Added Input validation
1.0 Added comments 
 */