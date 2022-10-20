using System;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;

namespace plusers



{
    internal class Program
    {

        // static class to hold global variables, etc.
        class Globals
        {
            // global int
            public static string AdDomain = "ads.bris.ac.uk";

            public static string AdSecurityGroup = "pl-app-sindri-p0-rw";

            public static string[] CommandSwitches = new string[2];

        }


        private static void GetListOfAdUsersByGroup(string stype)
        {
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain, Globals.AdDomain);

                // define a "query-by-example" principal - here, we search for a GroupPrincipal
                var group = GroupPrincipal.FindByIdentity(ctx, Globals.AdSecurityGroup);


                // if found....
                if (group != null)
                    // iterate over members
                    foreach (var p in group.GetMembers())
                        if (stype == "email")
                            Console.WriteLine("{0}: Login: {1} - Name: {2} Email:{3} ", p.StructuralObjectClass, p.Name,
                                p.DisplayName, p.UserPrincipalName);
                        // do whatever you need to do to those members
                        // var theUser = p as UserPrincipal;
                        //if (theUser != null)
                        //{
                        //     Console.WriteLine(theUser.Name, theUser.EmailAddress, theUser.GivenName);
                        //}
                        else
                            Console.WriteLine("{0}: {1} - {2}", p.StructuralObjectClass, p.Name, p.DisplayName);
            }
            catch (NullReferenceException err1)
            {
                Console.WriteLine(err1);
            }
        }


        private static void Main(string[] args)
        {
            
            if (args.Length == 0)
            {
                GetListOfAdUsersByGroup("basic");
                return;
            }

            //capture Command Arguments 
            Globals.CommandSwitches[0] = args[0].ToLower().Trim();
            if (args.Length == 2)
            {
                Globals.CommandSwitches[1] = args[1].ToLower().Trim();

            }

            switch (args[0].ToLower())
            {
                case "--email-address":
                    //Display Users and email addresses
                    GetListOfAdUsersByGroup("full");
                    break;
                case "-e":
                    goto case "--email-address";
                case "--add-user":
                    //Check username provided
                    if (CheckArgumentUsername() ==false)
                    //Add user to the security group
                    PlusersAddUser();
                    break;
                case "-a":
                    goto case "--add-user";
                case "--remove-user":
                    //Remove user from Security group
                    PlusersRemoveUser();
                    break;
                case "-r":
                    goto case "--remove-user";
                case "--help":
                    //Displays help
                    PlusersHelp();
                    break;
                case "-h":
                    goto case "--help";
                default:
                    Console.WriteLine("Not a valid switch");
                    break;
            }

           // Console.ReadKey();
        }

        private static void PlusersHelp()
        {
            var sb = new System.Text.StringBuilder("This is used to add users to a security group \"" + Globals.AdSecurityGroup + "\" for SSH Access\r\n");
            sb.Append("Command line options:\r\n");
            sb.Append("\t-a , --add-user \tAdd user to the security\r\n");
            sb.Append("\t-e , --email-address \tDisplay emails addresses of the users within the security\r\n");
            sb.Append("\t-r , --remove-user \tRemove user from security group\r\n");
            sb.Append("\t-h , --help \t\tDisplays help, this.\r\n");


            Console.WriteLine (sb.ToString());

        }
        private static void PlusersRemoveUser()
        {
            //code
        }
        private static void PlusersAddUser()
        {
            //code
        }

        private static bool CheckArgumentUsername()
        {
            if (null == Globals.CommandSwitches[1])
            {
                Console.WriteLine("Missing Username");
                return false;
            }

            //Check username provided is Correct
            //blah blah

            //return username is good
            return true;
        }

    }
}