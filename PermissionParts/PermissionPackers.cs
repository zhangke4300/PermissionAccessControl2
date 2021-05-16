// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PermissionParts
{
    public static class PermissionPackers
    {
        public static string PackPermissionsIntoString(this IEnumerable<Permissions> permissions)
        {
            return permissions.Aggregate("", (s, permission) => s + (char)((ushort)permission / 256) + (char)((ushort)permission % 256));
        }

        public static IEnumerable<Permissions> UnpackPermissionsFromString(this string packedPermissions)
        {
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            if (packedPermissions == string.Empty) yield return Permissions.NotSet;
            var perArray = packedPermissions.ToCharArray();
            for (int i = 0; i < perArray.Length / 2; i++)
            {
                yield return ((Permissions)((ushort)perArray[i * 2] * 256 + perArray[i * 2 + 1]));
            }
        }

        public static Permissions? FindPermissionViaName(this string permissionName)
        {
            return Enum.TryParse(permissionName, out Permissions permission)
                ? (Permissions?) permission
                : null;
        }

    }
}