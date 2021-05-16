using PermissionParts;
using System;
using System.IO;
using System.Linq;

namespace ConsoleTest
{
    class Program
    {
        private const string RolesFilename = "Roles.txt";

        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            var pathRolesData = Path.Combine(path,RolesFilename);
            var lines = File.ReadAllLines(pathRolesData);
            foreach (var line in lines)
            {
                var colonIndex = line.IndexOf(':');
                var roleName = line.Substring(0, colonIndex);
                var permissions = line.Substring(colonIndex + 1).Split(',')
                    .Select(x => Enum.TryParse(typeof(Permissions), x.Trim(), true,out object r) ? r: Permissions.NotSet)
                    .Cast<Permissions>().ToList();
               var result = PermissionPackers.PackPermissionsIntoString(permissions);
               var unresult = PermissionPackers.UnpackPermissionsFromString(result);
            }
        }
    }
}
