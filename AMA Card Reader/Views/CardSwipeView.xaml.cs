﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMA_Card_Reader.Views
{
    public partial class CardSwipeView : Window
    {
        public string DataString { get; set; }

        public CardSwipeView()
        {
            InitializeComponent();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) ||e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))  
            {
                if (e.Key == Key.V)
                    txtData.Text = Clipboard.GetText();
            }
            else
            {
                txtData.Text = await Framework.Framework.AddKeyToString(e.Key, txtData.Text);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            DataString = txtData.Text;
            this.Close();
        }
    }
}
