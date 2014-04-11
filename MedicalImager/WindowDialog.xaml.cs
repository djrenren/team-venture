﻿using System;
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
using System.Windows.Shapes;

namespace MedicalImager
{
    /// <summary>
    /// Interaction logic for WindowDialog.xaml
    /// </summary>
    public partial class WindowDialog : Window
    {
        public WindowDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Min = Convert.ToInt32(MinVal);
            Max = Convert.ToInt32(MaxVal);
            Layout = Command.invoker.Study.Layout;
            List = Command.invoker.Study.Layout.Images;
        }

        public int Min { get; set; }
        public int Max { get; set; }
        public StudyLayout Layout {get; set; }
        public List<VirtualImage> List { get; set; }
    }
}
