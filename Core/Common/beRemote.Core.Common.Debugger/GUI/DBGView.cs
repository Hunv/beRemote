using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace beRemote.Core.Common.Debugger.GUI
{
    public partial class DBGView : UserControl
    {
        internal Object _dbgObj = null;
        private String loggerContext = "DebugWorker";
        public DBGView(Object dbgObj)
        {
            
            InitializeComponent();

            if (dbgObj != null)
            {
                this._dbgObj = dbgObj;
                WriteLine(_dbgObj);
            }
            else
            {
                txtInput.Enabled = false;
            }

        }

        public void WriteLine(Object output)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker invoker = delegate
                {
                    rtbLog.AppendText(String.Format("{0}\r\n", output));
                    rtbLog.ScrollToCaret();
                };
                this.Invoke(invoker);
            }
            else
            {
                rtbLog.AppendText(String.Format("{0}\r\n", output));
                rtbLog.ScrollToCaret();
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (_dbgObj != null)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    if (txtInput.Text != "" || txtInput.Text != null)
                    {
                        ExecuteInput(txtInput.Text);
                        txtInput.Text = "";
                    }
                }
            }
        }

        private void ExecuteInput(String input)
        {
            if (input.StartsWith("?") || input.ToLower().StartsWith("help"))
            {
                String[] splitted = input.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted.Length == 1)
                    WriteMethods();
                else
                    WriteMethods(splitted[1]);
            }
            else if (input.ToLower() == "cls" || input.ToLower() == "clear")
            {
                rtbLog.Clear();

                WriteLine(_dbgObj);
            }
            else
            {
                String comm = txtInput.Text.Split(new String[] { ";" }, StringSplitOptions.None)[0];
                String[] parameter = txtInput.Text.Replace(comm, "").Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                WriteLine("Trying to execute: " + comm);

                try
                {
                    object val = this.Execute(comm, parameter);

                    if (val != null)
                    {
                        Type valueType = val.GetType();
                        if (valueType.IsArray)
                        {
                            foreach (Object obj in (Object[])val)
                            {
                                WriteLine(String.Format("\r\nExecution of {0} returned:\r\n{1}", comm, obj));
                            }
                        }
                        else
                            WriteLine(String.Format("\r\nExecution of {0} returned:\r\n{1}", comm, val));
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(String.Format("Exception while executing {0}:\r\n{1}", comm, ex));
                    beRemote.Core.Common.LogSystem.Logger.Log(Common.LogSystem.LogEntryType.Exception, "Exception while running protocols debug mode", ex, loggerContext);
                }
            }


        }
        public object Execute(string methodName, Object[] parameter)
        {
            WriteLine("WARNING: This debugger cannot distinguish between methods with the same parameter count! It will uses always the first method to get!");
            Type thisType = _dbgObj.GetType();
            //MethodInfo theMethod = thisType.GetMethod(methodName);

            //MethodInfo[] methods = thisType.GetMethods();
            MethodInfo theMethod = null;

            List<MethodInfo> filteredMethods = new List<MethodInfo>();

            foreach (MethodInfo mi in thisType.GetMethods((BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)))
            {
                if (mi.Name.Equals(methodName) && parameter.Length == mi.GetParameters().Length)
                    filteredMethods.Add(mi);
            }

            if (filteredMethods.Count == 1)
                theMethod = filteredMethods[0];
            else if (filteredMethods.Count > 1)
            {
                WriteLine("WARNING: got multiple methods. using the first one. NOTE: IMPLEMENT RETRY RO TEST THE METHODS!");
                theMethod = filteredMethods[0];
            }
            else
            {
                return "Method could not be executed";
            }


            // we have to check if the given parameters are usable for this method.
            // NOTE: parameter have to be in the same order as the result of theMethod.GetParameters();
            ParameterInfo[] parameterInformations = theMethod.GetParameters();
            List<Object> parametersToPass = new List<object>();
            if (parameterInformations.Length != parameter.Length)
                throw new ArgumentOutOfRangeException(String.Format("Cannot pass argument parameter to method {0} since the length does not equal!", methodName));

            for (int i = 0; i < parameterInformations.Length; i++)
            {
                Type piType = parameterInformations[i].ParameterType;
                Type parameterType = parameter[i].GetType();

                if (piType == parameterType)
                    parametersToPass.Add(parameter[i]);
                else if (piType == typeof(Int32))
                {
                    int val;
                    if (Int32.TryParse(parameter[i].ToString(), out val))
                    {
                        parametersToPass.Add(val);
                    }
                }
                else if (piType == typeof(Int64))
                {
                    Int64 val;
                    if (Int64.TryParse(parameter[i].ToString(), out val))
                    {
                        parametersToPass.Add(val);
                    }
                }
                else if (piType == typeof(Guid))
                {
                    Guid val;
                    if (Guid.TryParse(parameter[i].ToString(), out val))
                    {
                        parametersToPass.Add(val);
                    }
                }
                else if (piType == typeof(bool))
                {
                    Boolean val;
                    if (Boolean.TryParse(parameter[i].ToString(), out val))
                    {
                        parametersToPass.Add(val);
                    }
                }
            }

            if (parameterInformations.Length != parametersToPass.Count)
                throw new ArgumentOutOfRangeException(String.Format("Cannot pass argument parameter to method {0} since the length does not equal!", methodName));

            object ret = theMethod.Invoke(_dbgObj, parametersToPass.ToArray());

            return ret;
        }

        private void WriteMethods()
        {
            // more info at http://www.csharp-examples.net/get-method-names/
            // get all public methods of session type
            MethodInfo[] methodInfos = _dbgObj.GetType().GetMethods((BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
            // sort methods by name
            Array.Sort(methodInfos,
                    delegate(MethodInfo methodInfo1, MethodInfo methodInfo2)
                    { return methodInfo1.Name.CompareTo(methodInfo2.Name); });

            // write method names
            foreach (MethodInfo methodInfo in methodInfos)
            {
                WriteLine(String.Format("Method: {0}", methodInfo.Name));
                foreach (ParameterInfo pi in methodInfo.GetParameters())
                {
                    WriteLine(String.Format(".       Parameter: type: {0}; name: {1};", pi.ParameterType, pi.Name));
                }


            }
        }

        private void WriteMethods(String filter)
        {
            // more info at http://www.csharp-examples.net/get-method-names/
            // get all public methods of session type
            MethodInfo[] methodInfos = _dbgObj.GetType().GetMethods((BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
            // sort methods by name
            Array.Sort(methodInfos,
                    delegate(MethodInfo methodInfo1, MethodInfo methodInfo2)
                    { return methodInfo1.Name.CompareTo(methodInfo2.Name); });

            // write method names
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.Name.ToLower().StartsWith(filter.ToLower()))
                {
                    WriteLine(String.Format("Method: {0}", methodInfo.Name));
                    foreach (ParameterInfo pi in methodInfo.GetParameters())
                    {
                        WriteLine(String.Format(".       Parameter: type: {0}; name: {1};", pi.ParameterType, pi.Name));
                    }

                }
            }
        }
    }
}
