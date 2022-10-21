using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace plusers


{
    internal class Program
    {
        private static void GetListOfAdUsersByGroup(string stype)
        {
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain, Globals.AdDomain);

                // define a "query-by-example" principal - here, we search for a GroupPrincipal
                var group = GroupPrincipal.FindByIdentity(ctx, Globals.AdSecurityGroup);


                // if found....
                if (group == null) return;
                // iterate over members
                foreach (var p in group.GetMembers())
                    if (stype == "email")
                        Console.WriteLine("{0}: Login: {1} - Name: {2} Email:{3} ", p.StructuralObjectClass, p.Name,
                            p.DisplayName, p.UserPrincipalName);
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
            try
            {
                Console.WriteLine("PL User v0.1 - Tool to manage users in a Security Group\n");

                if (args.Length == 0)
                {
                    GetListOfAdUsersByGroup("basic");
                    return;
                }

                //capture Command Arguments 
                Globals.CommandSwitches[0] = args[0].ToLower().Trim();
                if (args.Length == 2) Globals.CommandSwitches[1] = args[1].ToLower().Trim();

                switch (args[0].ToLower())
                {
                    case "--email-addresses":
                        //Display Users and email addresses
                        GetListOfAdUsersByGroup("full");
                        break;
                    case "--add-user":
                        //Check username provided
                        if (CheckArgumentUsername())
                            //Add user to the security group
                            PlusersAddUser();
                        break;
                    case "-a":
                        goto case "--add-user";
                    case "--remove-user":
                        if (CheckArgumentUsername())
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

                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                throw;
            }
        }

        private static void PlusersHelp()
        {
            var sb = new StringBuilder("This is used to add users to a Security Group \"{0}\" for SSH Access\r\n\r\n");
            //sb.Append("Copyright (c) 2022 Chris Hawkins\r\n");
            sb.Append("Usage:\r\n");
            sb.Append("plusers.exe [option] <user name>\r\n\r\n");
            sb.Append("Command line options:\r\n");
            sb.Append("\t-a , --add-user \tAdd user to the security\r\n");
            sb.Append("\t-r , --remove-user \tRemove user from security group\r\n\r\n");
            sb.Append("\t--email-address \tDisplay emails addresses of the users within the security\r\n");

            Console.WriteLine(sb.ToString(), Globals.AdSecurityGroup);
        }

        private static void PlusersRemoveUser()
        {
            try
            {
                if (DoesUserExist())
                {
                    if (!IsInGroup())
                    {
                        Console.WriteLine("User {0} is not a member of {1}", Globals.CommandSwitches[1], Globals.AdSecurityGroup);
                        return;
                    }
                    using (var pc = new PrincipalContext(ContextType.Domain, Globals.AdDomain))
                    {
                        var group = GroupPrincipal.FindByIdentity(pc, Globals.AdSecurityGroup);
                        if (group == null) return;
                        group.Members.Remove(pc, IdentityType.SamAccountName,
                            Globals.AdDomain + "\\" + Globals.CommandSwitches[1]);
                        group.Save();
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                throw;
            }
        }

        private static void PlusersAddUser()
        {
            try
            {
                if (DoesUserExist())
                {
                    if (IsInGroup())
                    {
                        Console.WriteLine("User {0} is already a member of {1}", Globals.CommandSwitches[1],
                            Globals.AdSecurityGroup);
                        return;
                    }

                    using (var pc = new PrincipalContext(ContextType.Domain, Globals.AdDomain))
                    {
                        var group = GroupPrincipal.FindByIdentity(pc, Globals.AdSecurityGroup);
                        if (group != null)
                        {
                            group.Members.Add(pc, IdentityType.SamAccountName,
                                Globals.AdDomain + "\\" + Globals.CommandSwitches[1]);
                            group.Save();
                        }
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                throw;
            }
        }

        private static bool DoesUserExist()
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, Globals.AdDomain))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName,
                           Globals.CommandSwitches[1]))
                {
                    return foundUser != null;
                }
            }
        }

        private static bool IsInGroup()
        {
            string username = Environment.UserName;

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Globals.AdDomain);

            UserPrincipal userPrincipal =
                UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Globals.CommandSwitches[1]);

            bool isMember = userPrincipal.IsMemberOf(ctx, IdentityType.Name, Globals.AdSecurityGroup);

            return isMember;
        }

        private static bool CheckArgumentUsername()
        {
            if (null != Globals.CommandSwitches[1]) return true;
            Console.WriteLine("Missing Username");
            return false;

        }

        // static class to hold global variables, etc.
        private class Globals
        {
            // global variable
            public static readonly string AdDomain = "ads.bris.ac.uk";

            public static readonly string AdSecurityGroup = "pl-app-sindri-p0-rw";

            public static string[] CommandSwitches = new string[2];
        }
    }
}