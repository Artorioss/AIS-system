﻿#pragma checksum "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BB2D493754C56F3BDE5C180473336948CBBB63D4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using WpfAppMVVM.Behaviors;
using WpfAppMVVM.CustomComponents;
using WpfAppMVVM.Model;
using WpfAppMVVM.Views.OtherViews;


namespace WpfAppMVVM.Views.OtherViews {
    
    
    /// <summary>
    /// CreateDriverWindow
    /// </summary>
    public partial class CreateDriverWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfAppMVVM.Views.OtherViews.CreateDriverWindow Window;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfAppMVVM.CustomComponents.CustomComboBox comboBoxTransportCompanies;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfAppMVVM.CustomComponents.CustomComboBox comboBoxCars;
        
        #line default
        #line hidden
        
        
        #line 140 "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfAppMVVM.CustomComponents.CustomComboBox ComboBoxTraillers;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfAppMVVM;component/views/otherviews/createdriverwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\OtherViews\CreateDriverWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.4.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((WpfAppMVVM.Views.OtherViews.CreateDriverWindow)(target));
            return;
            case 2:
            this.comboBoxTransportCompanies = ((WpfAppMVVM.CustomComponents.CustomComboBox)(target));
            return;
            case 3:
            this.comboBoxCars = ((WpfAppMVVM.CustomComponents.CustomComboBox)(target));
            return;
            case 4:
            this.ComboBoxTraillers = ((WpfAppMVVM.CustomComponents.CustomComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

