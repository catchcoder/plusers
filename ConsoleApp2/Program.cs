using System;
using System.DirectoryServices.AccountManagement;

namespace plusers



{
    internal class Program
    {
        private string[] argu;

        private static void GetListOfAdUsersByGroup(string stype)
        {
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain, "ads.bris.ac.uk");

                // define a "query-by-example" principal - here, we search for a GroupPrincipal
                var group = GroupPrincipal.FindByIdentity(ctx, "pl-app-sindri-p0-rw");


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
            argu[] = args[];

            if (args.Length == 0)
            {
                GetListOfAdUsersByGroup("basic");
                return;
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
        }

        private static void PlusersHelp()
        {

        }
        private static void PlusersRemoveUser()
        {

        }
        private static void PlusersAddUser()
        {

        }

        private static void CheckArgumentUsername()
        {
            if (null == args[1])
            {

            } 
        }

    }
}