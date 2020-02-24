using System;
using Qmmands;

namespace contracker.Resources
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContrackerAttribute : Attribute
    {
        /*
         * TODO: Attribute implementation!
         *     Adding essentially the same if statement to commands that use the Contracker API makes code somewhat
         *     unclean. Still looking into how exactly attributes work.
         */
    }
}