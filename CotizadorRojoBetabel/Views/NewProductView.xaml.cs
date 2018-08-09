using CotizadorRojoBetabel.Models;
using ServiceStack.OrmLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CotizadorRojoBetabel.Views
{
    /// <summary>
    /// Interaction logic for NewProductView.xaml
    /// </summary>
    public partial class NewProductView : UserControl
    {
        private decimal _cost;
        private Products _product;

        internal decimal Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                CostTxt.Text = value.ToString("C");
            }
        }

        public NewProductView(Products product = null)
        {
            _product = product;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //limit characters on percent fields
            WasteTxt.MaxLength = 3;
            YieldTxt.MaxLength = 3;

            //load CategoryCombo
            var CategoryEnums = Enum.GetNames(typeof(ProductCategory)).ToList();
            var CategoryItems = new List<string>
            {
                "-Seleccione una opción-"
            };
            foreach (var productCategory in CategoryEnums)
            {
                CategoryItems.Add(productCategory);
            }
            CategoryCmb.ItemsSource = CategoryItems;

            //load PackageUnitCombo
            var UnitEnums = Enum.GetNames(typeof(PackageUnit)).ToList();
            var UnitItems = new List<string>
            {
                "-Seleccione una opción-"
            };
            foreach (var packageUnit in UnitEnums)
            {
                UnitItems.Add(packageUnit);
            }
            UnitCmb.ItemsSource = UnitItems;

            if (_product == null)
            {
                //initialize values
                Cost = 0;
                CategoryCmb.SelectedIndex = 0;
                UnitCmb.SelectedIndex = 0;
            }
            else
            {
                //initialize values
                ProductTxt.Text = _product.Name;
                CategoryCmb.SelectedValue = _product.Category.ToString();
                PriceTxt.Text = _product.Price.ToString();
                UnitCmb.SelectedValue= _product.Unit.ToString();
                WeightTxt.Text = _product.Weight.ToString();
                WasteTxt.Text = _product.Waste.ToString();
                YieldTxt.Text = _product.Yield.ToString();
                Cost = _product.Cost;
                CleanBtn.IsEnabled = false;
            }
        }

        private void ResetFields()
        {
            ProductTxt.Text = string.Empty;
            CategoryCmb.SelectedIndex = 0;
            PriceTxt.Text = string.Empty;
            UnitCmb.SelectedIndex = 0;
            WeightTxt.Text = string.Empty;
            WasteTxt.Text = string.Empty;
            YieldTxt.Text = string.Empty;
            CostTxt.Text = "$0.00";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ProductTxt.Text;
            var categoryParsed = Enum.TryParse(CategoryCmb.Text, out ProductCategory category);
            var priceParsed = decimal.TryParse(PriceTxt.Text, out decimal price);
            var packageUnitParsed = Enum.TryParse(UnitCmb.Text, out PackageUnit packageUnit);
            var weightParsed = decimal.TryParse(WeightTxt.Text, out decimal weight);
            var wasteParsed = decimal.TryParse(WasteTxt.Text, out decimal waste);
            var yieldParsed = decimal.TryParse(YieldTxt.Text, out decimal yield);

            if (name == "" || name == string.Empty)
            {
                WarningTbk.Text = "Ingrese el nombre del producto";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!categoryParsed || CategoryCmb.Text == "-Seleccione una opción-")
            {
                WarningTbk.Text = "Verifique que haya seleccionado una categoría";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!priceParsed)
            {
                WarningTbk.Text = "Ingrese el precio del producto";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!packageUnitParsed || UnitCmb.Text == "-Seleccione una opción-")
            {
                WarningTbk.Text = "Verifique que haya seleccionado una unidad de medida";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!weightParsed)
            {
                WarningTbk.Text = "Ingrese el peso del producto";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!wasteParsed)
            {
                WarningTbk.Text = "Ingrese el porcentaje de merma";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!yieldParsed)
            {
                WarningTbk.Text = "Verifique que todos los datos se hayan ingresado";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (Cost <= 0)
            {
                WarningTbk.Text = "Verifique que todos los datos se hayan ingresado";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else
            {
                if (_product != null)
                {
                    try
                    {
                        using (var db = App.DbFactory.Open())
                        {
                            db.Update(new Products
                            {
                                Id = _product.Id,
                                Name = name,
                                Category = category,
                                Price = price,
                                Unit = packageUnit,
                                Weight = weight,
                                Waste = waste,
                                Yield = yield,
                                Cost = Cost
                            });
                        }
                        ParentView.Show_MessageView("El producto se ha actualizado con éxito\n¿Deseas guardar uno nuevo?",
                            //affirmative action
                            delegate
                            {
                                ParentView.Show_NewProductView();
                            },
                            null,
                            //negative action
                            delegate
                            {
                                ParentView.Show_ProductsView();
                            },
                            null,
                            FontAwesome.WPF.FontAwesomeIcon.CheckCircle
                            );
                    }
                    catch (Exception)
                    {
                        ParentView.Show_MessageView("Hubo un problema al actualizar el producto\nComuniquese a soporte técnico",
                            //affirmative action
                            delegate
                            {
                                ParentView.Show_NewProductView();
                            },
                            "Aceptar",
                            //negative action
                            null,
                            null,
                            FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                            );
                    }
                }
                else
                {
                    var product = new Products
                    {
                        Name = name,
                        Category = category,
                        Price = price,
                        Unit = packageUnit,
                        Weight = weight,
                        Waste = waste,
                        Yield = yield,
                        Cost = Cost
                    };
                    try
                    {
                        using (var db = App.DbFactory.Open())
                        {
                            db.Save(product);
                        }
                        ParentView.Show_MessageView("El producto se ha guardado con éxito\n¿Deseas guardar otro?",
                            //affirmative action
                            delegate
                            {
                                ParentView.Show_NewProductView();
                            },
                            null,
                            //negative action
                            delegate
                            {
                                ParentView.Show_ProductsView();
                            },
                            null,
                            FontAwesome.WPF.FontAwesomeIcon.CheckCircle
                            );
                    }
                    catch (Exception)
                    {
                        ParentView.Show_MessageView("Hubo un problema al agregar el producto\nComuniquese a soporte técnico",
                            //affirmative action
                            delegate
                            {
                                ParentView.Show_NewProductView();
                            },
                            "Aceptar",
                            //negative action
                            null,
                            null,
                            FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                            );
                    }
                }
            }
        }

        private void CleanBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_ProductsView();
        }

        private void PriceTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void WasteTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void YieldTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void InitialWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void YieldFactorTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void FinalWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void DrainWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void ProductTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningTbk.Visibility = Visibility.Hidden;
        }

        private void CategoryCmb_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningTbk.Visibility = Visibility.Hidden;
        }

        private void PriceTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetYield();
            GetCost();
        }

        private void WeightTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetYield();
            GetCost();
        }

        private void WasteTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetYield();
            GetCost();
        }

        private void GetCost()
        {
            var weightParsed = decimal.TryParse(WeightTxt.Text, out decimal weight);
            var yieldParsed = decimal.TryParse(YieldTxt.Text, out decimal yield);
            var priceParsed = decimal.TryParse(PriceTxt.Text, out decimal price);

            if (!weightParsed)
            {
                weight = 0;
            }

            if (!yieldParsed)
            {
                yield = 0;
            }

            if (!priceParsed)
            {
                price = 0;
            }

            try
            {
                var yieldPercent = yield / 100;
                var yieldByWeight = weight / yieldPercent;
                Cost = Math.Round(yieldByWeight * price, 2);
            }
            catch (DivideByZeroException)
            {
                Cost = 0;
            }            
        }

        private void GetYield()
        {
            var wasteParsed = decimal.TryParse(WasteTxt.Text, out decimal waste);

            if (!wasteParsed)
            {
                waste = 0;
            }

            var yield = 100 - waste;

            YieldTxt.Text = yield.ToString();
        }
    }
}
