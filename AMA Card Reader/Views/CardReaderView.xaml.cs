using AMA_Card_Reader.Models;
using AMA_Card_Reader.ViewModels;
using AMA_Card_Reader.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMA_Card_Reader
{
    public partial class CardReaderView : Window
    {
        private string FilePath;
        private bool EditMode;
        private CardReaderViewModel vm;

        public CardReaderView()
        {
            InitializeComponent();
            vm = this.DataContext as CardReaderViewModel;

            var json = File.ReadAllText("Data/makemodels.json");

            var listOfVehicles = JsonConvert.DeserializeObject<VehicleList>(json);

            foreach (var vehicle in listOfVehicles.Vehicles)
            {
                vm.Vehicles.Add(vehicle);
            }

            cbMake.Items.Refresh();
            cbModels.Items.Refresh();
        }

        private AMACardEntry GetEntryFromUIElements()
        {
            try
            {
                var entry = new AMACardEntry();
                entry.RowNumber = vm.Entries.Count + 1;
                entry.Firstname = txtFirstName.Text.Trim();
                entry.Lastname = txtLastName.Text.Trim();
                entry.Address = txtAddress.Text.Trim();
                entry.City = txtCity.Text.Trim();
                entry.State = txtState.Text.Trim();
                entry.Zipcode = txtZipcode.Text.Trim();
                entry.AMACardNumber = txtAMANumber.Text.Trim();
                entry.Expiration = txtExpiration.Text.Trim();
                entry.RiderNumber = txtRideNumber.Text.Trim();
                entry.RideType = rbAdventure.IsChecked.Value ? "Adventure" : "Dual Sport";
                entry.Phone = txtPhoneNumber.Text.Trim();
                entry.Email = txtEmail.Text.Trim();
                entry.Make = cbMake.Text ?? "";
                entry.Model = cbModels.Text ?? "";
                entry.Year = txtYear.Text.Trim();
                entry.CC = txtCC.Text.Trim();
                entry.Day1 = checkboxDay1.IsChecked.Value ? "X" : "";
                entry.Day2 = checkboxDay2.IsChecked.Value ? "X" : "";
                entry.Barbecue = txtNumBarbecue.Text.Trim();
                entry.Camping = checkboxCamping.IsChecked.Value ? "X" : "";
                entry.TShirt = txtNumShirts.Text;
                entry.MannerOfPayment = cbPaymentMethod.Text ?? "";
                entry.PaidAmount = txtAmount.Text.Trim();
                entry.AMAPaidAmount = txtAMAAmount.Text.Trim();
                return entry;
            }
            catch (Exception Ex)
            {
                var bp = Ex.Message;
                return null;
            }
        }

        private void SetUIElementsToCardData(AMACardEntry entry)
        {
            ClearUIScreen(false);
            if (entry != null)
            {
                txtFirstName.Text = entry.Firstname;
                txtLastName.Text = entry.Lastname;
                txtAddress.Text = entry.Address;
                txtCity.Text = entry.City;
                txtState.Text = entry.State;
                txtZipcode.Text = entry.Zipcode;
                txtAMANumber.Text = entry.AMACardNumber;
                txtExpiration.Text = entry.Expiration;
                txtRideNumber.Text = entry.RiderNumber;
                txtEmail.Text = entry.Email;
                txtPhoneNumber.Text = entry.Phone;
                txtYear.Text = entry.Year;
                txtAmount.Text = entry.PaidAmount;
                txtCC.Text = entry.CC;
                txtNumBarbecue.Text = entry.Barbecue;
                txtNumShirts.Text = entry.TShirt;
                txtAMAAmount.Text = entry.AMAPaidAmount;

                double value = 0;
                if (Double.TryParse(entry.PaidAmount, out double dabbersPaid))
                {
                    if (Double.TryParse(entry.AMAPaidAmount, out double amaPaid))
                    {
                        value = dabbersPaid + amaPaid;
                    }
                    else
                    {
                        value = dabbersPaid;
                    }
                }
                else if (Double.TryParse(entry.AMAPaidAmount, out double amaPaid))
                {
                    value = amaPaid;
                }

                txtCombinedPayment.Text = $"${value.ToString()}";

                if (entry.RideType == "Dual Sport")
                    rbDualSport.IsChecked = true;
                else if (entry.RideType == "Adventure")
                    rbAdventure.IsChecked = true;

                if (entry.AMAPaidAmount == "49.00")
                    rbAnnual.IsChecked = true;
                else if (entry.AMAPaidAmount == "20.00")
                    rb1Day.IsChecked = true;
                else
                    rbNone.IsChecked = true;

                checkboxDay1.IsChecked = entry.Day1 == "X";
                checkboxDay2.IsChecked = entry.Day2 == "X";
                checkboxCamping.IsChecked = entry.Camping == "X";

                foreach (ComboBoxItem comboitem in cbPaymentMethod.Items)
                {
                    if (comboitem.Content.ToString() == entry.MannerOfPayment)
                    {
                        cbPaymentMethod.SelectedValue = comboitem;
                    }
                }

                var vehicleMakeMatch = vm.Vehicles.FirstOrDefault(x => x.Name == entry.Make);
                if (vehicleMakeMatch == null)
                {
                    cbMake.SelectedIndex = -1;
                    cbMake.Text = entry.Make;
                    cbModels.SelectedIndex = -1;
                    cbModels.Text = entry.Model;
                    return;
                }
                else
                    cbMake.SelectedItem = vehicleMakeMatch;

                var vehicleModelMatch = vehicleMakeMatch.Models.FirstOrDefault(x => x == entry.Model);
                if (vehicleModelMatch == null)
                {
                    cbModels.SelectedIndex = -1;
                    cbModels.Text = entry.Model;
                }
                else
                    cbModels.SelectedItem = vehicleModelMatch;
            }
        }

        private void UpdateEntryStatistics()
        {
            var numBBQ = vm.Entries.Where(x => !string.IsNullOrWhiteSpace(x.Barbecue)).Select(x => Convert.ToInt32(x.Barbecue)).Sum();
            var cumulativePayment = vm.Entries.Where(x => !string.IsNullOrWhiteSpace(x.PaidAmount)).Select(x => Convert.ToDouble(x.PaidAmount)).Sum();
            var cumulativeAmaPayment = vm.Entries.Where(x => !string.IsNullOrWhiteSpace(x.AMAPaidAmount)).Select(x => Convert.ToDouble(x.AMAPaidAmount)).Sum();
            txtNumRemainingBarbecue.Text = $"({numBBQ} Total)";
            txtTotalCumulativePayment.Text = $"(${cumulativePayment} Total)";
            txtTotalAMACumulativePayment.Text = $"(${cumulativeAmaPayment} Total)";
        }

        private void ClearUIScreen(bool resetButtonVisibility = true)
        {
            if (resetButtonVisibility)
            {
                btnLoadData.Visibility = Visibility.Visible;
                BtnAddEntry.Visibility = Visibility.Visible;
                BtnClear.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
                BtnUpdate.Visibility = Visibility.Collapsed;
            }

            EditMode = false;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZipcode.Text = string.Empty;
            txtAMANumber.Text = string.Empty;
            txtExpiration.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtCC.Text = string.Empty;
            txtAMAAmount.Text = string.Empty;
            txtNumShirts.Text = string.Empty;
            txtNumBarbecue.Text = string.Empty;
            txtRideNumber.Text = string.Empty;
            rbAdventure.IsChecked = false;
            rbDualSport.IsChecked = false;
            rbNone.IsChecked = false;
            rb1Day.IsChecked = false;
            rbAnnual.IsChecked = false;
            checkboxDay1.IsChecked = false;
            checkboxDay2.IsChecked = false;
            checkboxCamping.IsChecked = false;
            cbMake.SelectedIndex = -1;
            cbModels.SelectedIndex = -1;
            cbPaymentMethod.SelectedIndex = -1;
            UpdateEntryStatistics();
        }

        private void RewriteCSVFile()
        {
            File.Delete(FilePath);
            bool first = true;
            foreach (var entryObj in vm.Entries)
            {
                var csvRow = entryObj.ToCSVRow(!first);
                File.AppendAllText(FilePath, csvRow);
                first = false;
            }
        }

        private void ReadFile()
        {
            ClearUIScreen();
            vm.Entries.Clear();

            var data = File.ReadAllText(FilePath);
            if (!string.IsNullOrWhiteSpace(data))
            {
                var listOfEntries = data.Split('\r');
                int index = 1;

                bool error = false;
                foreach (var csvRow in listOfEntries)
                {
                    try
                    {
                        var entry = AMACardEntry.ParseEntryFromCSVRow(csvRow, index++);
                        vm.Entries.Add(entry);
                    }
                    catch
                    {
                        error = true;
                    }
                }

                UpdateEntryStatistics();

                if (error) MessageBox.Show("There was an error while trying to read this CSV File. Some rows may not have loaded.");
            }

            outerGrid.Visibility = Visibility.Visible;
        }

        private bool OpenFile()
        {
            var openfile = new OpenFileDialog() { Filter = "CSV Files | *.csv" };

            if (openfile.ShowDialog() == true)
            {
                FilePath = openfile.FileName;
                txtFileName.Text = Path.GetFileName(FilePath);
                return true;
            }
            return false;
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFile()) ReadFile();
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
            var saveFile = new SaveFileDialog() { FileName = "default", DefaultExt = "csv", Filter = "CSV Files | *.csv" };

            if (saveFile.ShowDialog() == true)
            {
                FilePath = saveFile.FileName;
                outerGrid.Visibility = Visibility.Visible;
                if (!File.Exists(FilePath)) File.Create(FilePath);
            }
        }

        private void BtnScanData_Click(object sender, RoutedEventArgs e)
        {
            var window = new CardSwipeView() { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            window.ShowDialog();

            var data = window.DataString;
            if (data?.Length >= 101)
            {
                try
                {
                    var entry = AMACardEntry.ParseEntryFromCardData(data, vm.Entries.Count + 1);

                    int nextRiderNumber = 1;

                    var last = vm.Entries.LastOrDefault();
                    if (last != null && int.TryParse(last.RiderNumber, out int result))
                    {
                        nextRiderNumber = result + 1;
                    }

                    entry.RiderNumber = nextRiderNumber.ToString();

                    SetUIElementsToCardData(entry);
                    rbDualSport.IsChecked = true;
                    rbNone.IsChecked = true;
                }
                catch
                {
                    MessageBox.Show("There was a problem trying to parse your card data. Please try reading again.");
                }
            }
        }

        private void BtnAddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (txtFirstName.Text.Length < 1 || txtAMANumber.Text.Length != 7)
            {
                MessageBox.Show("One of the following requirements were not met:\r\r1.Name must be at least 2 characters long\r2.AMA Must be a 7 digits long");
            }
            else
            {
                var entry = GetEntryFromUIElements();
                var csvRow = entry.ToCSVRow(vm.Entries.Count > 0);
                File.AppendAllText(FilePath, csvRow);

                vm.Entries.Add(entry);
                ClearUIScreen();
            }

            BtnDelete.Visibility = Visibility.Collapsed;
            BtnClear.Visibility = Visibility.Collapsed;
            BtnUpdate.Visibility = Visibility.Collapsed;

            ClearUIScreen();

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            lvAMAEntries.SelectedIndex = -1;
            ClearUIScreen();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var entry = lvAMAEntries.SelectedItem as AMACardEntry;
            BtnClear.Visibility = Visibility.Collapsed;
            BtnDelete.Visibility = Visibility.Collapsed;
            BtnUpdate.Visibility = Visibility.Collapsed;

            vm.Entries.Remove(entry);

            foreach (var entryRow in vm.Entries)
            {
                entryRow.RowNumber = vm.Entries.IndexOf(entryRow) + 1;
            }

            lvAMAEntries.Items.Refresh();

            if (vm.Entries.Count > 0)
            {
                lvAMAEntries.SelectedIndex = 0;
            }

            RewriteCSVFile();
            ClearUIScreen();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditMode = false;
            BtnUpdate.Visibility = Visibility.Collapsed;

            var index = lvAMAEntries.SelectedIndex;
            var updatedEntry = GetEntryFromUIElements();
            var item = lvAMAEntries.SelectedItem as AMACardEntry;
            var rowNumber = item.RowNumber;

            updatedEntry.RowNumber = rowNumber;
            vm.Entries[rowNumber - 1] = updatedEntry;

            RewriteCSVFile();
            SetUIElementsToCardData(updatedEntry);
            lvAMAEntries.SelectedIndex = index;
        }

        private void LvAMAEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AMACardEntry old = null;
            if (lvAMAEntries.SelectedIndex == -1) return;

            btnLoadData.Visibility = Visibility.Collapsed;
            BtnAddEntry.Visibility = Visibility.Collapsed;

            if (e.RemovedItems.Count > 0)
                old = e.RemovedItems[0] as AMACardEntry;
            if (e.AddedItems.Count > 0) { };

            if (EditMode)
            {
                var result = MessageBox.Show("You are currently editting a record. If you leave now changes will be lost. Press OK to continue. Press Cancel to stay on the current page and continue editing.", "Undo Changes", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    EditMode = false;
                    BtnUpdate.Visibility = Visibility.Collapsed;

                    var entry = (sender as ListView).SelectedItem as AMACardEntry;

                    BtnClear.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Visible;

                    SetUIElementsToCardData(entry);
                }
                else if (old != null)
                {
                    lvAMAEntries.SelectionChanged -= LvAMAEntries_SelectionChanged;
                    lvAMAEntries.SelectedIndex = old.RowNumber - 1;
                    lvAMAEntries.SelectionChanged += LvAMAEntries_SelectionChanged;
                }
            }
            else
            {
                var entry = (sender as ListView).SelectedItem as AMACardEntry;

                BtnClear.Visibility = Visibility.Visible;
                BtnDelete.Visibility = Visibility.Visible;
                SetUIElementsToCardData(entry);
            }
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lvAMAEntries.SelectedIndex > -1)
            {
                EditMode = true;
                BtnUpdate.Visibility = Visibility.Visible;
            }
        }

        private void OnlyAllow1ThroughN_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var n = Convert.ToInt32((sender as TextBox).DataContext);

            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            var value = int.Parse(e.Text);
            if (value <= 0 || value > n)
            {
                e.Handled = true;
                return;
            }
        }

        private void OnlyAllowNumbers_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void OnlyAllowCurrency_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textbox = sender as TextBox;
            var isDigit = char.IsDigit(e.Text, e.Text.Length - 1);

            if (!isDigit && e.Text != ".")
            {
                e.Handled = true;
            }
            else if (!isDigit && textbox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void UpdatePaidAmount(object sender, RoutedEventArgs e)
        {
            UpdatePaidAmount();
        }

        private void UpdatePaidAmount(object sender, TextChangedEventArgs e)
        {
            UpdatePaidAmount();
        }

        void UpdatePaidAmount()
        {
            var amount = 0;

            if (checkboxDay1.IsChecked.Value)
            {
                amount += 50;
            }
            if (checkboxDay2.IsChecked.Value)
            {
                amount += 50;
            }
            if (checkboxCamping.IsChecked.Value)
            {
                amount += 10;
            }
            if (txtNumShirts.Text.Length > 0 && int.TryParse(txtNumShirts.Text, out int numShirts))
            {
                amount += (numShirts * 15);
            }
            if (txtNumBarbecue.Text.Length > 0 && int.TryParse(txtNumBarbecue.Text, out int numBBQ))
            {
                amount += (numBBQ * 10);
            }

            txtAmount.Text = amount > 0 ? amount.ToString() : "";
            SetTotalPaid();

        }

        private void RbAnnual_Checked(object sender, RoutedEventArgs e)
        {
            txtAMAAmount.Text = "49.00";
            SetTotalPaid();
        }

        private void Rb1Day_Checked(object sender, RoutedEventArgs e)
        {
            txtAMAAmount.Text = "20.00";
            SetTotalPaid();
        }

        private void RbNone_Checked(object sender, RoutedEventArgs e)
        {
            txtAMAAmount.Text = "";
            SetTotalPaid();
        }

        private void SetTotalPaid()
        {
            double value = 0;
            if (Double.TryParse(txtAmount.Text, out double dabbersPaid))
            {
                if (Double.TryParse(txtAMAAmount.Text, out double amaPaid))
                {
                    value = dabbersPaid + amaPaid;
                }
                else
                {
                    value = dabbersPaid;
                }
            }
            else if (Double.TryParse(txtAMAAmount.Text, out double amaPaid))
            {
                value = amaPaid;
            }
            txtCombinedPayment.Text = $"${value.ToString()}";
        }

        private void TxtNumBarbecue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            var value = int.Parse(e.Text);
            if (value <= 0 || value > 3)
            {
                e.Handled = true;
                return;
            }

            var numBBQ = vm.Entries.Where(x => !string.IsNullOrWhiteSpace(x.Barbecue)).Select(x => Convert.ToInt32(x.Barbecue)).Sum();

            if (numBBQ + value > 150)
            {
                MessageBox.Show($"There are {numBBQ} BBQ units sold already. There is a limit of {150 - numBBQ} remaining.");
                e.Handled = true;
                return;
            }
        }
    }
}
