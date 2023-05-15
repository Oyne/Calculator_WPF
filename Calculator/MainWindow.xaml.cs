using Operations;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Max positive value that user can input.
        /// </summary>
        const double MaxPositive = 5000000;
        /// <summary>
        /// Min negative value that user can input.
        /// </summary>
        const double MinNegative = -3000000;

        /// <summary>
        /// Max lenght of number
        /// </summary>
        const int BigNumber = 19;

        /// <summary>
        /// Instanse of Digits class, for usin it instead of two variables.
        /// </summary>
        /// 
        Digits digits = new Digits();

        /// <summary>
        /// Enum for operations.
        /// </summary>
        enum OperationNumber
        {
            NoOperation,
            // Binary operations:
            Add,
            Subtract,
            Multiply,
            Divide,
            // Unary operations:
            SquareRoot,
            Cos,
            OneDividedBy
        }

        /// <summary>
        /// Number of operation.
        /// </summary>
        OperationNumber operation = OperationNumber.NoOperation;

        /// <summary>
        /// Sign variable.
        /// </summary> 
        String sign = "";

        // Exception for numbers that are too big to display.
        Exception BigNumberException = new Exception("Too big number");
        // Exception for calculation without operation.
        Exception NoOperationException = new Exception("No operation choosed");

        public void Calculator_Loaded(object sender, EventArgs e)
        {
            DigitTextBox.Text = "0";
        }

        /// <summary>
        /// Disables all digit and operation buttons.
        /// </summary>
        private void DisableDigitAndOperations()
        {
            ZeroButton.IsEnabled = false;
            OneButton.IsEnabled = false;
            TwoButton.IsEnabled = false;
            ThreeButton.IsEnabled = false;
            FourButton.IsEnabled = false;
            FiveButton.IsEnabled = false;
            SixButton.IsEnabled = false;
            SevenButton.IsEnabled = false;
            EightButton.IsEnabled = false;
            NineButton.IsEnabled = false;
            ComaButton.IsEnabled = false;
            SignButton.IsEnabled = false;
            AddButton.IsEnabled = false;
            SubtractButton.IsEnabled = false;
            MultiplyButton.IsEnabled = false;
            DivideButton.IsEnabled = false;
            OneDivideByButton.IsEnabled = false;
            CosButton.IsEnabled = false;
            SquareRootButton.IsEnabled = false;
            EqualButton.IsEnabled = false;
            BackspaceButton.IsEnabled = false;
            ClearEntryButton.IsEnabled = false;
        }

        /// <summary>
        /// Enables all digit and operation buttons.
        /// </summary>
        private void EnableDigitAndOperations()
        {
            ZeroButton.IsEnabled = true;
            OneButton.IsEnabled = true;
            TwoButton.IsEnabled = true;
            ThreeButton.IsEnabled = true;
            FourButton.IsEnabled = true;
            FiveButton.IsEnabled = true;
            SixButton.IsEnabled = true;
            SevenButton.IsEnabled = true;
            EightButton.IsEnabled = true;
            NineButton.IsEnabled = true;
            ComaButton.IsEnabled = true;
            SignButton.IsEnabled = true;
            AddButton.IsEnabled = true;
            SubtractButton.IsEnabled = true;
            MultiplyButton.IsEnabled = true;
            DivideButton.IsEnabled = true;
            OneDivideByButton.IsEnabled = true;
            CosButton.IsEnabled = true;
            SquareRootButton.IsEnabled = true;
            EqualButton.IsEnabled = true;
            BackspaceButton.IsEnabled = true;
            ClearEntryButton.IsEnabled = true;
        }

        /// <summary>
        /// Adds digit of the clicked button.
        /// </summary>
        /// <param name="sender">Digit button.</param>
        /// <param name="e">Button click.</param>
        private void DigitsAdd(object sender, EventArgs e)
        {
            Double tmpValue = Double.Parse(DigitTextBox.Text + (string)((Button)sender).Content);

            // Adding one digit after coma.
            if (DigitTextBox.Text.Contains(",") && DigitTextBox.Text.IndexOf(",") == DigitTextBox.Text.Length - 1)
                DigitTextBox.Text = DigitTextBox.Text + (string)((Button)sender).Content;
            // Adding digit if final value is in limits.
            else if (!DigitTextBox.Text.Contains(",") && (DigitTextBox.Text != "0" && sign == "" && tmpValue < MaxPositive
                        || DigitTextBox.Text != "0" && sign == "-" && tmpValue > MinNegative))
                DigitTextBox.Text = DigitTextBox.Text + (string)((Button)sender).Content;
            // Changing default value to digit
            else if (DigitTextBox.Text == "0")
                DigitTextBox.Text = (string)((Button)sender).Content;
        }

        /// <summary>
        /// Binary operation handler.
        /// </summary>
        /// <param name="sender">Operation button.</param>
        /// <param name="op">Operation number.</param>
        private void TwoDigitOperations(object sender, OperationNumber op)
        {
            // Change operation if the other one is already chosen and the second digit is not inputted.
            if (operation != OperationNumber.NoOperation && DigitTextBox.Text == "0")
            {
                // Changing operation to current.
                operation = op;
                // Changing history to display current operation.
                HistoryTextBox.Text = digits.ValueOfA.ToString() + " " + (string)((Button)sender).Content;
            }
            // Calculating previously chosen operation, and applying a new operation to calculation result.
            else if (operation != OperationNumber.NoOperation && DigitTextBox.Text != "0")
            {
                try
                {
                    // Second digit equals entered value.
                    digits.ValueOfB = Convert.ToDouble(DigitTextBox.Text);

                    // Calculation.
                    Double res = Calculation();
                    res = Math.Round(res, 1);

                    // Checking for exception.
                    if (res.ToString().Length > BigNumber) throw BigNumberException;

                    // Changing operation to current.
                    operation = op;

                    // Saving result.
                    digits.ValueOfA = res;

                    // Displaying history and result.
                    HistoryTextBox.Text = digits.ValueOfA.ToString() + " " + (string)((Button)sender).Content;
                    DigitTextBox.Text = digits.ValueOfA.ToString();
                }
                // Handling exceptions.
                catch (Exception ex)
                {
                    DisableDigitAndOperations();
                    HistoryTextBox.Clear();
                    DigitTextBox.Text = ex.Message;
                }
            }
            // Applying operation.
            else
            {
                // Changing operation.
                operation = op;

                // First digit equals entered value.
                digits.ValueOfA = Convert.ToDouble(DigitTextBox.Text);

                // Displaying history and default value
                HistoryTextBox.Text = DigitTextBox.Text + " " + (string)((Button)sender).Content;
                DigitTextBox.Text = "0";
            }
        }

        /// <summary>
        /// Unary operation handler.
        /// </summary>
        /// <param name="sender">Operation button.</param>
        /// <param name="op">Operation number.</param>
        private void OneDigitOperations(object sender, OperationNumber op)
        {
            Double res;
            try
            {
                // Calculating previously chosen operation, and applying a new operation to calculation result.
                if (operation != OperationNumber.NoOperation && DigitTextBox.Text != "0")
                {
                    // Previous operation.
                    // Second digit equals entered value.
                    digits.ValueOfB = Convert.ToDouble(DigitTextBox.Text);

                    // Calculating previous operation.
                    res = Calculation();
                    res = Math.Round(res, 1);

                    // Checking for exception.
                    if (res.ToString().Length > BigNumber) throw BigNumberException;

                    // Current operation.
                    // Changing operation to current.
                    operation = op;

                    // First digit equals result of previous operation.
                    digits.ValueOfA = res;

                    // Different output for 1/x operation.
                    if (operation == OperationNumber.OneDividedBy) HistoryTextBox.Text = ((string)((Button)sender).Content).Substring(0, 2) + digits.ValueOfA.ToString();
                    // Standart output for unary operation.
                    else HistoryTextBox.Text = (string)((Button)sender).Content + digits.ValueOfA.ToString();

                    // Calculating current operation.
                    res = Calculation();
                    res = Math.Round(res, 1);

                    // Checking for exception.
                    if (res.ToString().Length > BigNumber) throw BigNumberException;

                    // Saving result.
                    digits.ValueOfA = res;

                    // Setting default operation value.
                    operation = 0;

                    // Displaying result.
                    DigitTextBox.Text = digits.ValueOfA.ToString();
                }
                // Applying operation.
                else
                {
                    // Changing operation.
                    operation = op;

                    // First digit equals entered value.
                    digits.ValueOfA = Convert.ToDouble(DigitTextBox.Text);

                    // Different output for 1/x operation.
                    if (operation == OperationNumber.OneDividedBy) HistoryTextBox.Text = ((string)((Button)sender).Content).Substring(0, 2) + digits.ValueOfA.ToString();
                    // Standart output for unary operation.
                    else HistoryTextBox.Text = (string)((Button)sender).Content + digits.ValueOfA.ToString();

                    // Calculation.
                    res = Calculation();
                    res = Math.Round(res, 1);

                    // Checking for exception.
                    if (res.ToString().Length > BigNumber) throw BigNumberException;

                    // Saving result.
                    digits.ValueOfA = res;

                    // Setting default operation value.
                    operation = 0;

                    // Displaying result.
                    DigitTextBox.Text = digits.ValueOfA.ToString();
                }
            }
            // Handling exceptions
            catch (Exception ex)
            {
                DisableDigitAndOperations();
                HistoryTextBox.Clear();
                DigitTextBox.Text = ex.Message;
            }
        }

        /// <summary>
        /// Operation handler.
        /// </summary>
        /// <returns>Calculated value</returns>
        private double Calculation()
        {
            switch (operation)
            {
                case OperationNumber.Add:
                    return MathOperations.Add(digits.ValueOfA, digits.ValueOfB);
                case OperationNumber.Subtract:
                    return MathOperations.Substract(digits.ValueOfA, digits.ValueOfB);
                case OperationNumber.Multiply:
                    return MathOperations.Multiply(digits.ValueOfA, digits.ValueOfB);
                case OperationNumber.Divide:
                    return MathOperations.Divide(digits.ValueOfA, digits.ValueOfB);
                case OperationNumber.SquareRoot:
                    return MathOperations.SquareRoot(digits.ValueOfA);
                case OperationNumber.Cos:
                    return MathOperations.Cos(digits.ValueOfA);
                case OperationNumber.OneDividedBy:
                    return MathOperations.OneDividedBy(digits.ValueOfA);
                default:
                    throw NoOperationException;
            }
        }

        /// <summary>
        /// Change the sign of a digit.
        /// </summary>
        /// <param name="sender">Sign button.</param>
        /// <param name="e">Button click.</param>
        private void SignButton_Click(object sender, EventArgs e)
        {
            Double tmpValue = Double.Parse(DigitTextBox.Text);

            // Changig sign to "-" if value is in limits
            if (sign == "" && tmpValue < -MinNegative) sign = "-";
            // Changing sign to "" 
            else sign = "";

            // Removing "-" from digit
            if (DigitTextBox.Text[0] == '-') DigitTextBox.Text = DigitTextBox.Text.Remove(0, 1);
            // Adding "-" to digit
            else if (DigitTextBox.Text != "0") DigitTextBox.Text = sign + DigitTextBox.Text;
        }

        /// <summary>
        /// Adds coma.
        /// </summary>
        /// <param name="sender">Coma button.</param>
        /// <param name="e">Button click.</param>
        private void ComaButton_Click(object sender, EventArgs e)
        {
            if (!DigitTextBox.Text.Contains(',')) DigitTextBox.Text = DigitTextBox.Text + ',';
        }

        /// <summary>
        /// Binary operation with code 1.
        /// </summary>
        /// <param name="sender">Add button.</param>
        /// <param name="e">Button click.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            TwoDigitOperations(sender, op: OperationNumber.Add);
        }

        /// <summary>
        /// Binary operation with code 2.
        /// </summary>
        /// <param name="sender">Substract button.</param>
        /// <param name="e">Button click.</param>
        private void SubstractButton_Click(object sender, EventArgs e)
        {
            TwoDigitOperations(sender, op: OperationNumber.Subtract);
        }

        /// <summary>
        /// Binary operation with code 3.
        /// </summary>
        /// <param name="sender">Multiply button.</param>
        /// <param name="e">Button click.</param>
        private void MultiplyButton_Click(object sender, EventArgs e)
        {
            TwoDigitOperations(sender, op: OperationNumber.Multiply);
        }

        /// <summary>
        /// Binary operation with code 4.
        /// </summary>
        /// <param name="sender">Divide button.</param>
        /// <param name="e">Button click.</param>
        private void DivideButton_Click(object sender, EventArgs e)
        {
            TwoDigitOperations(sender, op: OperationNumber.Divide);
        }

        /// <summary>
        /// Unary operation with code 5.
        /// </summary>
        /// <param name="sender">Square root button.</param>
        /// <param name="e">Button click.</param>
        private void SquareRootButton_Click(object sender, EventArgs e)
        {
            OneDigitOperations(sender, op: OperationNumber.SquareRoot);
        }

        /// <summary>
        /// Unary operation with code 6.
        /// </summary>
        /// <param name="sender">Cos button.</param>
        /// <param name="e">Button click.</param>
        private void CosButton_Click(object sender, EventArgs e)
        {
            OneDigitOperations(sender, op: OperationNumber.Cos);
        }

        /// <summary>
        /// Unary operation with code 7.
        /// </summary>
        /// <param name="sender">One divided by button.</param>
        /// <param name="e">Button click.</param>
        private void OneDividedByButton_Click(object sender, EventArgs e)
        {
            OneDigitOperations(sender, op: OperationNumber.OneDividedBy);
        }

        /// <summary>
        /// Equal operation.
        /// </summary>
        /// <param name="sender">Equal button.</param>
        /// <param name="e">Button click.</param>
        private void EqualButton_Click(object sender, EventArgs e)
        {
            // No operation chosen.
            if (operation == OperationNumber.NoOperation)
                HistoryTextBox.Text = DigitTextBox.Text + " " + (string)EqualButton.Content;
            // Operation chosen.
            else
            {
                digits.ValueOfB = Convert.ToDouble(DigitTextBox.Text);
                try
                {
                    // Calculation.
                    Double res = Calculation();
                    res = Math.Round(res, 1);
                    res = res == 0 ? 0 : res;


                    // Checking for exception.
                    if (res.ToString().Length > BigNumber) throw BigNumberException;

                    // Setting default operation value.
                    operation = 0;

                    // Displaying history and result.
                    HistoryTextBox.Text += " " + digits.ValueOfB.ToString() + " " + (string)EqualButton.Content;
                    DigitTextBox.Text = res.ToString("0.#####");
                }
                // Handling exceptions.
                catch (Exception ex)
                {
                    DisableDigitAndOperations();
                    HistoryTextBox.Clear();
                    DigitTextBox.Text = ex.Message;
                }

            }
        }

        /// <summary>
        /// Clears entry.
        /// </summary>
        /// <param name="sender">Clear entry button.</param>
        /// <param name="e">Button click.</param>
        private void ClearEntryButton_Click(object sender, EventArgs e)
        {
            sign = "";
            DigitTextBox.Text = "0";
        }

        /// <summary>
        /// Clears all values.
        /// </summary>
        /// <param name="sender">Clear button.</param>
        /// <param name="e">Button click.</param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            EnableDigitAndOperations();
            sign = "";
            DigitTextBox.Text = "0";
            HistoryTextBox.Text = "";
            digits.Clear();
            operation = OperationNumber.NoOperation;
        }

        /// <summary>
        /// Removes last digit.
        /// </summary>
        /// <param name="sender">Backspace button.</param>
        /// <param name="e">Button click.</param>
        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            if (DigitTextBox.Text.Length > 0)
                // Places default value.
                if (DigitTextBox.Text.Length - 1 == 0)
                    DigitTextBox.Text = "0";
                // Removes last digit with '-' sign and places default value.
                else if (DigitTextBox.Text.Length - 1 == 1 && DigitTextBox.Text[0] == '-')
                    DigitTextBox.Text = "0";
                // Removes last digit.
                else DigitTextBox.Text = DigitTextBox.Text.Substring(0, DigitTextBox.Text.Length - 1);
        }
    }
}
