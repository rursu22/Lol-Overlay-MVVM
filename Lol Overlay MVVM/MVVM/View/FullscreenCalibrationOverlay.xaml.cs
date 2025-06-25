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

namespace Lol_Overlay_MVVM.MVVM.View
{
    
    public partial class FullscreenCalibrationOverlay : Window
    {
        public Point? ClickPoint { get; private set; }

        public FullscreenCalibrationOverlay(string instruction)
        {
            InitializeComponent();

            InstructionTextBox.Text = $"Click the {instruction}";
        }

        private void OnOverlayClick(object sender, MouseButtonEventArgs e)
        {
            ClickPoint = PointToScreen(e.GetPosition(this));
            this.DialogResult = true;
        }
    }
}
