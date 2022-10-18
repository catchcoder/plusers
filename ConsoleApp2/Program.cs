using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
//https://community.spiceworks.com/topic/1435726-best-way-to-get-members-of-an-ad-group-in-c-net-4-0

namespace ConsoleApp2
{
    internal class Program
    {


       static void GetListOfAdUsersByGroup()
        {
            try
            {
                // string userName = "cecth-a";
                // create your domain context
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "ads.bris.ac.uk");

                // define a "query-by-example" principal - here, we search for a GroupPrincipal
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, "pl-app-sindri-p0-rw");


                // if found....
                if (group != null)
                {
                    // iterate over members
                    foreach (Principal p in group.GetMembers())
                    {
                        Console.WriteLine("{0}: {1}", p.StructuralObjectClass, p.DisplayName);

                        // do whatever you need to do to those members
                        UserPrincipal theUser = p as UserPrincipal;

                        if (theUser != null)
                        {
                            if (theUser.IsAccountLockedOut())
                            {
                                //Console.WriteLine ("disabled");
                            }
                            else
                            {
                                //Console.WriteLine("enabled");
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException err1)
            {

                Console.WriteLine(err1);
            }

        }



        

        static void Main(string[] args)
        {
            GetListOfAdUsersByGroup();
            
        }
    }
}
