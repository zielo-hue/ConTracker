using System;
using contrackerNET;
using Disqord;

namespace contracker.Services
{
    public class ContrackerService
    {
        public ContrackerAPI Contracker;

        public ContrackerService(string key)
        {
            Contracker = new ContrackerAPI(key);
        }

        public bool IsRegistered(Snowflake discordId)
        {
            try
            {
                Contracker.GetPlayer(discordId: discordId.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("This is PROBABLY not an error:" + e);
                return false;
            }
        }
    }
}