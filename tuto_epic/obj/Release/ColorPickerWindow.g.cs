﻿#pragma checksum "..\..\ColorPickerWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "25663A5943A817D76A75A243124EDE09C864F268F1F39D7F055D2351A93E25DB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using tuto_epic;


namespace tuto_epic {
    
    
    /// <summary>
    /// ColorPickerWindow
    /// </summary>
    public partial class ColorPickerWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ColorPickerCanvas;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ColorDescrTBlock;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OkButton;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle sampleRec;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBoxR;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBoxG;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBoxB;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SliderR;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SliderG;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SliderB;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton FillRB;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\ColorPickerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton OutRB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/tuto_epic;component/colorpickerwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ColorPickerWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\ColorPickerWindow.xaml"
            ((tuto_epic.ColorPickerWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ColorPickerCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 16 "..\..\ColorPickerWindow.xaml"
            this.ColorPickerCanvas.Loaded += new System.Windows.RoutedEventHandler(this.ColorPickerCanvas_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ColorDescrTBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.OkButton = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\ColorPickerWindow.xaml"
            this.OkButton.Click += new System.Windows.RoutedEventHandler(this.OkButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.sampleRec = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 6:
            this.TBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.TBoxG = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.TBoxB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.SliderR = ((System.Windows.Controls.Slider)(target));
            
            #line 32 "..\..\ColorPickerWindow.xaml"
            this.SliderR.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderR_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.SliderG = ((System.Windows.Controls.Slider)(target));
            
            #line 33 "..\..\ColorPickerWindow.xaml"
            this.SliderG.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderG_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.SliderB = ((System.Windows.Controls.Slider)(target));
            
            #line 34 "..\..\ColorPickerWindow.xaml"
            this.SliderB.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderB_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.FillRB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 38 "..\..\ColorPickerWindow.xaml"
            this.FillRB.Checked += new System.Windows.RoutedEventHandler(this.FillRB_Checked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.OutRB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 39 "..\..\ColorPickerWindow.xaml"
            this.OutRB.Checked += new System.Windows.RoutedEventHandler(this.OutRB_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

