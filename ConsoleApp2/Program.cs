using System;
using System.DirectoryServices.AccountManagement;

//https://community.spiceworks.com/topic/1435726-best-way-to-get-members-of-an-ad-group-in-c-net-4-0

namespace plusers
{
    internal class Program
    {
        private static void GetListOfAdUsersByGroup()
        {
            try
            {
                // string userName = "cecth-a";
                // create your domain context
                var ctx = new PrincipalContext(ContextType.Domain, "ads.bris.ac.uk");

                // define a "query-by-example" principal - here, we search for a GroupPrincipal
                var group = GroupPrincipal.FindByIdentity(ctx, "pl-app-sindri-p0-rw");


                // if found....
                if (group != null)
                    // iterate over members
                    foreach (var p in group.GetMembers())
                    {
                        Console.WriteLine("{0}: {1} - {2}", p.StructuralObjectClass, p.Name, p.DisplayName);

                        // do whatever you need to do to those members
                        var theUser = p as UserPrincipal;

                        if (theUser != null)
                        {
                           // Console.WriteLine(theUser.Name);
                        }



                    }
            
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
                GetListOfAdUsersByGroup();
                return ;
            }

            switch (args[0].ToLower())
            {
                case "--full":
                    //do something
                    break;
                case "-f":
                    goto case "--full";
                case "--add-user":
                    //do something
                    break;
                case "-a":
                    goto case "--add-user";
                case "--remove-user":
                    //do something
                    break;
                case "-r":
                    goto case "--remove-user";
                case "--help":
                    //do something
                    break;
                case "-h":
                    goto case "--help";
                default:
                    Console.WriteLine("Not a valid switch");
                    break;
            }
            Console.WriteLine("param {0}", args[0]);
            Console.ReadKey();
            return ;
        }
    }
}