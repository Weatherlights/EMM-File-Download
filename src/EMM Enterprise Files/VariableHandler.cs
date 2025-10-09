using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    internal partial class VariableHandler
    {
        public static partial string GetDownloadFolder();
        public static partial string GetDocumentsFolder();
        public static partial string GetPicturesFolder();
        public static partial string GetExternalStorageFolder();
        public static partial string GetDataFolder();

        static public string ResolveVariables(string unresolvedString)
        {
            string resolvedString = unresolvedString;
            //  Regex rx = new Regex(@"\[(?>\[(?<LEVEL>)|\](?<-LEVEL>)|(?!\[|\]).)+(?(LEVEL)(?!))\]");
            Regex rx = new Regex(@"\[(?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!))\]");
            //Regex rx = new Regex(@"\[([^[]*?)\]");
            MatchCollection matches = rx.Matches(resolvedString);
            int totalMatches = matches.Count;
            int lostLength = 0;
            int gainedLength = 0;
            //while (totalMatches >= 0 )
            for (int i = 0; i < matches.Count; i++)
            {

                Match match = matches[i];

                GroupCollection groups = match.Groups;
                string variable = groups[0].Value;
                int index = groups[0].Index;
                int length = groups[0].Length;
                int actualIndex = index - lostLength + gainedLength;
                string data = getResolvedVariable(variable);
                if (data != null)
                {
                    resolvedString = resolvedString.Remove(actualIndex, length);
                    lostLength += length;
                    resolvedString = resolvedString.Insert(actualIndex, data);
                    gainedLength += data.Length;

                    //matches = rx.Matches(resolvedString);
                    //totalMatches = matches.Count;

                }
                else
                {

                }
            }

            static string[] prepareParameters(string variable)
            {
                string preparedVariable = variable.TrimStart('[').TrimEnd(']');
                Regex rx = new Regex(@"\[(?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!))\]");
                //Regex rx = new Regex(@"\[([^[]*?)\]");
                MatchCollection matches = rx.Matches(preparedVariable);
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    string otherVariable = groups[0].Value;
                    preparedVariable = preparedVariable.Replace(otherVariable, "[" + EncodeTo64(otherVariable) + "]");
                }
                string[] splittedVariable = preparedVariable.Split('|');
                for (int i = 0; i < splittedVariable.Length; i++)
                {
                    MatchCollection b64m = rx.Matches(splittedVariable[i]);
                    foreach (Match match in b64m)
                    {
                        GroupCollection groups = match.Groups;
                        string encodedVariable = groups[0].Value;
                        string decodedVariable = DecodeFrom64(encodedVariable.TrimStart('[').TrimEnd(']'));
                        splittedVariable[i] = splittedVariable[i].Replace(encodedVariable, decodedVariable);
                    }
                }


                return splittedVariable;


            }

            static string EncodeTo64(string toEncode)
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;

            }

            static string DecodeFrom64(string encodedData)
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                return returnValue;
            }

            static string getResolvedVariable(string variable)
            {
                //            LogWriter myLogWriter = new LogWriter("getResolvedVariable");
                //            myLogWriter.LogWrite("Resolving " + variable + " to ");
                //            Console.Write("Resolving " + variable + " to ");
                string[] parameters = prepareParameters(variable);
                string value = "";
                /* if ( variable.Contains("|") )
                // {

                     parameters = variable.Split('|');
                     variable = parameters[0];
                     parameters[parameters.Length - 1] = parameters[parameters.Length-1].TrimEnd(']');
                 }*/
                switch (parameters[0])
                {
                    //                case "[INSTALLDIR]":
                    //                    value = Package.Current.InstalledPath;
                    //                   break;
                    case "SPECIALFOLDER":
                        if (parameters.Length > 1)
                            switch (parameters[1])
                            {
                                case "DOWNLOAD":
                                    value = GetDownloadFolder();
                                    break;
                                case "DOCUMENTS":
                                    value = GetDocumentsFolder();
                                    break;
                                case "PICTURES":
                                    value = GetPicturesFolder();
                                    break;
                                case "DATA":
                                    value = GetDataFolder();
                                    break;
                                case "EXTERNALSTORAGE":
                                    value = GetExternalStorageFolder();
                                    break;
                            }

                           // value = System.Environment.GetEnvironmentVariable(parameters[1]);
                        break;
                    default:
                        string envvariabel = variable.TrimStart('[').TrimEnd(']');
                        value = System.Environment.GetEnvironmentVariable(envvariabel);
                        break;
                }

                //            myLogWriter.LogWrite(value);
                //            Console.WriteLine(value);
                return value;

            }

            return resolvedString;
        }
    }
}
