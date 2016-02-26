using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Common.Vpn.Cisco
{
    public enum VpnClientReturnCode
    {
        SUCCESS_START = 200,        //The VPN Client connection started successfully.
        SUCCESS_STOP = 201,         //The VPN Client connection has ended.
        SUCCESS_STAT = 202,         //The VPN Client has generated statistical information successfully.
        SUCCESS_ENUMPPP = 203,      //The enumppp command has succeeded. This command lists phone book entries when connecting to the Internet via dial-up.
        ERR_UNKNOWN = 1,            //An unidentifiable error has occurred during command-line parsing.
        ERR_MISSING_COMMAND = 2,    //Command is missing from command-line input.
        ERR_BAD_COMMAND = 3,        //There is an error in the command entered; check spelling.
        ERR_MISSING_PARAMS = 4,     //The command-line input is missing required parameter(s).
        ERR_BAD_PARAMS = 5,         //The parameter(s) in the command input are incorrect; check spelling.
        ERR_TOO_MANY_PARAMS = 6,    //The command-line input contains too many parameters.
        ERR_NO_PARAMS_NEEDED = 7,   //The command entered does not require parameters.
        ERR_ATTACH_FAILED = 8,      //Interprocess communication error occurred attaching to the generic interface.
        ERR_DETACH_FAILED = 9,      //Interprocess communication error occurred detaching from the generic interface.
        ERR_NO_PROFILE = 10,        //The VPN Client failed to read the profile.
        ERR_PWD_MISMATCHED = 11,    //Reserved
        ERR_PWD_TOO_LONG = 12,      //The password contains too many characters. The group password limit is 32 characters; the certificate password limit is 255 characters.
        ERR_TOO_MANY_TRIES = 13,    //Attempts to enter a valid password have exceed the amount allowed. The limit is three times.
        ERR_START_FAILED = 14,      //The connection attempt has failed; unable to connect.
        ERR_STOP_FAILED = 15,       //The disconnect action has failed; unable to disconnect.
        ERR_STAT_FAILED = 16,       //The attempt to display connection status has failed.
        ERR_ENUM_FAILED = 17,       //Unable to list phonebook entries.
        ERR_COMMUNICATION_FAILED = 18, //A serious interprocess communication error has occurred.
        ERR_SET_HANDLER_FAILED = 19, //Set console control handler failed.
        ERR_CLEAR_HANDLER_FAILED = 20, //Attempt to clean up after a user break failed.
        ERR_OUT_OF_MEMORY = 21,     //Out of memory. Memory allocation failed.
        ERR_BAD_INTERFACE = 22,     //Internal display error.
        ERR_UNEXPECTED_CALLBACK = 23, //In communicating with the Connection Manager, an unexpected callback (response) occurred.
        ERR_DO_NOT_CONTINUE = 24,   //User quit at a banner requesting "continue?"
        ERR_GUI_RUNNING = 25,       //Cannot use the command-line interface when connected through the graphical interface dialer application.
        ERR_SET_WORK_DIR_FAILED = 26, //The attempt to set the working directory has failed. This is the directory where the program files reside.
        ERR_NOT_CONNECTED = 27,     //Attempt to display status has failed because there is no connection in effect.
        ERR_BAD_GROUP_NAME = 28,    //The group name configured for the connection is too long. The limit is 128 characters.
        ERR_BAD_GROUP_PWD = 29,     //The group password configured for the connection is too long. The limit is 32 characters.
        ERR_BAD_AUTHTYPE = 30,      //The authentication type configured for the connection is invalid.
        RESERVED_01 = 31,           //Reserved.
        RESERVED_02 = 32,           //Reserved.
        ERR_COMMUNICATION_TIMED_OUT = 33, //Interprocess communication timed out.
        ERR_BAD_3RD_PARTY_DIAL = 34, //Failed to launch a third-party dialer.
        ERR_DAEMON_NOT_RUNNING = 35, //(CVPND.EXE)—Non-Windows only = 35, //Connection needs to be established for command to execute.
        ERR_DAEMON_ALREADY_RUNNING = 36 //(CVPND.EXE)—Non-Windows only = 36 //Command cannot work because connection is already established.
    }
}
