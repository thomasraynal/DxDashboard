using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Dx.Dashboard.Common.Infra
{
    public class DefaultExceptionHandler : ICanLog, IExceptionHandler
    {

        private long _lastErrorTimestamp = 0;

        public void Handle(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Handle(e.Exception);
        }

        public void Handle(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject != null && e.ExceptionObject is Exception ? e.ExceptionObject as Exception :
                new Exception("AppDomain throwed an unspecified exception");

            Handle(ex);
        }

        public void Handle(Exception ex)
        {
            this.Error(ex);
            ShowWindow(ex);
        }

        private Window _window;
        private Exception _exception;

        private void ShowWindow(Exception e)
        {
            _exception = e;

            if (_lastErrorTimestamp > 0)
            {
                if (TimeSpan.FromTicks(DateTime.Now.Ticks - _lastErrorTimestamp).Seconds <= 5) return;
            }

            _lastErrorTimestamp = DateTime.Now.Ticks;


            var message = GetMessage();
            _window = new Window()
            {
                Width = 600,
                Height = 400,
                WindowStyle = WindowStyle.ToolWindow,
                ShowActivated = true,
                Title = "Unhandled exception"
            };

            var grid = new Grid() { Margin = new Thickness(5) };
            var closeButton = new Button() { Content = "Close", Margin = new Thickness(3) };

            closeButton.Click += button_Click;
            var copyButton = new Button() { Content = "Copy error", Margin = new Thickness(3) };
            copyButton.Click += copyButton_Click;
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(stackPanel, 1);
            stackPanel.Children.Add(closeButton);
            stackPanel.Children.Add(copyButton);
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.Children.Add(stackPanel);
            grid.Children.Add(new TextBox() { Text = message, Margin = new Thickness(5) });
            _window.Content = grid;
            _window.ShowDialog();

        }

        private string GetMessage()
        {
            var result = new StringBuilder();
            if (Assembly.GetEntryAssembly() != null)
            {
                result.Append("EntryAssembly: ");
                result.Append(Assembly.GetEntryAssembly().Location);
                result.AppendLine();
            }
            result.AppendLine("UnhandledException:");
            PackException(result);
            return result.ToString();
        }

        private void PackException(StringBuilder stringBuilder, int index = 0)
        {
            while (true)
            {
                AppendLine(stringBuilder, _exception.Message, index);

                if (!string.IsNullOrWhiteSpace(_exception.StackTrace))
                {
                    AppendLine(stringBuilder, "StackTrace:", index);
                    AppendLine(stringBuilder, _exception.StackTrace, index);
                }

                if (_exception.InnerException == null) return;
                AppendLine(stringBuilder, "InnerException:", index);
                _exception = _exception.InnerException;
                index = ++index;
            }
        }

        private void AppendLine(StringBuilder stringBuilder, string text, int index)
        {
            var tabOffset = new string('\t', index);
            stringBuilder.Append(tabOffset);
            var regex = new Regex("\r\n*\\s");
            stringBuilder.AppendLine(regex.Replace(text, "\r\n" + tabOffset));
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetMessage());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            _window.Close();
        }
    }
}
