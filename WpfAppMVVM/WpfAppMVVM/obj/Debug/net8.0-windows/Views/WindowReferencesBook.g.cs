﻿#pragma checksum "..\..\..\..\Views\WindowReferencesBook.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FDAD5A2FB65BC6B80D5C9F35938F93E01ED2745A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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
using WpfAppMVVM.Views;


namespace WpfAppMVVM.Views {
    
    
    /// <summary>
    /// WindowReferencesBook
    /// </summary>
    public partial class WindowReferencesBook : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\Views\WindowReferencesBook.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGridView;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfAppMVVM;component/views/windowreferencesbook.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\WindowReferencesBook.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 16 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonCars_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 17 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonCarBrands_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 18 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonCustomers_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 19 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonDrivers_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 20 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonRoutePoints_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 21 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonRoutes_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 22 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonRussinaBrandNames_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 23 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonStateOrders_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 24 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonTraillers_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 25 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonTransportCompanies_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.dataGridView = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 12:
            
            #line 37 "..\..\..\..\Views\WindowReferencesBook.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonAdd_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
