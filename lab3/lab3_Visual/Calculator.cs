using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace lab3_Visual
{
    public class Calculator : INotifyPropertyChanged
    {

        private string _displayText = "0";

        private double _firstValue = 0;
        private double _lastValue = 0;
        private double _result = 0;
        private string _operation = "";
        private bool _isFirstValue = false;
        private bool _isLastValue = false;

        private bool _isDeleteResult = false;
        private string _deleteResult = "мер";

        private string _helpText =
            $"FirstNumber: 0 | False\n" +
            $"LastNumber: 0 | False\n" +
            $"Operation:  \n" +
            $"Rezult: 0           | DeleteResult: True\n";


        public string DisplayText
        {
            get
            {
                return _displayText;
            }

            set
            {
                _ = SetField(ref _displayText, value);
            }
        }

        public string HelpText
        {
            get
            {
                return _helpText;
            }

            set
            {
                _ = SetField(ref _helpText, value);
            }
        }
        public string DeleteResult
        {
            get
            {
                return _deleteResult;
            }

            set
            {
                _ = SetField(ref _deleteResult, value);
            }
        }

        public void ChangeHelpText()
        {
            HelpText =
                $"FirstNumber: {_firstValue} | {_isFirstValue}\n" +
                $"LastNumber: {_lastValue}  | {_isLastValue}\n" +
                $"Operation: {_operation}\n" +
                $"Rezult: {_result}           | DeleteResult: {_isDeleteResult}\n";
        }
        public void ClickToggleDeleteResult()
        {
            if (_isDeleteResult)
            {

                DeleteResult = "мер";
                _isDeleteResult = false;
            }
            else
            {
                DeleteResult = "дю";
                _isDeleteResult = true;
            }
            ChangeHelpText();
        }

        public void ClickNumber(object parameter)
        {
            double value = Convert.ToInt32(parameter);
            if (_operation == "")
            {
                if (_isFirstValue)
                {
                    _firstValue = Convert.ToInt32(Convert.ToString(_firstValue) + Convert.ToString(value));
                    DisplayText += Convert.ToString(value);
                }
                else
                {
                    DisplayText = Convert.ToString(value);
                    _firstValue = value;
                    _isFirstValue = true;
                    
                }

            }
            else
            {
                if(_operation == "!" || _operation == "lg") {}
                else
                {
                    if (_isLastValue)
                    {
                        _lastValue = Convert.ToInt32(Convert.ToString(_lastValue) + Convert.ToString(value));
                    }
                    else
                    {
                        _lastValue = value;
                        _isLastValue = true;
                    }
                    DisplayText += Convert.ToString(value);
                }
                
            }
            ChangeHelpText();
        }

        public double Calculete()
        {
            switch (_operation)
            {
                case "+": return _firstValue + _lastValue;
                case "-": return _firstValue - _lastValue;
                case "*": return _firstValue * _lastValue;
                case "/": return _firstValue / _lastValue;
                case "mod": return _firstValue % _lastValue;
                case "div": return (int)(_firstValue / _lastValue);
                case "!": return factorial(_firstValue);
                case "^": return Math.Pow(_firstValue, _lastValue);
                case "lg": return Math.Log10(_firstValue);
                case "ln": return Math.Log(_firstValue);
                case "sin": return Math.Sin(_firstValue);
                case "cos": return Math.Cos(_firstValue);
                case "tan": return Math.Tan(_firstValue);
                case "floor": return Math.Floor(_firstValue);
                case "ceil": return Math.Ceiling(_firstValue);
                default: return _result;
            }
        }

        public double factorial(double k) {
            double temp = 1;
            for(int i = 1; i != k+1; i++)
            {
                temp = temp * i;
            }
            return temp;
        }

        public void ClickSolve()
        {
            if(_operation == "")
            {
                return;
            }
            else if(_operation != "lg" && _operation != "ceil" && _operation != "floor" 
                && _operation != "!" && _operation != "ln" && _operation != "sin" 
                && _operation != "cos" && _operation != "tan")
            {
                if (!_isLastValue)
                {
                    return;
                }
            }
            _result = Calculete();

            DisplayText = Convert.ToString(_result);


            _lastValue = 0;
            _isLastValue = false;
            _operation = "";

            if (_isDeleteResult)
            {
                _firstValue = 0;
                _isFirstValue = false;
            }
            else
            {
                _firstValue = _result;
            }
            ChangeHelpText();
        }

        public void ClickOperand(object parameter)
        {
            if (!_isFirstValue)
            {
                return;
            }
            if (_operation != "")
            {
                DisplayText = DisplayText.Replace(_operation, "");
            }
            _operation = Convert.ToString(parameter);
            if (_operation == "floor" || _operation == "ceil")
            {
                ClickSolve();
                return;
            }
            if (_operation == "lg" || _operation == "ln" || _operation == "sin" || _operation == "cos" || _operation == "tan")
            {
                DisplayText = Convert.ToString(parameter) + DisplayText;
            }
            else
            {
                DisplayText += Convert.ToString(parameter);
            }
            ChangeHelpText();
        }

        public void ClickClear(object parameter)
        {
            DisplayText = "0";
            _firstValue = 0;
            _lastValue = 0;
            _result = 0;
            _operation = "";
            _isLastValue = false;
            _isFirstValue = false;
            ChangeHelpText();
        }

        public void ClickDel(object parameter)
        {
            if (_isLastValue)
            {
                string temp = Convert.ToString(_lastValue).Remove(Convert.ToString(_lastValue).Length - 1, 1);
                if (temp.Length <= 0)
                {
                    _lastValue = 0;
                    _isLastValue = false;
                }
                else
                {
                    _lastValue = Convert.ToInt32(temp);
                }
                DisplayText = DisplayText.Remove(DisplayText.Length - 1, 1);
            }
            else if (_operation != "")
            {
                DisplayText = DisplayText.Replace(_operation, "");
                _operation = "";
            }
            else if (_isFirstValue)
            {
                string temp = Convert.ToString(_firstValue).Remove(Convert.ToString(_firstValue).Length - 1, 1);
                if (temp.Length <= 0)
                {
                    _firstValue = 0;
                    _isFirstValue = false;
                }
                else
                {
                    _firstValue = Convert.ToInt32(temp);
                    DisplayText = DisplayText.Remove(DisplayText.Length - 1, 1);
                }
            }
            ChangeHelpText();
        }

        public void ClickNull(object parameter)
        {
           
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
