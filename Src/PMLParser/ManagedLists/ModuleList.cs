using System;
using System.Collections.Generic;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.ManagedLists
{
    internal static class ModuleList
    {
        private static List<PMLModule> knownModules = new List<PMLModule>();

        #region Methods
        internal static int LocateModuleInList(string modulePath)
        {
            return knownModules.FindIndex(module => module.Path.Equals(modulePath, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static int LocateInOrAddToModuleList(string modulePath)
        {
            var ix = knownModules.FindIndex(module => module.Path.Equals(modulePath, StringComparison.CurrentCultureIgnoreCase));
            if (ix < 0)
            {
                return AddModuleToList(PMLModule.CreateUnknownModule(modulePath));
            }
            return ix;
        }

        internal static int AddModuleToList(PMLModule module)
        {
            knownModules.Add(module);
            return knownModules.Count - 1;
        }

        internal static string GetModuleDescription(int index)
        {
            return knownModules[index].Description;
        }

        internal static string GetModulePath(int index)
        {
            return knownModules[index].Path;
        }
        #endregion
    }
}
