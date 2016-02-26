using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security;
using System.Data.SQLite;
using System.Data;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Common.SimpleSettings;
using beRemote.Core.StorageSystem.StorageBase;


namespace beRemote.Core.StorageSystem.SqLite
{    
    /// <summary>
    /// Communicates with the datasource (file)
    /// </summary>
    public class SqLiteCom
    {
        /// <summary>
        /// The name of the Datebase-File for SqLite
        /// </summary>
        private string _DBName = "%appdata%\beRemote\beRemote.db";

        /// <summary>
        /// Get the Name of the Databasefile, -Path, ...
        /// </summary>
        public string DBName
        {
            get { return (_DBName); }
            set { }
        }

        /// <summary>
        /// The Prefix of all SQL-Tables (Default: none)
        /// </summary>
        private string _DBPrefix = "";

        /// <summary>
        /// Get or Set the Prefix of all SQL-Tables
        /// </summary>
        public string DBPrefix { get { return _DBPrefix; } set {} }

        /// <summary>
        /// The Connection to the Database
        /// </summary>
        SQLiteConnection _DBConnection;

        /// <summary>
        /// The Versionnumber of the Database-Content; Used for possible Updates for the Database
        /// </summary>
        private int _DBVersion = 7;

        /// <summary>
        /// Contains the UserId of the LoggedIn User; Identified by the current User-Name
        /// </summary>
        private int _UserId = 0;

        /// <summary>
        /// Context for logging messages; don't change this
        /// </summary>
        private String _loggerContext = "StorageSystem";

        /// <summary>
        /// Gets the Database-Path
        /// </summary>
        /// <returns>The path of the Database</returns>
        private string getDBName()
        {
            //Load configfile
            IniFile _config = Helper.GetApplicationConfiguration();

            //Load Database-Path
            string dbPath = _config.GetValue("database", "dbpath");

            if (dbPath == "")
                return (@"%appdata%\beRemote\beremote.db");
            else
                return (dbPath);
        }

        /// <summary>
        /// Gets the Database-Prefix
        /// </summary>
        /// <returns>The Prefix for the Database-Tables</returns>
        private string getDBPrefix()
        {
            //Load configfile
            IniFile _config = Helper.GetApplicationConfiguration();

            //Load Database-Path
            string dbPrefix = _config.GetValue("database", "dbprefix");

            return(dbPrefix);
        }

        /// <summary>
        /// Initialisation of the Database-Connection
        /// </summary>
        public void InitConnection()
        {
            _DBName = getDBName().ToLower();
            _DBPrefix = getDBPrefix();
                        
            //Convert Environmentvariables for connectionstring
            IDictionary	environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                _DBName = _DBName.Replace("%" + de.Key.ToString().ToLower()+ "%" , de.Value.ToString());
            }        

            string connectionString = "Data Source=" + _DBName + ";"; //Password=beRemote2012;";
            
            Logger.Log(LogEntryType.Verbose, "Connecting to Database using ConnectionString: " + connectionString.Replace("beRemote2012", "*********"), _loggerContext);

            //Set Connection Parameter
            try
            {
                _DBConnection = new SQLiteConnection();
                _DBConnection.StateChange += _DBConnection_StateChange;
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Unable to connect to to SqLite Database. This usally happens, if your are using the wrong architecture-files for your OS (i.e. 32Bit instead of 64Bit). Please download the Downloadpack that fits to your environment!", ea);
            }
            _DBConnection.ConnectionString = connectionString;
            _DBConnection.Open();
            
            Logger.Log(LogEntryType.Verbose, "Connected...", _loggerContext);
        }

        void _DBConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState != ConnectionState.Open &&
                _DBConnection.ConnectionString.Length > 0)
                _DBConnection.Open();
        }

        /// <summary>
        /// Sets the local Username, based on local Domain\Username
        /// </summary>
        public void SetUserId()
        {
            //Set User or Create a new one
            _UserId = GetUserId(System.Environment.UserDomainName + "\\" + System.Environment.UserName);

            Logger.Log(LogEntryType.Verbose, "User ID set: " + _UserId.ToString(), _loggerContext);        
        }

        #region SQL-Get- and -Set-Helper

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an Int16
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A 16 Bit-Based Integer</returns>
        private Int16 sqlGetInt16(string sqltext)
        {
            //Create variable for return value
            Int16 returnValue = 0;

            Logger.Log(LogEntryType.Verbose, "Querying Int16: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {
                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    //Execute the command and store the result in slDataReader
                    SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value                
                    slDataReader.Read();

                    //Store the returned value in the returnvalue-variable, if there is content
                    if (slDataReader.HasRows == true)
                        returnValue = slDataReader.GetInt16(0);

                    slDataReader.Dispose();
                }
                
                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetInt16 while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying Int16 Returnvalue: " + returnValue.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an Int16
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A 16 Bit-Based Integer</returns>
        private Int16 sqlGetInt16Data(string sqltext, byte[] data)
        {
            //Create variable for return value
            Int16 returnValue = 0;

            Logger.Log(LogEntryType.Verbose, "Querying Int16: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                var param = new SQLiteParameter("@0", DbType.Binary);
                param.Value = data;
                slCmd.Parameters.Add(param);

                lock (slCmd)
                {
                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    //Execute the command and store the result in slDataReader
                    SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value                
                    slDataReader.Read();

                    //Store the returned value in the returnvalue-variable, if there is content
                    if (slDataReader.HasRows == true)
                        returnValue = slDataReader.GetInt16(0);

                    slDataReader.Dispose();
                }

                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetInt16 while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                    //Close the Databaseconnection in every situation
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying Int16 Returnvalue: " + returnValue.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an Int32
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A 32 Bit based Integer</returns>
        private int sqlGetInt32(string sqltext)
        {
            //Create variable for return value
            int returnValue = 0;

            Logger.Log(LogEntryType.Verbose, "Querying Int32: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                int errorCounter = 5;

                while (errorCounter > 0)
                {
                    try
                    {
                        lock (slCmd)
                        {
                            Logger.Log(LogEntryType.Verbose, "Current DB-Connectionstate: " + _DBConnection.State.ToString());

                            //Open the Databaseconnection
                            //if (_DBConnection.State != ConnectionState.Open)
                            //{
                            //    _DBConnection.Open();
                            //    Logger.Log(LogEntryType.Verbose, "Connection opend. State is now: " + _DBConnection.State.ToString());
                            //}

                            //Execute the command and store the result in slDataReader
                            SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                            //Read the first line of the returned value
                            slDataReader.Read();

                            //Store the returned value in the returnvalue-variable, if there is content
                            if (slDataReader.HasRows == true)
                                returnValue = slDataReader.GetInt32(0);

                            slDataReader.Dispose();

                            errorCounter = 0; //No retrys
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        errorCounter--;
                        Logger.Log(LogEntryType.Warning, "SQL-GetInt32 Statement failed. Retrys left: " + errorCounter.ToString(), _loggerContext);
                        System.Threading.Thread.Sleep(2);
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Exception, "Error in sqlGetInt32 while executing internal", ea, _loggerContext);
                        errorCounter = 0; //No retrys
                    }
                }
                slCmd.Dispose();
                
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetInt32 while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                    //Close the Databaseconnection in every situation
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying Int32 Returnvalue: " + returnValue.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an Int64
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A 64 Bit based Integer</returns>
        private Int64 sqlGetInt64(string sqltext)
        {
            //Create variable for return value
            Int64 returnValue = 0;

            Logger.Log(LogEntryType.Verbose, "Querying Int64: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {

                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    //Execute the command and store the result in slDataReader
                    SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value
                    slDataReader.Read();

                    //Store the returned value in the returnvalue-variable, if there is content
                    if (slDataReader.HasRows == true)
                        returnValue = slDataReader.GetInt64(0);

                    slDataReader.Dispose();
                }

                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetInt64 while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying Int64 Returnvalue: " + returnValue.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as a string
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A string</returns>
        private string sqlGetString(string sqltext)
        {
            //Create variable for return value
            string returnValue = "";

            Logger.Log(LogEntryType.Verbose, "Querying String: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {

                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    //Execute the command and store the result in slDataReader
                    SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value
                    slDataReader.Read();

                    //Store the returned value in the returnvalue-variable, if there is content
                    if (slDataReader.HasRows == true)
                        returnValue = slDataReader.GetString(0);

                    slDataReader.Dispose();
                }

                slCmd.Dispose();                
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetString while executing", ea, _loggerContext);
            }
            finally
            {
                //Close the Databaseconnection in every situation
                try
                {
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying String Returnvalue: " + returnValue, _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as a boolean
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A Boolean</returns>
        private bool sqlGetBool(string sqltext)
        {
            //Create variable for return value
            bool returnValue = false;

            Logger.Log(LogEntryType.Verbose, "Querying Boolean: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock(slCmd)
                {
                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    //Execute the command and store the result in slDataReader
                    SQLiteDataReader slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value
                    slDataReader.Read();

                    //Store the returned value in the returnvalue-variable, if there is content
                    if (slDataReader.HasRows == true)
                        returnValue = slDataReader.GetBoolean(0);
                    
                    slDataReader.Dispose();
                }

                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "Error in sqlGetBool while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Exception, "Querying Boolean Returnvalue: " + returnValue.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an DataTable
        /// Supported Datatypes: String, Bool, Byte, Int16, Int32, Int64, Datetime, Float, Decimal, Double, Object
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>A DataTable with the Queried Information; Take care of the supported DataTypes!</returns>
        private DataTable sqlGetDataTable(string sqltext)
        {
            Logger.Log(LogEntryType.Verbose, "Querying DataTable: " + sqltext, _loggerContext);

            //Create variable for return value
            DataTable returnValue = new DataTable();
            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;
                
                //if (_DBConnection.State != ConnectionState.Open) _DBConnection.Open();
                
                //while (_DBConnection.State != ConnectionState.Open)
                //{
                //    Logger.Log(LogEntryType.Info, "SQLite-Connection State was closed. Waiting for open...");
                //    try
                //    {
                //        //_DBConnection.Open();
                //        _DBConnection = _DBConnection.OpenAndReturn();
                //        System.Threading.Thread.Sleep(1000);
                //    }
                //    catch (Exception ea)
                //    {
                //        Logger.Log(LogEntryType.Info, "Failed to connect to SQLite DB; Retrying...", ea);
                //    }
                //}

                SQLiteDataReader slDataReader  = slCmd.ExecuteReader();
                lock (slDataReader)//Lock Datareader for preventing SQLite-Errors - hopefully
                {
                    #region Table Initialize
                    if (returnValue.Rows.Count == 0) //first call
                    {
                        returnValue = new DataTable(); //Create an new Datatable
                        for (int i = 0; i < slDataReader.FieldCount; i++) //For each field
                        {
                            //Add a column with the correct fieldtype
                            returnValue.Columns.Add(slDataReader.GetName(i), slDataReader.GetFieldType(i));
                        }
                    }
                    #endregion

                    //Read every Line
                    while (slDataReader.Read())
                    {
                        //Continue only, if there are rows
                        if (slDataReader.HasRows == false)
                            break;

                        //Initialize a new Datarow
                        DataRow dR = returnValue.NewRow();

                        //For each field, check the type, read the type in the correct format and write them to the Datarow
                        for (int i = 0; i < slDataReader.FieldCount; i++)
                        {
                            //Get the Value in the correct Datatype
                            switch (slDataReader.GetFieldType(i).ToString().ToLower())
                            {
                                case "system.string":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = "";
                                    else
                                        dR[i] = slDataReader.GetString(i);
                                    break;
                                case "system.boolean":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = true;
                                    else
                                    {
                                        var val = slDataReader.GetValue(i);
                                        if (val is bool)
                                            dR[i] = (bool)val;
                                        else if (val is int)
                                            dR[i] = Convert.ToBoolean(val);
                                        else if (val is string)
                                            dR[i] = val.ToString().ToLower() == "true";
                                    }
                                    break;
                                case "system.byte":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetByte(i);
                                    break;
                                case "system.int16":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetInt16(i);
                                    break;
                                case "system.int32":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetInt32(i);
                                    break;
                                case "system.int64":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetInt64(i);
                                    break;
                                case "system.datetime":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = DateTime.Now;
                                    else
                                        dR[i] = slDataReader.GetDateTime(i);
                                    break;
                                case "system.float":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetFloat(i);
                                    break;
                                case "system.decimal":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetDecimal(i);
                                    break;
                                case "system.double":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = 0;
                                    else
                                        dR[i] = slDataReader.GetDouble(i);
                                    break;
                                case "system.object":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = new object();
                                    else
                                        dR[i] = slDataReader.GetValue(i);
                                    break;
                                case "system.byte[]":
                                    if (slDataReader.IsDBNull(i))
                                        dR[i] = new byte[]{};
                                    else
                                        dR[i] = slDataReader.GetValue(i);
                                    break;
                            }
                        }

                        //Add the dataRow to the Returnvalue
                        returnValue.Rows.Add(dR);
                    }
                }
                slDataReader.Dispose();
                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlGetDatatable while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying DataTable Returnvalue: Rows: " + returnValue.Rows.Count.ToString() + "  Columns: " + returnValue.Columns.Count.ToString(), _loggerContext);

            //Return the value
            return (returnValue);
        }

        /// <summary>
        /// Executes a SQL-Command without any returnvalue
        /// </summary>
        /// <param name="sqltext">The SQL-Command to Execute</param>
        private void sqlExecuteNonQuery(string sqltext)
        {
            Logger.Log(LogEntryType.Verbose, "Executing NonQuery: " + sqltext, _loggerContext);

            try
            {
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                int errorCounter = 0;

                while (errorCounter < 3)
                {
                    try
                    {
                        lock (slCmd)
                        {
                            //_DBConnection.Open();
                            slCmd.ExecuteNonQuery();
                            //_DBConnection.Close();
                            slCmd.Dispose();
                        }
                        errorCounter = 3;
                    }
                    catch (System.AccessViolationException) //If an AccessViolationException occures, try again. This happens if multiple threads are trying to access the sqlitefile at the same time
                    {
                        errorCounter++;
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlExecuteNonQuery while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                    //Close the Databaseconnection in every situation
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Executes a SQL-Command without any returnvalue
        /// </summary>
        /// <param name="sqltext">The SQL-Command to Execute</param>
        /// <returns>A Int64 (long) with the added ID</returns>
        private long sqlExecuteNonQueryWithId(string sqltext)
        {
            Logger.Log(LogEntryType.Verbose, "Executing NonQuery with ID: " + sqltext, _loggerContext);

            long ret = 0;
            try
            {                
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;
                lock (slCmd)
                {
                    //_DBConnection.Open();
                    slCmd.ExecuteNonQuery();

                    ret = _DBConnection.LastInsertRowId;
                    //_DBConnection.Close();
                }
                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlExecuteNonQuery while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Verbose, "Querying NonQuery with ID Returnvalue: " + ret.ToString(), _loggerContext);

            //return the ID
            return (ret);
        }

        /// <summary>
        /// The Parametername is allways @0. i.e.: INSERT INTO user (image) VALUES (@0);
        /// </summary>
        /// <param name="sqltext">The SQL-Command to Execute</param>
        private void sqlExecuteNonQueryData(string sqltext, byte[] data)
        {
            Logger.Log(LogEntryType.Verbose, "Executing NonQueryData: " + sqltext, _loggerContext);

            try
            {
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {
                    SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
                    param.Value = data;
                    slCmd.Parameters.Add(param);

                    //_DBConnection.Open();
                    slCmd.ExecuteNonQuery();
                    //_DBConnection.Close();
                    slCmd.Dispose();
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlExecuteNonQueryData while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The Parametername is allways @0. i.e.: INSERT INTO user (image) VALUES (@0);
        /// </summary>
        /// <param name="sqltext">The SQL-Command to Execute</param>
        private long sqlExecuteNonQueryDataId(string sqltext, byte[] data)
        {
            Logger.Log(LogEntryType.Verbose, "Executing NonQueryData: " + sqltext, _loggerContext);

            long ret = 0;
            try
            {
                var slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {
                    var param = new SQLiteParameter("@0", DbType.Binary);
                    param.Value = data;
                    slCmd.Parameters.Add(param);

                    
                    slCmd.ExecuteNonQuery();
                    ret = _DBConnection.LastInsertRowId;
                    
                    slCmd.Dispose();
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlExecuteNonQueryData while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                    //Close the Databaseconnection in every situation
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }
            return (ret);
        }

        /// <summary>
        /// The Parametername is allways @0. i.e.: INSERT INTO user (image) VALUES (@0);
        /// </summary>
        /// <param name="sqltext">The SQL-Command to Execute</param>
        /// <returns>The added ID</returns>
        private long sqlExecuteNonQueryDataWithId(string sqltext, byte[] data)
        {
            Logger.Log(LogEntryType.Verbose, "Executing NonQueryData: " + sqltext, _loggerContext);
            long ret = 0;

            try
            {
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {
                    SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
                    param.Value = data;
                    slCmd.Parameters.Add(param);

                    //_DBConnection.Open();
                    slCmd.ExecuteNonQuery();
                    ret = _DBConnection.LastInsertRowId;
                    //_DBConnection.Close();
                    slCmd.Dispose();
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error in sqlExecuteNonQueryData while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                    //Close the Databaseconnection in every situation
                    //_DBConnection.Close();
                }
                catch
                {
                }
            }
            return (ret);
        }

        /// <summary>
        /// Executes a SQL-Command and returns the retunrvalue as an Int16
        /// </summary>
        /// <param name="sqltext">The SQL-Command to execute</param>
        /// <returns>The byte-Array with the queried Data</returns>
        private byte[] sqlGetData(string sqltext)
        {
            //Create variable for return value
            byte[] returnValue = new byte[0];

            Logger.Log(LogEntryType.Verbose, "Querying Data: " + sqltext, _loggerContext);

            try
            {
                //Initialize the Command
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);
                slCmd.CommandText = sqltext;

                lock (slCmd)
                {
                    //Open the Databaseconnection
                    //_DBConnection.Open();

                    IDataReader slDataReader = slCmd.ExecuteReader();


                    while (slDataReader.Read())
                    {
                        returnValue = (Byte[])slDataReader[0];
                    }

                    slDataReader.Dispose();
                }
                slCmd.Dispose();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "Error in sqlGetData while executing", ea, _loggerContext);
            }
            finally
            {
                try
                {
                //Close the Databaseconnection in every situation
                //_DBConnection.Close();
                }
                catch
                {
                }
            }

            Logger.Log(LogEntryType.Exception, "Querying Data returned data with bytes: " + returnValue.Length, _loggerContext);

            //Return the value
            return (returnValue);
        }

        #endregion


        /// <summary>
        /// Get the Count of all existing Datatables in the Databasefile
        /// </summary>
        /// <returns>Count of existing Tables</returns>
        public int GetTableCount()
        {
            return (sqlGetInt16("SELECT COUNT(*) FROM sqlite_master WHERE type='table';"));        
        }

        /// <summary>
        /// Get all DatabaseTables
        /// </summary>
        /// <returns>All Tables of the Database</returns>
        public DataTable GetDatabaseTables()
        {
            return (sqlGetDataTable("SELECT * FROM sqlite_master WHERE type='table';"));
        }

        /// <summary>
        /// Gets the complete content of a Databasetable
        /// </summary>
        /// <param name="table">Tablename to query</param>
        /// <returns>Complete content of the table</returns>
        public DataTable GetDatabaseTableContent(string table)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + table + ";"));
        }

        /// <summary>
        /// Modifies a Cellvalue
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <param name="column">Columnname</param>
        /// <param name="value">Value</param>
        /// <param name="isString">Is the Value a String (add ' ???)</param>
        /// <param name="whereClause">whereClause</param>        
        public void ModifyDatabaseTableContent(string table, string column, string value, bool isString, string whereClause)
        {
            Logger.Log(LogEntryType.Verbose, "Executing ModifyDatabaseTableContent", _loggerContext);
            sqlExecuteNonQuery("UPDATE " + DBPrefix + table + " SET " + column + "=" + (isString ? "'" : "") + value + (isString ? "'" : "") + " WHERE " + whereClause + ";");
        }


        /// <summary>
        /// Returns the database version number
        /// </summary>
        /// <returns>Database Version Number</returns>
        public int GetDatabaseVersion()
        {
            return (Convert.ToInt32(sqlGetString("SELECT value FROM " + DBPrefix + "settings WHERE setting='version';"))); 
        }


        /// <summary>
        /// Use _UserId for private-usage! Gets the UserId of the current User
        /// </summary>
        /// <returns>The UserId</returns>
        public int GetUserId()
        {
            return (_UserId);
        }
        
        /// <summary>
        /// Gets the Id of the User "username"
        /// </summary>
        /// <param name="username">The Username (Domain\Username) of the Windows-User</param>
        /// <returns>The UserId</returns>
        public int GetUserId(string username)
        {
            return(sqlGetInt32("SELECT id FROM " + DBPrefix + "user WHERE winname='" + username + "';"));
        }

        /// <summary>
        /// Returns a Datatable with UserId and Username
        /// </summary>
        /// <returns>Returns a Datatable with UserId and Username</returns>
        public DataTable GetUserList()
        {
            return (sqlGetDataTable("SELECT id, name FROM " + DBPrefix + "user"));
        }

        /// <summary>
        /// Changes the ID of the current User
        /// </summary>
        /// <param name="username">The Username the ID should be setted to</param>
        /// <returns>The ID of the Username</returns>
        public int ChangeUser(string username)
        {
            _UserId = GetUserId(username);
            return (_UserId);
        }

        /// <summary>
        /// Adds a new User
        /// </summary>
        /// <param name="username">Windows-Username (i.e. Domain\Username)</param>
        /// <param name="displayname">Displayname in beRemote (i.e. Nickname)</param>
        /// <param name="hashedPassword">3x MD5 salted Password</param>
        /// <returns>UserId of the new User</returns>
        public long AddUser(string username, string displayname, byte[] hashedPassword)
        {
            var ret = sqlExecuteNonQueryDataId("INSERT INTO " + DBPrefix + "user (winname, name, password, lastmachine, superadmin, lastlogin, lastlogout, logincount, defaultprotocol, defaultfolder, heartbeat,updatemode) VALUES(" +
                "'" + username + "','" + displayname + "', @0, '" + Environment.MachineName + "', 0, datetime('now','localtime'),datetime('now','localtime'),0, '', 0,datetime('now','localtime'),0);", hashedPassword);

            sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "uservisuals (userid, mainwindowx, mainwindowy, mainwindowmax, mainwindowwidth, mainwindowheight, ribbonexpanded, expandednodes, statusbarsetting, gridlayout, ribbonqat) VALUES (" +
                ret + ", 100, 100, 1, 800, 600, 1, '', 0, '', '');");
            return(ret);
        }
                
        /// <summary>
        /// Modifes a User, identified by ID
        /// </summary>
        /// <param name="userId">The UserId of the User</param>
        /// <param name="name">The new Displayname (i.e. Nickname)</param>
        /// <param name="hashedPassword">The new hashed Password</param>
        public void ModifyUser(long userId, string name, byte[] hashedPassword)
        {
            if (hashedPassword == null)
            {
                sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET " +
                                       (name != null ? "name='" + name + "', " : "") +
                                       " WHERE id=" + userId + ";");
            }
            else
            {
                sqlExecuteNonQueryData("UPDATE " + DBPrefix + "user SET " +
                                       (name != null ? "name='" + name + "', " : "") +
                                       "password=@0, " +
                                       " WHERE id=" + userId + ";", hashedPassword);
            }
        }

        /// <summary>
        /// Modifies the display- and username
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <param name="name">New Displayname</param>
        /// <param name="winname">New Username</param>        
        public void ModifyUser(long userId, string name, string winname)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET " +
                (name != null ? "name='" + name + "', " : "") +
                (winname != null ? "winname='" + winname + "'" : "") +
                " WHERE id=" + userId.ToString() + ";");
        }

        /// <summary>
        /// Modifies the last login parameter of a user
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="lastmachine">The machinename of the logged in system</param>
        /// <param name="lastlogin">Last timestamp (now!?)</param>
        public void ModifyUserLoginparameter(long userId, string lastmachine)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET " +
                "lastmachine='" + lastmachine + "', " +
                "lastlogin=datetime('now','localtime') WHERE id=" + userId + ";");
        }

        /// <summary>
        /// Modifies the Password of a User
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <param name="hashedPassword">The hashed Password</param>
        public void ModifyUserPassword(long userId, byte[] hashedPassword)
        {
            sqlExecuteNonQueryData("UPDATE " + DBPrefix + "user SET password=@0 WHERE id=" + userId + ";", hashedPassword);
        }

        /// <summary>
        /// Should a User be Superadmin?
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="isSuperadmin">Is Superadmin?</param>
        public void ModifyUserSuperadmin(long userId, bool isSuperadmin)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET superadmin=" + (isSuperadmin ? "1" : "0") + " WHERE id=" + userId + ";");
        }
        
        /// <summary>
        /// Gets Usersettings by userId except PasswordHash and IsSuperadmin
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        public Dictionary<string, object> GetUserSettings(long userId)
        {
            DataTable workerReturn = sqlGetDataTable("SELECT * FROM " + DBPrefix + "user WHERE id=" + userId + ";");
            var ReturnValue = new Dictionary<string, object>();

            if (workerReturn != null && workerReturn.Rows.Count > 0 && workerReturn.Columns.Count > 0)
            {
                for (int i = 0; i < workerReturn.Columns.Count; i++)
                {
                    ReturnValue.Add(workerReturn.Columns[i].ColumnName, workerReturn.Rows[0][i]);
                }
            }

            return (ReturnValue);
        }

        /// <summary>
        /// Sets a specific Setting of a User
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="setting"></param>
        /// <param name="value"></param>
        public void SetUserSetting(int userid, string setting, string value)
        {
            sqlExecuteNonQuery(string.Format("UPDATE {0}user SET {1}={2} WHERE id={3};", DBPrefix, setting, value, userid));
        }

        /// <summary>
        /// Gets the Salt1-Value for a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt1(long userId)
        {
            return sqlGetData("SELECT salt1 FROM " + DBPrefix + "user WHERE id=" + userId + ";");
        }

        /// <summary>
        /// Gets the Salt2-Value for a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt2(long userId)
        {
            return (sqlGetData("SELECT salt2 FROM " + DBPrefix + "user WHERE id=" + userId + ";"));
        }

        /// <summary>
        /// Gets the Salt3-Value for a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt3(long userId)
        {
            return (sqlGetData("SELECT salt3 FROM " + DBPrefix + "user WHERE id=" + userId + ";"));
        }

        /// <summary>
        /// Changes a Salt-Value of a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="salt">Value of the Salt</param>
        public void SetUserSalt1(long userId, byte[] salt)
        {
            sqlExecuteNonQueryData(string.Format("UPDATE {0}user SET salt1=@0 WHERE id={1};", DBPrefix, userId), salt);
        }
        
        /// <summary>
        /// Changes a Salt-Value of a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="salt">Value of the Salt</param>
        public void SetUserSalt2(long userId, byte[] salt)
        {
            sqlExecuteNonQueryData(string.Format("UPDATE {0}user SET salt2=@0 WHERE id={1};", DBPrefix, userId), salt);
        }

        /// <summary>
        /// Changes a Salt-Value of a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="salt">Value of the Salt</param>
        public void SetUserSalt3(long userId, byte[] salt)
        {
            sqlExecuteNonQueryData(string.Format("UPDATE {0}user SET salt3=@0 WHERE id={1};", DBPrefix, userId), salt);
        }

        /// <summary>
        /// Get if a User is Superadmin
        /// </summary>
        /// <param name="userId">UserId to check</param>
        /// <returns>yes/no (true/false)</returns>
        public bool GetUserSuperadmin(long userId)
        {
            return (sqlGetBool("SELECT superadmin FROM " + DBPrefix + "user WHERE id=" + userId + ";"));
        }
        
        /// <summary>
        /// Increases the LoginCount of userId by one
        /// </summary>
        /// <param name="userId">UserId</param>
        public void IncreaseUserLoginCount(long userId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET logincount=logincount+1 WHERE id=" + userId + ";");            
        }

        /// <summary>
        /// Validates the Password (Takes at least 1 Second!)
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="hashedPassword">Password</param>
        /// <returns>matches/not matches (true/false)</returns>
        public bool CheckUserPassword(long userId, byte[] hashedPassword)
        {
            return (sqlGetInt16Data("SELECT COUNT(*) FROM " + DBPrefix + "user WHERE id='" + userId + "' AND password=@0;", hashedPassword) == 1);
        }

        /// <summary>
        /// Changes the Default Folder for a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="newDefaultFolder">ID of the new Default folder</param>
        public void ModifyUserDefaultFolder(long userId, long newDefaultFolder)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET defaultfolder=" + newDefaultFolder + " WHERE id=" + userId + ";");
        }

        /// <summary>
        /// Gets the Default Folder ID for the given UserId
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>FolderID of the Defaultfolder</returns>
        public int GetUserDefaultFolder(long userId)
        {
            return (sqlGetInt32("SELECT defaultfolder FROM " + DBPrefix + "user WHERE id=" + userId + ";"));
        }

        /// <summary>
        /// Modfies the selected default protocol of a user
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <param name="newDefaultProtocol">internal Protocol-Name</param>
        public void ModifyUserDefaultProtocol(long userid, string newDefaultProtocol)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET defaultprotocol='" + newDefaultProtocol + "' WHERE id=" + userid.ToString() + ";");
        }

        /// <summary>
        /// Gets the default Protocol of a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The internal name of the protocol</returns>
        public string GetUserDefaultProtocol(long userId)
        {
            return (sqlGetString("SELECT defaultprotocol FROM " + DBPrefix + "user WHERE id=" + userId.ToString() + ";"));
        }

        /// <summary>
        /// Updates the Userheartbeat to the current timestamp
        /// </summary>
        /// <param name="userId">ID of the User</param>
        public void UpdateUserHeartbeat(long userId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET heartbeat=datetime('now','localtime') WHERE id=" + userId.ToString() + ";");
        }

        /// <summary>
        /// Sets the lastlogout-value to the current timestamp
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void LogoutUser(long userId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET lastlogout=datetime('now','localtime') WHERE id=" + userId.ToString() + ";");
        }

        /// <summary>
        /// Modifies the Updatemode. 0=Stable, 1=Nightly
        /// </summary>
        /// <param name="userId">ID user to modify</param>
        /// <param name="updatemode">The Mode-ID</param>
        public void ModifyUserUpdatemode(long userId, int updatemode)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET updatemode=" + updatemode.ToString() + " WHERE id=" + userId + ";");
        }

        /// <summary>
        /// Gets a list of users, that were online in the heartbeatinterval and not logged out in the heartbeat interval
        /// </summary>
        /// <returns></returns>
        public DataTable GetUsersOnline()
        { 
            TimeSpan aHeartbeat = new TimeSpan(0,0,1,0, Convert.ToInt32(GetSetting("heartbeat")));

            string heartbeatAgo = 
                DateTime.Now.Subtract(aHeartbeat).Year.ToString() +
                "-" +
                (DateTime.Now.Subtract(aHeartbeat).Month.ToString().Length == 1? "0" : "") + DateTime.Now.Subtract(aHeartbeat).Month.ToString() +
                "-" +
                (DateTime.Now.Subtract(aHeartbeat).Day.ToString().Length == 1 ? "0" : "") + DateTime.Now.Subtract(aHeartbeat).Day.ToString() +
                " " +
                (DateTime.Now.Subtract(aHeartbeat).Hour.ToString().Length == 1 ? "0" : "") + DateTime.Now.Subtract(aHeartbeat).Hour.ToString() +
                ":" +
                (DateTime.Now.Subtract(aHeartbeat).Minute.ToString().Length == 1 ? "0" : "") + DateTime.Now.Subtract(aHeartbeat).Minute.ToString() +
                ":" +
                (DateTime.Now.Subtract(aHeartbeat).Second.ToString().Length == 1 ? "0" : "") + DateTime.Now.Subtract(aHeartbeat).Second.ToString();


            return (sqlGetDataTable("SELECT winname, name FROM " + DBPrefix + "user WHERE heartbeat > '" + heartbeatAgo + "';"));// AND lastlogout < '" + heartbeatAgo + "';"));
        }

        /// <summary>
        /// Gets the visual settings of a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <returns>All visual settings</returns>
        public DataTable GetUserVisuals(long userId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "uservisuals WHERE userid=" + userId.ToString() + ";"));
        }

        /// <summary>
        /// Set the UserVisuals-Parameter
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="values">Dictionary with visual settings</param>
        public void SetUserVisual(long userId, Dictionary<string, object> values) //values = value,setting
        { 
            //generate set-Values
            string sqlCommand = "UPDATE " + DBPrefix + "uservisuals SET ";

            foreach (KeyValuePair<string, object> kvp in values)
            {
                sqlCommand += kvp.Key + "=";
                string temp = kvp.Value.GetType().ToString().ToLower();
                switch (kvp.Value.GetType().ToString().ToLower())
                { 
                    case "system.int16":
                    case "system.int32":
                    case "system.int64":
                    case "system.byte":
                    case "system.double":
                    case "system.float":
                        sqlCommand += kvp.Value.ToString();
                        break;
                    case "system.boolean":
                        sqlCommand += ((bool)kvp.Value == true ? "1": "0");
                        break;
                    default: //strings etc.
                        sqlCommand += "'" + kvp.Value.ToString() + "'";
                        break;
                }
                sqlCommand += ", ";
            }

            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 2); //Remove last ", "
            sqlCommand += " WHERE userid=" + userId.ToString() + ";";

            sqlExecuteNonQuery(sqlCommand);
        }

        /// <summary>
        /// Sets the global proxy-settings of a user
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="credentialid"></param>
        public void SetUserProxySettings(bool enabled, string host, int port, int credentialid, int flags, long userid)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "user SET proxyenabled=" + (enabled ? "1" : "0") + ", " +
                "proxyhost='" + host + "', " +
                "proxyport=" + port + ", " +
                "proxycredentials=" + credentialid + ", " +
                "proxyflags=" + flags + " WHERE id=" + userid + ";");
        }

        /// <summary>
        /// Gets the Proxysettings of a user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserProxySettings(long userid)
        {
            return (sqlGetDataTable("SELECT proxyenabled, proxyhost, proxyport, proxycredentials, proxyflags FROM " + DBPrefix + "user WHERE id=" + userid + ";"));
        }



        /// <summary>
        /// Gets a global Setting from the settings-Table
        /// </summary>
        /// <param name="setting">Name of the setting</param>
        /// <returns>The containing value to the setting</returns>
        public string GetSetting(string setting)
        {
            return (sqlGetString("SELECT value FROM " + DBPrefix + "settings WHERE setting='" + setting + "';"));
        }

        /// <summary>
        /// Sets a global Setting from the settings-Table
        /// </summary>        
        /// <returns>The containing value to the setting</returns>
        public void SetSetting(string setting, string value)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "settings SET value='" + value + "' WHERE setting='" + setting + "'");
        }

        /// <summary>
        /// Adds a new Setting to the Database
        /// </summary>
        /// <param name="setting">the Settingname</param>
        /// <param name="value">The initial value</param>
        public void AddSetting(string setting, string value)
        {
            sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "settings (setting, value) VALUES ('" + setting + "', '" + value + "');");
        }



        /// <summary>
        /// Updates an existing PluginSetting
        /// </summary>
        /// <param name="Setting">The setting</param>
        public void UpdateConnectionOption(string Settingvalue, string Settingname, long ConnectionSettingId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connectionoptions SET value='" + Settingvalue + "' WHERE connectionsettingid=" + ConnectionSettingId.ToString() + " AND optionname='" + Settingname + "';");
        }

        /// <summary>
        /// Adds a new Setting
        /// </summary>
        /// <param name="Setting">The new Setting</param>
        public long AddConnectionOption(string Settingvalue, string Settingname, long ConnectionSettingId)
        {
            SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);

            long ret = sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "connectionoptions (optionname, value, connectionsettingid) VALUES('" + Settingname + "','" + Settingvalue + "', " + ConnectionSettingId.ToString() + ");");
            return (ret);
        }

        /// <summary>
        /// Gets a Setting of a Plugin
        /// </summary>
        /// <param name="ConnectionSettingId">The connectionsettingid</param>
        /// <param name="Settingname">the name of the setting</param>
        /// <returns>The Setting</returns>
        public string GetConnectionOption(long ConnectionSettingId, string Settingname)
        {
            return (sqlGetString("SELECT value FROM " + DBPrefix + "connectionoptions WHERE optionname='" + Settingname + "' AND connectionsettingid='" + ConnectionSettingId + "';"));            
        }

        /// <summary>
        /// Gets all Options of given ConnectionSetting
        /// </summary>
        /// <param name="ConnectionSettingId">The ID of the ConnectionSetting</param>
        /// <returns>Datatable with a copy of the filtered table (id, connectionsettingid, optionname, value)</returns>
        public DataTable GetConnectionOptions(long ConnectionSettingId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "connectionoptions WHERE connectionsettingid=" + ConnectionSettingId.ToString() + ";"));
        }
        
        /// <summary>
        /// Checks if a Option already exists
        /// </summary>
        /// <param name="Settingname">Name of the Setting ("Optionname")</param>
        /// <param name="ConnectionSettingId">ID of the ConnectionSetting</param>
        /// <returns>yes/no (true/false)</returns>
        public bool GetConnectionOptionExists(string Settingname, long ConnectionSettingId)
        {
            return (sqlGetInt16("SELECT COUNT(*) FROM " + DBPrefix + "connectionoptions WHERE connectionsettingid=" + ConnectionSettingId.ToString() + " AND optionname='" + Settingname + "';") > 0 ? true : false);
        }

        /// <summary>
        /// Deletes a connectionoption
        /// </summary>
        /// <param name="optionId">the OptionID to delete</param>
        public void DeleteConnectionOption(long optionId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "connectionoptions WHERE id=" + optionId.ToString() + ";");
        }

        /// <summary>
        /// Deletes a connectionoption
        /// </summary>
        /// <param name="connectionSettingId">The connectionsettingid to delete</param>
        /// <param name="optionname">The optionname to delete (* = ALL)</param>
        public void DeleteConnectionOption(long connectionSettingId, string optionname)
        {
            if (optionname != "*")
                sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "connectionoptions WHERE connectionsettingid=" + connectionSettingId.ToString() + " AND optionname='" + optionname + "';");
            else
                sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "connectionoptions WHERE connectionsettingid=" + connectionSettingId.ToString() + ";");
        }



        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="Id">The ConnectionID that should get the new settings</param>        
        /// <param name="Port">The Port where the Plugin should connect to</param>
        public void ModifyConnectionSetting(long Id, int Port)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connectionsetting SET port=" + Port.ToString() + " WHERE id=" + Id.ToString() + ";");            
        }

        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="Id">The ConnectionID that should get the new settings</param>        
        /// <param name="Port">The Port where the Plugin should connect to</param>
        /// <param name="CredentialId">The UserID of the owner of this connection</param>
        public void ModifyConnectionSetting(long Id, int Port, int CredentialId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connectionsetting SET port=" + Port.ToString() + ", credentialid=" + CredentialId.ToString() + " WHERE id=" + Id.ToString() + ";");
        }

        /// <summary>
        /// Removes a ConnectionSettingCredential-Value
        /// </summary>
        /// <param name="Id">ID of the connectionSetting</param>
        public void DeleteConnectionSettingCredential(long connectionSetting)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connectionsetting SET credentialid=0 WHERE id=" + connectionSetting.ToString() + ";");
        }

        /// <summary>
        /// Adds a new ConnectionSetting
        /// </summary>
        /// <param name="ConnectionId">The ID of the Connection</param>
        /// <param name="Protocol">Protocolname (Pluginname)</param>
        /// <param name="Port">The Port where the Protocol connects to</param>
        /// <param name="CredentialId">ID of the used Credential (0 if not defined)</param>
        public long AddConnectionSetting(long ConnectionId, string Protocol, int Port, int CredentialId)
        {
            long ret = sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "connectionsetting (connectionid, protocol, port, credentialid) VALUES " +
                "(" + ConnectionId.ToString() + ", '" + Protocol + "', " + Port.ToString() + ", " + CredentialId.ToString() + ");");
            return (ret);
        }

        /// <summary>
        /// Gets a complete Connectionsetting
        /// </summary>
        /// <param name="Id">ID of Connectionsetting</param>
        /// <returns>A Dictionary with Columname as a Key an Columnvalue as a Value</returns>
        public Dictionary<string, object> GetConnectionSetting(long Id)
        {
            DataTable workerReturn = sqlGetDataTable("SELECT * FROM " + DBPrefix + "connectionsetting WHERE id=" + Id.ToString() + ";");
            Dictionary<string, object> ReturnValue = new Dictionary<string, object>();

            if (workerReturn != null && workerReturn.Rows.Count > 0 && workerReturn.Columns.Count > 0)
            {
                for (int i = 0; i < workerReturn.Columns.Count; i++)
                {
                    ReturnValue.Add(workerReturn.Columns[i].ColumnName, workerReturn.Rows[0][i]);
                }
            }

            return (ReturnValue);
        }

        /// <summary>
        /// Gets all ConnectionSettings
        /// </summary>
        /// <returns>A Datatable with all Settings</returns>
        public DataTable GetConnectionSettings()
        {
            //Get all Connection where it is Allowed to see the Connection
            return (sqlGetDataTable("SELECT connectionsetting.* FROM " + DBPrefix + "connections INNER JOIN " + DBPrefix + "connectionsetting ON connections.id = connectionsetting.connectionid WHERE owner=" + _UserId.ToString() + " OR ispublic=1;"));
        }

        /// <summary>
        /// Gets all ConnectionSettings for one Connection
        /// </summary>
        /// <param name="ConnectionId">The Id of the Connection</param>
        /// <returns>A Datatable with all Settings</returns>
        public DataTable GetConnectionSettings(long ConnectionId)
        {
            //Get all Connection where it is Allowed to see the Connection
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "connectionsetting WHERE connectionid=" + ConnectionId.ToString() + ";"));
        }

        /// <summary>
        /// Deletes a connectionsetting
        /// </summary>
        /// <param name="SettingId">ID of the Setting</param>
        public void DeleteConnectionSetting(long SettingId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "connectionsetting WHERE id=" + SettingId.ToString() + ";");
        }



        /// <summary>
        /// Checks if a Connection has a previous entry in the connectionlist
        /// </summary>
        /// <param name="ConnectionId">The connection that should be checked</param>
        /// <returns>The previous ID (if any)</returns>
        public long HasConnectionUp(long ConnectionId)
        {
            long val = sqlGetInt64("SELECT con2.id FROM " + DBPrefix + "connections INNER JOIN " + DBPrefix + "connections AS con2 ON connections.folder=con2.folder WHERE connections.id=" + ConnectionId + " AND con2.sortorder < connections.sortorder ORDER BY con2.sortorder DESC LIMIT 1");
            return (val);
        }

        /// <summary>
        /// Checks if a Connection has a following entry in the connectionlist
        /// </summary>
        /// <param name="ConnectionId">The connection that should be checked</param>
        /// <returns>The next ID (if any)</returns>
        public long HasConnectionDown(long ConnectionId)
        {
            long val = sqlGetInt64("SELECT con2.id FROM " + DBPrefix + "connections INNER JOIN " + DBPrefix + "connections AS con2 ON connections.folder=con2.folder WHERE connections.id=" + ConnectionId + " AND con2.sortorder > connections.sortorder ORDER BY con2.sortorder LIMIT 1");
            return (val);
        }




        /// <summary>
        /// Modifies an existing connection; identified by the ID
        /// </summary>
        /// <param name="Id">The ID of the connection to modify</param>
        /// <param name="Host">Hostname or IP of the Host</param>
        /// <param name="Name">Displayname of the Host</param>
        /// <param name="Description">Discription of the host</param>
        public void ModifyConnection(long Id, string Host, string Name, string Description, int OS, int owner, bool ispublic, int vpn)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connections SET host='" + Host + "', " +
                "name='" + Name + "', " +
                "description='" + Description + "', " +
                "os=" + OS.ToString() + ", " +
                "owner=" + owner.ToString() + ", " +
                "ispublic=" + (ispublic?"1":"0") + ", " +
                "vpn=" + vpn.ToString() +
                " WHERE id=" + Id.ToString() + ";");
        }

        /// <summary>
        /// Modifies an existing connection; identified by the ID
        /// </summary>
        /// <param name="Id">The ID of the connection to modify</param>
        /// <param name="ParentFolderId">Hostname or IP of the Host</param>
        public void ModifyConnection(long Id, long ParentFolderId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connections SET folder=" + ParentFolderId +
                " WHERE id=" + Id.ToString() + ";");
        }

        /// <summary>
        /// Modifies an existing connections Sortorder Up
        /// </summary>
        /// <param name="Id">ID of the Connection</param>
        /// <param name="SortOrder">new SortOrder</param>        
        public void ModifyConnectionSortOrderUp(long Id)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connections SET sortorder=(SELECT con2.sortorder FROM " + DBPrefix + "connections INNER JOIN " + DBPrefix + "connections AS con2 ON " + DBPrefix + "connections.folder=con2.folder WHERE " + DBPrefix + "connections.id=" + Id + " AND con2.sortorder < " + DBPrefix + "connections.sortorder ORDER BY con2.sortorder DESC LIMIT 1)  WHERE id=" + Id + ";");
        }

        /// <summary>
        /// Modifies an existing connections Sortorder Down
        /// </summary>
        /// <param name="Id">ID of the Connection</param>
        /// <param name="SortOrder">new SortOrder</param>        
        public void ModifyConnectionSortOrderDown(long Id)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connections SET sortorder=(SELECT con2.sortorder FROM " + DBPrefix + "connections INNER JOIN " + DBPrefix + "connections AS con2 ON " + DBPrefix + "connections.folder=con2.folder WHERE " + DBPrefix + "connections.id=" + Id + " AND con2.sortorder > " + DBPrefix + "connections.sortorder ORDER BY con2.sortorder LIMIT 1)  WHERE id=" + Id + ";");
        }


        /// <summary>
        /// Sets the SortOrder of an Id, regardless of any circumstances
        /// </summary>
        /// <param name="Id">The ID to set</param>
        /// <param name="SortOrder">The new SortOrder-Position</param>
        public void ModifyConnectionSortOrder(long Id, long SortOrder)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "connections SET sortorder=" + SortOrder + " WHERE id=" + Id + ";");
        }

        /// <summary>
        /// Adds a new Connection
        /// </summary>
        /// <param name="Host">Hostname or IP-Address of the Host</param>
        /// <param name="Name">Displayname of the Host</param>
        /// <param name="Description">Description of the Host</param>
        /// <returns>The added ID</returns>
        public long AddConnection(string Host, string Name, string Description, int OS, long FolderId, int owner, bool ispublic, int vpn)
        {
            long ret = sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "connections (host, name, description, os, folder, sortorder, owner, ispublic, vpn) VALUES " +
                "('" + Host + "', '" + 
                Name + "', '" + 
                Description + "', " + 
                OS.ToString() + ", " + 
                FolderId + ", " +
                "(SELECT COUNT(*) FROM " + DBPrefix + "connections WHERE folder=" + FolderId + "), " +
                owner.ToString() + ", " +
                (ispublic?"1":"0") + ", " +
                vpn.ToString() + ");");
            return (ret);
        }

        /// <summary>
        /// Gets a connection
        /// </summary>
        /// <param name="Id">The ID of the Connection</param>
        /// <returns>The Connection in a Dictionary where Key is the Column and Value is the Value</returns>
        public Dictionary<string, object> GetConnection(long Id)
        {
            DataTable workerReturn = sqlGetDataTable("SELECT * FROM " + DBPrefix + "connections WHERE id=" + Id.ToString() + ";");
            Dictionary<string, object> ReturnValue = new Dictionary<string, object>();

            if (workerReturn != null && workerReturn.Rows.Count > 0 && workerReturn.Columns.Count > 0)
            {
                for (int i = 0; i < workerReturn.Columns.Count; i++)
                {
                    ReturnValue.Add(workerReturn.Columns[i].ColumnName, workerReturn.Rows[0][i]);
                }
            }

            return (ReturnValue);
        }

        /// <summary>
        /// Returns a Datatable with all Connections
        /// </summary>
        /// <returns>All Connections</returns>
        public DataTable GetConnections(long userId)
        {
            //Get all Connections
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "connections WHERE ispublic=1 OR owner=" + userId.ToString() + " ORDER BY sortorder;"));
        }

        /// <summary>
        /// Returns a Datatable with all Connections that has displayname as name; CASE SENSITIVE!
        /// </summary>
        /// <returns>All Connections</returns>
        public DataTable GetConnections(string displayname, long userId)
        {
            //Get all Connections
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "connections WHERE (ispublic=1 OR owner=" + userId.ToString() + ") and name='" + displayname + "' ORDER BY id;"));
        }

        /// <summary>
        /// Gets all connections, that matches the given filters
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetConnections(List<List<string>> filterList, string wildcard, long userId)
        {
            //List<String> comDataPart = new List<string>();
            //comDataPart.Add(prefix);
            //comDataPart.Add(compare);
            //comDataPart.Add(conditionString);            
            //comDataPart.Add(filterValue);
            //comDataPart.Add(link); //AND or OR
            string statement = "SELECT * FROM connections WHERE owner=" + userId.ToString() + " AND (";

            foreach (List<string> aFilter in filterList)
            {
                statement += aFilter[0] + " " + aFilter[2] + " " + aFilter[1] + " '" + wildcard + aFilter[3] + wildcard + "' " + aFilter[4];
            }

            if (statement.EndsWith(" AND"))
                statement = statement.Substring(0, statement.Length - 3);
            else if (statement.EndsWith(" OR"))
                statement = statement.Substring(0, statement.Length - 2);

            statement += ");";
            
            return (sqlGetDataTable(statement));
        }

        /// <summary>
        /// Returns a Datatable with all Connections
        /// </summary>
        /// <returns>All Connections</returns>
        public DataTable GetConnectionsInFolder(long folderId)
        {
            //Get all Connections
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "connections WHERE folder=" + folderId.ToString() + " ORDER BY sortorder;"));
        }

        /// <summary>
        /// Deletes a connection
        /// </summary>
        /// <param name="id">ConnectionId to delete</param>
        public void DeleteConnection(long id)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "connections WHERE id=" + id.ToString() + ";");
        }




        /// <summary>
        /// Gets all Folders of a user
        /// </summary>        
        /// <returns>All queried folder</returns>
        public DataTable GetFolders()
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "folder WHERE owner=" + _UserId.ToString() + " or ispublic=1 ORDER BY parent,sortorder;"));
        }

        /// <summary>
        /// Gets a Folder
        /// </summary>
        /// <param name="FolderId">ID of the Folder to get</param>
        /// <returns>All information of the queried folder</returns>
        public DataTable GetFolder(long folderId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "folder WHERE id=" + folderId.ToString() + ";"));
        }

        /// <summary>
        /// Gets the ID of the first created folder with the given parameter
        /// </summary>
        /// <param name="foldername">The Name of the searched folder</param>
        /// <param name="userId">the ID if the user, that owns that folder</param>
        /// <returns>The ID of the serached folder</returns>
        public int GetFolderId(string foldername, long userId)
        {
            return (sqlGetInt32("SELECT id FROM " + DBPrefix + "folder WHERE owner=" + userId.ToString() + " AND foldername='" + foldername + "' LIMIT 1;"));
        }

        /// <summary>
        /// Gets the ID of the first created folder with the given parameter
        /// </summary>
        /// <param name="foldername">The Name of the searched folder</param>
        /// <param name="userId">the ID if the user, that owns that folder</param>
        /// <param name="parentFolderId">the ID if the user, that owns that folder</param>
        /// <returns>The ID of the serached folder</returns>
        public int GetFolderId(string foldername, long userId, long parentFolderId)
        {
            return (sqlGetInt32("SELECT id FROM " + DBPrefix + "folder WHERE owner=" + userId.ToString() + " AND foldername='" + foldername + "' AND parent=" + parentFolderId.ToString() + " LIMIT 1;"));
        }

        /// <summary>
        /// Gets all Subfolders from parentId
        /// </summary>
        /// <param name="parentId">The Parent-Folder-Id</param>
        /// <returns>A Datatable with the containing informations</returns>
        public DataTable GetSubfolders(long parentId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "folder WHERE parent=" + parentId.ToString() + " AND (owner=" + GetUserId() + " OR ispublic=1) ORDER BY sortorder;"));
        }

        /// <summary>
        /// Deletes a Folder
        /// </summary>
        /// <param name="folderId">The FolderId to delete</param>
        public void DeleteFolder(long folderId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "folder WHERE id=" + folderId.ToString() + ";");
        }

        /// <summary>
        /// Adds a Folder
        /// </summary>
        /// <param name="name">Name of the Folder</param>
        /// <param name="parentId">ID of the parent</param>
        /// <returns>Added ID</returns>
        public long AddFolder(string name, long parentId, int owner, bool ispublic)
        {
            long ret = sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "folder (foldername, parent, sortorder, owner, ispublic) VALUES " +
                "('" + name + "', " + parentId.ToString() + ", (SELECT COUNT(*) FROM " + DBPrefix + "folder WHERE parent=" + parentId.ToString() + "), " + 
                owner.ToString() + ", " + (ispublic?"1":"0") + ");");
            return (ret);
        }

        /// <summary>
        /// Moves a Folder to a new parentfolder
        /// </summary>
        /// <param name="folderId">FolderID to modify</param>
        /// <param name="newParentId">The new folder-ID of the parent</param>
        public void ModifyFolderParent(long folderId, long newParentId)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "folder SET parent=" + newParentId + " WHERE id=" + folderId + ";");
        }

        /// <summary>
        /// Sorts a Folder
        /// </summary>
        /// <param name="folderId">FolderID to modify</param>
        /// <param name="sortOrder">The new sortorder of the folder</param>
        public void ModifyFolderSortOrder(long folderId, long sortOrder)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "folder SET sort=" + sortOrder + " WHERE id=" + folderId + ";");
        }



        /// <summary>
        /// Gets the Information for a specific versionId
        /// </summary>
        /// <param name="versionId">The ID of the versionentry</param>
        /// <returns>Versioninformation</returns>
        public DataTable GetVersion(int versionId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "operatingsystems WHERE id=" + versionId.ToString() + ";"));
        }

        /// <summary>
        /// Gets all OperatingSystems
        /// </summary>        
        /// <returns>A Datatable with the containing informations</returns>
        public DataTable GetVersionlist()
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "operatingsystems ORDER BY family, distribution, version;"));
        }


        /// <summary>
        /// Adds new Credentials to the Database for a specific user
        /// </summary>
        /// <param name="username">The Username to login (i.e. Windows-Username)</param>
        /// <param name="password">The Password as a SecureString</param>
        /// <param name="domain">The Domain of the Username, if used</param>
        /// <param name="description"></param>
        /// <param name="owner">The ID of the User, that owns this credentials</param>
        /// <returns>The ID of the Added Credentials</returns>
        public long AddUserCredentials(string username, byte[] password, string domain, string description, int owner)
        {
            var ret = sqlExecuteNonQueryDataWithId("INSERT INTO " + DBPrefix + "credentials (username, password, domain, owner, description) VALUES " +
                "('" + username + "', @0, '" + domain + "', " + owner + ", '" + description + "');", password);
            return (ret);
        }

        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <param name="id">The ID of the Credential</param>
        /// <returns>Returns the Credentials</returns>
        public DataTable GetUserCredentials(int id)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "credentials WHERE id=" + id + " AND owner=" + _UserId + ";"));
        }

        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>Returns the Credentials</returns>
        public DataTable GetUserCredentialsAll(long userid)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "credentials WHERE owner=" + userid + ";"));
        }

        /// <summary>
        /// Deletes a UserCredential
        /// </summary>
        /// <param name="id">Id of the Credential to delete</param>
        public void DeleteUserCredential(long id)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "credentials WHERE id=" + id + ";");
            sqlExecuteNonQuery(string.Format("UPDATE {0}connectionsetting SET credentialid=0 WHERE credentialid={1};", DBPrefix, id));
        }

        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        public void ModifyUserCredential(long id, string username, byte[] password, string domain, int owner, string description)
        {
            sqlExecuteNonQueryData("UPDATE " + DBPrefix + "credentials SET username='" + username +
                "', password=@0" + 
                ", domain='" + domain + 
                "', owner=" + owner +
                ", description='" + description + 
                "' WHERE id=" + id + ";", password);
        }

        /// <summary>
        /// Resets a Credential-Password. Required on password-resets
        /// </summary>
        /// <param name="id"></param>
        public void ResetUserCredentialPassword(long id)
        {
            sqlExecuteNonQuery(String.Format("UPDATE {0}credentials SET password=NULL WHERE id={1};", DBPrefix, id));
        }

        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        public void ModifyUserCredential(int id, string username, string domain, int owner, string description)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "credentials SET username='" + username +
                "', domain='" + domain +
                "', owner=" + owner +
                ", description='" + description +
                "' WHERE id=" + id + ";");
        }


        /// <summary>
        /// Get a DataTable with all OperatingSystems
        /// </summary>
        /// <returns>DataTable with all OperatingSystems</returns>
        public DataTable GetOperatingSystemList()
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "operatingsystems order by id;"));
        }

        /// <summary>
        /// Get a DataTable with the selected OperatingSystemId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a DataTable with the selected OperatingSystemId</returns>
        public DataTable GetOperatingSystem(int id)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "operatingsystems WHERE id=" + id.ToString() + ";"));
        }

        /// <summary>
        /// Adds a new Operatingsystem to the list
        /// </summary>
        /// <param name="id">The ID of the OS</param>
        /// <param name="family">The Family</param>
        /// <param name="distribution">The Distribution</param>
        /// <param name="version">The Version </param>
        public void AddOperatingSystem(int id, string family, string distribution, string version)
        {
            sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "operatingsystems (id, family, distribution, version) VALUES (" + id.ToString() + ", '" + family + "', '" + distribution + "', '" + version + "');");
        }



        /// <summary>
        /// Get all Licenses
        /// </summary>
        /// <returns>A Table with all licenses</returns>
        public DataTable GetLicenses()
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "licenses;"));
        }

        /// <summary>
        /// Get all Licenses
        /// </summary>
        /// <returns>A Table with all licenses</returns>
        public DataTable GetUserLicenses(long userId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "licenses WHERE userid=" + userId.ToString() + ";"));
        }

        /// <summary>
        /// Adds a License to the Database
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="secret"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void AddUserLicense(string firstname, string lastname, string email, string secret, long userId)
        {
            sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "licenses (firstname, lastname, email, secret, userid) VALUES ('" +
                firstname + "', '" + lastname + "', '" + email + "', '" + secret + "', " + userId.ToString() + ");");
        }

        /// <summary>
        /// Delete a License
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="secret"></param>
        /// <param name="userId"></param>
        public void DeleteUserLicense(string firstname, string lastname, string email, string secret, long userId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "licenses WHERE firstname='" + firstname + "' AND " +
                                                            "lastname='" + lastname + "' AND " +
                                                            "email='" + email + "' AND " +
                                                            "userid=" + userId.ToString() + ";");
        }



        /// <summary>
        /// Adds a new history-Entry to the database
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <param name="connectionsettingid">ID of the connectionsetting</param>        
        public void AddHistoryEntry(long userid, long connectionsettingid)
        {
            string datetimestring = DateTime.Now.ToUniversalTime().Year.ToString("D4") + "-" + DateTime.Now.ToUniversalTime().Month.ToString("D2") + "-" + DateTime.Now.ToUniversalTime().Day.ToString("D2") + " " + DateTime.Now.ToUniversalTime().Hour.ToString("D2") + ":" + DateTime.Now.ToUniversalTime().Minute.ToString("D2") + ":" + DateTime.Now.ToUniversalTime().Second.ToString("D2") + "." + DateTime.Now.ToUniversalTime().Millisecond.ToString("D6");

            sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "userhistory (userid, connectionsettingid, pointoftime) VALUES (" +
                userid.ToString() + ", " + connectionsettingid.ToString() + ", datetime('" + datetimestring + "'));");//In other SQL-Implementations the Time of the Server should be taken!
        }

        /// <summary>
        /// Returns the number of Enties in the history of a user
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <returns></returns>
        public int GetHistoryEntriesCount(long userid)
        {
            return (sqlGetInt32("SELECT COUNT(*) FROM " + DBPrefix + "userhistory WHERE userid=" + userid.ToString() + ";"));
        }

        /// <summary>
        /// Get an Entry on a special ID
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public DataTable GetHistoryEntry(int entryId)
        {
            return (sqlGetDataTable("SELECT " + DBPrefix + "userhistory.*, " + DBPrefix + "connectionsetting.protocol, " + DBPrefix + "connectionsetting.connectionid, " + DBPrefix + "connectionsetting.port, " + DBPrefix + "connections.host, " + DBPrefix + "connections.name FROM " + DBPrefix + "userhistory INNER JOIN " + DBPrefix + "connectionsetting ON " + DBPrefix + "userhistory.connectionsettingid=" + DBPrefix + "connectionsetting.id INNER JOIN " + DBPrefix + "connections ON " + DBPrefix + "connectionsetting.connectionid=" + DBPrefix + "connections.id WHERE " + DBPrefix + "userhistory.id=" + entryId + "ORDER BY " + DBPrefix + "userhistory.pointoftime DESC;"));
            //return(sqlGetDataTable("SELECT * FROM userhistory WHERE id=" +  entryId + ";"));
        }

        /// <summary>
        /// Get the complete history of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHistory(long userId)
        {
            return (sqlGetDataTable("SELECT " + DBPrefix + "userhistory.*, " + DBPrefix + "connectionsetting.protocol, " + DBPrefix + "connectionsetting.connectionid, " + DBPrefix + "connectionsetting.port, " + DBPrefix + "connections.host, " + DBPrefix + "connections.name FROM " + DBPrefix + "userhistory INNER JOIN " + DBPrefix + "connectionsetting ON " + DBPrefix + "userhistory.connectionsettingid=" + DBPrefix + "connectionsetting.id INNER JOIN " + DBPrefix + "connections ON " + DBPrefix + "connectionsetting.connectionid=" + DBPrefix + "connections.id WHERE " + DBPrefix + "userhistory.userid=" + userId + " ORDER BY " + DBPrefix + "userhistory.pointoftime DESC;"));
            //return (sqlGetDataTable("SELECT * FROM userhistory WHERE userid=" + userId.ToString() + ";"));
        }

        /// <summary>
        /// Get a number of entries of the history of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHistoryDate(long userId, DateTime date)
        {
            string firstDate = "'" + date.Year + "-" + date.Month.ToString("00") + "-" + date.Day.ToString("00") + " 00:00:00'";
            string lastDate = "'" + date.Year + "-" + date.Month.ToString("00") + "-" + date.Day.ToString("00") + " 23:59:59'";

            return (sqlGetDataTable("SELECT " + DBPrefix + "userhistory.*, " + DBPrefix + "connectionsetting.protocol, " + DBPrefix + "connectionsetting.connectionid, " + DBPrefix + "connectionsetting.port, " + DBPrefix + "connections.host, " + DBPrefix + "connections.name FROM " + DBPrefix + "userhistory INNER JOIN " + DBPrefix + "connectionsetting ON " + DBPrefix + "userhistory.connectionsettingid=" + DBPrefix + "connectionsetting.id INNER JOIN " + DBPrefix + "connections ON " + DBPrefix + "connectionsetting.connectionid=" + DBPrefix + "connections.id WHERE " + DBPrefix + "userhistory.userid=" + userId + " AND pointoftime >= " + firstDate + " AND pointoftime <= " + lastDate + " ORDER BY " + DBPrefix + "userhistory.pointoftime DESC;"));
            //return (sqlGetDataTable("SELECT * FROM userhistory WHERE userid=" + userId.ToString() + " AND pointoftime >= " + firstDate + " AND pointoftime <= " + lastDate + " ORDER BY pointoftime DESC;"));
        }

        /// <summary>
        /// Get a number of entries of the history of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHistoryCount(long userId, int count, int offset)
        {
            return (sqlGetDataTable("SELECT " + DBPrefix + "userhistory.*, " + DBPrefix + "connectionsetting.protocol, " + DBPrefix + "connectionsetting.connectionid, " + DBPrefix + "connectionsetting.port, " + DBPrefix + "connections.host, " + DBPrefix + "connections.name FROM " + DBPrefix + "userhistory INNER JOIN " + DBPrefix + "connectionsetting ON " + DBPrefix + "userhistory.connectionsettingid=" + DBPrefix + "connectionsetting.id INNER JOIN " + DBPrefix + "connections ON " + DBPrefix + "connectionsetting.connectionid=" + DBPrefix + "connections.id WHERE " + DBPrefix + "userhistory.userid=" + userId + " ORDER BY " + DBPrefix + "userhistory.pointoftime DESC LIMIT " + count.ToString() + " OFFSET " + offset.ToString() + ";"));
            //return (sqlGetDataTable("SELECT * FROM userhistory WHERE userid=" + userId.ToString() + " ORDER BY pointoftime DESC LIMIT " + count.ToString() + " OFFSET " + offset.ToString() + ";"));
        }

        /// <summary>
        /// Get the numberth Entry of a user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public DataTable GetHistoryEntryNumber(long userid, int number)
        {
            return (sqlGetDataTable("SELECT " + DBPrefix + "userhistory.*, " + DBPrefix + "connectionsetting.protocol, " + DBPrefix + "connectionsetting.connectionid, " + DBPrefix + "connectionsetting.port, " + DBPrefix + "connections.host, " + DBPrefix + "connections.name FROM " + DBPrefix + "userhistory INNER JOIN " + DBPrefix + "connectionsetting ON " + DBPrefix + "userhistory.connectionsettingid=" + DBPrefix + "connectionsetting.id INNER JOIN " + DBPrefix + "connections ON " + DBPrefix + "connectionsetting.connectionid=" + DBPrefix + "connections.id WHERE userid=" + userid.ToString() + " LIMIT 1 OFFSET " + number.ToString() + ";"));
            //return(sqlGetDataTable("SELECT * FROM userhistory WHERE userid=" + userid.ToString() + " LIMIT 1 OFFSET " + number.ToString() + ";"));
        }

        /// <summary>
        /// Cleans the database of old history-Entries
        /// </summary>
        /// <param name="userid">The Id of the User</param>
        /// <param name="olderstId">the ID of the oldest Entry that should stay</param>
        public void TidyUpHistory(long userid, int oldestId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "userhistory WHERE userid = " + userid.ToString() + " AND id < " + oldestId + ";");
        }

        /// <summary>
        /// Deletes all Entries from a specific ConnectionSettingId
        /// </summary>
        /// <param name="connectionSettingId">If of the ConnectionSetting</param>
        public void DeleteUserHistoryConnectionSettingId(long connectionSettingId)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "userhistory WHERE connectionsettingid=" + connectionSettingId.ToString() + ";");
        }



        /// <summary>
        /// Gets all VPNs of a User
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        public DataTable GetUserVpnConnections(long userId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "vpn WHERE owner=" + userId + ";"));
        }

        /// <summary>
        /// Gets the Definde VPN
        /// </summary>
        /// <param name="id">ID of the VPN</param>
        /// <returns></returns>
        public DataTable GetUserVpnConnection(int id)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "vpn WHERE id=" + id + ";"));
        }

        /// <summary>
        /// Adds a new VPN
        /// </summary>
        /// <param name="userId">ID of the owner</param>
        /// <param name="type">Type of the VPN</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="parameter4"></param>
        /// <param name="parameter5"></param>
        /// <param name="parameter6"></param>
        /// <param name="parameter7"></param>
        /// <param name="parameter8"></param>
        /// <param name="parameter9"></param>
        /// <param name="parameter10"></param>
        /// <param name="name">Name of the VPN</param>
        /// <returns></returns>
        public long AddUserVpnConnection(long userId, int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name)
        {
            return (
                sqlExecuteNonQueryWithId(
                    String.Format(
                        "INSERT INTO {0}vpn ('type', 'parameter1', 'parameter2', 'parameter3', 'parameter4', 'parameter5', 'parameter6', 'parameter7', 'parameter8', 'parameter9', 'parameter10', 'owner', 'name') " +
                        "VALUES ({1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', {12}, '{13}');",
                        new object[]
                        {
                            DBPrefix,
                            type,
                            parameter1,
                            parameter2,
                            parameter3,
                            parameter4,
                            parameter5,
                            parameter6,
                            parameter7,
                            parameter8,
                            parameter9,
                            parameter10,
                            userId,
                            name
                        }
                        )
                    )
                );
        }

        /// <summary>
        /// Deletes the VPN with the given ID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUserVpnConnection(int id)
        {
            sqlExecuteNonQuery(String.Format("DELETE FROM {0}vpn WHERE id={1};", DBPrefix, id));
        }

        /// <summary>
        /// Edit the parameter of an existing VPN
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="parameter4"></param>
        /// <param name="parameter5"></param>
        /// <param name="parameter6"></param>
        /// <param name="parameter7"></param>
        /// <param name="parameter8"></param>
        /// <param name="parameter9"></param>
        /// <param name="parameter10"></param>
        /// <param name="name"></param>
        public void EditUserVpnConnection(int id, int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name)
        {
            sqlExecuteNonQuery
                (
                    String.Format
                        (
                            "UPDATE {0}vpn SET 'type'={1}, 'parameter1'='{2}', 'parameter2'='{3}', 'parameter3'='{4}', 'parameter4'='{5}', 'parameter5'='{6}', 'parameter6'='{7}', 'parameter7'='{8}', 'parameter8'='{9}', 'parameter9'='{10}', 'parameter10'='{11}', 'name'='{12}' WHERE id={13};",
                            new object[]
                            {
                                DBPrefix,
                                type,
                                parameter1,
                                parameter2,
                                parameter3,
                                parameter4,
                                parameter5,
                                parameter6,
                                parameter7,
                                parameter8,
                                parameter9,
                                parameter10,
                                name,
                                id
                            }
                        )
                );
        }



        /// <summary>
        /// Sets data to the database
        /// </summary>
        /// <param name="data">The Data to add to the Database</param>        
        public long AddData(byte[] data)
        {
            return (sqlExecuteNonQueryDataWithId("INSERT INTO " + DBPrefix + "data (data) VALUES (@0);", data));
        }

        /// <summary>
        /// Sets data to the database
        /// </summary>
        /// <param name="data">The data to add to the Database</param>
        /// <param name="currentId">The data-ID that should be updated</param>
        public void SetData(byte[] data, int currentId)
        {
            sqlExecuteNonQueryData("UPDATE " + DBPrefix + "data SET data=@0 WHERE id=" + currentId + ";", data);
        }

        /// <summary>
        /// Gets stored data
        /// </summary>
        /// <param name="id">DataID</param>
        /// <returns>The byte-Array with the queried data</returns>
        public byte[] GetData(int id)
        {
            return (sqlGetData("SELECT data FROM " + DBPrefix + "data WHERE id=" + id.ToString() + ";"));
        }



        /// <summary>
        /// Retuns the complete Table
        /// </summary>
        /// <param name="tablename">Name of the Table to return</param>
        /// <returns>the complete table</returns>
        public DataTable TableGet(string tablename)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + tablename + ";"));
        }

        /// <summary>
        /// Deletes a complete Table ("Drop")
        /// </summary>
        /// <param name="tablename">Table to delete</param>
        /// <returns>success/failed</returns>
        public bool TableDelete(string tablename)
        {
            try
            {
                sqlExecuteNonQuery("DROP TABLE " + DBPrefix + tablename + ";");
                Logger.Log(LogEntryType.Info, "Table " + tablename + " deleted");
                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Cannot delete table " + tablename + ". Error: " + ea.ToString());
                return (false);
            }
        }

        /// <summary>
        /// Adds a new Table
        /// </summary>
        /// <param name="table">All the Table-Information</param>
        /// <returns>success/failed</returns>
        public bool TableAdd(DataTable table)
        {
            string sqlCommand = "BEGIN; "; //Begin Transaction
            try
            {
                #region Add Table
                sqlCommand += "CREATE TABLE '" + DBPrefix + table.TableName + "'("; //Start Create table
                string columnString = "("; //is used in content-Adding

                //Add Columns to sqlCommand
                foreach (DataColumn dC in table.Columns)
                {
                    sqlCommand += "'" + dC.ColumnName + "' " + ConvertToSqLiteDataType(dC.DataType.ToString()); //Base declaration
                    if (dC.AutoIncrement) sqlCommand += " PRIMARY KEY AUTOINCREMENT"; //If the Column is the Primary key, declare it
                    sqlCommand += ", "; //End this Column

                    columnString += dC.ColumnName + ", ";  //Used in Content-Adding
                }

                sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 2); //Remove the last ", "

                sqlCommand += "); "; //Ends the Create-Command
                #endregion

                #region Add Content
                columnString = columnString.Substring(0, columnString.Length - 2); //Removes the last ", ";
                columnString += ")"; //Closes the brackets

                foreach (DataRow dR in table.Rows)
                {
                    sqlCommand += "INSERT INTO '" + DBPrefix + table.TableName + "' " + columnString + " VALUES (";

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (table.Columns[i].DataType == typeof(byte) ||
                            table.Columns[i].DataType == typeof(Int16) ||
                            table.Columns[i].DataType == typeof(Int32) ||
                            table.Columns[i].DataType == typeof(Int64) ||
                            table.Columns[i].DataType == typeof(bool))
                        {
                            sqlCommand += dR[i].ToString() + ", ";
                        }
                        else
                        {
                            sqlCommand += "'" + dR[i].ToString() + "', ";
                        }
                    }
                    sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 2); //Removes the last ", ";
                    sqlCommand += ";"; //Finishes the INSERT-Command
                }
                #endregion

                sqlCommand += "COMMIT;";

                sqlExecuteNonQuery(sqlCommand);

                Logger.Log(LogEntryType.Info, "Table " + DBPrefix + table.TableName + " created successful");
                return(true);
            }
            catch(Exception ea)
            { 
                Logger.Log(LogEntryType.Exception, "Error on Table-Creation with sql-Code: \"" + sqlCommand + "\"" + System.Environment.NewLine + "Errormessage: " + ea.ToString());
                return(false);
            }
        }

        /// <summary>
        /// Adds a Column to an existing table
        /// </summary>
        /// <param name="columnname">The unique name of the new Column</param>
        /// <param name="datatype">The datatype of the new Column</param>
        /// <param name="datatype">The name of the table where the column should be added to</param>
        /// <returns>success/fail</returns>
        public bool TableColumnAdd(string columnname, string datatype, string table, string defValue)
        {
            try
            {
                sqlExecuteNonQuery("ALTER TABLE '" + DBPrefix + table + "' ADD COLUMN '" + columnname + "' " + datatype + " DEFAULT " + defValue + ";");
                return(true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error on adding column " + columnname + " to table " + DBPrefix + table, ea);
                return(false);
            }
        }

        /// <summary>
        /// Cleans all Entries from a Table
        /// </summary>
        /// <param name="tablename">Name of the Table</param>
        /// <returns>success/failed</returns>
        public bool TableFlush(string tablename)
        {
            try
            {
                sqlExecuteNonQuery("DELETE FROM '" + DBPrefix + tablename + "';");
                Logger.Log(LogEntryType.Info, "Table " + DBPrefix + tablename + " flushed");
                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error flushing Table " + DBPrefix + tablename + " Error: " + ea.ToString());
                return (false);
            }
        }

        /// <summary>
        /// Adds a Value to a Table
        /// </summary>
        /// <param name="tablename">Name of the Table</param>
        /// <returns>success/failed</returns>
        public bool TableValueAdd(Dictionary<string,string> RowValues, string tablename)
        {
            try
            {
                string sqlCommand = "INSERT INTO '" + DBPrefix + tablename + "' (";
                string valueString = "";

                foreach (KeyValuePair<string, string> kvp in RowValues)
                {
                    sqlCommand += kvp.Key + ", ";

                    if (kvp.Value.GetType() == typeof(byte) ||
                            kvp.Value.GetType() == typeof(Int16) ||
                            kvp.Value.GetType() == typeof(Int32) ||
                            kvp.Value.GetType() == typeof(Int64) ||
                            kvp.Value.GetType() == typeof(bool))
                    {
                        valueString += kvp.Value + ", ";
                    }
                    else
                    {
                        valueString += "'" + kvp.Value + "', ";
                    }
                }

                sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 2); //Remove the last ", ";
                valueString = valueString.Substring(0, valueString.Length - 2); //Remove the last ", ";

                sqlCommand += ") VALUES (" + valueString + ");"; //Fill the sqlCommand-String

                //Execute Command
                sqlExecuteNonQuery(sqlCommand);

                Logger.Log(LogEntryType.Info, "Value to Table " + DBPrefix + tablename + " added successful");
                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error adding value to Table " + DBPrefix + tablename + " Error: " + ea.ToString());
                return (false);
            }
        }

        /// <summary>
        /// Deletes a Value from 
        /// </summary>
        /// <param name="Filter">Column/Value-Pair for deleting the corret value</param>
        /// <param name="tablename">The table to delete from</param>
        /// <returns>Success/Fail</returns>
        public bool TableValueDelete(Dictionary<string, string> Filter, string tablename)
        {
            try
            {
                string sqlCommand = "DELETE FROM '" + DBPrefix + tablename + "' WHERE ";

                //Build the WHERE-Clause
                foreach (KeyValuePair<string, string> kvp in Filter)
                {
                    sqlCommand += "(" + kvp.Key + "='" + kvp.Value+ "') AND ";
                }

                sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 5); //Remove the " AND "

                sqlCommand += ";";

                sqlExecuteNonQuery(sqlCommand);

                Logger.Log(LogEntryType.Info, "Deleted Value from Table " + DBPrefix + tablename + " successful");
                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Error deleting value from Table " + DBPrefix + tablename + " Error: " + ea.ToString());
                return (false);
            }
        }

        /// <summary>
        /// Gets Information about a Table (i.e. the containing Columns and its datatype)
        /// </summary>
        /// <param name="table">The Table</param>
        /// <returns>All TableInfos</returns>
        public DataTable GetTableInfos(string table)
        {
            return (sqlGetDataTable("PRAGMA table_info('" + DBPrefix + table + "');"));
        }


        /// <summary>
        /// Adds a new FilterSet (without Filters)
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="owner">The UserID of the owner</param>
        /// <param name="isPublic">Is the FilterSet public?</param>
        /// <returns>The ID of the new FilterSet</returns>
        public long AddFilterSet(string name, int owner, bool isPublic)
        {
            return (sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "filterset (title, owner, public) VALUES ('" + name + "', " + owner.ToString() + ", " + (isPublic?"1":"0") + ");"));
        }

        /// <summary>
        /// Adds a new FilterSet (without Filters)
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="owner">The UserID of the owner</param>
        /// <param name="isPublic">Is the FilterSet public?</param>
        /// <param name="parent">Is the FilterSet public?</param>
        /// <returns>The ID of the new FilterSet</returns>
        public long AddFilterSet(string name, int owner, bool isPublic, long parent, bool hide)
        {
            return (sqlExecuteNonQueryWithId("INSERT INTO " + DBPrefix + "filterset (title, owner, public, parent, hide) VALUES ('" + name + "', " + owner.ToString() + ", " + (isPublic ? "1" : "0") + ", " + parent + ", " + (hide?"1":"0") + ");"));
        }

        /// <summary>
        /// Adds a new Filter to a FilterSet
        /// </summary>
        /// <param name="condition">The condition of this filter (i.e. Name, Host, OS, ...)</param>
        /// <param name="isNegative">Is this condition negetiated?</param>
        /// <param name="isOr">Is after this Filter an "OR" instead of an "AND"?</param>
        /// <param name="isLike">Is this a "LIKE"-Filter instead of an "EQUAL"?</param>
        /// <param name="phrase">This phrase that the condition is compared to as byte-Array</param>
        /// <param name="filterSetId">The Filterset this filter belongs to</param>
        /// <returns></returns>
        public long AddFilter(string condition, bool isNegative, bool isOr, bool isLike, byte[] phrase, string description, long filterSetId)
        {
            return (sqlExecuteNonQueryDataWithId("INSERT INTO " + DBPrefix + "filter (condition, isnegative, isor, islike, phrase, filtersetid, description, sortorder) VALUES (" +
                "'" + condition + "', " +
                (isNegative ? "1" : "0") + ", " +
                (isOr ? "1" : "0") + ", " +
                (isLike ? "1" : "0") + ", " +
                "@0, " +
                filterSetId.ToString() + ", " +
                "'" + description + "', " +
                "(SELECT COUNT(*) FROM "+DBPrefix +"filter WHERE filtersetid=" + filterSetId.ToString() + ")" + //Currently Count of filters
                ");", phrase));
        }

        /// <summary>
        /// Deletes a filterset and all connected filters
        /// </summary>
        /// <param name="id">the ID of the filterset</param>
        public void DeleteFilterSet(long id)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "filter WHERE filtersetid=" + id + ";"); //Delete Filters
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "filterset WHERE id=" + id + ";"); //Delete the Filterset
        }

        /// <summary>
        /// Gets all FilterSets of a User
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetFilterSets(long userid, long FilterSetId)
        {
            return(sqlGetDataTable("SELECT * FROM " + DBPrefix + "filterset WHERE owner=" + userid + (FilterSetId>0?" AND id=" + FilterSetId:"") + ";"));
        }

        /// <summary>
        /// Gets all FilterSet IDs and Names of a User
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetFilterSetsBasic(long userid)
        {
            return (sqlGetDataTable("SELECT id, title FROM " + DBPrefix + "filterset WHERE owner=" + userid + " AND hide=0;"));
        }

        /// <summary>
        /// Gets a FilterSet
        /// </summary>
        /// <param name="filtersetid"></param>
        /// <returns></returns>
        public DataTable GetFilterSet(long filtersetid)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "filterset WHERE id=" + filtersetid + ";"));
        }

        /// <summary>
        /// Gets a Filter with the given filterSetId
        /// </summary>
        /// <param name="filterSetId">The ID of the FilterSet</param>
        /// <returns>All details to the selected filter</returns>
        public DataTable GetFilter(long filterSetId)
        {
            return (sqlGetDataTable("SELECT * FROM " + DBPrefix + "filter WHERE filtersetid=" + filterSetId + " ORDER BY sortorder;"));
        }

        /// <summary>
        /// Modifies settings of a Filterset
        /// </summary>
        /// <param name="id">THe ID of the FilterSet to modify</param>
        /// <param name="title">The new Title of the FilterSet</param>
        public void ModifyFilterSet(long id, string title, bool hide)
        {
            sqlExecuteNonQuery("UPDATE " + DBPrefix + "filterset SET title='" + title + "', hide=" + (hide?"1":"0") + " WHERE id=" + id.ToString() + ";");
        }

        /// <summary>
        /// Gets all Ids of the Filters of a FilterSet
        /// </summary>
        /// <param name="filterSetId"></param>
        /// <returns></returns>
        public DataTable GetFilterIds(long filterSetId)
        {
            return (sqlGetDataTable("SELECT id FROM " + DBPrefix + "filter WHERE filtersetid=" + filterSetId.ToString()));
        }

        /// <summary>
        /// Updates the parameters of a Filter
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isNegative"></param>
        /// <param name="isOr"></param>
        /// <param name="isLike"></param>
        /// <param name="phrase"></param>
        /// <param name="filterSetId"></param>
        public void ModifyFilter(string condition, bool isNegative, bool isOr, bool isLike, byte[] phrase, string description, long filterSetId, long filterId)
        {
            sqlExecuteNonQueryData("UPDATE " + DBPrefix + "filter SET " +
                "condition='" + condition + "', " +
                "isnegative=" + (isNegative ? "1" : "0") + ", " +
                "isor=" + (isOr ? "1" : "0") + ", " +
                "islike=" + (isLike ? "1" : "0") + ", " +
                "phrase=@0, " +
                "description='" + description + "', " +
                "filtersetid=" + filterSetId.ToString() +" " +
                "WHERE id=" + filterId.ToString() + ";", phrase);
        }

        /// <summary>
        /// Delete a filter
        /// </summary>
        /// <param name="id">ID of the filter to delete</param>
        public void DeleteFilter(long id)
        {
            sqlExecuteNonQuery("DELETE FROM " + DBPrefix + "filter WHERE id=" + id + ";");
        }

        /// <summary>
        /// Gets the Filterresults of Connectionproperties
        /// </summary>
        /// <param name="host"></param>
        /// <param name="name"></param>
        /// <param name="Description"></param>
        /// <param name="folder"></param>
        /// <param name="owner"></param>
        /// <param name="os"></param>
        /// <param name="ispublic"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public DataTable GetFilterResults(List<List<object>> host, List<List<object>> name, List<List<object>> Description, List<List<object>> folder, List<List<object>> owner, List<List<object>> os, List<List<object>> ispublic, List<List<object>> id, long userId)
        //{
        //    string sqlCmd = "SELECT * FROM " + DBPrefix + "connections WHERE (owner=" + userId + " OR ispublic=1)"; //Basic search-String without conditions

        //    foreach (List<object> aHost in host) sqlCmd += getFilterResultsHelper("host",  aHost[0].ToString(), (int)aHost[1], true);
        //    foreach (List<object> aName in name) sqlCmd += getFilterResultsHelper("name", aName[0].ToString(), (int)aName[1], true);
        //    foreach (List<object> aDescr in Description) sqlCmd += getFilterResultsHelper("description", aDescr[0].ToString(), (int)aDescr[1], true);
        //    foreach (List<object> aFolder in folder) sqlCmd += getFilterResultsHelper("folder", aFolder[0].ToString(), (int)aFolder[1], false);
        //    foreach (List<object> aOwner in owner) sqlCmd += getFilterResultsHelper("owner", aOwner[0].ToString(), (int)aOwner[1], false);
        //    foreach (List<object> aOS in os) sqlCmd += getFilterResultsHelper("os", aOS[0].ToString(), (int)aOS[1], false);
        //    foreach (List<object> aIsPub in ispublic) sqlCmd += getFilterResultsHelper("ispublic", (((bool)aIsPub[0])?"1":"0"), (int)aIsPub[1], false);
        //    foreach (List<object> aId in id) sqlCmd += getFilterResultsHelper("id", aId[0].ToString(), (int)aId[1], false);

        //    sqlCmd += ";";

        //    DataTable dT = sqlGetDataTable(sqlCmd);

        //    return (dT);
        //}

        /// <summary>
        /// Gets Results of ConnectionSetting related properties
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilterResults(List<List<object>> parameter, long userId)
        {
            string sqlCmd = "SELECT DISTINCT con.* FROM " + DBPrefix + "connections AS con INNER JOIN " + DBPrefix + "connectionsetting AS cs ON con.id=cs.connectionid " +
            "WHERE (con.owner=" + userId + " OR " + DBPrefix + "con.ispublic=1)"; //Basic search-String without conditions

            foreach (List<object> para in parameter)
            {
                string typ = para[2].ToString();
                string filterValue = para[0].ToString();
                int filterOperation = (int)para[1];

                switch (typ)
                {                    
                    //All string properties (include ' in around value) for connections
                    case "host":
                    case "name":
                    case "description":
                        sqlCmd += getFilterResultsHelper("UPPER(con." + typ + ")", filterValue, filterOperation, true);
                        break;

                    //All numeric properties for connections
                    case "folder":
                    case "owner":
                    case "os":                    
                    case "id":
                        sqlCmd += getFilterResultsHelper("con." + typ, filterValue, filterOperation, false);
                        break;

                    //Boolean value (incuding a "is"-Prefix)
                    case "public":
                        sqlCmd += getFilterResultsHelper("con.is" + typ, (filterValue == "True" ? "1" : "0"), filterOperation, false);
                        break;


                    //All numeric properties for connectionsettings
                    case "port":
                        sqlCmd += getFilterResultsHelper("cs." + typ, filterValue, filterOperation, false);
                        break;

                    case "credential":
                        sqlCmd += getFilterResultsHelper("cs." + typ + "id", filterValue, filterOperation, false);
                        break;

                    //All string properties (include ' in around value) for connectionsettings
                    case "protocol":
                        sqlCmd += getFilterResultsHelper("UPPER(cs." + typ + ")", filterValue, filterOperation, true);
                        break;
                }
            }
            
            sqlCmd += ";";

            DataTable dT = sqlGetDataTable(sqlCmd);

            return (dT);
        }

        #region SmartStorage
        /// <summary>
        /// Creates a Table, if it doesn't exists for the SmartStorage-System
        /// </summary>
        /// <param name="tablename">The name of the new table</param>
        /// <returns></returns>
        public bool SmartStorageCreateTable(string tablename)
        {
            try
            {
                sqlExecuteNonQuery("CREATE TABLE IF NOT EXISTS '" + DBPrefix + tablename + "' (id VARCHAR(100) PRIMARY KEY, data BLOB, userid INT);");
                return (true);
            }
            catch
            {
                return (false);
            }
        }

        /// <summary>
        /// Creates a new Value for SmartStorage-Plugins
        /// </summary>
        /// <param name="name">The name of the new value</param>
        /// <param name="tablename">The tablename for the new value</param>
        /// <returns></returns>
        public bool SmartStorageCreateValue(string name, string tablename, long userId)
        {
            try
            {
                sqlExecuteNonQueryData("INSERT INTO '" + DBPrefix + tablename + "' (id, data, userid) VALUES ('" + name + "', @0, " + userId + ");", null);
                return (true);
            }
            catch
            {
                return (false);
            }
        }

        /// <summary>
        /// Write data to the Storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public bool SmartStorageWriteValue(string name, byte[] data, string tablename, long userId)
        {
            try
            {
                sqlExecuteNonQueryData("UPDATE '" + DBPrefix + tablename + "' SET 'data'=@0 WHERE userid=" + userId + " AND id='" + name + "';", data);                
                return(true);
            }
            catch
            {
                return(false);
            }
        }

        /// <summary>
        /// Reads a value for SmartStorage-Plugin
        /// </summary>
        /// <param name="name">The Name of the value to get</param>
        /// <param name="tablename">The tablename for the SmartStorage-Plugin</param>
        /// <returns>The serialized object</returns>
        public byte[] SmartStorageReadValue(string name, string tablename, long userId)
        {
            return(sqlGetData("SELECT data FROM '" + DBPrefix + tablename + "' WHERE id='" + name.ToString() + "' AND userid=" + userId + ";"));
        }

        /// <summary>
        /// Deletes a already stored value of a SmartStorage-Plugins
        /// </summary>
        /// <param name="name">Name of the value</param>
        /// <param name="tablename">Table of the SmartStorage-Plugin</param>
        /// <returns></returns>
        public bool SmartStorageDeleteValue(string name, string tablename, long userId)
        {
            try
            {
                sqlExecuteNonQuery("DELETE FROM '" + DBPrefix + tablename + "' WHERE id='" + name + "' AND userid=" + userId + ";");
                return (true);
            }
            catch
            {
                return (false);
            }
        }

        /// <summary>
        /// Checks if a specific value of a SmartStorage-Plugin exists
        /// </summary>
        /// <param name="name">The name of the value</param>
        /// <param name="tablename">The table of the SmartStorage-Plugin</param>
        /// <returns></returns>
        public bool SmartStorageValueExists(string name, string tablename, long userId)
        {
            if (sqlGetInt16("SELECT COUNT(*) FROM '" + DBPrefix + tablename + "' WHERE id='" + name + "' AND userid=" + userId + ";") > 0)
                return (true);
            else
                return (false);
        }
        #endregion


        #region Helper
        private string getFilterResultsHelper(string condition, string compareValue, int flag, bool valueIsString)
        {
            string ret = "";
            bool isOr = false;
            bool isLike = false;
            bool isNot = false;
            
            //Decode the flags
            if (flag >= 2097152)
            {
                flag -= 2097152;
                isLike = true;
            }
            if (flag >= 1048576)
            {
                flag -= 1048576;
                isNot = true;
            }
            if (flag == 1)
            {
                flag -= 1;
                isOr = true;
            }


            //Build the WHERE-Clause
            if (isOr)
                ret = " OR";
            else
                ret = " AND";

            if (isNot && !isLike)
                ret += " NOT";

            ret += " " + condition;

            if (!isLike)
                ret += "=";
            else if (isLike && isNot)
                ret += " NOT LIKE ";
            else if (isLike)
                ret += " LIKE ";

            if (valueIsString)
                ret += "UPPER('";

            if (isLike)
                ret += "%";

            ret += compareValue;

            if (isLike)
                ret += "%";

            if (valueIsString)
                ret += "')";

            return (ret);
        }


        /// <summary>
        /// Converts a Datatype-String to the Datatype-Definition auf SQLite
        /// </summary>
        /// <param name="datatype">i.e. System.String</param>
        /// <returns>i.e. VARCHAR(100)</returns>
        private string ConvertToSqLiteDataType(string datatype)
        {
            switch (datatype.ToLower())
            { 
                case "system.string":
                    return ("VARCHAR(100)");
                case "system.int16":                    
                case "system.int32":
                case "system.int64":                
                case "system.byte":
                    return ("INTEGER");
                case "system.boolean":
                    return ("BIT");
                case "system.datetime":
                    return ("DATETIME");
                case "system.double":
                    return ("DOUBLE");
                case "system.float":
                    return ("FLOAT");
                case "system.object":
                    return ("BLOB");
                default:
                    return ("TEXT");
            }
        }
        #endregion

        #region schema

        public DataTable SchemaGetTableNames()
        {
            return (sqlGetDataTable("SELECT name FROM sqlite_master WHERE type='table';"));
        }

        public DataTable SchemaGetTableDetails(string tableName)
        {
            return (sqlGetDataTable(String.Format("PRAGMA table_info({0});", tableName)));
        }

        public void SchemaAddTable(string tableName, string columnDefinitions)
        {
            sqlExecuteNonQuery(string.Format("CREATE TABLE IF NOT EXISTS {0}{1} ({2});", _DBPrefix, tableName, columnDefinitions));
        }

        public void SchemaAddColumn(string tableName, string columnName, string type, bool notNull, bool isPrimaryKey, string defaultValue)
        {
            var parameter = "";
            if (notNull)
                parameter = " NOT NULL";
            if (isPrimaryKey)
                parameter += " PRIMARY KEY";
            if (defaultValue.Length != 0)
                parameter += " DEFAULT " + defaultValue;

            sqlExecuteNonQuery(string.Format("ALTER TABLE {0} ADD COLUMN {1} {2}{3};", tableName, columnName, type, parameter));
        }

        public void SchemaAddRow(string columnNames, string content, string tableName)
        {
            sqlExecuteNonQuery(string.Format("INSERT INTO {0}{1} ({2}) VALUES ({3});", _DBPrefix, tableName, columnNames, content));
        }

        public void SchemaUpdateRow(string changeString, string whereString, string tablename)
        {
            sqlExecuteNonQuery(string.Format("UPDATE {0}{1} SET {2} WHERE {3};", _DBPrefix, tablename, changeString, whereString));
        }

        #endregion

        #region Table-Managemagement (install/update)
        /// <summary>
        /// Installs the Tables to the current version at first execution of beRemote
        /// </summary>
        /// <param name="generateGuid">A Database GUID should be generated</param>
        public void InstallDatabaseTables(bool generateGuid)
        {
            return;

            //Check if it is the first run of the Database
            int tableCount = GetTableCount();
            if (tableCount != 0)
            {
                return; //It is not the first run; cancel here
            }

            string installCommands = "";

//******************* CREATE TABLES ***************
            //Installs the settings-Table and Version of the Database (nesseccary, because the Version has to be present all the time!)
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "settings (setting VARCHAR(100) NOT NULL PRIMARY KEY, value VARCHAR(100));" +
                                 "INSERT INTO " + DBPrefix + "settings (setting, value) VALUES ('version','" + _DBVersion.ToString() + "');";

            //Installs the connections-Table (Host)
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "connections (id INTEGER PRIMARY KEY, host VARCHAR(256), name VARCHAR(100), " +
                "description VARCHAR(200), folder INT, sortorder INT, os INT, owner INT, ispublic BIT);";

            //Installs the connectionsetting-Table (Saved connection incl. protocol)
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "connectionsetting (id INTEGER PRIMARY KEY, connectionid INT, protocol VARCHAR(100), " +
                "port INT, credentialid INT);";
            
            //Installs the connectionoptions-Table (Protocol-Settings)
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "connectionoptions (id INTEGER PRIMARY KEY, connectionsettingid INT, " +
                "optionname VARCHAR(100), value VARCHAR(100));";

            //Installs the user-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "user (id INTEGER PRIMARY KEY, name VARCHAR(100), password VARCHAR(100), " +
                "winname VARCHAR(250), autologin BIT, lastmachine VARCHAR(100), superadmin BIT, lastlogin DATETIME, logincount INT, " +
                "lastlogout DATETIME, defaultfolder INT, defaultprotocol VARCHAR(100), heartbeat DATETIME, updatemode INT, " +
                "salt1 CHAR(512), salt2 CHAR(512), salt3 CHAR(512));";

            //Installs uservisuals-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "uservisuals (userid INT, " +
                "mainwindowx INT, mainwindowy INT, mainwindowmax BIT, " + //Mainwindow-Settings (Position x/y or is maximized)
                "mainwindowwidth INT, mainwindowheight INT, " + //Mainwindow-Settings (Width/Height)
                "ribbonexpanded TEXT, " +//Is the Ribbon-Expanded
                "expandednodes TEXT, " + //The View of the Connectiontreeview with all ids, that are expanded (i.e. d1,h2 = DirectoryID1 and HostID2 are expanded)
                "listwidth INT, " + //the width of the connectionlist
                "statusbarsetting INT, " + //0 = everthing enabled; 1 = statusbar disabled; 2=no user; 4=no time; 8=no active connections; 16=no stopwatch
                "favorites TEXT" + //contains connectionid;connectionid;.... to store the Favorite buttons
                "gridlayout TEXT" + //contains the Layout of the Avalon-Grid
                "ribbonqat TEXT" + //contains the ribbon-Buttons in the QAT-Area
                ");";  
            
            //Installs the folder-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "folder (id INTEGER PRIMARY KEY, foldername VARCHAR(100), parent INT, sortorder INT, " +
                "owner INT, ispublic BIT);";

            //Installs the operatingsystems-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "operatingsystems (id INTEGER, family VARCHAR(100), distribution VARCHAR(100), " +
                "version VARCHAR(100));";

            //Installs the credentials-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "credentials (id INTEGER PRIMARY KEY, username VARCHAR(100), password VARCHAR(100), " +
                "domain VARCHAR(100), owner INT, description VARCHAR(100));";

            //Installs the licenses-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "licenses (firstname VARCHAR(100), lastname VARCHAR(100), email VARCHAR(100), " +
                "secret VARCHAR(100), userid INT);";

            //Install the history-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "userhistory (id INTEGER PRIMARY KEY, userid INTEGER, connectionsettingid INTEGER, pointoftime DATETIME);";

            //Install the data-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "data (id INTEGER PRIMARY KEY, data BLOB);";

            //Install the filterset-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "filterset (id INTEGER PRIMARY KEY, title VARCHAR(100), owner INTEGER, public BIT, parent INTEGER DEFAULT 0, hide BIT);";

            //Install the filter-Table
            installCommands += "CREATE TABLE IF NOT EXISTS " + DBPrefix + "filter (id INTEGER PRIMARY KEY, condition VARCHAR(50), isnegative BIT, isor BIT, islike BIT, " +
                "phrase BLOB, filtersetid INTEGER, sortorder INTEGER, description VARCHAR(100));";

            //Install the vpn-Table
            installCommands += "CREATE Table IF NOT EXISTS " + DBPrefix + "'vpn' ('id' INTEGER PRIMARY KEY AUTOINCREMENT, 'type' INTEGER NOT NULL, 'parameter1' TEXT, 'parameter2' TEXT, 'parameter3' TEXT, " +
                               "'parameter4' TEXT, 'parameter5' TEXT, 'parameter6' TEXT, 'parameter7' TEXT, 'parameter8' TEXT, 'parameter9' TEXT, 'parameter10' TEXT, 'owner' INTEGER, " +
                               "'name' TEXT DEFAULT ('Unnamed') NOT NULL);";

//******************* ADD DATA ***************
            //Add the superadmin (has to be directly added because of logincount = 1)
            installCommands += "INSERT INTO " + DBPrefix + "user (name, password, winname, autologin, lastmachine, superadmin, lastlogin, logincount, lastlogout, defaultfolder, defaultprotocol, heartbeat, updatemode) VALUES " +
                "('superadmin','B675BF6C4229105AC877634F7F13FC89','superadmin',0,'" + System.Environment.MachineName + "', 1, date('now','localtime'), 1, date('now','localtime'), 0, '', date('now','localtime'),0);";

            installCommands += "INSERT INTO " + DBPrefix + "uservisuals (userid, mainwindowx, mainwindowy, mainwindowmax, mainwindowwidth, mainwindowheight, ribbonexpanded, expandednodes, listwidth, statusbarsetting, favorites) VALUES " +
                "(1, 100, 100, 0, 1000, 800, 0, '', 150,0, '');";

            //Add Operating Systems --- Will be maintained in the dbmaint.cs
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (0, 'Undefined', 'Operating', 'System');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1, 'Website', '', '');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (990, 'Microsoft', 'Windows', '8.1');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1000, 'Microsoft', 'Windows', '8');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1100, 'Microsoft', 'Windows', '7');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1200, 'Microsoft', 'Windows', 'Vista');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1300, 'Microsoft', 'Windows', 'XP');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1400, 'Microsoft', 'Windows', '2000');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (1990, 'Microsoft', 'Windows Server', '2012 R2');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2000, 'Microsoft', 'Windows Server', '2012');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2100, 'Microsoft', 'Windows Server', '2008 R2');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2200, 'Microsoft', 'Windows Server', '2008');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2300, 'Microsoft', 'Windows Server', '2003 R2');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2400, 'Microsoft', 'Windows Server', '2003');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (2500, 'Microsoft', 'Windows Server', '2000');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (4000, 'Microsoft', 'Hyper-V Server', '2008');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (4100, 'Microsoft', 'Hyper-V Server', '2008 R2');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (4110, 'Microsoft', 'Hyper-V Server', '2012');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (4120, 'Microsoft', 'Hyper-V Server', '2012 R2');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (5000, 'Apple', 'Mac OS X', '10.5 Leopard');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (5100, 'Apple', 'Mac OS X', '10.6 Snow Leopard');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (5200, 'Apple', 'Mac OS X', '10.7 Lion');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (5300, 'Apple', 'Mac OS X', '10.8 Mountain Lion');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (5350, 'Apple', 'Mac OS X', '10.9 Mavericks');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7000, 'Linux', 'Ubuntu', '7');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7100, 'Linux', 'Ubuntu', '8');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7200, 'Linux', 'Ubuntu', '9');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7300, 'Linux', 'Ubuntu', '10');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7400, 'Linux', 'Ubuntu', '11');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7500, 'Linux', 'Ubuntu', '12');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7500, 'Linux', 'Ubuntu', '13');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (7500, 'Linux', 'Ubuntu', '14');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (8000, 'Linux', 'Debian', '4');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (8100, 'Linux', 'Debian', '5');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (8200, 'Linux', 'Debian', '6');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (8300, 'Linux', 'Debian', '7');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (9000, 'Linux', 'openSuse', '10');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (9100, 'Linux', 'openSuse', '11');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (9200, 'Linux', 'openSuse', '12');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (9200, 'Linux', 'openSuse', '13');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (10000, 'Linux', 'FreeBSD', '6');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (10100, 'Linux', 'FreeBSD', '7');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (10200, 'Linux', 'FreeBSD', '8');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (10300, 'Linux', 'FreeBSD', '9');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (20000, 'Cisco', 'iOS', '12');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (20010, 'Cisco', 'iOS', '13');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (20020, 'Cisco', 'iOS', '14');";
            //installCommands += "INSERT INTO operatingsystems (id, family, distribution, version) VALUES (20030, 'Cisco', 'iOS', '15');";

            //Adding Heartbeatinterval; default = 1 Heartbeat/Minute
            installCommands += "INSERT INTO " + DBPrefix + "settings (setting, value) VALUES('heartbeat', '60000');";

            //Adding Maintanance-Key; 0=off 1=on
            installCommands += "INSERT INTO " + DBPrefix + "settings (setting, value) VALUES('maintmode', '0');";

            //Adding If users can create their own account on login-page
            installCommands += "INSERT INTO " + DBPrefix + "settings (setting, value) VALUES('useraccountcreation', '1');";
            
            //Adding the DB GUID
            if (generateGuid == true)
                installCommands += "INSERT INTO " + DBPrefix + "settings (setting, value) VALUES('guid', '" + Guid.NewGuid().ToString() + "');";

            //Adding ID for Watermark
            installCommands += "INSERT INTO " + DBPrefix + "settings (Setting, value) VALUES('ribbonimg', '0');";
            
            //This HAS TO BE the last Command of all Installation-Commands!
            //Finalize Creation by setting Version to current version
            installCommands += "UPDATE " + DBPrefix + "settings SET value='" + _DBVersion.ToString() + "' WHERE setting='version';";

            try
            {
                SQLiteCommand slCmd = new SQLiteCommand(_DBConnection);

                //_DBConnection.Open();

                slCmd.CommandText = installCommands;
                slCmd.ExecuteNonQuery();            

                //_DBConnection.Close();
            }
            catch (Exception ea) 
            {
                Logger.Log(LogEntryType.Exception, "Cannot install Database", ea, _loggerContext);
                throw new Exception("Cannot install Database. File write-protected?" + System.Environment.NewLine + ea.ToString()); 
            }
        }

        /// <summary>
        /// Generates a new DatabaseGUID
        /// </summary>
        /// <param name="overwriteIfExisting">Overwrite Database-GUID, if there is already a GUID?</param>
        public void CreateDatabaseGuid(bool overwriteIfExisting)
        {
            //Get GUID
            var guid = sqlGetString("SELECT value FROM " + DBPrefix + "settings WHERE setting='guid';");

            if (guid == "") //No GUID exists, create a GUID
            {
                sqlExecuteNonQuery("INSERT INTO " + DBPrefix + "settings (setting, value) VALUES('guid', '" + Guid.NewGuid() + "');");
            }
            else if (overwriteIfExisting) //GUID already exists but it should be regenerated
            {
                sqlExecuteNonQuery("UPDATE " + DBPrefix + "settings SET value='" + Guid.NewGuid() + "' WHERE setting='guid';");
            }
        }

        /// <summary>
        /// Gets the Database-GUID
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseGuid()
        {
            return(sqlGetString("SELECT value FROM " + DBPrefix + "settings WHERE setting='guid';"));
        }

        #endregion
    }
}
