using System;
using System.Windows;
using System.Windows.Input;

namespace AdministratorFormsWPF.View
{
    /// <summary>
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
            this.textBoxValue.PreviewTextInput += new TextCompositionEventHandler(textBoxValue_PreviewTextInput);
        }
        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != "." && e.Text != "-") e.Handled = true;
        }
    }
}
